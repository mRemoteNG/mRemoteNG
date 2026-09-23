using System;
using NUnit.Framework;
using mRemoteNG.Tools;

namespace mRemoteNGTests.Tools
{
    [TestFixture]
    public class SftpFileItemTests
    {
        [Test]
        public void SftpFileItem_DefaultValues_AreCorrect()
        {
            var item = new SftpFileItem();

            Assert.That(item.Name, Is.Null);
            Assert.That(item.FullPath, Is.Null);
            Assert.That(item.IsDirectory, Is.False);
            Assert.That(item.IsSymlink, Is.False);
            Assert.That(item.LinkTarget, Is.Null);
            Assert.That(item.Size, Is.EqualTo(0));
            Assert.That(item.LastModified, Is.EqualTo(DateTime.MinValue));
            Assert.That(item.Permissions, Is.Null);
            Assert.That(item.Owner, Is.Null);
            Assert.That(item.Group, Is.Null);
        }

        [Test]
        public void SftpFileItem_SetProperties_RetainsValues()
        {
            var now = DateTime.UtcNow;
            var item = new SftpFileItem
            {
                Name = "test.txt",
                FullPath = "/home/user/test.txt",
                IsDirectory = false,
                IsSymlink = true,
                LinkTarget = "/tmp/target",
                Size = 1024,
                LastModified = now,
                Permissions = "-rw-r--r--",
                Owner = "1000",
                Group = "1000"
            };

            Assert.That(item.Name, Is.EqualTo("test.txt"));
            Assert.That(item.FullPath, Is.EqualTo("/home/user/test.txt"));
            Assert.That(item.IsDirectory, Is.False);
            Assert.That(item.IsSymlink, Is.True);
            Assert.That(item.LinkTarget, Is.EqualTo("/tmp/target"));
            Assert.That(item.Size, Is.EqualTo(1024));
            Assert.That(item.LastModified, Is.EqualTo(now));
            Assert.That(item.Permissions, Is.EqualTo("-rw-r--r--"));
            Assert.That(item.Owner, Is.EqualTo("1000"));
            Assert.That(item.Group, Is.EqualTo("1000"));
        }

        [Test]
        public void SftpFileItem_Directory_HasExpectedProperties()
        {
            var item = new SftpFileItem
            {
                Name = "Documents",
                IsDirectory = true,
                Permissions = "drwxr-xr-x",
                Size = 0
            };

            Assert.That(item.IsDirectory, Is.True);
            Assert.That(item.Permissions[0], Is.EqualTo('d'));
        }

        [Test]
        public void SftpFileItem_Symlink_HasExpectedProperties()
        {
            var item = new SftpFileItem
            {
                Name = "link",
                IsSymlink = true,
                Permissions = "lrwxrwxrwx"
            };

            Assert.That(item.IsSymlink, Is.True);
            Assert.That(item.Permissions[0], Is.EqualTo('l'));
        }
    }

    [TestFixture]
    public class SftpFileServiceTests
    {
        [Test]
        public void SftpFileService_InitialState_IsNotConnected()
        {
            using var service = new SftpFileService();

            Assert.That(service.IsConnected, Is.False);
            Assert.That(service.CurrentPath, Is.EqualTo("/"));
            Assert.That(service.HomePath, Is.EqualTo("/"));
            Assert.That(service.Host, Is.Null);
            Assert.That(service.User, Is.Null);
        }

        [Test]
        public void SftpFileService_Dispose_DoesNotThrow()
        {
            var service = new SftpFileService();
            Assert.DoesNotThrow(() => service.Dispose());
        }

        [Test]
        public void SftpFileService_DoubleDispose_DoesNotThrow()
        {
            var service = new SftpFileService();
            service.Dispose();
            Assert.DoesNotThrow(() => service.Dispose());
        }

        [Test]
        public void SftpFileService_ConnectToInvalidHost_ThrowsException()
        {
            using var service = new SftpFileService();
            Assert.Throws<System.Net.Sockets.SocketException>(() =>
                service.Connect("invalid.host.that.does.not.exist.local", "user", "pass", 22));
        }
    }
}
