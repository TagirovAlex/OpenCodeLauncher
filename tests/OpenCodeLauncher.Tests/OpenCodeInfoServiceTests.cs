using OpenCodeLauncher.Core.Services;

namespace OpenCodeLauncher.Tests;

public class OpenCodeInfoServiceTests
{
    // ---------- ParseSessions ----------

    [Fact]
    public void ParseSessions_EmptyOutput_ReturnsEmpty()
    {
        Assert.Empty(OpenCodeInfoService.ParseSessions(""));
    }

    [Fact]
    public void ParseSessions_WhitespaceOnly_ReturnsEmpty()
    {
        Assert.Empty(OpenCodeInfoService.ParseSessions("\n  \n\t\n"));
    }

    [Fact]
    public void ParseSessions_HeaderAndSeparatorLines_AreSkipped()
    {
        const string output = "ID    TITLE    CREATED\n----  --------  ----------\nsess1  Первая   2025-01-01";

        var sessions = OpenCodeInfoService.ParseSessions(output);

        var session = Assert.Single(sessions);
        Assert.Equal("sess1", session.Id);
        Assert.Equal("Первая", session.Title);
        Assert.Equal(new DateTime(2025, 1, 1), session.CreatedAt);
    }

    [Fact]
    public void ParseSessions_SimpleLines_IdIsFirstTokenTitleIsRest()
    {
        const string output = "abc123  Проект Alpha\ndef456  Проект Beta\n";

        var sessions = OpenCodeInfoService.ParseSessions(output);

        Assert.Equal(2, sessions.Count);
        Assert.Equal("abc123", sessions[0].Id);
        Assert.Equal("Проект Alpha", sessions[0].Title);
        Assert.Equal("def456", sessions[1].Id);
        Assert.Equal("Проект Beta", sessions[1].Title);
    }

    [Fact]
    public void ParseSessions_LineWithTrailingDate_SetsCreatedAtAndStripsItFromTitle()
    {
        const string output = "sessX  Задача по API  2025-03-01 10:30";

        var session = Assert.Single(OpenCodeInfoService.ParseSessions(output));

        Assert.Equal("sessX", session.Id);
        Assert.Equal("Задача по API", session.Title);
        Assert.Equal(new DateTime(2025, 3, 1, 10, 30, 0), session.CreatedAt);
    }

    [Fact]
    public void ParseSessions_LineWithoutDate_CreatedAtIsNull()
    {
        const string output = "sessY  Просто название";

        var session = Assert.Single(OpenCodeInfoService.ParseSessions(output));

        Assert.Equal("sessY", session.Id);
        Assert.Equal("Просто название", session.Title);
        Assert.Null(session.CreatedAt);
    }

    [Fact]
    public void ParseSessions_SingleTokenLines_AreSkipped()
    {
        const string output = "only-one-token\n\n---\n";

        Assert.Empty(OpenCodeInfoService.ParseSessions(output));
    }

    // ---------- ParseStats ----------

    [Fact]
    public void ParseStats_EmptyOutput_ReturnsNull()
    {
        Assert.Null(OpenCodeInfoService.ParseStats(""));
    }

    [Fact]
    public void ParseStats_CostWithDollarSign_ParsesCost()
    {
        const string output = "Total cost: $12.34";

        var stats = OpenCodeInfoService.ParseStats(output);

        Assert.NotNull(stats);
        Assert.Equal(12.34m, stats.Cost);
        Assert.Equal(0, stats.TokensIn);
        Assert.Equal(0, stats.TokensOut);
    }

    [Fact]
    public void ParseStats_CostWithoutDollarSign_ParsesCost()
    {
        const string output = "cost: 5.5\n";

        var stats = OpenCodeInfoService.ParseStats(output);

        Assert.NotNull(stats);
        Assert.Equal(5.5m, stats.Cost);
    }

    [Fact]
    public void ParseStats_TokensInOut_ParsesCounts()
    {
        const string output = "Input tokens: 1,500,000\nOutput tokens: 250,000";

        var stats = OpenCodeInfoService.ParseStats(output);

        Assert.NotNull(stats);
        Assert.Equal(1_500_000, stats.TokensIn);
        Assert.Equal(250_000, stats.TokensOut);
    }

    [Fact]
    public void ParseStats_TokensInOutInverseOrder_ParsesCounts()
    {
        const string output = "tokens in 1000, tokens out 2000";

        var stats = OpenCodeInfoService.ParseStats(output);

        Assert.NotNull(stats);
        Assert.Equal(1000, stats.TokensIn);
        Assert.Equal(2000, stats.TokensOut);
    }

    [Fact]
    public void ParseStats_PartialData_FillsMissingWithZeros()
    {
        const string output = "cost: 5";

        var stats = OpenCodeInfoService.ParseStats(output);

        Assert.NotNull(stats);
        Assert.Equal(5m, stats.Cost);
        Assert.Equal(0, stats.TokensIn);
        Assert.Equal(0, stats.TokensOut);
    }

    [Fact]
    public void ParseStats_UnrecognizedOutput_ReturnsNull()
    {
        const string output = "some random text without meaningful numbers";

        Assert.Null(OpenCodeInfoService.ParseStats(output));
    }

    [Fact]
    public void ParseStats_TableLikeOutput_ParsesAllValues()
    {
        const string output = "metric       value\n" +
                              "cost         $1.25\n" +
                              "input        1200\n" +
                              "output       800\n";

        var stats = OpenCodeInfoService.ParseStats(output);

        Assert.NotNull(stats);
        Assert.Equal(1.25m, stats.Cost);
        Assert.Equal(1200, stats.TokensIn);
        Assert.Equal(800, stats.TokensOut);
    }
}