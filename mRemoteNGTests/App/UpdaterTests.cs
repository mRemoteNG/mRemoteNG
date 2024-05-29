using System;
using mRemoteNG.App.Info;
using mRemoteNG.App.Update;
using mRemoteNGTests.Properties;
using NUnit.Framework;

namespace mRemoteNGTests.App;

[TestFixture]
public class UpdaterTests
{
    private readonly Version TestApplicationVersion = new("1.0.0.0");

    [Test]
    public void UpdateStableChannel()
    {
        var CurrentUpdateInfo = UpdateInfo.FromString(Resources.update);
        Assert.That(CurrentUpdateInfo.CheckIfValid(), Is.True);
        bool IsNewer = CurrentUpdateInfo.Version > TestApplicationVersion;
        Assert.That(IsNewer, Is.True);
    }

    [Test]
    public void UpdateBetaChannel()
    {
        var CurrentUpdateInfo = UpdateInfo.FromString(Resources.beta_update);
        Assert.That(CurrentUpdateInfo.CheckIfValid(), Is.True);
        bool IsNewer = CurrentUpdateInfo.Version > TestApplicationVersion;
        Assert.That(IsNewer, Is.True);
    }

    [Test]
    public void UpdateDevChannel()
    {
        var CurrentUpdateInfo = UpdateInfo.FromString(Resources.dev_update);
        Assert.That(CurrentUpdateInfo.CheckIfValid(), Is.True);
        bool IsNewer = CurrentUpdateInfo.Version > TestApplicationVersion;
        Assert.That(IsNewer, Is.True);
    }

    [Test]
    public void UpdateStablePortableChannel()
    {
        var CurrentUpdateInfo = UpdateInfo.FromString(Resources.update_portable);
        Assert.That(CurrentUpdateInfo.CheckIfValid(), Is.True);
        bool IsNewer = CurrentUpdateInfo.Version > TestApplicationVersion;
        Assert.That(IsNewer, Is.True);
    }

    [Test]
    public void UpdateBetaPortableChannel()
    {
        var CurrentUpdateInfo = UpdateInfo.FromString(Resources.beta_update_portable);
        Assert.That(CurrentUpdateInfo.CheckIfValid(), Is.True);
        bool IsNewer = CurrentUpdateInfo.Version > TestApplicationVersion;
        Assert.That(IsNewer, Is.True);
    }

    [Test]
    public void UpdateDevPortableChannel()
    {
        var CurrentUpdateInfo = UpdateInfo.FromString(Resources.dev_update_portable);
        Assert.That(CurrentUpdateInfo.CheckIfValid(), Is.True);
        bool IsNewer = CurrentUpdateInfo.Version > TestApplicationVersion;
        Assert.That(IsNewer, Is.True);
    }
}