using OpenCodeLauncher.Core.Models;
using OpenCodeLauncher.Core.Settings;

namespace OpenCodeLauncher.Core.Services;

/// <summary>CRUD сохранённых профилей запуска.</summary>
public interface IProfileService
{
    /// <summary>Возвращает все профили в порядке сохранения.</summary>
    IReadOnlyList<LaunchProfile> GetProfiles();

    /// <summary>Добавляет новый профиль. При ошибке валидации или дубликате имени — ArgumentException.</summary>
    void Add(LaunchProfile profile);

    /// <summary>Заменяет существующий профиль с тем же именем (по имени).</summary>
    void Update(LaunchProfile profile);

    /// <summary>Удаляет профиль по имени; отсутствующий профиль — no-op.</summary>
    void Remove(string name);

    /// <summary>Ищет профиль по имени (регистронезависимо); null, если не найден.</summary>
    LaunchProfile? Find(string name);

    /// <summary>Преобразует профиль в параметры запуска; пустые опциональные поля становятся null.</summary>
    LaunchParams ToLaunchParams(LaunchProfile profile);
}