using System.Text.Json;
using OpenCodeLauncher.Core.Services;

namespace OpenCodeLauncher.Tests;

/// <summary>
/// Тесты сырого (дословного) редактирования конфига: комментарии JSONC и настройки
/// провайдеров должны сохраняться без потерь.
/// </summary>
public class ConfigRawTests
{
    private readonly string _dir;

    public ConfigRawTests()
    {
        _dir = Path.Combine(Path.GetFullPath("."), "temp", "config-raw-tests");
        Directory.CreateDirectory(_dir);
    }

    [Fact]
    public void SaveRawText_PreservesCommentsAndFormatting()
    {
        var path = Path.Combine(_dir, Guid.NewGuid() + ".jsonc");
        var service = new OpenCodeConfigService(path);

        const string content = """
        {
          // Комментарий о провайдере
          "provider": {
            "aitunnel": {
              "name": "aitunnel",
              "options": { "baseURL": "https://ru-api.aitunnel.ru/v1" }
            }
          },
        }
        """;

        service.SaveRawText(content);

        Assert.Equal(content, service.ReadRawText());
    }

    [Fact]
    public void SaveRawText_InvalidJson_Throws()
    {
        var path = Path.Combine(_dir, Guid.NewGuid() + ".jsonc");
        var service = new OpenCodeConfigService(path);

        Assert.ThrowsAny<JsonException>(() => service.SaveRawText("{ not valid"));
    }

    [Fact]
    public void SaveRawText_CreatesBackup()
    {
        var path = Path.Combine(_dir, Guid.NewGuid() + ".jsonc");
        var service = new OpenCodeConfigService(path);

        service.SaveRawText("{ \"a\": 1 }");
        service.SaveRawText("{ \"a\": 2 }");

        Assert.True(File.Exists(path + ".bak"));
        Assert.Equal("{ \"a\": 1 }", File.ReadAllText(path + ".bak"));
    }
}