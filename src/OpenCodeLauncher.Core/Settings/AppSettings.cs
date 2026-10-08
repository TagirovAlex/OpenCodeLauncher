namespace OpenCodeLauncher.Core.Settings;

/// <summary>Настройки приложения. Сохраняются в папке рядом с exe (settings.json).</summary>
public sealed class AppSettings
{
    public string LastProjectPath { get; set; } = string.Empty;

    public string LastHostname { get; set; } = string.Empty;

    public int LastPort { get; set; } = 4000;

    public bool StartWithWindows { get; set; }

    public bool AutoStartServer { get; set; }

    /// <summary>Логин для базовой авторизации сервера (по умолчанию "opencode").</summary>
    public string ServerUsername { get; set; } = "opencode";

    /// <summary>Пароль для базовой авторизации сервера. Пустой — сервер без пароля.</summary>
    public string ServerPassword { get; set; } = string.Empty;

    public List<string> FavoriteProjects { get; set; } = new();

    public List<LaunchProfile> Profiles { get; set; } = new();
}