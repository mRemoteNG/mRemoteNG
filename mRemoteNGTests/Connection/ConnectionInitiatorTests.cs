using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Messages;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.Connection
{
    [TestFixture]
    public class ConnectionInitiatorTests
    {
        private ConnectionInitiator _connectionInitiator;
        private MessageCollector _messageCollector;

        [SetUp]
        public void Setup()
        {
            _connectionInitiator = new ConnectionInitiator();
            _messageCollector = Runtime.MessageCollector;
            _messageCollector.ClearMessages();
        }

        [TearDown]
        public void Teardown()
        {
            _messageCollector?.ClearMessages();
        }

        [Test]
        public void OpenConnection_WithEmptyHostname_AddsWarningMessage()
        {
            // Arrange
            var connectionInfo = new ConnectionInfo
            {
                Name = "Test Connection",
                Hostname = "", // Empty hostname
                Protocol = ProtocolType.RDP // RDP doesn't support blank hostname
            };

            // Act
            _connectionInitiator.OpenConnection(connectionInfo);

            // Assert - poll for message with timeout
            // ConnectionInitiator reports a missing hostname as a warning.
            var foundMessage = WaitForMessage(MessageClass.WarningMsg, timeoutMs: 1000);
            Assert.That(foundMessage, Is.Not.Null, "Expected a warning to be added");
            Assert.That(foundMessage.Text, Is.EqualTo("Cannot open connection: No hostname specified!"));
        }

        [Test]
        public void OpenConnection_WithNullHostname_AddsWarningMessage()
        {
            // Arrange
            var connectionInfo = new ConnectionInfo
            {
                Name = "Test Connection",
                Hostname = null, // Null hostname
                Protocol = ProtocolType.SSH2 // SSH doesn't support blank hostname
            };

            // Act
            _connectionInitiator.OpenConnection(connectionInfo);

            // Assert - poll for message with timeout
            // ConnectionInitiator reports a missing hostname as a warning.
            var foundMessage = WaitForMessage(MessageClass.WarningMsg, timeoutMs: 1000);
            Assert.That(foundMessage, Is.Not.Null, "Expected a warning to be added");
            Assert.That(foundMessage.Text, Is.EqualTo("Cannot open connection: No hostname specified!"));
        }

        [Test]
        public void OpenConnection_WithValidHostname_DoesNotAddHostnameError()
        {
            // Arrange
            var connectionInfo = new ConnectionInfo
            {
                Name = "Test Connection",
                // TEST-NET-1 (RFC 5737), reserved for documentation: the RDP connection the test
                // opens can't reach a real device, such as the router of the network it runs on.
                Hostname = "192.0.2.1",
                Protocol = ProtocolType.RDP
            };

            // Act
            _connectionInitiator.OpenConnection(connectionInfo);

            // Give a moment for any potential async operations
            System.Threading.Thread.Sleep(200);

            // Assert
            var hostnameErrors = _messageCollector.Messages
                .Where(m => m.Text == "Cannot open connection: No hostname specified!")
                .ToList();

            Assert.That(hostnameErrors, Is.Empty, 
                "Should not have hostname error when hostname is provided");
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void ClosingAnAttemptThatNeverConnected_KeepsTheConnectedOneActive()
        {
            var connectionInfo = new ConnectionInfo { Name = "Test Connection", Hostname = "server.example" };
            using var parent = new Panel();
            ProtocolBase connected = NewProtocol(parent, connectionInfo);
            ProtocolBase neverConnected = NewProtocol(parent, connectionInfo);
            // As OpenConnection does once Connect succeeds.
            connectionInfo.OpenConnections.Add(connected);
            ActiveConnectionsList().Add(connectionInfo.ConstantID);

            RaiseClosed(neverConnected);

            Assert.That(_connectionInitiator.ActiveConnections, Does.Contain(connectionInfo.ConstantID));
            Assert.That(connectionInfo.OpenConnections.Count, Is.EqualTo(1));

            RaiseClosed(connected);

            Assert.That(_connectionInitiator.ActiveConnections, Does.Not.Contain(connectionInfo.ConstantID));
            Assert.That(connectionInfo.OpenConnections.Count, Is.Zero);
        }

        private static ProtocolBase NewProtocol(Control parent, ConnectionInfo connectionInfo)
        {
            ProtocolBase protocol = Substitute.For<ProtocolBase>();
            protocol.InterfaceControl = new InterfaceControl(parent, protocol, connectionInfo) { OriginalInfo = connectionInfo };
            return protocol;
        }

        private List<string> ActiveConnectionsList() =>
            (List<string>)typeof(ConnectionInitiator)
                .GetField("_activeConnections", BindingFlags.NonPublic | BindingFlags.Instance)!
                .GetValue(_connectionInitiator);

        // What Close does from its background thread
        private void RaiseClosed(ProtocolBase protocol) =>
            typeof(ConnectionInitiator)
                .GetMethod("Prot_Event_Closed", BindingFlags.NonPublic | BindingFlags.Instance)!
                .Invoke(_connectionInitiator, [protocol]);

        /// <summary>
        /// Polls the message collector for a message of the specified class
        /// </summary>
        private IMessage WaitForMessage(MessageClass messageClass, int timeoutMs = 1000)
        {
            var startTime = DateTime.Now;
            while ((DateTime.Now - startTime).TotalMilliseconds < timeoutMs)
            {
                var message = _messageCollector.Messages
                    .FirstOrDefault(m => m.Class == messageClass);
                
                if (message != null)
                    return message;

                System.Threading.Thread.Sleep(50); // Poll every 50ms
            }
            return null;
        }
    }
}
