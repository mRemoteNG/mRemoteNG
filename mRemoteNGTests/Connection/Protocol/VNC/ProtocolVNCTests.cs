using System;
using System.Linq;
using System.Threading;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Messages;
using NUnit.Framework;

namespace mRemoteNGTests.Connection.Protocol.VNC
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class ProtocolVNCTests
    {
        private ProtocolVNC _vncProtocol;
        private ConnectionInfo _connectionInfo;

        [SetUp]
        public void Setup()
        {
            _connectionInfo = new ConnectionInfo
            {
                Hostname = "test.host.com",
                Port = 5900,
                Protocol = ProtocolType.VNC,
                Password = "testpassword",
                VNCSmartSizeMode = ProtocolVNC.SmartSizeMode.SmartSNo
            };

            _vncProtocol = new ProtocolVNC();
            _vncProtocol.InterfaceControl = new InterfaceControl(null, _vncProtocol, _connectionInfo);

            Runtime.MessageCollector.ClearMessages();
        }

        [TearDown]
        public void Teardown()
        {
            try
            {
                _vncProtocol?.Disconnect();
            }
            catch (Exception)
            {
                // Ignore exceptions during disconnect in teardown
            }
        }

        [Test]
        public void StartChat_AddsInformationMessage()
        {
            ProtocolVNC.StartChat();

            var lastMessage = Runtime.MessageCollector.Messages.LastOrDefault();
            Assert.That(lastMessage, Is.Not.Null);
            Assert.That(lastMessage.Class, Is.EqualTo(MessageClass.InformationMsg));
            Assert.That(lastMessage.Text, Does.Contain("VNC chat is not supported"));
        }

        [Test]
        public void StartFileTransfer_AddsInformationMessage()
        {
            ProtocolVNC.StartFileTransfer();

            var lastMessage = Runtime.MessageCollector.Messages.LastOrDefault();
            Assert.That(lastMessage, Is.Not.Null);
            Assert.That(lastMessage.Class, Is.EqualTo(MessageClass.InformationMsg));
            Assert.That(lastMessage.Text, Does.Contain("VNC file transfer is not supported"));
        }
    }
}
