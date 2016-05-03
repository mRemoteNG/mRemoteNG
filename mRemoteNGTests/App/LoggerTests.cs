using System;
using NUnit.Framework;
using mRemoteNG.App;
using log4net;

namespace mRemoteNGTests.App
{
    [TestFixture]
    public class LoggerTests
    {
        [Test]
        public void GetSingletonInstanceReturnsAnILogObject()
        {
            Assert.That(Logger.GetSingletonInstance(), Is.InstanceOf<ILog>());
        }

        [Test]
        public void SingletonOnlyEverReturnsTheSameInstance()
        {
            ILog log1 = Logger.GetSingletonInstance();
            ILog log2 = Logger.GetSingletonInstance();
            Assert.That(log1, Is.EqualTo(log2));
        }
    }
}