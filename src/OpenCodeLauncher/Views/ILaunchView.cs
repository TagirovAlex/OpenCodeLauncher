using OpenCodeLauncher.Core.Models;
using OpenCodeLauncher.Core.Services;
using OpenCodeLauncher.Core.Settings;

namespace OpenCodeLauncher.Views;

/// <summary>Представление вкладки «Запуск»: контракт между вью и презентером.</summary>
/// <remarks>
/// Все методы интерфейса должны быть безопасны для вызова из любого потока:
/// вью самостоятельно маршалит вызов в UI-поток при необходимости.
/// </remarks>
public interface ILaunchView
{
    /// <summary>Путь к выбранному проекту (null — не задан).</summary>
    string? ProjectPath { get; }

    /// <summary>Выбранный адрес сетевого интерфейса (null — не выбран).</summary>
    string? SelectedHostname { get; }

    /// <summary>Выбранный порт (1..65535).</summary>
    int Port { get; }

    /// <summary>Путь выбранного пункта избранного (null — нет выбора).</summary>
    string? SelectedFavorite { get; }

    /// <summary>Логин базовой авторизации сервера (null — пусто).</summary>
    string? ServerUsername { get; }

    /// <summary>Пароль базовой авторизации сервера (null — пусто).</summary>
    string? ServerPassword { get; }

    /// <summary>Задаёт логин/пароль для базовой авторизации сервера.</summary>
    void SetServerCredentials(string username, string password);

    /// <summary>Задаёт путь к проекту.</summary>
    void SetProjectPath(string projectPath);

    /// <summary>Выбирает адрес интерфейса; если адреса нет в списке — добавляет его.</summary>
    void SetHostname(string hostname);

    /// <summary>Задаёт порт.</summary>
    void SetPort(int port);

    /// <summary>Выводит читаемый статус сервера.</summary>
    void SetStatusText(string text);

    /// <summary>Добавляет строку вывода в лог (stream: "stdout" или "stderr").</summary>
    void AppendLog(string stream, string line);

    /// <summary>Разрешает или запрещает кнопку «Запустить сервер».</summary>
    void SetStartEnabled(bool enabled);

    /// <summary>Разрешает или запрещает кнопку «Остановить».</summary>
    void SetStopEnabled(bool enabled);

    /// <summary>Заполняет список сетевых интерфейсов и выбирает адрес по умолчанию.</summary>
    void PopulateInterfaces(IReadOnlyList<NetworkInterfaceInfo> interfaces, string defaultHostname);

    /// <summary>Заполняет список избранных проектов.</summary>
    void PopulateFavorites(IReadOnlyList<string> favorites);

    /// <summary>Показывает сообщение об ошибке.</summary>
    void ShowError(string message);
}