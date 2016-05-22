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
            Assert.That(Logger.Instance, Is.InstanceOf<ILog>());
        }

        [Test]
        public void SingletonOnlyEverReturnsTheSameInstance()
        {
            ILog log1 = Logger.Instance;
            ILog log2 = Logger.Instance;
            Assert.That(log1, Is.EqualTo(log2));
        }
    }
}