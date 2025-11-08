using System;
using mRemoteNG.UI.Controls.SCP;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Controls.SCP
{
    [TestFixture]
    public class FileListItemTests
    {
        #region IconIndex Tests

        [Test]
        public void IconIndex_RegularFile_Returns1()
        {
            // Arrange
            var item = new FileListItem
            {
                IsDirectory = false,
                IsMountPoint = false
            };

            // Act & Assert
            Assert.That(item.IconIndex, Is.EqualTo(1));
        }

        [Test]
        public void IconIndex_RegularFolder_Returns0()
        {
            // Arrange
            var item = new FileListItem
            {
                IsDirectory = true,
                IsMountPoint = false
            };

            // Act & Assert
            Assert.That(item.IconIndex, Is.EqualTo(0));
        }

        [Test]
        public void IconIndex_MountPointFolder_Returns2()
        {
            // Arrange
            var item = new FileListItem
            {
                IsDirectory = true,
                IsMountPoint = true
            };

            // Act & Assert
            Assert.That(item.IconIndex, Is.EqualTo(2));
        }

        [Test]
        public void IconIndex_FileWithMountPointFlag_Returns1()
        {
            // Arrange - IsMountPoint should be ignored for files
            var item = new FileListItem
            {
                IsDirectory = false,
                IsMountPoint = true // This shouldn't affect file icon
            };

            // Act & Assert
            Assert.That(item.IconIndex, Is.EqualTo(1),
                "Files should always use file icon even if IsMountPoint is set");
        }

        #endregion

        #region Type Property Tests

        [Test]
        public void Type_RegularFolder_ReturnsFolder()
        {
            // Arrange
            var item = new FileListItem
            {
                IsDirectory = true,
                IsMountPoint = false
            };

            // Act & Assert
            Assert.That(item.Type, Is.EqualTo("Folder"));
        }

        [Test]
        public void Type_MountPoint_ReturnsMountPoint()
        {
            // Arrange
            var item = new FileListItem
            {
                IsDirectory = true,
                IsMountPoint = true
            };

            // Act & Assert
            Assert.That(item.Type, Is.EqualTo("Mount Point"));
        }

        [Test]
        public void Type_FileWithNoExtension_ReturnsFile()
        {
            // Arrange
            var item = new FileListItem
            {
                IsDirectory = false,
                Extension = ""
            };

            // Act & Assert
            Assert.That(item.Type, Is.EqualTo("File"));
        }

        [Test]
        public void Type_FileWithExtension_ReturnsExtensionType()
        {
            // Arrange
            var item = new FileListItem
            {
                IsDirectory = false,
                Extension = ".txt"
            };

            // Act & Assert
            Assert.That(item.Type, Is.EqualTo("TXT File"));
        }

        [Test]
        public void Type_FileWithUppercaseExtension_ReturnsUppercaseType()
        {
            // Arrange
            var item = new FileListItem
            {
                IsDirectory = false,
                Extension = ".LOG"
            };

            // Act & Assert
            Assert.That(item.Type, Is.EqualTo("LOG File"));
        }

        [Test]
        public void Type_FileWithLowercaseExtension_ReturnsUppercaseType()
        {
            // Arrange
            var item = new FileListItem
            {
                IsDirectory = false,
                Extension = ".log"
            };

            // Act & Assert
            Assert.That(item.Type, Is.EqualTo("LOG File"));
        }

        #endregion

        #region SizeFormatted Tests

        [Test]
        public void SizeFormatted_Directory_ReturnsDIR()
        {
            // Arrange
            var item = new FileListItem
            {
                IsDirectory = true,
                Size = 12345 // Should be ignored for directories
            };

            // Act & Assert
            Assert.That(item.SizeFormatted, Is.EqualTo("<DIR>"));
        }

        [Test]
        public void SizeFormatted_ZeroBytes_Returns0B()
        {
            // Arrange
            var item = new FileListItem
            {
                IsDirectory = false,
                Size = 0
            };

            // Act & Assert
            Assert.That(item.SizeFormatted, Is.EqualTo("0 B"));
        }

        [Test]
        public void SizeFormatted_BytesUnder1KB_ReturnsBytes()
        {
            // Arrange
            var item = new FileListItem
            {
                IsDirectory = false,
                Size = 512
            };

            // Act & Assert
            Assert.That(item.SizeFormatted, Is.EqualTo("512 B"));
        }

        [Test]
        public void SizeFormatted_Kilobytes_ReturnsKB()
        {
            // Arrange
            var item = new FileListItem
            {
                IsDirectory = false,
                Size = 5120 // 5 KB
            };

            // Act & Assert
            Assert.That(item.SizeFormatted, Is.EqualTo("5 KB"));
        }

        [Test]
        public void SizeFormatted_Megabytes_ReturnsMB()
        {
            // Arrange
            var item = new FileListItem
            {
                IsDirectory = false,
                Size = 5242880 // 5 MB
            };

            // Act & Assert
            Assert.That(item.SizeFormatted, Is.EqualTo("5 MB"));
        }

        [Test]
        public void SizeFormatted_Gigabytes_ReturnsGB()
        {
            // Arrange
            var item = new FileListItem
            {
                IsDirectory = false,
                Size = 5368709120 // 5 GB
            };

            // Act & Assert
            Assert.That(item.SizeFormatted, Is.EqualTo("5 GB"));
        }

        [Test]
        public void SizeFormatted_FractionalSize_RoundsToOneDecimal()
        {
            // Arrange
            var item = new FileListItem
            {
                IsDirectory = false,
                Size = 1536 // 1.5 KB
            };

            // Act & Assert
            Assert.That(item.SizeFormatted, Is.EqualTo("1.5 KB"));
        }

        #endregion

        #region ModifiedFormatted Tests

        [Test]
        public void ModifiedFormatted_Today_ReturnsTime()
        {
            // Arrange
            var now = DateTime.Now;
            var item = new FileListItem
            {
                Modified = now
            };

            // Act
            var formatted = item.ModifiedFormatted;

            // Assert
            Assert.That(formatted, Does.Match(@"^\d{2}:\d{2}$"),
                "Today's files should show time in HH:mm format");
        }

        [Test]
        public void ModifiedFormatted_Yesterday_ReturnsYesterday()
        {
            // Arrange
            var yesterday = DateTime.Now.AddDays(-1);
            var item = new FileListItem
            {
                Modified = yesterday
            };

            // Act & Assert
            Assert.That(item.ModifiedFormatted, Is.EqualTo("Yesterday"));
        }

        [Test]
        public void ModifiedFormatted_WithinWeek_ReturnsDayAndTime()
        {
            // Arrange
            var threeDaysAgo = DateTime.Now.AddDays(-3);
            var item = new FileListItem
            {
                Modified = threeDaysAgo
            };

            // Act
            var formatted = item.ModifiedFormatted;

            // Assert
            Assert.That(formatted, Does.Match(@"^\w{3} \d{2}:\d{2}$"),
                "Files within a week should show day and time (e.g., 'Mon 14:30')");
        }

        [Test]
        public void ModifiedFormatted_ThisYear_ReturnsMonthAndDay()
        {
            // Arrange
            var twoMonthsAgo = DateTime.Now.AddMonths(-2);
            var item = new FileListItem
            {
                Modified = twoMonthsAgo
            };

            // Act
            var formatted = item.ModifiedFormatted;

            // Assert
            Assert.That(formatted, Does.Match(@"^\w{3} \d{2}$"),
                "Files from this year should show month and day (e.g., 'Oct 15')");
        }

        [Test]
        public void ModifiedFormatted_PreviousYear_ReturnsFullDate()
        {
            // Arrange
            var lastYear = DateTime.Now.AddYears(-1);
            var item = new FileListItem
            {
                Modified = lastYear
            };

            // Act
            var formatted = item.ModifiedFormatted;

            // Assert
            Assert.That(formatted, Does.Match(@"^\w{3} \d{2} \d{4}$"),
                "Files from previous years should show full date (e.g., 'Oct 15 2024')");
        }

        #endregion

        #region Property Assignment Tests

        [Test]
        public void Properties_CanBeSetAndRetrieved()
        {
            // Arrange
            var item = new FileListItem
            {
                Name = "test.txt",
                FullPath = "/home/user/test.txt",
                Size = 1024,
                Modified = new DateTime(2025, 11, 8, 10, 30, 0),
                Extension = ".txt",
                IsDirectory = false,
                IsMountPoint = false
            };

            // Assert
            Assert.That(item.Name, Is.EqualTo("test.txt"));
            Assert.That(item.FullPath, Is.EqualTo("/home/user/test.txt"));
            Assert.That(item.Size, Is.EqualTo(1024));
            Assert.That(item.Modified, Is.EqualTo(new DateTime(2025, 11, 8, 10, 30, 0)));
            Assert.That(item.Extension, Is.EqualTo(".txt"));
            Assert.That(item.IsDirectory, Is.False);
            Assert.That(item.IsMountPoint, Is.False);
        }

        #endregion
    }
}
