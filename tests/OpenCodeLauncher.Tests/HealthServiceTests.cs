using System.Net;
using System.Net.Sockets;
using OpenCodeLauncher.Core.Services;

namespace OpenCodeLauncher.Tests;

/// <summary>Юнит-тесты <see cref="HealthService"/> (только локальные сети, без внешнего интернета).</summary>
public class HealthServiceTests
{
    [Fact]
    public async Task CheckAsync_ServerResponds_ReturnsReachable()
    {
        using var listener = new HttpListener();
        var port = GetFreePort();
        listener.Prefixes.Add($"http://127.0.0.1:{port}/");
        listener.Start();

        var server = Task.Run(async () =>
        {
            while (listener.IsListening)
            {
                try
                {
                    var context = await listener.GetContextAsync();
                    context.Response.StatusCode = 200;
                    context.Response.ContentLength64 = 0;
                    context.Response.Close();
                }
                catch (HttpListenerException)
                {
                    // Listener остановлен — выходим из цикла.
                    break;
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
            }
        });

        try
        {
            var service = new HealthService();
            var result = await service.CheckAsync($"http://127.0.0.1:{port}/", TimeSpan.FromSeconds(5));

            Assert.True(result.IsReachable);
            Assert.NotNull(result.Latency);
            Assert.Null(result.Error);
        }
        finally
        {
            listener.Stop();
            listener.Close();
            await server;
        }
    }

    [Fact]
    public async Task CheckAsync_AddsTrailingSlash()
    {
        using var listener = new HttpListener();
        var port = GetFreePort();
        listener.Prefixes.Add($"http://127.0.0.1:{port}/");
        listener.Start();

        var server = Task.Run(async () =>
        {
            while (listener.IsListening)
            {
                try
                {
                    var context = await listener.GetContextAsync();
                    context.Response.StatusCode = 200;
                    context.Response.ContentLength64 = 0;
                    context.Response.Close();
                }
                catch (HttpListenerException)
                {
                    break;
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
            }
        });

        try
        {
            var service = new HealthService();
            var result = await service.CheckAsync($"http://127.0.0.1:{port}", TimeSpan.FromSeconds(5));

            Assert.True(result.IsReachable);
        }
        finally
        {
            listener.Stop();
            listener.Close();
            await server;
        }
    }

    [Fact]
    public async Task CheckAsync_ConnectionRefused_ReturnsUnreachable()
    {
        var port = GetFreePort();

        var service = new HealthService();
        var result = await service.CheckAsync($"http://127.0.0.1:{port}/", TimeSpan.FromSeconds(3));

        Assert.False(result.IsReachable);
        Assert.NotNull(result.Error);
        Assert.Null(result.Latency);
    }

    /// <summary>Возвращает свободный TCP-порт (занят порт сразу освобождается).</summary>
    private static int GetFreePort()
    {
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        try
        {
            return ((IPEndPoint)listener.LocalEndpoint).Port;
        }
        finally
        {
            listener.Stop();
        }
    }
}