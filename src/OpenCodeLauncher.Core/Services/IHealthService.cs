using OpenCodeLauncher.Core.Models;

namespace OpenCodeLauncher.Core.Services;

/// <summary>Проверка доступности запущенного сервера по HTTP.</summary>
public interface IHealthService
{
    /// <summary>Проверяет, что по <paramref name="baseUrl"/> (например, http://ip:port/) отвечает веб-интерфейс.</summary>
    Task<HealthCheckResult> CheckAsync(string baseUrl, TimeSpan timeout, CancellationToken cancellationToken = default);
}