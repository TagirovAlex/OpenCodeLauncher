using OpenCodeLauncher.Core.Models;
using OpenCodeLauncher.Core.Settings;

namespace OpenCodeLauncher.Views;

/// <summary>Представление вкладки «Профили»: контракт между вью и презентером.</summary>
/// <remarks>
/// Все методы интерфейса должны быть безопасны для вызова из любого потока:
/// вью самостоятельно маршалит вызов в UI-поток при необходимости.
/// </remarks>
public interface IProfilesView
{
    /// <summary>Имя профиля из поля ввода (null — пусто).</summary>
    string? ProfileName { get; }

    /// <summary>Путь к проекту из поля ввода (null — пусто).</summary>
    string? ProjectPath { get; }

    /// <summary>Выбранный адрес сетевого интерфейса (null — не выбран).</summary>
    string? SelectedHostname { get; }

    /// <summary>Выбранный порт (1..65535).</summary>
    int Port { get; }

    /// <summary>Модель из поля ввода (null — не задана).</summary>
    string? Model { get; }

    /// <summary>Продолжать ли последнюю сессию.</summary>
    bool ContinueLastSession { get; }

    /// <summary>Идентификатор сессии из поля ввода (null — не задан).</summary>
    string? SessionId { get; }

    /// <summary>Доп. аргументы из поля ввода (null — не заданы).</summary>
    string? ExtraArgs { get; }

    /// <summary>Логин базовой авторизации сервера (null — пусто).</summary>
    string? ServerUsername { get; }

    /// <summary>Пароль базовой авторизации сервера (null — пусто).</summary>
    string? ServerPassword { get; }

    /// <summary>Имя выбранного в списке профиля (null — нет выбора).</summary>
    string? SelectedProfileName { get; }

    /// <summary>Заполняет список профилей.</summary>
    void PopulateProfiles(IReadOnlyList<LaunchProfile> profiles);

    /// <summary>Заполняет список хостов и выбирает адрес по умолчанию.</summary>
    void PopulateInterfaces(IReadOnlyList<NetworkInterfaceInfo> interfaces, string defaultHostname);

    /// <summary>Заполняет поля редактирования из профиля.</summary>
    void SetFields(LaunchProfile profile);

    /// <summary>Очищает поля редактирования.</summary>
    void ClearFields();

    /// <summary>Выделяет профиль с заданным именем в списке.</summary>
    void SelectProfile(string name);

    /// <summary>Выводит читаемый статус сервера.</summary>
    void SetStatusText(string text);

    /// <summary>Выводит статус последнего действия (добавление/запуск и т.п.).</summary>
    void SetActionText(string text);

    /// <summary>Разрешает или запрещает кнопку «Запустить».</summary>
    void SetLaunchEnabled(bool enabled);

    /// <summary>Разрешает или запрещает кнопку «Остановить».</summary>
    void SetStopEnabled(bool enabled);

    /// <summary>Показывает сообщение об ошибке.</summary>
    void ShowError(string message);
}