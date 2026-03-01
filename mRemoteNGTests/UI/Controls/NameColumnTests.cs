using NUnit.Framework;
using mRemoteNG.UI.Controls.ConnectionTree;
using mRemoteNG.Container;
using mRemoteNG.Connection;
using mRemoteNG.Tree;
using BrightIdeasSoftware;

namespace mRemoteNGTests.UI.Controls
{
    [TestFixture]
    public class NameColumnTests
    {
        [Test]
        public void AspectGetter_ReturnsNameWithCount_ForContainer()
        {
            var container = new ContainerInfo { Name = "MyFolder" };
            var child1 = new ConnectionInfo { Name = "C1" };
            var child2 = new ConnectionInfo { Name = "C2" };
            container.AddChild(child1);
            container.AddChild(child2);

            var column = new NameColumn(delegate { return null; });
            var result = column.AspectGetter(container);

            Assert.That(result, Is.EqualTo("MyFolder (2)"));
        }

        [Test]
        public void AspectGetter_ReturnsNameWithRecursiveCount_ForContainer()
        {
            var root = new ContainerInfo { Name = "Root" };
            var subFolder = new ContainerInfo { Name = "Sub" };
            var child1 = new ConnectionInfo { Name = "C1" };
            
            root.AddChild(subFolder);
            subFolder.AddChild(child1);

            var column = new NameColumn(delegate { return null; });
            var rootResult = column.AspectGetter(root);
            var subResult = column.AspectGetter(subFolder);

            Assert.That(rootResult, Is.EqualTo("Root (1)"));
            Assert.That(subResult, Is.EqualTo("Sub (1)"));
        }
        
        [Test]
        public void AspectGetter_ReturnsNameOnly_ForConnection()
        {
            var connection = new ConnectionInfo { Name = "MyConnection" };
            var column = new NameColumn(delegate { return null; });
            var result = column.AspectGetter(connection);

            Assert.That(result, Is.EqualTo("MyConnection"));
        }

        [Test]
        public void AspectGetter_ReturnsNameWithZeroCount_ForEmptyContainer()
        {
            var container = new ContainerInfo { Name = "EmptyFolder" };
            var column = new NameColumn(delegate { return null; });
            var result = column.AspectGetter(container);

            // My implementation uses "if (count > 0)", so it should be just name.
            Assert.That(result, Is.EqualTo("EmptyFolder")); 
        }
    }
}
