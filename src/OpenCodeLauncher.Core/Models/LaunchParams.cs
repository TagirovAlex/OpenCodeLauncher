namespace OpenCodeLauncher.Core.Models;

/// <summary>Параметры запуска opencode-сервера.</summary>
public sealed record LaunchParams(
    string ProjectPath,
    string Hostname,
    int Port,
    string? Model = null,
    bool ContinueLastSession = false,
    string? SessionId = null,
    string? ExtraArgs = null,
    string? ServerUsername = null,
    string? ServerPassword = null);