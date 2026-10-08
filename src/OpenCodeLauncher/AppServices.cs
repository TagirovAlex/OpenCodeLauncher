using OpenCodeLauncher.Core.Services;
using OpenCodeLauncher.Core.Settings;

namespace OpenCodeLauncher;

/// <summary>
/// Композиционный корень: общие экземпляры сервисов приложения.
/// Настройки и логи — в папке рядом с exe (правило проекта).
/// </summary>
public static class AppServices
{
    private static readonly string BaseDirectory = AppContext.BaseDirectory;

    public static ISettingsStore<AppSettings> Settings { get; } =
        new JsonSettingsStore<AppSettings>(Path.Combine(BaseDirectory, "settings.json"));

    public static INetworkService Network { get; } = new NetworkService();

    public static IProcessService Process { get; } = new ProcessService();

    public static IHealthService Health { get; } = new HealthService();

    public static IOpenCodeInfoService Info { get; } = new OpenCodeInfoService();

    public static IServerLauncher Launcher { get; } = new ServerLauncher(Process);
}