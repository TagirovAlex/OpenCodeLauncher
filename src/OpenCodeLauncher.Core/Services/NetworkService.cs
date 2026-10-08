using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using OpenCodeLauncher.Core.Models;

namespace OpenCodeLauncher.Core.Services;

/// <summary>Получает список сетевых интерфейсов с IPv4-адресами и предпочтительный адрес по умолчанию.</summary>
public sealed class NetworkService : INetworkService
{
    /// <inheritdoc />
    public IReadOnlyList<NetworkInterfaceInfo> GetInterfaces()
    {
        var result = new List<NetworkInterfaceInfo>();

        foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (networkInterface.OperationalStatus != OperationalStatus.Up)
                continue;

            string? ipv4 = null;
            foreach (var unicast in networkInterface.GetIPProperties().UnicastAddresses)
            {
                if (unicast.Address.AddressFamily != AddressFamily.InterNetwork)
                    continue;

                if (IsIgnoredAddress(unicast.Address))
                    continue;

                ipv4 = unicast.Address.ToString();
                break;
            }

            if (ipv4 is null)
                continue;

            var ip = IPAddress.Parse(ipv4);
            var isLoopback = IPAddress.IsLoopback(ip);
            var isTailscale = IsTailscaleAddress(ip) || IsTailscaleInterfaceName(networkInterface.Name);

            result.Add(new NetworkInterfaceInfo(
                networkInterface.Name,
                networkInterface.Description,
                ipv4,
                isLoopback,
                isTailscale,
                true));
        }

        return result
            .OrderBy(x => GetSortRank(x))
            .ThenBy(x => x.InterfaceName, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    /// <inheritdoc />
    public string GetDefaultHostname() => GetDefaultHostname(GetInterfaces());

    /// <summary>Версия с подменой списка интерфейсов (используется в тестах).</summary>
    public string GetDefaultHostname(IReadOnlyList<NetworkInterfaceInfo>? interfaces)
    {
        var list = interfaces ?? GetInterfaces();

        return list.FirstOrDefault(x => x.IsTailscale)?.IpAddress
            ?? list.FirstOrDefault(x => !x.IsLoopback)?.IpAddress
            ?? "127.0.0.1";
    }

    /// <summary>Проверяет, относится ли IPv4-адрес к диапазону Tailscale (100.64.0.0/10).</summary>
    public static bool IsTailscaleAddress(IPAddress address)
    {
        if (address.AddressFamily != AddressFamily.InterNetwork)
            return false;

        var bytes = address.GetAddressBytes();
        return bytes[0] == 100 && bytes[1] >= 64 && bytes[1] <= 127;
    }

    /// <summary>Проверяет, содержит ли имя интерфейса признак Tailscale (регистронезависимо).</summary>
    public static bool IsTailscaleInterfaceName(string name)
        => name.Contains("tailscale", StringComparison.OrdinalIgnoreCase);

    /// <summary>Определяет, является ли адрес служебным: 0.0.0.0 или link-local (169.254.0.0/16).</summary>
    private static bool IsIgnoredAddress(IPAddress address)
    {
        if (address.Equals(IPAddress.Any))
            return true;

        var bytes = address.GetAddressBytes();
        return bytes[0] == 169 && bytes[1] == 254;
    }

    /// <summary>Приоритет сортировки: Tailscale, затем non-loopback, затем loopback.</summary>
    private static int GetSortRank(NetworkInterfaceInfo info)
        => info.IsTailscale ? 0 : info.IsLoopback ? 2 : 1;
}