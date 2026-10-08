using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using OpenCodeLauncher.Core.Models;

namespace OpenCodeLauncher.Core.Services;

/// <summary>
/// Управление жизненным циклом процесса <c>opencode serve</c>.
/// </summary>
/// <remarks>
/// События <see cref="OutputReceived"/> и <see cref="StatusChanged"/> могут подниматься
/// из любых потоков (поток чтения вывода, поток ожидания процесса). UI-подписчик
/// должен самостоятельно маршалить вызовы в UI-поток (например, через <c>Invoke</c>).
/// </remarks>
public sealed class ProcessService : IProcessService
{
    /// <summary>Кандидаты на запуск opencode в порядке предпочтения.</summary>
    private static readonly string[] OpenCodeCandidates = new[] { "opencode.exe", "opencode.cmd", "opencode.bat" };

    private static readonly TimeSpan GracePeriod = TimeSpan.FromMilliseconds(300);
    private static readonly TimeSpan WaitStep = TimeSpan.FromMilliseconds(50);

    private readonly object _lifecycleLock = new();
    private Process? _process;
    private LaunchParams? _current;
    private ServerStatus _status = ServerStatus.Stopped;
    private bool _stopping;

    /// <inheritdoc/>
    public event EventHandler<ProcessOutputEventArgs>? OutputReceived;

    /// <inheritdoc/>
    public event EventHandler<ServerStatusChangedEventArgs>? StatusChanged;

    /// <summary>Ищет исполняемый файл opencode в PATH; возвращает null, если не найден.</summary>
    public static string? TryLocateOpenCode()
    {
        try
        {
            return LocateOpenCode();
        }
        catch (FileNotFoundException)
        {
            return null;
        }
    }

    /// <summary>
    /// Ищет opencode в PATH с предпочтением <c>opencode.exe</c> → <c>opencode.cmd</c> → <c>opencode.bat</c>.
    /// </summary>
    /// <param name="searchPath">
    /// Путь поиска, разделённый <see cref="Path.PathSeparator"/>. Если не задан — берётся переменная окружения PATH.
    /// Параметр нужен для тестируемости.
    /// </param>
    public static string LocateOpenCode(string? searchPath = null)
    {
        var pathValue = searchPath ?? Environment.GetEnvironmentVariable("PATH");

        if (string.IsNullOrEmpty(pathValue))
        {
            throw new FileNotFoundException("opencode не найден. Установите его через npm: npm i -g opencode-ai");
        }

        foreach (var executable in OpenCodeCandidates)
        {
            foreach (var directory in pathValue.Split(Path.PathSeparator))
            {
                if (string.IsNullOrEmpty(directory))
                {
                    continue;
                }

                var candidate = Path.Combine(directory, executable);
                if (File.Exists(candidate))
                {
                    return candidate;
                }
            }
        }

        throw new FileNotFoundException("opencode не найден. Установите его через npm: npm i -g opencode-ai");
    }

    /// <summary>
    /// Собирает список аргументов для <c>opencode serve</c> в фиксированном порядке:
    /// базовые флаги, затем опциональные model/continue/session, затем ExtraArgs.
    /// </summary>
    public static IReadOnlyList<string> BuildArguments(LaunchParams p)
    {
        var arguments = new List<string> { "serve", "--hostname", p.Hostname, "--port", p.Port.ToString() };

        if (!string.IsNullOrEmpty(p.Model))
        {
            arguments.Add("--model");
            arguments.Add(p.Model);
        }

        if (p.ContinueLastSession)
        {
            arguments.Add("--continue");
        }

        if (!string.IsNullOrEmpty(p.SessionId))
        {
            arguments.Add("--session");
            arguments.Add(p.SessionId);
        }

        if (!string.IsNullOrEmpty(p.ExtraArgs))
        {
            foreach (var part in Regex.Split(p.ExtraArgs.Trim(), "\\s+"))
            {
                if (!string.IsNullOrEmpty(part))
                {
                    arguments.Add(part);
                }
            }
        }

        return arguments;
    }

    /// <inheritdoc/>
    public bool IsRunning
    {
        get
        {
            lock (_lifecycleLock)
            {
                return _status != ServerStatus.Stopped && _status != ServerStatus.Crashed;
            }
        }
    }

    /// <inheritdoc/>
    public LaunchParams? CurrentLaunch => _current;

    /// <inheritdoc/>
    public ServerStatus Status
    {
        get
        {
            lock (_lifecycleLock)
            {
                return _status;
            }
        }
    }

    /// <inheritdoc/>
    public void Start(LaunchParams parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);
        Validate(parameters);

        var executable = LocateOpenCode();

        lock (_lifecycleLock)
        {
            if (_status != ServerStatus.Stopped && _status != ServerStatus.Crashed)
            {
                throw new InvalidOperationException("Сервер уже запущен");
            }

            _current = parameters;
            _stopping = false;

            var startInfo = new ProcessStartInfo
            {
                FileName = executable,
                WorkingDirectory = parameters.ProjectPath,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };

            foreach (var argument in BuildArguments(parameters))
            {
                startInfo.ArgumentList.Add(argument);
            }

            // Базовая авторизация сервера: заданный в приложении пароль передаём серверу
            // через окружение (иначе сервер наследует системные переменные или работает без пароля).
            if (!string.IsNullOrEmpty(parameters.ServerPassword))
            {
                startInfo.Environment["OPENCODE_SERVER_PASSWORD"] = parameters.ServerPassword;
                startInfo.Environment["OPENCODE_SERVER_USERNAME"] =
                    string.IsNullOrWhiteSpace(parameters.ServerUsername) ? "opencode" : parameters.ServerUsername;
            }

            var process = new Process { StartInfo = startInfo };
            process.EnableRaisingEvents = true;
            process.Start();
            _process = process;

            process.OutputDataReceived += (_, e) => RaiseOutput("stdout", e.Data);
            process.ErrorDataReceived += (_, e) => RaiseOutput("stderr", e.Data);
            process.Exited += (_, _) => OnProcessExited(process);
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
        }

        SetStatus(ServerStatus.Starting);
    }

    /// <inheritdoc/>
    public void MarkRunning()
    {
        SetStatus(ServerStatus.Running);
    }

    /// <inheritdoc/>
    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        Process? process;
        lock (_lifecycleLock)
        {
            if (_process == null || _stopping)
            {
                return;
            }

            _stopping = true;
            process = _process;
        }

        SetStatus(ServerStatus.Stopping);

        await StopProcessAsync(process!, cancellationToken);

        lock (_lifecycleLock)
        {
            if (_process == process)
            {
                _process = null;
            }

            _current = null;
            _stopping = false;
        }

        SetStatus(ServerStatus.Stopped);
    }

    /// <summary>Освобождает ресурсы: останавливает процесс, если он запущен.</summary>
    public void Dispose()
    {
        Process? process;
        lock (_lifecycleLock)
        {
            if (_process == null || _stopping)
            {
                return;
            }

            _stopping = true;
            process = _process;
        }

        SetStatus(ServerStatus.Stopping);

        StopProcess(process!, null);

        lock (_lifecycleLock)
        {
            if (_process == process)
            {
                _process = null;
            }

            _current = null;
            _stopping = false;
        }

        SetStatus(ServerStatus.Stopped);
    }

    private static void Validate(LaunchParams parameters)
    {
        if (string.IsNullOrWhiteSpace(parameters.Hostname))
        {
            throw new ArgumentException("Hostname не должен быть пустым");
        }

        if (parameters.Port < 1 || parameters.Port > 65535)
        {
            throw new ArgumentException("Port должен быть в диапазоне 1..65535");
        }

        if (!Directory.Exists(parameters.ProjectPath))
        {
            throw new ArgumentException("ProjectPath должен указывать на существующий каталог: " + parameters.ProjectPath);
        }
    }

    private void OnProcessExited(Process process)
    {
        bool wasStopping;
        lock (_lifecycleLock)
        {
            wasStopping = _stopping;
            if (_process == process)
            {
                _process = null;
            }
        }

        SetStatus(wasStopping ? ServerStatus.Stopped : ServerStatus.Crashed);
    }

    private void RaiseOutput(string stream, string? data)
    {
        if (string.IsNullOrEmpty(data))
        {
            return;
        }

        OutputReceived?.Invoke(this, new ProcessOutputEventArgs(stream, data));
    }

    /// <summary>
    /// Асинхронная остановка процесса: короткая пауза на штатный выход, при необходимости Kill
    /// всего дерева процесса, затем ожидание завершения с учётом отмены.
    /// </summary>
    private async Task StopProcessAsync(Process process, CancellationToken cancellationToken)
    {
        using var gracefulTimeout = new CancellationTokenSource(GracePeriod);
        try
        {
            await process.WaitForExitAsync(gracefulTimeout.Token);
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            // Штатный выход не произошёл за отведённое время — принудительно завершаем дерево.
            process.Kill(entireProcessTree: true);
            try
            {
                await process.WaitForExitAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Внешняя отмена: процесс может остаться жив.
            }
        }
    }

    /// <summary>Синхронная остановка процесса (для Dispose).</summary>
    private void StopProcess(Process process, CancellationToken? cancellationToken)
    {
        if (!process.WaitForExit(GracePeriod))
        {
            process.Kill(entireProcessTree: true);
        }

        while (!process.WaitForExit(WaitStep))
        {
            if (cancellationToken is { IsCancellationRequested: true })
            {
                break;
            }
        }
    }

    private void SetStatus(ServerStatus newStatus)
    {
        lock (_lifecycleLock)
        {
            if (_status == newStatus)
            {
                return;
            }

            _status = newStatus;
        }

        StatusChanged?.Invoke(this, new ServerStatusChangedEventArgs(newStatus));
    }
}