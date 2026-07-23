using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.UI.Window;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading;
using WeifenLuo.WinFormsUI.Docking;


namespace mRemoteNGTests.UI.Window
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class ConnectionTreeWindowGetDirectChildConnectionsTests
    {
        private ConnectionInfo _mockConnection1;
        private ConnectionInfo _mockConnection2;
        private ConnectionInfo _mockConnection3;
        private ContainerInfo _folder;
        private ContainerInfo _nestedFolder;

        [SetUp]
        public void Setup()
        {
            _mockConnection1 = new ConnectionInfo { Name = "Connection1" };
            _mockConnection2 = new ConnectionInfo { Name = "Connection2" };
            _mockConnection3 = new ConnectionInfo { Name = "Connection3" };
            _folder = new ContainerInfo { Name = "TestFolder" };
            _nestedFolder = new ContainerInfo { Name = "NestedFolder" };
        }

        [Test]
        public void GetDirectChildConnections_WithFolderContainingOnlyConnections_ReturnsAllDirectChildren()
        {
            // Arrange
            _folder.AddChild(_mockConnection1);
            _folder.AddChild(_mockConnection2);
            var window = new ConnectionTreeWindow(new DockContent());

            try
            {
                // Act
                var result = window.PublicGetDirectChildConnections(_folder);

                // Assert
                Assert.That(result.Count, Is.EqualTo(2));
                Assert.That(result, Contains.Item(_mockConnection1));
                Assert.That(result, Contains.Item(_mockConnection2));
            }
            finally
            {
                window.Dispose();
            }
        }

        [Test]
        public void GetDirectChildConnections_WithFolderContainingNestedFolders_ReturnsOnlyDirectConnections()
        {
            // Arrange
            _folder.AddChild(_mockConnection1);
            _folder.AddChild(_mockConnection2);

            var nestedConnection = new ConnectionInfo { Name = "NestedConnection" };
            _nestedFolder.AddChild(nestedConnection);
            _folder.AddChild(_nestedFolder);
            var window = new ConnectionTreeWindow(new DockContent());

            try
            {
                // Act
                var result = window.PublicGetDirectChildConnections(_folder);

                // Assert
                Assert.That(result.Count, Is.EqualTo(2));
                Assert.That(result, Contains.Item(_mockConnection1));
                Assert.That(result, Contains.Item(_mockConnection2));
                Assert.That(result, Does.Not.Contains(nestedConnection));
            }
            finally
            {
                window.Dispose();
            }
        }

        [Test]
        public void GetDirectChildConnections_WithEmptyFolder_ReturnsEmpty()
        {
            // Arrange
            var window = new ConnectionTreeWindow(new DockContent());

            try
            {
                // Act
                var result = window.PublicGetDirectChildConnections(_folder);

                // Assert
                Assert.That(result, Is.Empty);
            }
            finally
            {
                window.Dispose();
            }
        }

        [Test]
        public void GetDirectChildConnections_WithNonContainerNode_ReturnsEmpty()
        {
            // Arrange
            var window = new ConnectionTreeWindow(new DockContent());

            try
            {
                // Act
                var result = window.PublicGetDirectChildConnections(_mockConnection1);

                // Assert
                Assert.That(result, Is.Empty);
            }
            finally
            {
                window.Dispose();
            }
        }

        [Test]
        public void GetDirectChildConnections_WithFolderContainingMixedTypes_ReturnsOnlyConnections()
        {
            // Arrange
            _folder.AddChild(_mockConnection1);
            _folder.AddChild(_nestedFolder);
            _folder.AddChild(_mockConnection2);
            var window = new ConnectionTreeWindow(new DockContent());

            try
            {
                // Act
                var result = window.PublicGetDirectChildConnections(_folder);

                // Assert
                Assert.That(result.Count, Is.EqualTo(2));
                Assert.That(result, Contains.Item(_mockConnection1));
                Assert.That(result, Contains.Item(_mockConnection2));
                Assert.That(result, Does.Not.Contains(_nestedFolder));
            }
            finally
            {
                window.Dispose();
            }
        }

        [Test]
        public void GetDirectChildConnections_WithDeeplyNestedStructure_ReturnsOnlyDirectChildren()
        {
            // Arrange
            var level1Folder = new ContainerInfo { Name = "Level1" };
            var level2Folder = new ContainerInfo { Name = "Level2" };
            var deepConnection = new ConnectionInfo { Name = "DeepConnection" };
            var directConnection = new ConnectionInfo { Name = "DirectConnection" };

            level2Folder.AddChild(deepConnection);
            level1Folder.AddChild(level2Folder);
            level1Folder.AddChild(directConnection);
            var window = new ConnectionTreeWindow(new DockContent());

            try
            {
                // Act
                var result = window.PublicGetDirectChildConnections(level1Folder);

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
                Assert.That(result, Contains.Item(directConnection));
                Assert.That(result, Does.Not.Contains(deepConnection));
            }
            finally
            {
                window.Dispose();
            }
        }

        [Test]
        public void GetDirectChildConnections_FiltersOutFolders_OnlyReturnsConnections()
        {
            // Arrange
            var folder1 = new ContainerInfo { Name = "Folder1" };
            var folder2 = new ContainerInfo { Name = "Folder2" };
            _folder.AddChild(folder1);
            _folder.AddChild(_mockConnection1);
            _folder.AddChild(folder2);
            _folder.AddChild(_mockConnection2);
            var window = new ConnectionTreeWindow(new DockContent());

            try
            {
                // Act
                var result = window.PublicGetDirectChildConnections(_folder);

                // Assert
                Assert.That(result.Count, Is.EqualTo(2));
                Assert.That(result, Does.Not.Contains(folder1));
                Assert.That(result, Does.Not.Contains(folder2));
            }
            finally
            {
                window.Dispose();
            }
        }
    }
}