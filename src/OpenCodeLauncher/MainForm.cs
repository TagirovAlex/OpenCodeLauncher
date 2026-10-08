using OpenCodeLauncher.Core.Models;
using OpenCodeLauncher.Core.Services;
using OpenCodeLauncher.Core.Settings;
using OpenCodeLauncher.Presenters;
using OpenCodeLauncher.Views;

namespace OpenCodeLauncher;

public partial class MainForm : Form
{
    private readonly ISettingsStore<AppSettings>? _settingsStore;
    private readonly AppSettings? _settings;
    private readonly IWindowsStartupService? _startupService;

    /// <summary>Разрешает реальное закрытие окна (иначе сворачивание в трей).</summary>
    private bool _allowClose;

    public MainForm()
    {
        InitializeComponent();
    }

    public MainForm(ISettingsStore<AppSettings> settingsStore, IWindowsStartupService? startupService = null) : this()
    {
        _settingsStore = settingsStore ?? throw new ArgumentNullException(nameof(settingsStore));
        _settings = _settingsStore.Load();
        Text = "OpenCodeLauncher";
        InitializeViews();
        SetStatus("Готов к работе");

        _startupService = startupService;
        notifyIcon.DoubleClick += (_, _) => ShowWindow();
        trayAutostartToolStripMenuItem.Checked = _startupService?.IsEnabled() ?? false;
    }

    /// <summary>Создаёт презентеры и вью, размещает вкладки в TabControl.</summary>
    private void InitializeViews()
    {
        // Вкладка «Запуск».
        var launchPresenter = new LaunchPresenter(
            _settings!, _settingsStore!, AppServices.Network, AppServices.Process, AppServices.Launcher);
        tabPageLaunch.Controls.Add(new LaunchView(launchPresenter) { Dock = DockStyle.Fill });

        // Вкладка «Дашборд».
        var dashboardPresenter = new DashboardPresenter(
            _settings!, _settingsStore!, AppServices.Process, AppServices.Info);
        tabPageDashboard.Controls.Add(new DashboardView(dashboardPresenter) { Dock = DockStyle.Fill });

        // Вкладка «Профили».
        var profileService = new ProfileService(_settings!, _settingsStore!);
        var profilesPresenter = new ProfilesPresenter(
            profileService, AppServices.Network, AppServices.Process, AppServices.Launcher);
        tabPageProfiles.Controls.Add(new ProfilesView(profilesPresenter) { Dock = DockStyle.Fill });
    }

    private void SetStatus(string message) => statusLabel.Text = message;

    /// <summary>Показывает и активирует главное окно.</summary>
    private void ShowWindow()
    {
        this.Show();
        this.WindowState = FormWindowState.Normal;
        this.Activate();
    }

    private void trayOpen_Click(object? sender, EventArgs e) => ShowWindow();

    private async void trayStart_Click(object? sender, EventArgs e)
    {
        if (AppServices.Process.IsRunning)
        {
            return;
        }

        if (_settings is null || string.IsNullOrWhiteSpace(_settings.LastProjectPath))
        {
            notifyIcon.ShowBalloonTip(2000, "OpenCodeLauncher", "Сначала настройте запуск во вкладке «Запуск»", ToolTipIcon.Warning);
            return;
        }

        var hostname = string.IsNullOrWhiteSpace(_settings.LastHostname) ? "127.0.0.1" : _settings.LastHostname.Trim();
        var parameters = new LaunchParams(_settings.LastProjectPath, hostname, _settings.LastPort);

        try
        {
            await AppServices.Launcher.StartAndWaitReadyAsync(parameters);
        }
        catch (Exception ex)
        {
            notifyIcon.ShowBalloonTip(3000, "OpenCodeLauncher", $"Не удалось запустить сервер: {ex.Message}", ToolTipIcon.Error);
        }
    }

    private async void trayStop_Click(object? sender, EventArgs e)
    {
        try
        {
            await AppServices.Process.StopAsync();
        }
        catch (Exception ex)
        {
            notifyIcon.ShowBalloonTip(3000, "OpenCodeLauncher", $"Не удалось остановить сервер: {ex.Message}", ToolTipIcon.Error);
        }
    }

    private void trayAutostart_Click(object? sender, EventArgs e)
    {
        if (_startupService is null)
        {
            return;
        }

        try
        {
            if (trayAutostartToolStripMenuItem.Checked)
            {
                _startupService.Enable();
            }
            else
            {
                _startupService.Disable();
            }

            if (_settings is not null && _settingsStore is not null)
            {
                _settings.StartWithWindows = trayAutostartToolStripMenuItem.Checked;
                _settingsStore.Save(_settings);
            }
        }
        catch (Exception ex)
        {
            notifyIcon.ShowBalloonTip(3000, "OpenCodeLauncher", $"Не удалось изменить автозапуск: {ex.Message}", ToolTipIcon.Error);
            trayAutostartToolStripMenuItem.Checked = _startupService.IsEnabled();
        }
    }

    private void trayExit_Click(object? sender, EventArgs e)
    {
        _allowClose = true;
        this.Close();
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        // Закрытие крестиком сворачивает приложение в трей; реальный выход — только через пункт меню трея.
        if (!_allowClose && e.CloseReason == CloseReason.UserClosing)
        {
            e.Cancel = true;
            this.Hide();
            notifyIcon.ShowBalloonTip(2000, "OpenCodeLauncher", "Приложение свёрнуто в трей", ToolTipIcon.Info);
            return;
        }

        // Настройки сохраняются при закрытии приложения.
        if (_settingsStore is not null && _settings is not null)
        {
            _settingsStore.Save(_settings);
        }

        // Если сервер ещё работает — останавливаем (с принудительным завершением дерева).
        if (AppServices.Process.IsRunning)
        {
            AppServices.Process.Dispose();
        }
    }
}