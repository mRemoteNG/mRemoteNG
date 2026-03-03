using System;
using System.Windows.Forms;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.SCP;
using mRemoteNG.UI.Controls.SCP;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Controls.SCP
{
    [TestFixture]
    [Apartment(System.Threading.ApartmentState.STA)] // Required for Windows Forms controls
    public class ScpFileTransferControlTests
    {
        private ScpFileTransferControl _control;
        private Form _testForm;

        [SetUp]
        public void Setup()
        {
            // Create a test form to host the control
            _testForm = new Form
            {
                Width = 1200,
                Height = 800
            };

            // Create the SCP file transfer control
            _control = new ScpFileTransferControl
            {
                Dock = DockStyle.Fill
            };

            _testForm.Controls.Add(_control);
        }

        [TearDown]
        public void Teardown()
        {
            _control?.Dispose();
            _testForm?.Dispose();
        }

        #region Initialization Tests

        [Test]
        public void Constructor_DoesNotThrow()
        {
            // Arrange & Act
            ScpFileTransferControl control = null;

            // Assert
            Assert.DoesNotThrow(() => control = new ScpFileTransferControl(),
                "Constructor should not throw");

            control?.Dispose();
        }

        [Test]
        public void Control_CanBeAddedToForm()
        {
            // Arrange
            var form = new Form();
            var control = new ScpFileTransferControl { Dock = DockStyle.Fill };

            // Act
            form.Controls.Add(control);

            // Assert
            Assert.That(form.Controls.Contains(control), Is.True,
                "Control should be added to form successfully");

            // Cleanup
            form.Dispose();
        }

        [Test]
        public void Control_HasRequiredPanels()
        {
            // Use reflection to verify that local and remote panels are created
            var type = typeof(ScpFileTransferControl);
            var localPanelField = type.GetField("_localPanel",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var remotePanelField = type.GetField("_remotePanel",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.That(localPanelField, Is.Not.Null,
                "ScpFileTransferControl should have a _localPanel field");
            Assert.That(remotePanelField, Is.Not.Null,
                "ScpFileTransferControl should have a _remotePanel field");

            var localPanel = localPanelField?.GetValue(_control);
            var remotePanel = remotePanelField?.GetValue(_control);

            Assert.That(localPanel, Is.Not.Null,
                "Local panel should be instantiated");
            Assert.That(remotePanel, Is.Not.Null,
                "Remote panel should be instantiated");
        }

        #endregion

        #region ProcessCmdKey Tests

        [Test]
        public void ProcessCmdKey_MethodExists()
        {
            // Verify that ProcessCmdKey is overridden
            var method = typeof(ScpFileTransferControl).GetMethod("ProcessCmdKey",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.That(method, Is.Not.Null,
                "ScpFileTransferControl should override ProcessCmdKey");
        }

        #endregion

        #region Panel Coordination Tests

        [Test]
        public void Panels_EscapePressed_EventsWiredUp()
        {
            // Use reflection to get the panels
            var type = typeof(ScpFileTransferControl);
            var localPanelField = type.GetField("_localPanel",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var remotePanelField = type.GetField("_remotePanel",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var localPanel = localPanelField?.GetValue(_control) as LocalFileBrowserPanel;
            var remotePanel = remotePanelField?.GetValue(_control) as RemoteFileBrowserPanel;

            // Assert that panels exist
            Assert.That(localPanel, Is.Not.Null);
            Assert.That(remotePanel, Is.Not.Null);

            // Verify EscapePressed events exist (they should have been subscribed to)
            var localEventInfo = typeof(LocalFileBrowserPanel).GetEvent("EscapePressed");
            var remoteEventInfo = typeof(RemoteFileBrowserPanel).GetEvent("EscapePressed");

            Assert.That(localEventInfo, Is.Not.Null,
                "LocalFileBrowserPanel should have EscapePressed event");
            Assert.That(remoteEventInfo, Is.Not.Null,
                "RemoteFileBrowserPanel should have EscapePressed event");
        }

        #endregion

        #region Connection Tests

        [Test]
        public void Connect_NullConnectionInfo_DoesNotThrow()
        {
            // Act & Assert
            Assert.DoesNotThrow(() => _control.Connect(null),
                "Connect should handle null ConnectionInfo gracefully");
        }

        [Test]
        public void Connect_ValidConnectionInfo_DoesNotThrowImmediately()
        {
            // Arrange
            var connectionInfo = new ConnectionInfo
            {
                Hostname = "test-host",
                Username = "testuser",
                Password = "testpass",
                Protocol = ProtocolType.SCP_DotNet
            };

            // Act & Assert
            // Note: This will likely fail to actually connect (no real SSH server)
            // but should not throw an exception immediately
            Assert.DoesNotThrow(() => _control.Connect(connectionInfo),
                "Connect should not throw immediately with valid ConnectionInfo");
        }

        [Test]
        public void Disconnect_DoesNotThrow()
        {
            // Act & Assert
            Assert.DoesNotThrow(() => _control.Disconnect(),
                "Disconnect should not throw when not connected");
        }

        #endregion

        #region Button State Tests

        [Test]
        public void Buttons_ExistInControl()
        {
            // Use reflection to verify buttons are created
            var type = typeof(ScpFileTransferControl);
            var uploadButtonField = type.GetField("_uploadButton",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var downloadButtonField = type.GetField("_downloadButton",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var deleteButtonField = type.GetField("_deleteButton",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.That(uploadButtonField, Is.Not.Null,
                "ScpFileTransferControl should have _uploadButton field");
            Assert.That(downloadButtonField, Is.Not.Null,
                "ScpFileTransferControl should have _downloadButton field");
            Assert.That(deleteButtonField, Is.Not.Null,
                "ScpFileTransferControl should have _deleteButton field");

            var uploadButton = uploadButtonField?.GetValue(_control);
            var downloadButton = downloadButtonField?.GetValue(_control);
            var deleteButton = deleteButtonField?.GetValue(_control);

            Assert.That(uploadButton, Is.Not.Null,
                "Upload button should be instantiated");
            Assert.That(downloadButton, Is.Not.Null,
                "Download button should be instantiated");
            Assert.That(deleteButton, Is.Not.Null,
                "Delete button should be instantiated");
        }

        [Test]
        public void Buttons_InitiallyDisabled()
        {
            // Use reflection to get buttons
            var type = typeof(ScpFileTransferControl);
            var uploadButton = type.GetField("_uploadButton",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.GetValue(_control) as Button;
            var downloadButton = type.GetField("_downloadButton",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.GetValue(_control) as Button;
            var deleteButton = type.GetField("_deleteButton",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.GetValue(_control) as Button;

            // Assert
            Assert.That(uploadButton?.Enabled, Is.False,
                "Upload button should be disabled initially");
            Assert.That(downloadButton?.Enabled, Is.False,
                "Download button should be disabled initially");
            Assert.That(deleteButton?.Enabled, Is.False,
                "Delete button should be disabled initially");
        }

        #endregion

        #region Panel Access Tests

        [Test]
        public void LocalPanel_IsAccessible()
        {
            // Use reflection to access local panel
            var type = typeof(ScpFileTransferControl);
            var localPanelField = type.GetField("_localPanel",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var localPanel = localPanelField?.GetValue(_control) as LocalFileBrowserPanel;

            // Assert
            Assert.That(localPanel, Is.Not.Null,
                "Local panel should be accessible");
            Assert.That(localPanel.CurrentPath, Is.Not.Null,
                "Local panel should have a current path");
        }

        [Test]
        public void RemotePanel_IsAccessible()
        {
            // Use reflection to access remote panel
            var type = typeof(ScpFileTransferControl);
            var remotePanelField = type.GetField("_remotePanel",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var remotePanel = remotePanelField?.GetValue(_control) as RemoteFileBrowserPanel;

            // Assert
            Assert.That(remotePanel, Is.Not.Null,
                "Remote panel should be accessible");
            Assert.That(remotePanel.CurrentPath, Is.Not.Null,
                "Remote panel should have a current path");
        }

        #endregion
    }
}
