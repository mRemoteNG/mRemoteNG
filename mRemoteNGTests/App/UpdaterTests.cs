using System;
using mRemoteNG.App.Info;
using mRemoteNG.App.Update;
using mRemoteNGTests.Properties;
using NUnit.Framework;

namespace mRemoteNGTests.App;

[TestFixture]
public class UpdaterTests
{
    [Test]
    public void UpdateStableChannel()
    {
        GeneralAppInfo.ApplicationVersion = "1.0.0.0";
        var CurrentUpdateInfo = UpdateInfo.FromString(Resources.update);
        Assert.That(CurrentUpdateInfo.CheckIfValid(), Is.True);
        Version v;
        Version.TryParse(GeneralAppInfo.ApplicationVersion, out v);
        var IsNewer = CurrentUpdateInfo.Version > v;
        Assert.That(IsNewer, Is.True);
    }

    [Test]
    public void UpdateBetaChannel()
    {
        GeneralAppInfo.ApplicationVersion = "1.0.0.0";
        var CurrentUpdateInfo = UpdateInfo.FromString(Resources.beta_update);
        Assert.That(CurrentUpdateInfo.CheckIfValid(), Is.True);
        Version v;
        Version.TryParse(GeneralAppInfo.ApplicationVersion, out v);
        var IsNewer = CurrentUpdateInfo.Version > v;
        Assert.That(IsNewer, Is.True);
    }

    [Test]
    public void UpdateDevChannel()
    {
        GeneralAppInfo.ApplicationVersion = "1.0.0.0";
        var CurrentUpdateInfo = UpdateInfo.FromString(Resources.dev_update);
        Assert.That(CurrentUpdateInfo.CheckIfValid(), Is.True);
        Version v;
        Version.TryParse(GeneralAppInfo.ApplicationVersion, out v);
        var IsNewer = CurrentUpdateInfo.Version > v;
        Assert.That(IsNewer, Is.True);
    }

    [Test]
    public void UpdateStablePortableChannel()
    {
        GeneralAppInfo.ApplicationVersion = "1.0.0.0";
        var CurrentUpdateInfo = UpdateInfo.FromString(Resources.update_portable);
        Assert.That(CurrentUpdateInfo.CheckIfValid(), Is.True);
        Version v;
        Version.TryParse(GeneralAppInfo.ApplicationVersion, out v);
        var IsNewer = CurrentUpdateInfo.Version > v;
        Assert.That(IsNewer, Is.True);
    }

    [Test]
    public void UpdateBetaPortableChannel()
    {
        GeneralAppInfo.ApplicationVersion = "1.0.0.0";
        var CurrentUpdateInfo = UpdateInfo.FromString(Resources.beta_update_portable);
        Assert.That(CurrentUpdateInfo.CheckIfValid(), Is.True);
        Version v;
        Version.TryParse(GeneralAppInfo.ApplicationVersion, out v);
        var IsNewer = CurrentUpdateInfo.Version > v;
        Assert.That(IsNewer, Is.True);
    }

    [Test]
    public void UpdateDevPortableChannel()
    {
        GeneralAppInfo.ApplicationVersion = "1.0.0.0";
        var CurrentUpdateInfo = UpdateInfo.FromString(Resources.dev_update_portable);
        Assert.That(CurrentUpdateInfo.CheckIfValid(), Is.True);
        Version v;
        Version.TryParse(GeneralAppInfo.ApplicationVersion, out v);
        var IsNewer = CurrentUpdateInfo.Version > v;
        Assert.That(IsNewer, Is.True);
    }
}