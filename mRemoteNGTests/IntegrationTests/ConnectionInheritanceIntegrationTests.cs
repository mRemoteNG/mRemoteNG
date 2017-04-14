using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Tree.Root;
using NUnit.Framework;


namespace mRemoteNGTests.IntegrationTests
{
    public class ConnectionInheritanceIntegrationTests
    {
        private RootNodeInfo _rootNode;

        [SetUp]
        public void Setup()
        {
            _rootNode = new RootNodeInfo(RootNodeType.Connection);
        }

        [TearDown]
        public void Teardown()
        {
            _rootNode = null;
        }

        [Test]
        public void ConnectionsInheritFromCorrectFolder()
        {
            /**
             * Root
             * --Folder1
             * ----Connection1
             * ----Folder2
             * ------Connection2
             * ----Connection3 (inherits username)
             */
            var folder1 = new ContainerInfo { Icon = "someicon1" };
            var folder2 = new ContainerInfo { Icon = "someicon2" };
            var connection1 = new ConnectionInfo();
            var connection2 = new ConnectionInfo();
            var connection3 = new ConnectionInfo {Inheritance = {Icon = true}};
            _rootNode.AddChild(folder1);
            folder1.AddChildRange(new []{connection1, folder2, connection3});
            folder2.AddChild(connection2);
            Assert.That(connection3.Icon, Is.EqualTo(folder1.Icon));
        }

        [Test]
        public void ConnectionWillInheritAllFromFolder()
        {
            var folder = new ContainerInfo {Protocol = ProtocolType.SSH2, Icon = "someicon", CacheBitmaps = true};
            var connection = new ConnectionInfo {Inheritance = {EverythingInherited = true}};
            _rootNode.AddChild(folder);
            folder.AddChild(connection);
            Assert.That(new object[] { connection.Protocol, connection.Icon, connection.CacheBitmaps }, Is.EquivalentTo(new object[] {folder.Protocol, folder.Icon, folder.CacheBitmaps }));
        }

        [Test]
        public void CanInheritThroughMultipleFolderLevels()
        {
            var folder1 = new ContainerInfo { Icon = "someicon"};
            var folder2 = new ContainerInfo {Inheritance = { Icon = true}};
            var folder3 = new ContainerInfo {Inheritance = { Icon = true}};
            var connection = new ConnectionInfo {Inheritance = { Icon = true}};
            _rootNode.AddChild(folder1);
            folder1.AddChild(folder2);
            folder2.AddChild(folder3);
            folder3.AddChild(connection);
            Assert.That(connection.Icon, Is.EqualTo(folder1.Icon));
        }
    }
}