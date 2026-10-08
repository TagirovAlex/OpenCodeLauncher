using System.Diagnostics;
using OpenCodeLauncher.Core.Models;

namespace OpenCodeLauncher.Core.Services;

/// <summary>Реализация проверки доступности запущенного сервера по HTTP.</summary>
public sealed class HealthService : IHealthService
{
    /// <summary>Общий HttpClient для всех проверок (переиспользование сокетов и DNS-кэша).</summary>
    private static readonly HttpClient SharedClient = new();

    /// <inheritdoc/>
    public async Task<HealthCheckResult> CheckAsync(string baseUrl, TimeSpan timeout, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(baseUrl);
        if (timeout <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(timeout), "Таймаут должен быть положительным.");
        }

        var url = baseUrl.EndsWith("/", StringComparison.Ordinal) ? baseUrl : baseUrl + "/";
        var stopwatch = Stopwatch.StartNew();

        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeoutCts.CancelAfter(timeout);

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            using var response = await SharedClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, timeoutCts.Token)
                .ConfigureAwait(false);

            stopwatch.Stop();
            return new HealthCheckResult(true, stopwatch.Elapsed);
        }
        catch (OperationCanceledException)
        {
            stopwatch.Stop();
            var message = cancellationToken.IsCancellationRequested
                ? "Операция отменена"
                : "Таймаут подключения";
            return new HealthCheckResult(false, null, message);
        }
        catch (HttpRequestException ex)
        {
            stopwatch.Stop();
            return new HealthCheckResult(false, null, Shorten(ex.Message));
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return new HealthCheckResult(false, null, Shorten(ex.Message));
        }
    }

    /// <summary>Обрезает сообщение об ошибке до разумной длины.</summary>
    private static string Shorten(string message)
    {
        const int maxLength = 300;
        if (string.IsNullOrEmpty(message))
        {
            return "Неизвестная ошибка";
        }

        return message.Length <= maxLength ? message : message[..maxLength] + "...";
    }
}