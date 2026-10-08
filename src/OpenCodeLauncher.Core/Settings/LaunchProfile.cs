namespace OpenCodeLauncher.Core.Settings;

/// <summary>Профиль запуска: проект + сеть + дополнительные флаги.</summary>
public sealed class LaunchProfile
{
    public string Name { get; set; } = string.Empty;

    public string ProjectPath { get; set; } = string.Empty;

    public string Hostname { get; set; } = string.Empty;

    public int Port { get; set; } = 4000;

    public string? Model { get; set; }

    public bool ContinueLastSession { get; set; }

    public string? SessionId { get; set; }

    public string? ExtraArgs { get; set; }

    /// <summary>Логин для базовой авторизации сервера (null — "opencode").</summary>
    public string? ServerUsername { get; set; }

    /// <summary>Пароль для базовой авторизации сервера (null/пусто — без пароля).</summary>
    public string? ServerPassword { get; set; }
}