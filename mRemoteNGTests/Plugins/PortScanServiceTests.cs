using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using mRemoteNG.Plugins.PortScan;
using NUnit.Framework;

namespace mRemoteNGTests.Plugins;

[TestFixture]
public class PortScanServiceTests
{
    [Test]
    public void ExpandAddressesUsesUnsignedIpv4Ordering()
    {
        List<IPAddress> addresses = PortScanService.ExpandAddresses(
            IPAddress.Parse("127.255.255.255"),
            IPAddress.Parse("128.0.0.0"));

        Assert.That(addresses, Is.EqualTo(new[]
        {
            IPAddress.Parse("127.255.255.255"),
            IPAddress.Parse("128.0.0.0"),
        }));
    }

    [Test]
    public void ExpandAddressesReconstructsIpv6Addresses()
    {
        List<IPAddress> addresses = PortScanService.ExpandAddresses(
            IPAddress.Parse("2001:db8::fffe"),
            IPAddress.Parse("2001:db8::1:1"));

        Assert.That(addresses.Select(address => address.ToString()), Is.EqualTo(new[]
        {
            "2001:db8::fffe",
            "2001:db8::ffff",
            "2001:db8::1:0",
            "2001:db8::1:1",
        }));
    }

    [Test]
    public void ExpandAddressesNormalizesReversedBounds()
    {
        List<IPAddress> addresses = PortScanService.ExpandAddresses(
            IPAddress.Parse("192.168.1.3"),
            IPAddress.Parse("192.168.1.1"));

        Assert.That(addresses.Select(address => address.ToString()), Is.EqualTo(new[]
        {
            "192.168.1.1",
            "192.168.1.2",
            "192.168.1.3",
        }));
    }

    [Test]
    public void ExpandAddressesAccepts65536Addresses()
    {
        List<IPAddress> addresses = PortScanService.ExpandAddresses(
            IPAddress.Parse("10.0.0.0"),
            IPAddress.Parse("10.0.255.255"));

        Assert.That(addresses, Has.Count.EqualTo(65536));
    }

    [Test]
    public void ExpandAddressesRejectsRangesLargerThan65536Addresses()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => PortScanService.ExpandAddresses(
            IPAddress.Parse("10.0.0.0"),
            IPAddress.Parse("10.1.0.0")));
    }

    [Test]
    public void ExpandAddressesPreservesIpv6ScopeId()
    {
        List<IPAddress> addresses = PortScanService.ExpandAddresses(
            IPAddress.Parse("fe80::1%12"),
            IPAddress.Parse("fe80::2%12"));

        Assert.That(addresses.Select(address => address.ScopeId), Is.EqualTo(new long[] { 12, 12 }));
    }

    [Test]
    public void ExpandAddressesRejectsDifferentIpv6ScopeIds()
    {
        Assert.Throws<ArgumentException>(() => PortScanService.ExpandAddresses(
            IPAddress.Parse("fe80::1%12"),
            IPAddress.Parse("fe80::2%13")));
    }
}
