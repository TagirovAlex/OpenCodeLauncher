using OpenCodeLauncher.Core.Models;

namespace OpenCodeLauncher.Core.Services;

/// <summary>Получение информации о сессиях и статистике из CLI opencode.</summary>
public interface IOpenCodeInfoService
{
    /// <summary>
    /// Возвращает список сессий проекта (<c>opencode session list</c>).
    /// При неожиданном формате вывода или таймауте возвращает пустой список.
    /// Бросает <see cref="FileNotFoundException"/>, если opencode не найден.
    /// </summary>
    Task<IReadOnlyList<OpenCodeSession>> GetSessionsAsync(string projectPath, CancellationToken ct = default);

    /// <summary>
    /// Возвращает статистику токенов/стоимости (<c>opencode stats</c>).
    /// При неожиданном формате вывода или таймауте возвращает null.
    /// Бросает <see cref="FileNotFoundException"/>, если opencode не найден.
    /// </summary>
    Task<UsageStats?> GetStatsAsync(string projectPath, CancellationToken ct = default);
}