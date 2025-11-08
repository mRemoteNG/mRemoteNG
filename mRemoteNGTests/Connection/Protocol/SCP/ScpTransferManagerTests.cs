using System;
using System.Reflection;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.SCP;
using NUnit.Framework;

namespace mRemoteNGTests.Connection.Protocol.SCP
{
    [TestFixture]
    public class ScpTransferManagerTests
    {
        #region Mount Point Detection Tests

        [Test]
        public void IsMountPoint_NullPath_ReturnsFalse()
        {
            // Arrange
            var connectionInfo = CreateConnectionInfo();
            var manager = new ScpTransferManager(connectionInfo);

            // Act
            var result = manager.IsMountPoint(null);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsMountPoint_EmptyPath_ReturnsFalse()
        {
            // Arrange
            var connectionInfo = CreateConnectionInfo();
            var manager = new ScpTransferManager(connectionInfo);

            // Act
            var result = manager.IsMountPoint("");

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsMountPoint_WhitespacePath_ReturnsFalse()
        {
            // Arrange
            var connectionInfo = CreateConnectionInfo();
            var manager = new ScpTransferManager(connectionInfo);

            // Act
            var result = manager.IsMountPoint("   ");

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsMountPoint_BeforeDetection_ReturnsFalse()
        {
            // Arrange
            var connectionInfo = CreateConnectionInfo();
            var manager = new ScpTransferManager(connectionInfo);

            // Act - Before calling DetectMountPoints(), _mountPoints is null
            var result = manager.IsMountPoint("/");

            // Assert
            Assert.That(result, Is.False,
                "IsMountPoint should return false before mount points are detected");
        }

        [Test]
        public void IsMountPoint_RootPath_ReturnsTrueAfterAdding()
        {
            // Arrange
            var connectionInfo = CreateConnectionInfo();
            var manager = new ScpTransferManager(connectionInfo);
            AddMountPoint(manager, "/");

            // Act
            var result = manager.IsMountPoint("/");

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsMountPoint_PathWithTrailingSlash_MatchesWithoutSlash()
        {
            // Arrange
            var connectionInfo = CreateConnectionInfo();
            var manager = new ScpTransferManager(connectionInfo);
            AddMountPoint(manager, "/home");

            // Act
            var result = manager.IsMountPoint("/home/");

            // Assert
            Assert.That(result, Is.True,
                "Trailing slashes should be normalized when checking mount points");
        }

        [Test]
        public void IsMountPoint_CommonLinuxMountPoints_Work()
        {
            // Arrange
            var connectionInfo = CreateConnectionInfo();
            var manager = new ScpTransferManager(connectionInfo);

            // Simulate common Linux mount points
            AddMountPoint(manager, "/");
            AddMountPoint(manager, "/home");
            AddMountPoint(manager, "/boot");
            AddMountPoint(manager, "/mnt/data");
            AddMountPoint(manager, "/var");

            // Assert
            Assert.That(manager.IsMountPoint("/"), Is.True);
            Assert.That(manager.IsMountPoint("/home"), Is.True);
            Assert.That(manager.IsMountPoint("/boot"), Is.True);
            Assert.That(manager.IsMountPoint("/mnt/data"), Is.True);
            Assert.That(manager.IsMountPoint("/var"), Is.True);
            Assert.That(manager.IsMountPoint("/tmp"), Is.False,
                "/tmp should not be a mount point if not added");
        }

        [Test]
        public void IsMountPoint_NonMountPoint_ReturnsFalse()
        {
            // Arrange
            var connectionInfo = CreateConnectionInfo();
            var manager = new ScpTransferManager(connectionInfo);
            AddMountPoint(manager, "/");
            AddMountPoint(manager, "/home");

            // Act & Assert
            Assert.That(manager.IsMountPoint("/home/user"), Is.False,
                "Subdirectories of mount points are not themselves mount points");
            Assert.That(manager.IsMountPoint("/etc"), Is.False,
                "Paths not in the mount point list should return false");
        }

        #endregion

        #region Octal Escape Sequence Tests

        [Test]
        public void UnescapeOctalSequences_Space_Unescaped()
        {
            // Arrange
            var input = "/mnt/my\\040folder";
            var expected = "/mnt/my folder";

            // Act
            var result = InvokeUnescapeOctalSequences(input);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void UnescapeOctalSequences_Tab_Unescaped()
        {
            // Arrange
            var input = "/mnt/folder\\011with\\011tabs";
            var expected = "/mnt/folder\twith\ttabs";

            // Act
            var result = InvokeUnescapeOctalSequences(input);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void UnescapeOctalSequences_Newline_Unescaped()
        {
            // Arrange
            var input = "line1\\012line2";
            var expected = "line1\nline2";

            // Act
            var result = InvokeUnescapeOctalSequences(input);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void UnescapeOctalSequences_Backslash_Unescaped()
        {
            // Arrange
            var input = "/mnt/path\\134with\\134backslashes";
            var expected = "/mnt/path\\with\\backslashes";

            // Act
            var result = InvokeUnescapeOctalSequences(input);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void UnescapeOctalSequences_MultipleEscapes_AllUnescaped()
        {
            // Arrange
            var input = "/mnt/my\\040shared\\040folder\\011test";
            var expected = "/mnt/my shared folder\ttest";

            // Act
            var result = InvokeUnescapeOctalSequences(input);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void UnescapeOctalSequences_NoEscapes_ReturnsUnchanged()
        {
            // Arrange
            var input = "/mnt/normalfolder";

            // Act
            var result = InvokeUnescapeOctalSequences(input);

            // Assert
            Assert.That(result, Is.EqualTo(input),
                "Paths without escape sequences should remain unchanged");
        }

        [Test]
        public void UnescapeOctalSequences_NullInput_ReturnsNull()
        {
            // Act
            var result = InvokeUnescapeOctalSequences(null);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void UnescapeOctalSequences_EmptyInput_ReturnsEmpty()
        {
            // Act
            var result = InvokeUnescapeOctalSequences("");

            // Assert
            Assert.That(result, Is.EqualTo(""));
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Creates a minimal ConnectionInfo for testing.
        /// </summary>
        private ConnectionInfo CreateConnectionInfo()
        {
            return new ConnectionInfo
            {
                Hostname = "test-host",
                Username = "testuser",
                Password = "testpass",
                Protocol = ProtocolType.SCP_DotNet
            };
        }

        /// <summary>
        /// Uses reflection to add a mount point to the internal _mountPoints HashSet.
        /// This simulates what would happen after DetectMountPoints() is called.
        /// </summary>
        private void AddMountPoint(ScpTransferManager manager, string mountPoint)
        {
            // Use reflection to access the private _mountPoints field
            var type = manager.GetType();
            var field = type.GetField("_mountPoints", BindingFlags.NonPublic | BindingFlags.Instance);

            if (field == null)
            {
                throw new InvalidOperationException("Could not find _mountPoints field");
            }

            var mountPoints = field.GetValue(manager) as System.Collections.Generic.HashSet<string>;
            if (mountPoints == null)
            {
                mountPoints = new System.Collections.Generic.HashSet<string>();
                field.SetValue(manager, mountPoints);
            }

            mountPoints.Add(mountPoint);
        }

        /// <summary>
        /// Uses reflection to invoke the private static UnescapeOctalSequences method.
        /// </summary>
        private string InvokeUnescapeOctalSequences(string input)
        {
            var type = typeof(ScpTransferManager);
            var method = type.GetMethod("UnescapeOctalSequences",
                BindingFlags.NonPublic | BindingFlags.Static);

            if (method == null)
            {
                throw new InvalidOperationException("Could not find UnescapeOctalSequences method");
            }

            return (string)method.Invoke(null, new object[] { input });
        }

        #endregion
    }
}
