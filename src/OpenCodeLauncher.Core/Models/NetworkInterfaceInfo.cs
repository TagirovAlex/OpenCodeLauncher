namespace OpenCodeLauncher.Core.Models;

/// <summary>Сетевой интерфейс с IPv4-адресом для выбора при запуске сервера.</summary>
public sealed record NetworkInterfaceInfo(
    string InterfaceName,
    string? InterfaceDescription,
    string IpAddress,
    bool IsLoopback,
    bool IsTailscale,
    bool IsUp);