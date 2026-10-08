namespace OpenCodeLauncher.Core.Models;

/// <summary>
/// Статистика использования токенов и стоимость, полученная из <c>opencode stats</c>.
/// Если какая-то величина отсутствует в выводе — она равна 0.
/// </summary>
public sealed record UsageStats(
    long TokensIn,
    long TokensOut,
    decimal Cost);