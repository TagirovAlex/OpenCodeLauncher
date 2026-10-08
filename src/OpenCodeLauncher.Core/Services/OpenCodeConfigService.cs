using System.Text.Json;
using System.Text.Json.Nodes;

namespace OpenCodeLauncher.Core.Services;

/// <summary>Реализация чтения/записи конфигурационного файла OpenCode на основе System.Text.Json.</summary>
public sealed class OpenCodeConfigService : IOpenCodeConfigService
{
    private const string BackupExtension = ".bak";

    private static readonly JsonSerializerOptions WriteOptions = new() { WriteIndented = true };

    public OpenCodeConfigService(string configFilePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(configFilePath);
        ConfigFilePath = ResolveExistingFile(configFilePath);
    }

    /// <summary>
    /// Если файл не существует — ищет альтернативу в том же каталоге:
    /// opencode.json → opencode.jsonc → config.json (opencode поддерживает JSONC).
    /// Если ничего нет — возвращает исходный путь (файл будет создан при сохранении).
    /// </summary>
    private static string ResolveExistingFile(string path)
    {
        if (File.Exists(path))
        {
            return path;
        }

        var directory = Path.GetDirectoryName(path);
        var fileName = Path.GetFileName(path);
        if (string.IsNullOrEmpty(directory))
        {
            return path;
        }

        string[] candidates = fileName.Equals("opencode.json", StringComparison.OrdinalIgnoreCase)
            ? new[] { "opencode.jsonc", "config.json" }
            : fileName.Equals("opencode.jsonc", StringComparison.OrdinalIgnoreCase)
                ? new[] { "config.json" }
                : Array.Empty<string>();

        foreach (var candidate in candidates)
        {
            var candidatePath = Path.Combine(directory, candidate);
            if (File.Exists(candidatePath))
            {
                return candidatePath;
            }
        }

        return path;
    }

    public string ConfigFilePath { get; }

    /// <summary>
    /// Путь по умолчанию без создания экземпляра: переменная OPENCODE_CONFIG,
    /// иначе %USERPROFILE%\.config\opencode\opencode.json.
    /// </summary>
    public static string GetDefaultPath()
    {
        var envPath = Environment.GetEnvironmentVariable("OPENCODE_CONFIG");
        if (!string.IsNullOrWhiteSpace(envPath))
        {
            return envPath;
        }

        return Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".config",
            "opencode",
            "opencode.json");
    }

    public string GetDefaultConfigPath() => GetDefaultPath();

    public JsonObject LoadConfig()
    {
        if (!File.Exists(ConfigFilePath))
        {
            return new JsonObject();
        }

        var json = File.ReadAllText(ConfigFilePath);
        // JSONC: допускаем комментарии и висячие запятые.
        var options = new JsonDocumentOptions
        {
            CommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        };

        var node = JsonNode.Parse(json, documentOptions: options);
        return node as JsonObject
            ?? throw new JsonException("Файл конфигурации должен содержать JSON-объект.");
    }

    public void SaveConfig(JsonObject config)
    {
        ArgumentNullException.ThrowIfNull(config);

        var directory = Path.GetDirectoryName(ConfigFilePath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (File.Exists(ConfigFilePath))
        {
            File.Copy(ConfigFilePath, ConfigFilePath + BackupExtension, overwrite: true);
        }

        File.WriteAllText(ConfigFilePath, config.ToJsonString(WriteOptions));
    }

    /// <inheritdoc/>
    public string ReadRawText()
    {
        return File.Exists(ConfigFilePath) ? File.ReadAllText(ConfigFilePath) : "{}";
    }

    /// <inheritdoc/>
    public void SaveRawText(string content)
    {
        ArgumentNullException.ThrowIfNull(content);

        // Валидация: текст должен быть валидным JSONC-объектом (комментарии допускаются).
        ValidateRawText(content);

        var directory = Path.GetDirectoryName(ConfigFilePath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (File.Exists(ConfigFilePath))
        {
            File.Copy(ConfigFilePath, ConfigFilePath + BackupExtension, overwrite: true);
        }

        // Пишем дословно: сохраняем комментарии и форматирование JSONC.
        File.WriteAllText(ConfigFilePath, content);
    }

    /// <summary>Проверяет, что текст — валидный JSONC-объект. Иначе — JsonException.</summary>
    private static void ValidateRawText(string content)
    {
        var options = new JsonDocumentOptions
        {
            CommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        };

        var node = JsonNode.Parse(content, documentOptions: options);
        if (node is not JsonObject)
        {
            throw new JsonException("Конфиг должен быть JSON-объектом (корневой элемент {}).");
        }
    }
}