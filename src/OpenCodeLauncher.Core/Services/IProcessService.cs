using OpenCodeLauncher.Core.Models;

namespace OpenCodeLauncher.Core.Services;

/// <summary>Аргументы события вывода процесса.</summary>
public sealed class ProcessOutputEventArgs(string stream, string text) : EventArgs
{
    /// <summary>"stdout" или "stderr".</summary>
    public string Stream { get; } = stream;

    public string Text { get; } = text;
}

/// <summary>Аргументы события изменения состояния процесса.</summary>
public sealed class ServerStatusChangedEventArgs(ServerStatus status) : EventArgs
{
    public ServerStatus Status { get; } = status;
}

/// <summary>Управление жизненным циклом процесса opencode-сервера.</summary>
public interface IProcessService : IDisposable
{
    bool IsRunning { get; }

    LaunchParams? CurrentLaunch { get; }

    ServerStatus Status { get; }

    event EventHandler<ProcessOutputEventArgs>? OutputReceived;

    event EventHandler<ServerStatusChangedEventArgs>? StatusChanged;

    /// <summary>Запускает <c>opencode serve</c> с заданными параметрами. Повторный вызов без остановки — ошибка.</summary>
    void Start(LaunchParams parameters);

    /// <summary>Переводит статус в Running после успешной проверки доступности сервера.</summary>
    void MarkRunning();

    /// <summary>Корректно останавливает процесс.</summary>
    Task StopAsync(CancellationToken cancellationToken = default);
}