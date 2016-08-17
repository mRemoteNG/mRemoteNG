using mRemoteNG.Connection;
using mRemoteNG.Container;
using NUnit.Framework;


namespace mRemoteNGTests.Container
{
    public class ContainerInfoTests
    {
        private ContainerInfo _containerInfo;
        private ConnectionInfo _connectionInfo;

        [SetUp]
        public void Setup()
        {
            _containerInfo = new ContainerInfo();
            _connectionInfo = new ConnectionInfo();
        }

        [TearDown]
        public void Teardown()
        {
            _containerInfo = null;
            _connectionInfo = null;
        }

        [Test]
        public void AddSetsParentPropertyOnTheChild()
        {
            _containerInfo.Add(_connectionInfo);
            Assert.That(_connectionInfo.Parent, Is.EqualTo(_containerInfo));
        }

        [Test]
        public void AddAddsChildToChildrenList()
        {
            _containerInfo.Add(_connectionInfo);
            Assert.That(_containerInfo.Children, Does.Contain(_connectionInfo));
        }

        [Test]
        public void AddRangeAddsAllItems()
        {
            var collection = new[] {new ConnectionInfo(),new ConnectionInfo(), new ContainerInfo()};
            _containerInfo.AddRange(collection);
            Assert.That(_containerInfo.Children, Is.EquivalentTo(collection));
        }

        [Test]
        public void RemoveUnsetsParentPropertyOnChild()
        {
            _containerInfo.Add(_connectionInfo);
            _containerInfo.Remove(_connectionInfo);
            Assert.That(_connectionInfo.Parent, Is.Not.EqualTo(_containerInfo));
        }

        [Test]
        public void RemoveRemovesChildFromChildrenList()
        {
            _containerInfo.Add(_connectionInfo);
            _containerInfo.Remove(_connectionInfo);
            Assert.That(_containerInfo.Children, Does.Not.Contains(_connectionInfo));
        }

        [Test]
        public void RemoveRangeRemovesAllIndicatedItems()
        {
            var collection = new[] { new ConnectionInfo(), new ConnectionInfo(), new ContainerInfo() };
            _containerInfo.AddRange(collection);
            _containerInfo.RemoveRange(collection);
            Assert.That(_containerInfo.Children, Does.Not.Contains(collection[0]).And.Not.Contains(collection[1]).And.Not.Contains(collection[2]));
        }

        [Test]
        public void RemoveRangeDoesNotRemoveUntargetedMembers()
        {
            var collection = new[] { new ConnectionInfo(), new ConnectionInfo(), new ContainerInfo() };
            _containerInfo.AddRange(collection);
            _containerInfo.Add(_connectionInfo);
            _containerInfo.RemoveRange(collection);
            Assert.That(_containerInfo.Children, Does.Contain(_connectionInfo));
        }
    }
}