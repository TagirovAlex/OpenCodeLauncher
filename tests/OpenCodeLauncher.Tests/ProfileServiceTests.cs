using OpenCodeLauncher.Core.Services;
using OpenCodeLauncher.Core.Settings;

namespace OpenCodeLauncher.Tests;

/// <summary>Тесты <see cref="ProfileService"/>: CRUD, валидация, маппинг в <c>LaunchParams</c>.</summary>
public class ProfileServiceTests : IDisposable
{
    private readonly string _tempRoot;
    private readonly string _projectDir;

    public ProfileServiceTests()
    {
        _tempRoot = Path.Combine(GetTempRoot(), "ProfileServiceTests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_tempRoot);
        _projectDir = Path.Combine(_tempRoot, "Project");
        Directory.CreateDirectory(_projectDir);
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
            // Файл занят/не удалён — не роняем остальные тесты.
        }
        catch (UnauthorizedAccessException)
        {
        }
    }

    [Fact]
    public void Add_AddsProfileToCollectionAndPersists()
    {
        var service = CreateService(out var storePath);
        var profile = CreateProfile();

        service.Add(profile);

        Assert.Single(service.GetProfiles());
        Assert.Same(profile, service.GetProfiles()[0]);

        var reloaded = new JsonSettingsStore<AppSettings>(storePath).Load();
        Assert.Single(reloaded.Profiles);
        Assert.Equal(profile.Name, reloaded.Profiles[0].Name);
        Assert.Equal(profile.Port, reloaded.Profiles[0].Port);
    }

    [Fact]
    public void Get_ReturnsAllProfiles()
    {
        var settings = new AppSettings { Profiles = new List<LaunchProfile>() };
        settings.Profiles.Add(CreateProfile("A"));
        settings.Profiles.Add(CreateProfile("B"));

        var service = CreateService(out _, settings);

        var profiles = service.GetProfiles();

        Assert.Equal(2, profiles.Count);
        Assert.Contains(profiles, p => p.Name == "A");
        Assert.Contains(profiles, p => p.Name == "B");
    }

    [Fact]
    public void Update_ReplacesProfileByNameAndPersists()
    {
        var service = CreateService(out var storePath);
        service.Add(CreateProfile("Мой профиль"));

        var updated = CreateProfile("Мой профиль");
        updated.Port = 5000;
        updated.Model = "claude-sonnet-4";

        service.Update(updated);

        Assert.Equal(5000, service.Find("Мой профиль")?.Port);
        Assert.Same(updated, service.Find("Мой профиль"));

        var reloaded = new JsonSettingsStore<AppSettings>(storePath).Load();
        Assert.Equal(5000, reloaded.Profiles[0].Port);
        Assert.Equal("claude-sonnet-4", reloaded.Profiles[0].Model);
    }

    [Fact]
    public void Remove_RemovesProfileAndPersists()
    {
        var service = CreateService(out var storePath);
        service.Add(CreateProfile("A"));
        service.Add(CreateProfile("B"));

        service.Remove("A");

        Assert.Null(service.Find("A"));
        Assert.NotNull(service.Find("B"));

        var reloaded = new JsonSettingsStore<AppSettings>(storePath).Load();
        Assert.Single(reloaded.Profiles);
        Assert.Equal("B", reloaded.Profiles[0].Name);
    }

    [Fact]
    public void Remove_UnknownName_DoesNothing()
    {
        var service = CreateService(out _);
        service.Add(CreateProfile("A"));

        service.Remove("Отсутствует");

        Assert.Single(service.GetProfiles());
    }

    [Fact]
    public void Find_ReturnsProfileByName_OrNull()
    {
        var service = CreateService(out _);
        service.Add(CreateProfile("A"));

        Assert.NotNull(service.Find("A"));
        Assert.Null(service.Find("B"));
    }

    [Fact]
    public void Add_DuplicateName_ThrowsArgumentException()
    {
        var service = CreateService(out _);
        service.Add(CreateProfile("Один"));

        var duplicate = CreateProfile("Один");
        duplicate.Port = 5000;

        Assert.Throws<ArgumentException>(() => service.Add(duplicate));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Add_EmptyOrWhitespaceName_ThrowsArgumentException(string? name)
    {
        var service = CreateService(out _);
        var profile = CreateProfile();
        profile.Name = name ?? string.Empty;

        Assert.Throws<ArgumentException>(() => service.Add(profile));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(65536)]
    public void Add_PortOutOfRange_ThrowsArgumentException(int port)
    {
        var service = CreateService(out _);
        var profile = CreateProfile();
        profile.Port = port;

        Assert.Throws<ArgumentException>(() => service.Add(profile));
    }

    [Fact]
    public void Add_NonexistentProjectDirectory_ThrowsArgumentException()
    {
        var service = CreateService(out _);
        var profile = CreateProfile();
        profile.ProjectPath = Path.Combine(_tempRoot, "НетТакогоКаталога");

        Assert.Throws<ArgumentException>(() => service.Add(profile));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Add_EmptyHostname_ThrowsArgumentException(string hostname)
    {
        var service = CreateService(out _);
        var profile = CreateProfile();
        profile.Hostname = hostname;

        Assert.Throws<ArgumentException>(() => service.Add(profile));
    }

    [Fact]
    public void Update_UnknownName_ThrowsArgumentException()
    {
        var service = CreateService(out _);
        service.Add(CreateProfile("A"));

        var profile = CreateProfile("B");

        Assert.Throws<ArgumentException>(() => service.Update(profile));
    }

    [Fact]
    public void ToLaunchParams_MapsAllFields()
    {
        var service = CreateService(out _);
        var profile = CreateProfile();

        var result = service.ToLaunchParams(profile);

        Assert.Equal(profile.ProjectPath, result.ProjectPath);
        Assert.Equal(profile.Hostname, result.Hostname);
        Assert.Equal(profile.Port, result.Port);
        Assert.Equal(profile.Model, result.Model);
        Assert.True(result.ContinueLastSession);
        Assert.Equal(profile.SessionId, result.SessionId);
        Assert.Equal(profile.ExtraArgs, result.ExtraArgs);
    }

    [Fact]
    public void ToLaunchParams_EmptyOptionalFieldsBecomeNull()
    {
        var service = CreateService(out _);
        var profile = CreateProfile();
        profile.Model = "   ";
        profile.ContinueLastSession = false;
        profile.SessionId = string.Empty;
        profile.ExtraArgs = null;

        var result = service.ToLaunchParams(profile);

        Assert.Null(result.Model);
        Assert.False(result.ContinueLastSession);
        Assert.Null(result.SessionId);
        Assert.Null(result.ExtraArgs);
    }

    [Fact]
    public void ToLaunchParams_InvalidPort_ThrowsArgumentException()
    {
        var service = CreateService(out _);
        var profile = CreateProfile();
        profile.Port = 0;

        Assert.Throws<ArgumentException>(() => service.ToLaunchParams(profile));
    }

    private ProfileService CreateService(out string storePath, AppSettings? settings = null)
    {
        storePath = Path.Combine(_tempRoot, "settings.json");
        var store = new JsonSettingsStore<AppSettings>(storePath);
        var appSettings = settings ?? new AppSettings { Profiles = new List<LaunchProfile>() };
        return new ProfileService(appSettings, store);
    }

    private LaunchProfile CreateProfile(string name = "Мой профиль")
    {
        return new LaunchProfile
        {
            Name = name,
            ProjectPath = _projectDir,
            Hostname = "100.64.0.2",
            Port = 4000,
            Model = "gpt-4o",
            ContinueLastSession = true,
            SessionId = "sess-abc",
            ExtraArgs = "--max-turns 10",
        };
    }

    /// <summary>Возвращает путь к папке temp/ корня проекта (тесты не пишут вне проекта).</summary>
    private static string GetTempRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "OpenCodeLauncher.sln")))
        {
            directory = directory.Parent;
        }

        if (directory is null)
        {
            throw new InvalidOperationException("Корень проекта не найден.");
        }

        return Path.Combine(directory.FullName, "temp");
    }
}