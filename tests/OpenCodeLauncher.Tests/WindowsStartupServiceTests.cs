using System.Runtime.Versioning;
using OpenCodeLauncher.Core.Services;

namespace OpenCodeLauncher.Tests;

/// <summary>Юнит-тесты чистых хелперов <see cref="WindowsStartupService"/> (без обращения к реестру).</summary>
[SupportedOSPlatform("windows")]
public class WindowsStartupServiceTests
{
    [Fact]
    public void BuildCommandLine_QuotesPath()
    {
        const string path = @"C:\Program Files\OpenCodeLauncher\opencode-launcher.exe";

        var result = WindowsStartupService.BuildCommandLine(path);

        Assert.Equal("\"C:\\Program Files\\OpenCodeLauncher\\opencode-launcher.exe\"", result);
    }

    [Fact]
    public void BuildCommandLine_EmptyPath_Throws()
    {
        Assert.Throws<ArgumentException>(() => WindowsStartupService.BuildCommandLine(string.Empty));
        Assert.Throws<ArgumentException>(() => WindowsStartupService.BuildCommandLine("   "));
    }

    [Fact]
    public void NormalizeExePath_RemovesQuotesAndTrims()
    {
        var result = WindowsStartupService.NormalizeExePath("\" C:\\Program Files\\app.exe \"");
        Assert.Equal(@"C:\Program Files\app.exe", result);
    }

    [Fact]
    public void NormalizeExePath_WithoutQuotes_JustTrims()
    {
        var result = WindowsStartupService.NormalizeExePath("  C:\\app.exe  ");
        Assert.Equal(@"C:\app.exe", result);
    }

    [Fact]
    public void NormalizeExePath_NullOrWhitespace_ReturnsEmpty()
    {
        Assert.Equal(string.Empty, WindowsStartupService.NormalizeExePath(null));
        Assert.Equal(string.Empty, WindowsStartupService.NormalizeExePath("   "));
        Assert.Equal(string.Empty, WindowsStartupService.NormalizeExePath("\"\""));
    }
}