using System;
using mRemoteNG.Connection.Protocol.VNC;
using NUnit.Framework;

namespace mRemoteNGTests.Connection.Protocol;

public class VncStallWatchdogTests
{
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(30);
    private TimeSpan _now;
    private VncStallWatchdog _watchdog;

    [SetUp]
    public void Setup()
    {
        _now = TimeSpan.FromMinutes(5);
        _watchdog = new VncStallWatchdog(() => _now, Timeout, maxReconnects: 2);
    }

    private void Advance(double seconds) => _now += TimeSpan.FromSeconds(seconds);

    [Test]
    public void Check_BeforeStart_DoesNothing()
    {
        Advance(600);
        Assert.That(_watchdog.Check(), Is.EqualTo(VncStallAction.None));
    }

    [Test]
    public void Check_JustUnderTimeout_DoesNothing()
    {
        _watchdog.Start();
        Advance(29.9);
        Assert.That(_watchdog.Check(), Is.EqualTo(VncStallAction.None));
    }

    [Test]
    public void Check_AtTimeoutWithoutFrame_AsksForReconnect()
    {
        _watchdog.Start();
        Advance(30);
        Assert.That(_watchdog.Check(), Is.EqualTo(VncStallAction.Reconnect));
    }

    [Test]
    public void FrameReceived_RestartsTheTimeout()
    {
        _watchdog.Start();
        Advance(25);
        _watchdog.FrameReceived();
        Advance(25);
        Assert.That(_watchdog.Check(), Is.EqualTo(VncStallAction.None));
        Advance(5);
        Assert.That(_watchdog.Check(), Is.EqualTo(VncStallAction.Reconnect));
    }

    [Test]
    public void Reconnect_DisarmsUntilTheNewConnectionStarts()
    {
        _watchdog.Start();
        Advance(30);
        Assert.That(_watchdog.Check(), Is.EqualTo(VncStallAction.Reconnect));
        Assert.That(_watchdog.IsArmed, Is.False);

        // A slow reconnect must not be reported again while it is still under way.
        Advance(120);
        Assert.That(_watchdog.Check(), Is.EqualTo(VncStallAction.None));

        _watchdog.Start();
        Advance(29);
        Assert.That(_watchdog.Check(), Is.EqualTo(VncStallAction.None));
    }

    [Test]
    public void ReconnectsWithoutAnyFrame_GiveUpAtTheLimit()
    {
        for (var i = 0; i < 2; i++)
        {
            _watchdog.Start();
            Advance(30);
            Assert.That(_watchdog.Check(), Is.EqualTo(VncStallAction.Reconnect), $"reconnect {i + 1}");
        }

        _watchdog.Start();
        Advance(30);
        Assert.That(_watchdog.Check(), Is.EqualTo(VncStallAction.GiveUp));
        Assert.That(_watchdog.IsArmed, Is.False);

        Advance(30);
        Assert.That(_watchdog.Check(), Is.EqualTo(VncStallAction.None));
    }

    [Test]
    public void FrameAfterReconnect_ResetsTheReconnectLimit()
    {
        for (var i = 0; i < 2; i++)
        {
            _watchdog.Start();
            Advance(30);
            Assert.That(_watchdog.Check(), Is.EqualTo(VncStallAction.Reconnect));
        }

        _watchdog.Start();
        _watchdog.FrameReceived();
        Advance(30);
        Assert.That(_watchdog.Check(), Is.EqualTo(VncStallAction.Reconnect));
    }

    [Test]
    public void Stop_PreventsFurtherActions()
    {
        _watchdog.Start();
        _watchdog.Stop();
        Advance(300);
        Assert.That(_watchdog.Check(), Is.EqualTo(VncStallAction.None));
    }

    [Test]
    public void Constructor_RejectsInvalidArguments()
    {
        Assert.Throws<ArgumentNullException>(() => new VncStallWatchdog(null, Timeout, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => new VncStallWatchdog(() => _now, TimeSpan.Zero, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => new VncStallWatchdog(() => _now, Timeout, -1));
    }
}
