namespace OpenCodeLauncher.Core.Models;

/// <summary>Сессия opencode внутри проекта.</summary>
public sealed record OpenCodeSession(
    string Id,
    string Title,
    DateTime? CreatedAt = null);