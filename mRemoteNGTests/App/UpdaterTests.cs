using System;
using System.ComponentModel;
using mRemoteNG.App.Info;
using mRemoteNG.App.Update;
using NUnit.Framework;

namespace mRemoteNGTests.App
{
    [TestFixture]
    public class UpdaterTests
    {
        private AppUpdater _appUpdate;

        [SetUp]
        public void Setup()
        {
            GeneralAppInfo.ApplicationVersion = "1.0.0.0";
            _appUpdate = new AppUpdater();
            _appUpdate.GetUpdateInfoCompletedEvent += TestGetUpdateInfoCompleted;

        }

        [Test]
        public void TestStableChannel()
        {
        }

        private void TestGetUpdateInfoCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                _appUpdate.GetUpdateInfoCompletedEvent -= TestGetUpdateInfoCompleted;

                if (_appUpdate.IsUpdateAvailable())
                {
                    
                }
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
