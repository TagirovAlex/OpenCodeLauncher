using System.Net;
using System.Net.Sockets;
using OpenCodeLauncher.Core.Models;
using OpenCodeLauncher.Core.Services;

namespace OpenCodeLauncher.Tests;

/// <summary>
/// Сквозной тест жизненного цикла: реальный запуск opencode-сервера через ProcessService,
/// проверка доступности через HealthService, остановка. Пропускается, если opencode не установлен.
/// </summary>
public class OpenCodeSmokeTests
{
    [Fact]
    public async Task Start_Reachability_Stop_Lifecycle()
    {
        if (ProcessService.TryLocateOpenCode() is null)
        {
            return; // opencode не установлен — тест не применим
        }

        var projectDir = Path.Combine(Path.GetFullPath("."), "temp", "smoke-project");
        Directory.CreateDirectory(projectDir);

        var service = new ProcessService();
        try
        {
            var health = new HealthService();
            var port = GetFreePort();
            var parameters = new LaunchParams(projectDir, "127.0.0.1", port);

            service.Start(parameters);
            Assert.Equal(ServerStatus.Starting, service.Status);

            // Ждём доступности сервера до 15 секунд.
            HealthCheckResult? result = null;
            var deadline = DateTime.UtcNow.AddSeconds(15);
            while (DateTime.UtcNow < deadline)
            {
                result = await health.CheckAsync($"http://127.0.0.1:{port}/", TimeSpan.FromSeconds(2));
                if (result.IsReachable)
                {
                    break;
                }

                await Task.Delay(500);
            }

            Assert.True(result?.IsReachable ?? false, "Сервер не стал доступен: " + result?.Error);

            service.MarkRunning();
            Assert.Equal(ServerStatus.Running, service.Status);

            await service.StopAsync();
            Assert.Equal(ServerStatus.Stopped, service.Status);
        }
        finally
        {
            service.Dispose();
            Directory.Delete(projectDir, true);
        }
    }

    private static int GetFreePort()
    {
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        var port = ((IPEndPoint)listener.LocalEndpoint).Port;
        listener.Stop();
        return port;
    }
}