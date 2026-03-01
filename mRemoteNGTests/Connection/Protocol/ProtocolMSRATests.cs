using System;
using System.Reflection;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol.MSRA;
using NUnit.Framework;

namespace mRemoteNGTests.Connection.Protocol
{
    [TestFixture]
    public class ProtocolMSRATests
    {
        private ProtocolMSRA _protocolMsra;
        private ConnectionInfo _connectionInfo;

        [SetUp]
        public void Setup()
        {
            _connectionInfo = new ConnectionInfo();
            // We need to set a dummy hostname so the constructor or initialization doesn't fail if it checks
            _connectionInfo.Hostname = "localhost";
            _protocolMsra = new ProtocolMSRA(_connectionInfo);
        }

        [TearDown]
        public void Teardown()
        {
            _protocolMsra?.Close();
            _protocolMsra = null;
            _connectionInfo = null;
        }

        #region IsValidHostname Tests

        [Test]
        public void IsValidHostname_ValidHostname_ReturnsTrue()
        {
            Assert.That(InvokeIsValidHostname("localhost"), Is.True);
            Assert.That(InvokeIsValidHostname("server-01"), Is.True);
            Assert.That(InvokeIsValidHostname("192.168.1.1"), Is.True);
            Assert.That(InvokeIsValidHostname("server.domain.local"), Is.True);
            Assert.That(InvokeIsValidHostname("server_01"), Is.True);
        }

        [Test]
        public void IsValidHostname_InvalidCharacters_ReturnsFalse()
        {
            Assert.That(InvokeIsValidHostname("server;cmd"), Is.False);
            Assert.That(InvokeIsValidHostname("server&cmd"), Is.False);
            Assert.That(InvokeIsValidHostname("server|cmd"), Is.False);
            Assert.That(InvokeIsValidHostname("server>out"), Is.False);
            Assert.That(InvokeIsValidHostname("server<in"), Is.False);
            Assert.That(InvokeIsValidHostname("server space"), Is.False);
            Assert.That(InvokeIsValidHostname("server\"quote"), Is.False);
            Assert.That(InvokeIsValidHostname("server'quote"), Is.False);
        }

        [Test]
        public void IsValidHostname_EmptyOrNull_ReturnsFalse()
        {
            Assert.That(InvokeIsValidHostname(""), Is.False);
            Assert.That(InvokeIsValidHostname(null), Is.False);
            Assert.That(InvokeIsValidHostname("   "), Is.False);
        }

        #endregion

        #region Helper Methods

        private static bool InvokeIsValidHostname(string hostname)
        {
            var method = typeof(ProtocolMSRA).GetMethod("IsValidHostname",
                BindingFlags.NonPublic | BindingFlags.Static);

            if (method == null)
            {
                throw new Exception("IsValidHostname method not found.");
            }

            return (bool)method.Invoke(null, new object[] { hostname });
        }

        #endregion
    }
}
