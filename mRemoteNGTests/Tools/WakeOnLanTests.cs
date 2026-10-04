using System;
using mRemoteNG.Tools;
using NUnit.Framework;

namespace mRemoteNGTests.Tools;

public class WakeOnLanTests
{
    [TestCase("00:11:22:AA:BB:CC")]
    [TestCase("00-11-22-AA-BB-CC")]
    [TestCase("001122AABBCC")]
    [TestCase("0011.22AA.BBCC")]
    [TestCase("00:11:22:aa:bb:cc")]
    public void ParseMacAddress_AcceptsCommonFormats(string macAddress)
    {
        byte[] expected = { 0x00, 0x11, 0x22, 0xAA, 0xBB, 0xCC };
        Assert.That(WakeOnLan.ParseMacAddress(macAddress), Is.EqualTo(expected));
    }

    [TestCase("")]
    [TestCase(null)]
    [TestCase("00:11:22:AA:BB")]
    [TestCase("00:11:22:AA:BB:CC:DD")]
    [TestCase("00:11:22:AA:BB:ZZ")]
    public void ParseMacAddress_InvalidInput_Throws(string macAddress)
    {
        Assert.That(() => WakeOnLan.ParseMacAddress(macAddress), Throws.ArgumentException);
    }

    [Test]
    public void BuildMagicPacket_HasCorrectLength()
    {
        byte[] mac = { 0x00, 0x11, 0x22, 0xAA, 0xBB, 0xCC };
        Assert.That(WakeOnLan.BuildMagicPacket(mac).Length, Is.EqualTo(102));
    }

    [Test]
    public void BuildMagicPacket_StartsWithSixFfBytes()
    {
        byte[] mac = { 0x00, 0x11, 0x22, 0xAA, 0xBB, 0xCC };
        byte[] packet = WakeOnLan.BuildMagicPacket(mac);
        for (int i = 0; i < 6; i++)
            Assert.That(packet[i], Is.EqualTo(0xFF));
    }

    [Test]
    public void BuildMagicPacket_RepeatsMacSixteenTimes()
    {
        byte[] mac = { 0x00, 0x11, 0x22, 0xAA, 0xBB, 0xCC };
        byte[] packet = WakeOnLan.BuildMagicPacket(mac);
        for (int repeat = 1; repeat <= 16; repeat++)
        {
            for (int b = 0; b < 6; b++)
                Assert.That(packet[(repeat * 6) + b], Is.EqualTo(mac[b]));
        }
    }

    [TestCase(new byte[] { 0x00, 0x11, 0x22 })]
    [TestCase(new byte[] { 0x00, 0x11, 0x22, 0xAA, 0xBB, 0xCC, 0xDD })]
    public void BuildMagicPacket_InvalidMacLength_Throws(byte[] mac)
    {
        Assert.That(() => WakeOnLan.BuildMagicPacket(mac), Throws.ArgumentException);
    }

    [Test]
    public void BuildMagicPacket_NullMac_Throws()
    {
        Assert.That(() => WakeOnLan.BuildMagicPacket(null), Throws.ArgumentException);
    }
}
