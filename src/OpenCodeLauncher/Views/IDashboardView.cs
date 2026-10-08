using OpenCodeLauncher.Core.Models;

namespace OpenCodeLauncher.Views;

/// <summary>
/// Представление вкладки «Дашборд». «Глупое» вью: вся логика — в презентере,
/// методы вью вызываются из любых потоков (вью само маршалит в UI-поток).
/// </summary>
public interface IDashboardView
{
    /// <summary>Путь к проекту, для которого показывается дашборд (null — проекта нет).</summary>
    string? ProjectPath { get; }

    /// <summary>Включено ли автообновление (флажок на форме).</summary>
    bool AutoRefresh { get; }

    /// <summary>Отображает текст статуса сервера (например «Сервер запущен»).</summary>
    void SetServerStatus(string text);

    /// <summary>Отображает URL сервера; пустая строка скрывает URL.</summary>
    void SetServerUrl(string url);

    /// <summary>Отображает список сессий.</summary>
    void ShowSessions(IReadOnlyList<OpenCodeSession> sessions);

    /// <summary>Отображает статистику; null — данные недоступны.</summary>
    void ShowStats(UsageStats? stats);

    /// <summary>Устанавливает текст строки состояния.</summary>
    void SetStatusText(string text);

    /// <summary>Показывает ошибку (в строке состояния).</summary>
    void ShowError(string message);
}