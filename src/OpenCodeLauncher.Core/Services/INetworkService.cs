using OpenCodeLauncher.Core.Models;

namespace OpenCodeLauncher.Core.Services;

/// <summary>Предоставляет список доступных сетевых интерфейсов с IPv4-адресами.</summary>
public interface INetworkService
{
    /// <summary>Список интерфейсов, пригодных для запуска сервера (включая Tailscale).</summary>
    IReadOnlyList<NetworkInterfaceInfo> GetInterfaces();

    /// <summary>Предпочтительный адрес по умолчанию: Tailscale, иначе первый внешний, иначе loopback.</summary>
    string GetDefaultHostname();
}