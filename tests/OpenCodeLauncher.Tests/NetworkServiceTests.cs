using System.Net;
using OpenCodeLauncher.Core.Models;
using OpenCodeLauncher.Core.Services;

namespace OpenCodeLauncher.Tests;

/// <summary>Тесты хелперов NetworkService и выбора адреса по умолчанию.</summary>
public class NetworkServiceTests
{
    [Theory]
    [InlineData("100.64.0.1", true)]
    [InlineData("100.127.255.254", true)]
    [InlineData("100.63.255.255", false)]
    [InlineData("100.128.0.1", false)]
    [InlineData("192.168.1.1", false)]
    public void IsTailscaleAddress_Checks100_64_0_0_10Range(string address, bool expected)
    {
        Assert.Equal(expected, NetworkService.IsTailscaleAddress(IPAddress.Parse(address)));
    }

    [Theory]
    [InlineData("Tailscale", true)]
    [InlineData("Ethernet", false)]
    [InlineData("tailscale", true)]
    public void IsTailscaleInterfaceName_DetectsTailscale(string name, bool expected)
    {
        Assert.Equal(expected, NetworkService.IsTailscaleInterfaceName(name));
    }

    [Fact]
    public void GetDefaultHostname_PrefersTailscale()
    {
        var service = new NetworkService();
        var interfaces = new[]
        {
            new NetworkInterfaceInfo("Ethernet", null, "192.168.1.10", false, false, true),
            new NetworkInterfaceInfo("tailscale0", null, "100.64.0.2", false, true, true),
            new NetworkInterfaceInfo("Loopback", null, "127.0.0.1", true, false, true),
        };

        Assert.Equal("100.64.0.2", service.GetDefaultHostname(interfaces));
    }

    [Fact]
    public void GetDefaultHostname_PrefersNonLoopback_WhenNoTailscale()
    {
        var service = new NetworkService();
        var interfaces = new[]
        {
            new NetworkInterfaceInfo("Loopback", null, "127.0.0.1", true, false, true),
            new NetworkInterfaceInfo("Ethernet", null, "192.168.1.10", false, false, true),
        };

        Assert.Equal("192.168.1.10", service.GetDefaultHostname(interfaces));
    }

    [Fact]
    public void GetDefaultHostname_ReturnsLoopback_AsLastResort()
    {
        var service = new NetworkService();
        var interfaces = new[]
        {
            new NetworkInterfaceInfo("Loopback", null, "127.0.0.1", true, false, true),
        };

        Assert.Equal("127.0.0.1", service.GetDefaultHostname(interfaces));
    }

    [Fact]
    public void GetDefaultHostname_Returns127_0_0_1_WhenListEmpty()
    {
        var service = new NetworkService();

        Assert.Equal("127.0.0.1", service.GetDefaultHostname(Array.Empty<NetworkInterfaceInfo>()));
    }
}