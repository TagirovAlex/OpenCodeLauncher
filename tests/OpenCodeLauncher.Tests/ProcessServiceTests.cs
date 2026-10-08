using OpenCodeLauncher.Core.Models;
using OpenCodeLauncher.Core.Services;

namespace OpenCodeLauncher.Tests;

public class ProcessServiceTests
{
    [Fact]
    public void BuildArguments_FullSet_ReturnsArgumentsInExpectedOrder()
    {
        var parameters = new LaunchParams(
            "C:\\Projects\\App",
            "127.0.0.1",
            4000,
            Model: "gpt-4o",
            ContinueLastSession: true,
            SessionId: "sess-123",
            ExtraArgs: "--verbose --log-level debug");

        var arguments = ProcessService.BuildArguments(parameters);

        Assert.Equal(new List<string>
        {
            "serve",
            "--hostname", "127.0.0.1",
            "--port", "4000",
            "--model", "gpt-4o",
            "--continue",
            "--session", "sess-123",
            "--verbose", "--log-level", "debug"
        }, arguments);
    }

    [Fact]
    public void BuildArguments_EmptyOptionals_AreOmitted()
    {
        var parameters = new LaunchParams("C:\\Projects\\App", "localhost", 8080);

        var arguments = ProcessService.BuildArguments(parameters);

        Assert.Equal(new List<string> { "serve", "--hostname", "localhost", "--port", "8080" }, arguments);
    }

    [Fact]
    public void BuildArguments_ExtraArgs_AppendedToTheEnd()
    {
        var parameters = new LaunchParams("C:\\Projects\\App", "localhost", 8080, ExtraArgs: "--debug");

        var arguments = ProcessService.BuildArguments(parameters);

        Assert.Equal(new List<string> { "serve", "--hostname", "localhost", "--port", "8080", "--debug" }, arguments);
    }

    [Fact]
    public void BuildArguments_ExtraArgsWithWhitespace_SplitAndEmptiesRemoved()
    {
        var parameters = new LaunchParams("C:\\Projects\\App", "localhost", 8080, ExtraArgs: "  --a   --b  ");

        var arguments = ProcessService.BuildArguments(parameters);

        Assert.Equal(new List<string> { "serve", "--hostname", "localhost", "--port", "8080", "--a", "--b" }, arguments);
    }

    [Fact]
    public void Start_NonExistentProjectPath_ThrowsArgumentException()
    {
        var service = new ProcessService();
        var parameters = new LaunchParams(Path.Combine(Path.GetFullPath("."), "no-such-directory"), "127.0.0.1", 4000);

        Assert.Throws<ArgumentException>(() => service.Start(parameters));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(70000)]
    public void Start_InvalidPort_ThrowsArgumentException(int port)
    {
        var service = new ProcessService();
        var parameters = new LaunchParams(Path.GetFullPath("."), "127.0.0.1", port);

        Assert.Throws<ArgumentException>(() => service.Start(parameters));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Start_EmptyHostname_ThrowsArgumentException(string hostname)
    {
        var service = new ProcessService();
        var parameters = new LaunchParams(Path.GetFullPath("."), hostname, 4000);

        Assert.Throws<ArgumentException>(() => service.Start(parameters));
    }

    [Fact]
    public void LocateOpenCode_EmptyPath_ThrowsFileNotFoundException()
    {
        Assert.Throws<FileNotFoundException>(() => ProcessService.LocateOpenCode(" "));
    }

    [Fact]
    public void LocateOpenCode_NotFoundInPath_ThrowsFileNotFoundException()
    {
        Assert.Throws<FileNotFoundException>(() => ProcessService.LocateOpenCode("C:\\no-such-opencode-directory"));
    }

    [Fact]
    public void LocateOpenCode_FindsExecutableInPath()
    {
        var searchDir = Path.Combine(Path.GetFullPath("."), "temp", "locator-test");
        Directory.CreateDirectory(searchDir);
        var fakeExe = Path.Combine(searchDir, "opencode.exe");
        File.WriteAllText(fakeExe, "");

        try
        {
            var result = ProcessService.LocateOpenCode(searchDir);

            Assert.Equal(fakeExe, result);
        }
        finally
        {
            File.Delete(fakeExe);
            Directory.Delete(searchDir, true);
        }
    }

    [Fact]
    public void LocateOpenCode_PrefersExeOverCmd()
    {
        var searchDir = Path.Combine(Path.GetFullPath("."), "temp", "locator-preference-test");
        Directory.CreateDirectory(searchDir);
        var fakeExe = Path.Combine(searchDir, "opencode.exe");
        var fakeCmd = Path.Combine(searchDir, "opencode.cmd");
        File.WriteAllText(fakeExe, "");
        File.WriteAllText(fakeCmd, "");

        try
        {
            var result = ProcessService.LocateOpenCode(searchDir);

            Assert.Equal(fakeExe, result);
        }
        finally
        {
            File.Delete(fakeExe);
            File.Delete(fakeCmd);
            Directory.Delete(searchDir, true);
        }
    }
}