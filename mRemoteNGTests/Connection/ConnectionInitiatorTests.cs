using System;
using System.Linq;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Messages;
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
        public void OpenConnection_WithEmptyHostname_AddsErrorMessage()
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
            var foundMessage = WaitForMessage(MessageClass.ErrorMsg, timeoutMs: 1000);
            Assert.That(foundMessage, Is.Not.Null, "Expected an error message to be added");
            Assert.That(foundMessage.Text, Is.EqualTo("Cannot open connection: No hostname specified!"));
        }

        [Test]
        public void OpenConnection_WithNullHostname_AddsErrorMessage()
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
            var foundMessage = WaitForMessage(MessageClass.ErrorMsg, timeoutMs: 1000);
            Assert.That(foundMessage, Is.Not.Null, "Expected an error message to be added");
            Assert.That(foundMessage.Text, Is.EqualTo("Cannot open connection: No hostname specified!"));
        }

        [Test]
        public void OpenConnection_WithValidHostname_DoesNotAddHostnameError()
        {
            // Arrange
            var connectionInfo = new ConnectionInfo
            {
                Name = "Test Connection",
                Hostname = "192.168.1.1", // Valid hostname
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
