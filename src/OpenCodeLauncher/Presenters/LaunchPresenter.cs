using System.Diagnostics;
using OpenCodeLauncher.Core.Models;
using OpenCodeLauncher.Core.Services;
using OpenCodeLauncher.Core.Settings;
using OpenCodeLauncher.Views;

namespace OpenCodeLauncher.Presenters;

/// <summary>Презентер вкладки «Запуск»: координирует вью и сервисы запуска opencode-сервера.</summary>
/// <remarks>
/// Презентер не маршалит вызовы в UI-поток — это делает вью в реализациях
/// интерфейса <see cref="ILaunchView"/>.
/// </remarks>
public sealed class LaunchPresenter
{
    private readonly AppSettings _settings;
    private readonly ISettingsStore<AppSettings> _settingsStore;
    private readonly INetworkService _networkService;
    private readonly IProcessService _processService;
    private readonly IServerLauncher _launcher;

    private ILaunchView? _view;

    /// <summary>Создаёт презентер вкладки «Запуск».</summary>
    public LaunchPresenter(
        AppSettings settings,
        ISettingsStore<AppSettings> settingsStore,
        INetworkService networkService,
        IProcessService processService,
        IServerLauncher launcher)
    {
        _settings = settings;
        _settingsStore = settingsStore;
        _networkService = networkService;
        _processService = processService;
        _launcher = launcher;
    }

    /// <summary>Привязывает вью к презентеру.</summary>
    public void Attach(ILaunchView view)
    {
        _view = view;
    }

    /// <summary>Вызывается после загрузки вкладки: заполняет UI и подписывается на события процесса.</summary>
    public void OnLoaded()
    {
        var view = _view;
        if (view is null)
        {
            return;
        }

        view.SetProjectPath(_settings.LastProjectPath);
        view.SetPort(_settings.LastPort);
        view.PopulateFavorites(_settings.FavoriteProjects);
        view.PopulateInterfaces(_networkService.GetInterfaces(), _networkService.GetDefaultHostname());
        view.SetHostname(_settings.LastHostname);
        view.SetServerCredentials(_settings.ServerUsername, _settings.ServerPassword);

        _processService.OutputReceived += (_, e) => view.AppendLog(e.Stream, e.Text);
        _processService.StatusChanged += (_, e) => OnStatusChanged(e.Status);

        OnStatusChanged(_processService.Status);
    }

    /// <summary>Обработчик кнопки «Запустить сервер».</summary>
    public async void OnStartClicked()
    {
        var view = _view;
        if (view is null)
        {
            return;
        }

        var projectPath = view.ProjectPath?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(projectPath))
        {
            view.ShowError("Укажите путь к проекту.");
            return;
        }

        if (!Directory.Exists(projectPath))
        {
            view.ShowError("Каталог проекта не найден: " + projectPath);
            return;
        }

        var hostname = view.SelectedHostname?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(hostname))
        {
            view.ShowError("Не выбран адрес сетевого интерфейса.");
            return;
        }

        var parameters = new LaunchParams(
            projectPath,
            hostname,
            view.Port,
            ServerUsername: view.ServerUsername,
            ServerPassword: view.ServerPassword);

        bool ready;
        try
        {
            ready = await _launcher.StartAndWaitReadyAsync(parameters);
        }
        catch (Exception ex)
        {
            view.ShowError("Не удалось запустить сервер: " + ex.Message);
            return;
        }

        if (!ready)
        {
            view.ShowError("Сервер не стал доступен в течение 15 секунд.");
            return;
        }

        _settings.LastProjectPath = projectPath;
        _settings.LastHostname = hostname;
        _settings.LastPort = parameters.Port;
        _settings.ServerUsername = view.ServerUsername ?? "opencode";
        _settings.ServerPassword = view.ServerPassword ?? string.Empty;
        SaveSettings();

        if (!string.IsNullOrEmpty(_settings.ServerPassword))
        {
            view.AppendLog("info", $"Сервер защищён паролем. Логин: {_settings.ServerUsername}; введите пароль в браузере на телефоне.");
        }
    }

    /// <summary>Обработчик кнопки «Остановить».</summary>
    public async void OnStopClicked()
    {
        var view = _view;
        if (view is null)
        {
            return;
        }

        try
        {
            await _processService.StopAsync();
        }
        catch (Exception ex)
        {
            view.ShowError("Не удалось остановить сервер: " + ex.Message);
        }
    }

    /// <summary>Обработчик кнопки «Открыть в браузере».</summary>
    public void OnOpenInBrowserClicked()
    {
        var view = _view;
        if (view is null)
        {
            return;
        }

        if (_processService.Status != ServerStatus.Running)
        {
            view.ShowError("Сервер не запущен.");
            return;
        }

        var launch = _processService.CurrentLaunch;
        if (launch is null)
        {
            view.ShowError("Сервер не запущен.");
            return;
        }

        Process.Start(new ProcessStartInfo
        {
            FileName = $"http://{launch.Hostname}:{launch.Port}/",
            UseShellExecute = true
        });
    }

    /// <summary>Обработчик кнопки «В избранное».</summary>
    public void OnAddFavoriteClicked()
    {
        var view = _view;
        if (view is null)
        {
            return;
        }

        var path = view.ProjectPath?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(path))
        {
            view.ShowError("Сначала укажите путь к проекту.");
            return;
        }

        if (!Directory.Exists(path))
        {
            view.ShowError("Каталог проекта не найден: " + path);
            return;
        }

        if (_settings.FavoriteProjects.Contains(path))
        {
            return;
        }

        _settings.FavoriteProjects.Add(path);
        SaveSettings();
        view.PopulateFavorites(_settings.FavoriteProjects);
    }

    /// <summary>Обработчик кнопки «Убрать из избранного».</summary>
    public void OnRemoveFavoriteClicked()
    {
        var view = _view;
        if (view is null)
        {
            return;
        }

        var selected = view.SelectedFavorite?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(selected))
        {
            return;
        }

        _settings.FavoriteProjects.Remove(selected);
        SaveSettings();
        view.PopulateFavorites(_settings.FavoriteProjects);
    }

    /// <summary>Обновляет статус и доступность кнопок при изменении состояния процесса.</summary>
    private void OnStatusChanged(ServerStatus status)
    {
        var view = _view;
        if (view is null)
        {
            return;
        }

        view.SetStatusText(StatusText(status));

        switch (status)
        {
            case ServerStatus.Stopped:
            case ServerStatus.Crashed:
                view.SetStartEnabled(true);
                view.SetStopEnabled(false);
                break;

            case ServerStatus.Starting:
            case ServerStatus.Running:
                view.SetStartEnabled(false);
                view.SetStopEnabled(true);
                break;

            case ServerStatus.Stopping:
                view.SetStartEnabled(false);
                view.SetStopEnabled(false);
                break;
        }
    }

    /// <summary>Сохраняет настройки; при ошибке показывает сообщение.</summary>
    private void SaveSettings()
    {
        try
        {
            _settingsStore.Save(_settings);
        }
        catch (Exception ex)
        {
            _view?.ShowError("Не удалось сохранить настройки: " + ex.Message);
        }
    }

    /// <summary>Возвращает читаемый текст статуса сервера.</summary>
    private static string StatusText(ServerStatus status)
    {
        switch (status)
        {
            case ServerStatus.Stopped: return "Остановлен";
            case ServerStatus.Starting: return "Запуск…";
            case ServerStatus.Running: return "Работает";
            case ServerStatus.Stopping: return "Остановка…";
            case ServerStatus.Crashed: return "Упал";
            default: return "Неизвестно";
        }
    }
}