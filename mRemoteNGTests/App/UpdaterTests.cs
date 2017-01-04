using System;
using mRemoteNG.App.Info;
using mRemoteNG.App.Update;
using NUnit.Framework;

namespace mRemoteNGTests.App
{
    [TestFixture]
    public class UpdaterTests
    {
        [Test]
        public void UpdateStableChannel()
        {
            GeneralAppInfo.ApplicationVersion = "1.0.0.0";
            var _upd = new AppUpdater();
            var e = _upd.DownloadString(UpdateChannelInfo.GetUpdateChannelInfo(UpdateChannelInfo.STABLE));
            Assert.That(e.Cancelled, Is.False);
            Assert.That(e.Error, Is.Null);
            var CurrentUpdateInfo = UpdateInfo.FromString(e.Result);
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
            var _upd = new AppUpdater();
            var e = _upd.DownloadString(UpdateChannelInfo.GetUpdateChannelInfo(UpdateChannelInfo.BETA));
            Assert.That(e.Cancelled, Is.False);
            Assert.That(e.Error, Is.Null);
            var CurrentUpdateInfo = UpdateInfo.FromString(e.Result);
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
            var _upd = new AppUpdater();
            var e = _upd.DownloadString(UpdateChannelInfo.GetUpdateChannelInfo(UpdateChannelInfo.DEV));
            Assert.That(e.Cancelled, Is.False);
            Assert.That(e.Error, Is.Null);
            var CurrentUpdateInfo = UpdateInfo.FromString(e.Result);
            Assert.That(CurrentUpdateInfo.CheckIfValid(), Is.True);
            Version v;
            Version.TryParse(GeneralAppInfo.ApplicationVersion, out v);
            var IsNewer = CurrentUpdateInfo.Version > v;
            Assert.That(IsNewer, Is.True);
        }
    }
}
