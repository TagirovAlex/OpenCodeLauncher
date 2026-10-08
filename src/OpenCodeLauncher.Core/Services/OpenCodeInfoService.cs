using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using OpenCodeLauncher.Core.Models;

namespace OpenCodeLauncher.Core.Services;

/// <summary>
/// Реализация <see cref="IOpenCodeInfoService"/>: запускает <c>opencode session list</c> и
/// <c>opencode stats</c> отдельными процессами в рабочей директории проекта и защитно парсит вывод.
/// </summary>
public sealed class OpenCodeInfoService : IOpenCodeInfoService
{
    /// <summary>Таймаут выполнения каждой команды CLI.</summary>
    private static readonly TimeSpan CommandTimeout = TimeSpan.FromSeconds(15);

    /// <summary>Стоимость: число после "$" или после слов cost/price.</summary>
    private static readonly Regex CostPattern = new(
        @"(?:\$|(?:cost|price)\s*[:=]?)\s*([0-9]+(?:\.[0-9]+)?)",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    /// <summary>Входные токены: число рядом с "tokens in"/"input tokens"/"input:".</summary>
    private static readonly Regex TokensInPattern = new(
        @"(?:tokens?\s*(?:in|input)\s*[:=]?\s*|(?:in|input)\s*tokens?\s*[:=]?\s*|input\s*[:=]?\s*)\s*([0-9][0-9,]*)",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    /// <summary>Выходные токены: число рядом с "tokens out"/"output tokens"/"output:".</summary>
    private static readonly Regex TokensOutPattern = new(
        @"(?:tokens?\s*(?:out|output)\s*[:=]?\s*|(?:out|output)\s*tokens?\s*[:=]?\s*|output\s*[:=]?\s*)\s*([0-9][0-9,]*)",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    /// <summary>Слова, которыми может начинаться строка-заголовок таблицы сессий.</summary>
    private static readonly HashSet<string> HeaderWords = new(StringComparer.OrdinalIgnoreCase)
    {
        "id", "session", "session_id", "sessionid", "title", "name", "created", "createdat",
        "updated", "time", "model", "messages"
    };

    /// <inheritdoc/>
    public async Task<IReadOnlyList<OpenCodeSession>> GetSessionsAsync(string projectPath, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(projectPath);

        if (!Directory.Exists(projectPath))
        {
            return Array.Empty<OpenCodeSession>();
        }

        var output = await RunOpenCodeAsync(new[] { "session", "list" }, projectPath, ct).ConfigureAwait(false);
        return string.IsNullOrEmpty(output) ? Array.Empty<OpenCodeSession>() : ParseSessions(output);
    }

    /// <inheritdoc/>
    public async Task<UsageStats?> GetStatsAsync(string projectPath, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(projectPath);

        if (!Directory.Exists(projectPath))
        {
            return null;
        }

        var output = await RunOpenCodeAsync(new[] { "stats" }, projectPath, ct).ConfigureAwait(false);
        return string.IsNullOrEmpty(output) ? null : ParseStats(output);
    }

    /// <summary>
    /// Разбирает вывод <c>opencode session list</c>.
    /// Ожидаемый формат — построчный: первый токен строки — id, остальное — название,
    /// последний токен может быть датой создания. Заголовки, разделители и нераспознанные
    /// строки пропускаются.
    /// </summary>
    public static IReadOnlyList<OpenCodeSession> ParseSessions(string output)
    {
        var sessions = new List<OpenCodeSession>();

        if (string.IsNullOrWhiteSpace(output))
        {
            return sessions;
        }

        foreach (var rawLine in output.Split('\n'))
        {
            var line = rawLine.Trim();
            if (line.Length == 0)
            {
                continue;
            }

            var tokens = Regex.Split(line, @"\s+")
                .Where(t => t.Length > 0)
                .ToArray();

            if (tokens.Length < 2)
            {
                continue;
            }

            var id = tokens[0];
            if (HeaderWords.Contains(id) || IsSeparator(id))
            {
                continue;
            }

            // Дата создания может быть в конце строки: одним токеном (2025-01-01)
            // или двумя токенами (2025-01-01 10:30).
            DateTime? createdAt = null;
            var titleTokens = tokens[1..].ToList();
            if (titleTokens.Count >= 1 && TryParseDate(titleTokens[^1], out var lastDate))
            {
                createdAt = lastDate;
                titleTokens.RemoveAt(titleTokens.Count - 1);
            }
            else if (titleTokens.Count >= 2
                     && TryParseDate(string.Join(" ", titleTokens[^2], titleTokens[^1]), out var twoTokenDate))
            {
                createdAt = twoTokenDate;
                titleTokens.RemoveRange(titleTokens.Count - 2, 2);
            }

            var title = string.Join(" ", titleTokens).Trim();
            sessions.Add(new OpenCodeSession(id, title, createdAt));
        }

        return sessions;
    }

    /// <summary>
    /// Разбирает вывод <c>opencode stats</c> по ключевым словам через регулярные выражения.
    /// Если ничего не распознано — возвращает null; если распознана хотя бы одна величина,
    /// остальные заполняются нулями.
    /// </summary>
    public static UsageStats? ParseStats(string output)
    {
        if (string.IsNullOrWhiteSpace(output))
        {
            return null;
        }

        var costMatch = CostPattern.Match(output);
        var inMatch = TokensInPattern.Match(output);
        var outMatch = TokensOutPattern.Match(output);

        if (!costMatch.Success && !inMatch.Success && !outMatch.Success)
        {
            return null;
        }

        var cost = costMatch.Success ? ParseCost(costMatch.Groups[1].Value) : 0m;
        var tokensIn = inMatch.Success ? ParseCount(inMatch.Groups[1].Value) : 0;
        var tokensOut = outMatch.Success ? ParseCount(outMatch.Groups[1].Value) : 0;

        return new UsageStats(tokensIn, tokensOut, cost);
    }

    /// <summary>
    /// Запускает команду opencode в директории проекта, ждёт завершения с таймаутом ~15 с
    /// и возвращает stdout целиком. При таймауте процесс завершается принудительно
    /// (вместе с деревом), возвращается null.
    /// </summary>
    private static async Task<string?> RunOpenCodeAsync(
        IReadOnlyList<string> arguments,
        string projectPath,
        CancellationToken cancellationToken)
    {
        var executable = ProcessService.TryLocateOpenCode()
            ?? throw new FileNotFoundException("opencode не найден. Установите его через npm: npm i -g opencode-ai");

        var startInfo = new ProcessStartInfo
        {
            FileName = executable,
            WorkingDirectory = projectPath,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8
        };

        foreach (var argument in arguments)
        {
            startInfo.ArgumentList.Add(argument);
        }

        using var process = new Process { StartInfo = startInfo };
        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeoutCts.CancelAfter(CommandTimeout);

        try
        {
            process.Start();

            var stdoutTask = process.StandardOutput.ReadToEndAsync(timeoutCts.Token);
            var stderrTask = process.StandardError.ReadToEndAsync(timeoutCts.Token);

            await process.WaitForExitAsync(timeoutCts.Token).ConfigureAwait(false);

            var stdout = await stdoutTask.ConfigureAwait(false);
            await stderrTask.ConfigureAwait(false);
            return stdout;
        }
        catch (OperationCanceledException)
        {
            KillProcess(process);
            return null;
        }
        catch (Win32Exception ex)
        {
            throw new InvalidOperationException("Не удалось запустить opencode: " + ex.Message, ex);
        }
        catch (Exception ex) when (ex is IOException or InvalidOperationException)
        {
            KillProcess(process);
            return null;
        }
    }

    /// <summary>Принудительно завершает процесс вместе с деревом (если ещё жив).</summary>
    private static void KillProcess(Process process)
    {
        try
        {
            if (!process.HasExited)
            {
                process.Kill(entireProcessTree: true);
                process.WaitForExit();
            }
        }
        catch (InvalidOperationException)
        {
            // Процесс уже завершился.
        }
        catch (Win32Exception)
        {
            // Не удалось завершить — игнорируем.
        }
    }

    private static bool IsSeparator(string token) => Regex.IsMatch(token, @"^[-=_.]+$");

    /// <summary>
    /// Пытается распознать дату. Токен вроде "10:30" (только время) не считается датой,
    /// чтобы не вырезать его из названия.
    /// </summary>
    private static bool TryParseDate(string text, out DateTime value)
    {
        if (!text.Contains('-') && !text.Contains('/') && !Regex.IsMatch(text, @"\d{4}"))
        {
            value = default;
            return false;
        }

        return DateTime.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out value);
    }

    /// <summary>Число с разделителями тысяч (запятые удаляются).</summary>
    private static long ParseCount(string raw)
    {
        var digits = raw.Replace(",", "");
        return long.TryParse(digits, NumberStyles.None, CultureInfo.InvariantCulture, out var value) ? value : 0;
    }

    /// <summary>Денежное значение (символ "$" и разделители тысяч удаляются).</summary>
    private static decimal ParseCost(string raw)
    {
        var digits = raw.Replace("$", "").Replace(",", "");
        return decimal.TryParse(digits, NumberStyles.Number, CultureInfo.InvariantCulture, out var value) ? value : 0m;
    }
}