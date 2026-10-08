using System.Text.Json;
using System.Text.Json.Nodes;
using OpenCodeLauncher.Core.Services;

namespace OpenCodeLauncher.Tests;

/// <summary>Юнит-тесты <see cref="OpenCodeConfigService"/> (только файлы в temp/ проекта).</summary>
public class OpenCodeConfigServiceTests : IDisposable
{
    private readonly string _tempRoot;

    public OpenCodeConfigServiceTests()
    {
        _tempRoot = Path.Combine(GetProjectRoot(), "temp", "config-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_tempRoot);
    }

    public void Dispose()
    {
        try
        {
            if (Directory.Exists(_tempRoot))
            {
                Directory.Delete(_tempRoot, recursive: true);
            }
        }
        catch (IOException)
        {
        }
        catch (UnauthorizedAccessException)
        {
        }
    }

    [Fact]
    public void SaveAndLoad_RoundTrip_PreservesContent()
    {
        var path = GetTempPath();
        var service = new OpenCodeConfigService(path);

        var config = new JsonObject
        {
            ["model"] = "gpt-4",
            ["provider"] = new JsonObject { ["apiKey"] = "secret" },
            ["list"] = new JsonArray("a", "b"),
        };

        service.SaveConfig(config);
        var loaded = service.LoadConfig();

        Assert.Equal("gpt-4", (string?)loaded["model"]);
        Assert.Equal("secret", (string?)loaded["provider"]?["apiKey"]);
        Assert.Equal(2, loaded["list"]?.AsArray().Count);
    }

    [Fact]
    public void Save_CreatesBackupWithPreviousContent()
    {
        var path = GetTempPath();
        File.WriteAllText(path, "{\"model\":\"old\"}");

        var service = new OpenCodeConfigService(path);
        service.SaveConfig(new JsonObject { ["model"] = "new" });

        Assert.True(File.Exists(path + ".bak"));
        var backup = JsonNode.Parse(File.ReadAllText(path + ".bak"));
        Assert.Equal("old", (string?)backup?["model"]);
    }

    [Fact]
    public void Load_InvalidJson_ThrowsJsonException()
    {
        var path = GetTempPath();
        File.WriteAllText(path, "{ not valid json");

        var service = new OpenCodeConfigService(path);

        Assert.ThrowsAny<JsonException>(() => service.LoadConfig());
    }

    [Fact]
    public void Load_MissingFile_ReturnsEmptyObject()
    {
        var service = new OpenCodeConfigService(GetTempPath("missing.json"));

        var loaded = service.LoadConfig();

        Assert.NotNull(loaded);
        Assert.Empty(loaded);
    }

    [Fact]
    public void Save_CreatesMissingDirectories()
    {
        var path = Path.Combine(_tempRoot, "nested", "deep", "opencode.json");
        var service = new OpenCodeConfigService(path);

        service.SaveConfig(new JsonObject { ["model"] = "gpt-4" });

        Assert.True(File.Exists(path));
        Assert.Equal("gpt-4", (string?)service.LoadConfig()["model"]);
    }

    [Fact]
    public void GetDefaultConfigPath_UsesOpenCodeConfigEnvVarWhenSet()
    {
        var original = Environment.GetEnvironmentVariable("OPENCODE_CONFIG");
        try
        {
            Environment.SetEnvironmentVariable("OPENCODE_CONFIG", @"C:\custom\opencode.json");
            var service = new OpenCodeConfigService(GetTempPath("unused.json"));

            Assert.Equal(@"C:\custom\opencode.json", service.GetDefaultConfigPath());
        }
        finally
        {
            Environment.SetEnvironmentVariable("OPENCODE_CONFIG", original);
        }
    }

    [Fact]
    public void GetDefaultConfigPath_FallsBackToUserProfile()
    {
        var original = Environment.GetEnvironmentVariable("OPENCODE_CONFIG");
        try
        {
            Environment.SetEnvironmentVariable("OPENCODE_CONFIG", null);
            var expected = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                ".config",
                "opencode",
                "opencode.json");
            var service = new OpenCodeConfigService(GetTempPath("unused.json"));

            Assert.Equal(expected, service.GetDefaultConfigPath());
        }
        finally
        {
            Environment.SetEnvironmentVariable("OPENCODE_CONFIG", original);
        }
    }

    private static string GetProjectRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null && !string.Equals(dir.Name, "OpenCodeLauncher", StringComparison.OrdinalIgnoreCase))
        {
            dir = dir.Parent;
        }

        return dir?.FullName ?? throw new InvalidOperationException("Корень проекта не найден.");
    }

    private string GetTempPath(string fileName = "opencode.json") => Path.Combine(_tempRoot, fileName);
}