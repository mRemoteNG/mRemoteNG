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
        public void OpenConnection_WithEmptyHostname_UsesNameAsHostname()
        {
            // Arrange
            var connectionInfo = new ConnectionInfo
            {
                Name = "Test Connection",
                Hostname = "", // Empty hostname — should fall back to Name
                Protocol = ProtocolType.RDP
            };

            // Act
            _connectionInitiator.OpenConnection(connectionInfo);

            // Assert - should get an info message about using name as hostname, not a warning
            var foundMessage = WaitForMessage(MessageClass.InformationMsg, timeoutMs: 1000);
            Assert.That(foundMessage, Is.Not.Null, "Expected an info message about using name as hostname");
            Assert.That(foundMessage.Text, Does.Contain("using connection name as hostname"));
        }

        [Test]
        public void OpenConnection_WithNullHostname_UsesNameAsHostname()
        {
            // Arrange
            var connectionInfo = new ConnectionInfo
            {
                Name = "Test Connection",
                Hostname = null, // Null hostname — should fall back to Name
                Protocol = ProtocolType.SSH2
            };

            // Act
            _connectionInitiator.OpenConnection(connectionInfo);

            // Assert - should get an info message about using name as hostname, not a warning
            var foundMessage = WaitForMessage(MessageClass.InformationMsg, timeoutMs: 1000);
            Assert.That(foundMessage, Is.Not.Null, "Expected an info message about using name as hostname");
            Assert.That(foundMessage.Text, Does.Contain("using connection name as hostname"));
        }

        [Test]
        public void OpenConnection_WithEmptyHostnameAndEmptyName_AddsErrorMessage()
        {
            // Arrange
            var connectionInfo = new ConnectionInfo
            {
                Name = "",
                Hostname = "",
                Protocol = ProtocolType.RDP
            };

            // Act
            _connectionInitiator.OpenConnection(connectionInfo);

            // Assert - no name to fall back to, so should get the warning
            var foundMessage = WaitForMessage(MessageClass.WarningMsg, timeoutMs: 1000);
            var expectedNoHostnameText = GetNoHostnameMessage();
            Assert.That(foundMessage, Is.Not.Null, "Expected an error message to be added");
            Assert.That(expectedNoHostnameText, Is.Not.Null.And.Not.Empty, "Could not resolve expected resource text");
            Assert.That(foundMessage.Text, Is.EqualTo(expectedNoHostnameText));
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
            var expectedNoHostnameText = GetNoHostnameMessage();
            Assert.That(expectedNoHostnameText, Is.Not.Null.And.Not.Empty, "Could not resolve expected resource text");

            var hostnameErrors = _messageCollector.Messages
                .Where(m => m.Text == expectedNoHostnameText)
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

        /// <summary>
        /// Reads the internal localized resource value via reflection.
        /// This avoids requiring direct access to an internal resource class.
        /// </summary>
        private static string GetNoHostnameMessage()
        {
            var resourceType = typeof(ConnectionInitiator).Assembly.GetType("mRemoteNG.Resources.Language.Language");
            var property = resourceType?.GetProperty(
                "ConnectionOpenFailedNoHostname",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);

            return property?.GetValue(null) as string;
        }
    }
}
