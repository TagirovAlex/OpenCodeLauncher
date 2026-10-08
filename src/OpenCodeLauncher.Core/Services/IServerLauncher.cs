using OpenCodeLauncher.Core.Models;

namespace OpenCodeLauncher.Core.Services;

/// <summary>Запуск сервера с ожиданием готовности по HTTP.</summary>
public interface IServerLauncher
{
    /// <summary>
    /// Запускает сервер и ждёт готовности. При успехе вызывает <see cref="IProcessService.MarkRunning"/>.
    /// Возвращает <c>true</c>, если сервер стал доступен; <c>false</c>, если процесс упал или истёк таймаут.
    /// Исключения <see cref="IProcessService.Start"/> (InvalidOperationException, FileNotFoundException и т.п.) пробрасываются наружу.
    /// </summary>
    Task<bool> StartAndWaitReadyAsync(LaunchParams parameters, CancellationToken cancellationToken = default);
}