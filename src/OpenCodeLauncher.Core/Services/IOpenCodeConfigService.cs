using System.Text.Json.Nodes;

namespace OpenCodeLauncher.Core.Services;

/// <summary>Чтение и запись конфигурационного файла OpenCode (opencode.json).</summary>
public interface IOpenCodeConfigService
{
    /// <summary>Полный путь к файлу конфигурации, с которым работает сервис.</summary>
    string ConfigFilePath { get; }

    /// <summary>Возвращает путь по умолчанию: %USERPROFILE%\.config\opencode\opencode.json (или OPENCODE_CONFIG, если задан).</summary>
    string GetDefaultConfigPath();

    /// <summary>Читает конфиг как JsonObject (System.Text.Json.Nodes); если файла нет — пустой JsonObject. Невалидный JSON — бросает JsonException.</summary>
    JsonObject LoadConfig();

    /// <summary>Пишет конфиг. Перед записью делает резервную копию &lt;путь&gt;.bak (если файл существует). Создаёт каталоги при необходимости.</summary>
    void SaveConfig(JsonObject config);

    /// <summary>
    /// Читает содержимое файла дословно (без перегенерации), чтобы сохранить комментарии JSONC.
    /// Если файла нет — возвращает "{}".
    /// </summary>
    string ReadRawText();

    /// <summary>
    /// Пишет переданный текст дословно (сохраняет комментарии JSONC). Перед записью валидирует
    /// текст как JSONC (иначе JsonException) и делает резервную копию &lt;путь&gt;.bak.
    /// </summary>
    void SaveRawText(string content);
}