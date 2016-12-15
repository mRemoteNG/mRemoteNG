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
        [SetUp]
        public void Setup()
        {
            GeneralAppInfo.ApplicationVersion = "1.0.0.0";
        }

        [Test]
        public void TestStableChannel()
        {
        }
    }
}
