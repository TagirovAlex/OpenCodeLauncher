using System.Text.Json;

namespace OpenCodeLauncher.Core.Settings;

/// <summary>Загрузка и сохранение настроек в JSON-файл.</summary>
public interface ISettingsStore<T> where T : class, new()
{
    string FilePath { get; }

    T Load();

    void Save(T settings);
}

/// <summary>Реализация хранилища настроек на основе System.Text.Json.</summary>
public sealed class JsonSettingsStore<T> : ISettingsStore<T> where T : class, new()
{
    private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };

    public JsonSettingsStore(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        FilePath = filePath;
    }

    public string FilePath { get; }

    public T Load()
    {
        if (!File.Exists(FilePath))
        {
            return new T();
        }

        try
        {
            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<T>(json, Options) ?? new T();
        }
        catch (JsonException)
        {
            // Битый файл настроек — не роняем приложение, начинаем с чистых настроек.
            return new T();
        }
    }

    public void Save(T settings)
    {
        ArgumentNullException.ThrowIfNull(settings);
        var directory = Path.GetDirectoryName(FilePath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var json = JsonSerializer.Serialize(settings, Options);
        File.WriteAllText(FilePath, json);
    }
}