using OpenCodeLauncher.Core.Models;
using OpenCodeLauncher.Core.Settings;

namespace OpenCodeLauncher.Core.Services;

/// <summary>Управление профилями запуска: CRUD с валидацией и сохранением в настройках.</summary>
public sealed class ProfileService : IProfileService
{
    private readonly AppSettings _settings;
    private readonly ISettingsStore<AppSettings> _settingsStore;

    /// <summary>Создаёт сервис профилей.</summary>
    public ProfileService(AppSettings settings, ISettingsStore<AppSettings> settingsStore)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _settingsStore = settingsStore ?? throw new ArgumentNullException(nameof(settingsStore));
    }

    /// <inheritdoc/>
    public IReadOnlyList<LaunchProfile> GetProfiles() => _settings.Profiles;

    /// <inheritdoc/>
    public void Add(LaunchProfile profile)
    {
        ArgumentNullException.ThrowIfNull(profile);
        ValidateProfile(profile);

        if (Find(profile.Name) is not null)
        {
            throw new ArgumentException($"Профиль «{profile.Name}» уже существует.");
        }

        _settings.Profiles.Add(profile);
        Save();
    }

    /// <inheritdoc/>
    public void Update(LaunchProfile profile)
    {
        ArgumentNullException.ThrowIfNull(profile);
        ValidateProfile(profile);

        var index = _settings.Profiles.FindIndex(p => string.Equals(p.Name, profile.Name, StringComparison.OrdinalIgnoreCase));
        if (index < 0)
        {
            throw new ArgumentException($"Профиль «{profile.Name}» не найден.");
        }

        _settings.Profiles[index] = profile;
        Save();
    }

    /// <inheritdoc/>
    public void Remove(string name)
    {
        var index = _settings.Profiles.FindIndex(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));
        if (index < 0)
        {
            return;
        }

        _settings.Profiles.RemoveAt(index);
        Save();
    }

    /// <inheritdoc/>
    public LaunchProfile? Find(string name)
    {
        return _settings.Profiles.FirstOrDefault(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));
    }

    /// <inheritdoc/>
    public LaunchParams ToLaunchParams(LaunchProfile profile)
    {
        ArgumentNullException.ThrowIfNull(profile);
        ValidateProfile(profile);

        return new LaunchParams(
            profile.ProjectPath,
            profile.Hostname,
            profile.Port,
            Normalize(profile.Model),
            profile.ContinueLastSession,
            Normalize(profile.SessionId),
            Normalize(profile.ExtraArgs),
            Normalize(profile.ServerUsername),
            Normalize(profile.ServerPassword));
    }

    /// <summary>Приводит пустые/пробельные опциональные значения к null и обрезает остальные.</summary>
    private static string? Normalize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return value.Trim();
    }

    /// <summary>Проверяет обязательные поля профиля; при ошибке — ArgumentException с понятным сообщением.</summary>
    private static void ValidateProfile(LaunchProfile profile)
    {
        if (string.IsNullOrWhiteSpace(profile.Name))
        {
            throw new ArgumentException("Имя профиля не может быть пустым.");
        }

        if (string.IsNullOrWhiteSpace(profile.ProjectPath))
        {
            throw new ArgumentException("Укажите путь к проекту.");
        }

        if (!Directory.Exists(profile.ProjectPath))
        {
            throw new ArgumentException($"Каталог проекта не найден: {profile.ProjectPath}");
        }

        if (string.IsNullOrWhiteSpace(profile.Hostname))
        {
            throw new ArgumentException("Укажите адрес сетевого интерфейса.");
        }

        if (profile.Port is < 1 or > 65535)
        {
            throw new ArgumentException("Порт должен быть в диапазоне 1..65535.");
        }
    }

    /// <summary>Сохраняет настройки после мутации.</summary>
    private void Save() => _settingsStore.Save(_settings);
}