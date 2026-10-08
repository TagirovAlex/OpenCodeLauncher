using OpenCodeLauncher.Core.Models;
using OpenCodeLauncher.Core.Services;
using OpenCodeLauncher.Core.Settings;
using OpenCodeLauncher.Views;

namespace OpenCodeLauncher.Presenters;

/// <summary>
/// Презентер вкладки «Дашборд»: отображает статус сервера, список сессий и статистику opencode.
/// Вся логика здесь; вью остаётся «глупым».
/// </summary>
public sealed class DashboardPresenter
{
    /// <summary>Интервал автообновления.</summary>
    private static readonly TimeSpan AutoRefreshInterval = TimeSpan.FromSeconds(10);

    private readonly ISettingsStore<AppSettings> _settingsStore;
    private readonly IProcessService _processService;
    private readonly IOpenCodeInfoService _infoService;
    private readonly System.Windows.Forms.Timer _autoRefreshTimer;

    private AppSettings _settings;
    private IDashboardView? _view;
    private CancellationTokenSource? _refreshCts;
    private bool _isRefreshing;

    public DashboardPresenter(
        AppSettings settings,
        ISettingsStore<AppSettings> settingsStore,
        IProcessService processService,
        IOpenCodeInfoService infoService)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _settingsStore = settingsStore ?? throw new ArgumentNullException(nameof(settingsStore));
        _processService = processService ?? throw new ArgumentNullException(nameof(processService));
        _infoService = infoService ?? throw new ArgumentNullException(nameof(infoService));

        _processService.StatusChanged += OnServerStatusChanged;

        _autoRefreshTimer = new System.Windows.Forms.Timer { Interval = (int)AutoRefreshInterval.TotalMilliseconds };
        _autoRefreshTimer.Tick += (_, _) => OnAutoRefreshTick();
    }

    /// <summary>Путь к проекту: из текущего запуска либо из последнего проекта в настройках.</summary>
    public string? ProjectPath
    {
        get
        {
            var path = _processService.CurrentLaunch?.ProjectPath;
            if (string.IsNullOrWhiteSpace(path))
            {
                path = _settings.LastProjectPath;
            }

            return string.IsNullOrWhiteSpace(path) ? null : path;
        }
    }

    /// <summary>Подключает вью (вызывается из конструктора <see cref="DashboardView"/>).</summary>
    public void AttachView(IDashboardView view)
    {
        _view = view ?? throw new ArgumentNullException(nameof(view));
        UpdateServerStatus();
    }

    /// <summary>Вызывается при загрузке вью: показывает статус и запускает автообновление.</summary>
    public void OnLoaded()
    {
        if (_view is null)
        {
            return;
        }

        UpdateServerStatus();

        if (_view.AutoRefresh)
        {
            StartAutoRefresh();
        }
    }

    /// <summary>Вызывается при изменении флажка автообновления.</summary>
    public void OnAutoRefreshChanged()
    {
        if (_view is null)
        {
            return;
        }

        if (_view.AutoRefresh)
        {
            StartAutoRefresh();
        }
        else
        {
            StopAutoRefresh();
        }
    }

    /// <summary>Обработчик кнопки «Обновить».</summary>
    public async void OnRefreshClicked()
    {
        try
        {
            await RefreshAsync().ConfigureAwait(true);
        }
        catch (Exception ex)
        {
            _view?.ShowError(ex.Message);
        }
    }

    /// <summary>
    /// Обновляет сессии и статистику. Если сервер не запущен или проекта нет —
    /// показывает «Сервер не запущен» и данные не запрашивает.
    /// </summary>
    private async Task RefreshAsync()
    {
        if (_view is null || _isRefreshing)
        {
            return;
        }

        // Настройки могли измениться на других вкладках — перечитываем актуальный проект.
        _settings = _settingsStore.Load();

        var projectPath = ProjectPath;
        if (!_processService.IsRunning || string.IsNullOrWhiteSpace(projectPath) || !Directory.Exists(projectPath))
        {
            _view.SetServerStatus("Сервер не запущен");
            _view.SetServerUrl(string.Empty);
            _view.ShowSessions(Array.Empty<OpenCodeSession>());
            _view.ShowStats(null);
            return;
        }

        _isRefreshing = true;
        _view.SetStatusText("Обновление…");

        try
        {
            _refreshCts?.Cancel();
            _refreshCts?.Dispose();
            _refreshCts = new CancellationTokenSource();
            var token = _refreshCts.Token;

            var sessionsTask = _infoService.GetSessionsAsync(projectPath, token);
            var statsTask = _infoService.GetStatsAsync(projectPath, token);

            await Task.WhenAll(sessionsTask, statsTask).ConfigureAwait(true);

            _view.ShowSessions(sessionsTask.Result);
            _view.ShowStats(statsTask.Result);
            _view.SetStatusText($"Обновлено: {DateTime.Now:HH:mm:ss}");
        }
        finally
        {
            _isRefreshing = false;
        }
    }

    private void OnAutoRefreshTick()
    {
        if (_view is { AutoRefresh: true } && !_isRefreshing && _processService.IsRunning)
        {
            OnRefreshClicked();
        }
    }

    /// <summary>Обработчик смены статуса сервера (вызывается из любого потока).</summary>
    private void OnServerStatusChanged(object? sender, ServerStatusChangedEventArgs e)
    {
        if (_view is null)
        {
            return;
        }

        _view.SetServerStatus(DescribeStatus(e.Status));
        _view.SetServerUrl(BuildUrl());

        if (e.Status == ServerStatus.Stopped || e.Status == ServerStatus.Crashed)
        {
            _view.ShowSessions(Array.Empty<OpenCodeSession>());
            _view.ShowStats(null);
        }
    }

    private void UpdateServerStatus()
    {
        if (_view is null)
        {
            return;
        }

        _view.SetServerStatus(DescribeStatus(_processService.Status));
        _view.SetServerUrl(BuildUrl());
    }

    private void StartAutoRefresh() => _autoRefreshTimer.Start();

    private void StopAutoRefresh() => _autoRefreshTimer.Stop();

    /// <summary>URL запущенного сервера вида http://host:port/; пустая строка, если запуска нет.</summary>
    private string BuildUrl()
    {
        var launch = _processService.CurrentLaunch;
        return launch is null ? string.Empty : $"http://{launch.Hostname}:{launch.Port}/";
    }

    private static string DescribeStatus(ServerStatus status) => status switch
    {
        ServerStatus.Running => "Сервер запущен",
        ServerStatus.Starting => "Запускается…",
        ServerStatus.Stopping => "Останавливается…",
        ServerStatus.Crashed => "Сервер упал",
        _ => "Сервер не запущен"
    };
}