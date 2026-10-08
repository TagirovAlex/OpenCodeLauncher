using OpenCodeLauncher.Core.Models;
using OpenCodeLauncher.Core.Services;
using OpenCodeLauncher.Core.Settings;
using OpenCodeLauncher.Views;

namespace OpenCodeLauncher.Presenters;

/// <summary>Презентер вкладки «Профили»: CRUD профилей и запуск выбранного профиля.</summary>
/// <remarks>
/// Презентер не маршалит вызовы в UI-поток — это делает вью в реализациях
/// интерфейса <see cref="IProfilesView"/>.
/// </remarks>
public sealed class ProfilesPresenter
{
    private readonly IProfileService _profileService;
    private readonly INetworkService _networkService;
    private readonly IProcessService _processService;
    private readonly IServerLauncher _serverLauncher;

    private IProfilesView? _view;

    /// <summary>Создаёт презентер вкладки «Профили».</summary>
    public ProfilesPresenter(
        IProfileService profileService,
        INetworkService networkService,
        IProcessService processService,
        IServerLauncher serverLauncher)
    {
        _profileService = profileService ?? throw new ArgumentNullException(nameof(profileService));
        _networkService = networkService ?? throw new ArgumentNullException(nameof(networkService));
        _processService = processService ?? throw new ArgumentNullException(nameof(processService));
        _serverLauncher = serverLauncher ?? throw new ArgumentNullException(nameof(serverLauncher));
    }

    /// <summary>Привязывает вью к презентеру.</summary>
    public void Attach(IProfilesView view)
    {
        _view = view ?? throw new ArgumentNullException(nameof(view));
    }

    /// <summary>Вызывается после загрузки вкладки: заполняет UI и подписывается на события процесса.</summary>
    public void OnLoaded()
    {
        var view = _view;
        if (view is null)
        {
            return;
        }

        view.PopulateProfiles(_profileService.GetProfiles());
        view.PopulateInterfaces(_networkService.GetInterfaces(), _networkService.GetDefaultHostname());

        _processService.StatusChanged += (_, e) => OnStatusChanged(e.Status);

        OnStatusChanged(_processService.Status);
    }

    /// <summary>Обработчик выбора профиля в списке: заполняет поля редактирования.</summary>
    public void OnProfileSelected()
    {
        var view = _view;
        if (view is null)
        {
            return;
        }

        var name = view.SelectedProfileName;
        if (string.IsNullOrEmpty(name))
        {
            return;
        }

        var profile = _profileService.Find(name);
        if (profile is not null)
        {
            view.SetFields(profile);
        }
    }

    /// <summary>Обработчик кнопки «Добавить».</summary>
    public void OnAddClicked()
    {
        var view = _view;
        if (view is null)
        {
            return;
        }

        try
        {
            var profile = BuildProfileFromFields();
            _profileService.Add(profile);
            RefreshProfiles();
            view.SelectProfile(profile.Name);
            view.SetActionText($"Профиль «{profile.Name}» добавлен.");
        }
        catch (Exception ex)
        {
            view.ShowError(ex.Message);
        }
    }

    /// <summary>Обработчик кнопки «Сохранить изменения».</summary>
    public void OnSaveClicked()
    {
        var view = _view;
        if (view is null)
        {
            return;
        }

        if (string.IsNullOrEmpty(view.SelectedProfileName))
        {
            view.ShowError("Сначала выберите профиль в списке.");
            return;
        }

        try
        {
            var profile = BuildProfileFromFields();
            _profileService.Update(profile);
            RefreshProfiles();
            view.SelectProfile(profile.Name);
            view.SetActionText($"Профиль «{profile.Name}» сохранён.");
        }
        catch (Exception ex)
        {
            view.ShowError(ex.Message);
        }
    }

    /// <summary>Обработчик кнопки «Удалить».</summary>
    public void OnDeleteClicked()
    {
        var view = _view;
        if (view is null)
        {
            return;
        }

        var name = view.SelectedProfileName;
        if (string.IsNullOrEmpty(name))
        {
            view.ShowError("Сначала выберите профиль в списке.");
            return;
        }

        try
        {
            _profileService.Remove(name);
            RefreshProfiles();
            view.ClearFields();
            view.SetActionText($"Профиль «{name}» удалён.");
        }
        catch (Exception ex)
        {
            view.ShowError(ex.Message);
        }
    }

    /// <summary>Обработчик кнопки «Запустить»: запускает выбранный профиль и ждёт готовности.</summary>
    public async void OnLaunchClicked()
    {
        var view = _view;
        if (view is null)
        {
            return;
        }

        LaunchParams parameters;
        LaunchProfile? profile;
        try
        {
            profile = BuildProfileFromFields();
            parameters = _profileService.ToLaunchParams(profile);
        }
        catch (Exception ex)
        {
            view.ShowError(ex.Message);
            return;
        }

        view.SetActionText("Запуск сервера…");
        try
        {
            var started = await _serverLauncher.StartAndWaitReadyAsync(parameters);
            view.SetActionText(started ? "Сервер запущен." : "Сервер не стал доступен.");

            if (started && !string.IsNullOrEmpty(profile.ServerPassword))
            {
                view.SetActionText(
                    $"Сервер запущен и защищён паролем (логин: {profile.ServerUsername ?? "opencode"}). Введите их в браузере.");
            }
        }
        catch (Exception ex)
        {
            view.ShowError("Не удалось запустить сервер: " + ex.Message);
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
            view.SetActionText("Сервер остановлен.");
        }
        catch (Exception ex)
        {
            view.ShowError("Не удалось остановить сервер: " + ex.Message);
        }
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
                view.SetLaunchEnabled(true);
                view.SetStopEnabled(false);
                break;

            case ServerStatus.Starting:
            case ServerStatus.Running:
                view.SetLaunchEnabled(false);
                view.SetStopEnabled(true);
                break;

            case ServerStatus.Stopping:
                view.SetLaunchEnabled(false);
                view.SetStopEnabled(false);
                break;
        }
    }

    /// <summary>Перечитывает список профилей во вью.</summary>
    private void RefreshProfiles()
    {
        _view?.PopulateProfiles(_profileService.GetProfiles());
    }

    /// <summary>Собирает профиль из полей редактирования вью.</summary>
    private LaunchProfile BuildProfileFromFields()
    {
        var view = _view ?? throw new InvalidOperationException("Вью не привязано.");

        return new LaunchProfile
        {
            Name = view.ProfileName?.Trim() ?? string.Empty,
            ProjectPath = view.ProjectPath?.Trim() ?? string.Empty,
            Hostname = view.SelectedHostname?.Trim() ?? string.Empty,
            Port = view.Port,
            Model = view.Model,
            ContinueLastSession = view.ContinueLastSession,
            SessionId = view.SessionId,
            ExtraArgs = view.ExtraArgs,
            ServerUsername = view.ServerUsername,
            ServerPassword = view.ServerPassword,
        };
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