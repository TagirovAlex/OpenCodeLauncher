using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using OpenCodeLauncher.Core.Services;
using OpenCodeLauncher.Core.Settings;
using Serilog;

namespace OpenCodeLauncher;

/// <summary>
/// Точка входа приложения: инициализация, глобальные обработчики ошибок, проверка opencode, запуск UI.
/// </summary>
static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        // Логи — в папке logs рядом с exe (правило проекта).
        var baseDirectory = AppContext.BaseDirectory;
        var logDirectory = Path.Combine(baseDirectory, "logs");
        Directory.CreateDirectory(logDirectory);

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(
                Path.Combine(logDirectory, "app-.log"),
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7)
            .CreateLogger();

        // Глобальные обработчики ошибок UI-потока (устанавливаются до Application.Run).
        Application.ThreadException += (s, e) => HandleFatal("Необработанная ошибка UI", e.Exception);
        AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            Log.Fatal(e.ExceptionObject as Exception, "Необработанное исключение");
        TaskScheduler.UnobservedTaskException += (s, e) => OnUnobservedTaskException(e);

        // Проверка наличия opencode при старте (приложение продолжает запуск).
        var openCodePath = ProcessService.TryLocateOpenCode();
        if (openCodePath is null)
        {
            Log.Warning("opencode не найден. Установите его через npm: npm i -g opencode-ai");
            MessageBox.Show(
                "opencode не найден. Установите его через npm: npm i -g opencode-ai",
                "OpenCodeLauncher",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        try
        {
            var settingsPath = Path.Combine(baseDirectory, "settings.json");
            BackupSettings(settingsPath);

            var settingsStore = new JsonSettingsStore<AppSettings>(settingsPath);
            Log.Information("Приложение запущено, версия " + Assembly.GetExecutingAssembly().GetName().Version);
            Application.Run(new MainForm(settingsStore, new WindowsStartupService()));
        }
        catch (Exception ex)
        {
            HandleFatal("Критическая ошибка приложения", ex);
        }
        finally
        {
            Log.Information("Приложение завершено");
            Log.CloseAndFlush();
        }
    }

    /// <summary>
    /// Делает резервную копию настроек (settings.json.bak) перед запуском, чтобы
    /// настройки не терялись при обновлении/замене приложения.
    /// </summary>
    private static void BackupSettings(string settingsPath)
    {
        try
        {
            if (File.Exists(settingsPath))
            {
                File.Copy(settingsPath, settingsPath + ".bak", overwrite: true);
            }
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "Не удалось создать резервную копию настроек");
        }
    }

    /// <summary>
    /// Логирует фатальную ошибку и показывает её пользователю в MessageBox.
    /// Вызывается из обработчика исключений Main и глобального обработчика ThreadException.
    /// </summary>
    private static void HandleFatal(string context, Exception ex)
    {
        Log.Fatal(ex, context);
        MessageBox.Show(
            context + ": " + ex.Message,
            "OpenCodeLauncher",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
    }

    /// <summary>
    /// Логирует необработанное исключение Task и помечает его как обработанное,
    /// чтобы процесс не завершился по умолчанию.
    /// </summary>
    private static void OnUnobservedTaskException(UnobservedTaskExceptionEventArgs e)
    {
        Log.Error(e.Exception, "Необработанное исключение Task");
        e.SetObserved();
    }
}