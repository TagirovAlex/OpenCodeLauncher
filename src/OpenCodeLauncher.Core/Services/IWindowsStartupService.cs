namespace OpenCodeLauncher.Core.Services;

/// <summary>Управление автозапуском приложения при входе в Windows.</summary>
public interface IWindowsStartupService
{
    /// <summary>Возвращает <c>true</c>, если автозапуск включён для текущего exe-файла.</summary>
    bool IsEnabled();

    /// <summary>Включает автозапуск. При отсутствии доступа к реестру — <see cref="InvalidOperationException"/>.</summary>
    void Enable();

    /// <summary>Отключает автозапуск. При отсутствии доступа к реестру — <see cref="InvalidOperationException"/>.</summary>
    void Disable();
}