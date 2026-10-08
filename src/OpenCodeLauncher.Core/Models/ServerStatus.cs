namespace OpenCodeLauncher.Core.Models;

/// <summary>Состояние запущенного opencode-процесса.</summary>
public enum ServerStatus
{
    Stopped,
    Starting,
    Running,
    Stopping,
    Crashed
}