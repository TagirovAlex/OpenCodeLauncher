using System.Text.RegularExpressions;
using OpenCodeLauncher.Core.Models;

namespace OpenCodeLauncher.Core.Services;

/// <summary>
/// Запускает opencode-сервер и ожидает его готовности.
/// Готовность определяется по строке <c>opencode server listening on http://...</c> в stdout
/// (локальный HTTP-пробинг ненадёжен: Windows/Tailscale не позволяют достучаться до собственного
/// Tailscale-IP с той же машины).
/// </summary>
public sealed class ServerLauncher : IServerLauncher
{
    private static readonly Regex ListeningPattern = new(
        @"listening on http://", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    /// <summary>Интервал опроса состояния процесса при ожидании готовности.</summary>
    private static readonly TimeSpan ReadyCheckInterval = TimeSpan.FromMilliseconds(250);

    /// <summary>Общий лимит ожидания готовности сервера.</summary>
    private static readonly TimeSpan ReadyLimit = TimeSpan.FromSeconds(15);

    private readonly IProcessService _processService;

    /// <summary>Создаёт лаунчер сервера.</summary>
    public ServerLauncher(IProcessService processService)
    {
        _processService = processService ?? throw new ArgumentNullException(nameof(processService));
    }

    /// <inheritdoc/>
    public async Task<bool> StartAndWaitReadyAsync(LaunchParams parameters, CancellationToken cancellationToken = default)
    {
        _processService.Start(parameters);

        var readyTcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        void OnOutput(object? sender, ProcessOutputEventArgs e)
        {
            if (ListeningPattern.IsMatch(e.Text))
            {
                readyTcs.TrySetResult(true);
            }
        }

        _processService.OutputReceived += OnOutput;
        try
        {
            var deadline = DateTime.UtcNow + ReadyLimit;
            while (!readyTcs.Task.IsCompleted)
            {
                var status = _processService.Status;
                if (status == ServerStatus.Stopped || status == ServerStatus.Crashed)
                {
                    return false;
                }

                var remaining = deadline - DateTime.UtcNow;
                if (remaining <= TimeSpan.Zero)
                {
                    return false;
                }

                var delay = remaining < ReadyCheckInterval ? remaining : ReadyCheckInterval;
                try
                {
                    var completed = await Task.WhenAny(readyTcs.Task, Task.Delay(delay, cancellationToken)).ConfigureAwait(false);
                    if (completed == readyTcs.Task)
                    {
                        _processService.MarkRunning();
                        return true;
                    }
                }
                catch (OperationCanceledException)
                {
                    return false; // внешняя отмена или таймаут
                }
            }

            _processService.MarkRunning();
            return true;
        }
        finally
        {
            _processService.OutputReceived -= OnOutput;
        }
    }
}