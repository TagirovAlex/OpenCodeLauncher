namespace OpenCodeLauncher.Core.Models;

/// <summary>Результат проверки доступности сервера.</summary>
public sealed record HealthCheckResult(
    bool IsReachable,
    TimeSpan? Latency = null,
    string? Error = null);