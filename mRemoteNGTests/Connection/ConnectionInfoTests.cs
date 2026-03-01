using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.SSH;
using mRemoteNG.Container;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using NUnit.Framework;


namespace mRemoteNGTests.Connection
{
	public class ConnectionInfoTests
    {
        private ConnectionInfo _connectionInfo;
        private const string TestDomain = "somedomain";

        [SetUp]
        public void Setup()
        {
            _connectionInfo = new ConnectionInfo();
        }

        [TearDown]
        public void Teardown()
        {
            _connectionInfo = null;
        }

        [Test]
        public void CopyCreatesMemberwiseCopy()
        {
            _connectionInfo.Domain = TestDomain;
            var secondConnection = _connectionInfo.Clone();
            Assert.That(secondConnection.Domain, Is.EqualTo(_connectionInfo.Domain));
        }

        [Test]
        public void CloneDoesNotSetParentOfNewConnectionInfo()
        {
            _connectionInfo.SetParent(new ContainerInfo());
            var clonedConnection = _connectionInfo.Clone();
            Assert.That(clonedConnection.Parent, Is.Null);
        }

        [Test]
        public void CloneAlsoCopiesInheritanceObject()
        {
            var clonedConnection = _connectionInfo.Clone();
            Assert.That(clonedConnection.Inheritance, Is.Not.EqualTo(_connectionInfo.Inheritance));
        }

        [Test]
        public void CloneDoesNotCopyLinkedConnectionId()
        {
            _connectionInfo.LinkedConnectionId = "source-id";

            var clonedConnection = _connectionInfo.Clone();

            Assert.That(clonedConnection.LinkedConnectionId, Is.EqualTo(string.Empty));
        }

        [Test]
        public void SerializablePropertiesDoNotIncludeLinkedConnectionId()
        {
            var serializablePropertyNames = _connectionInfo.GetSerializableProperties().Select(prop => prop.Name).ToArray();

            Assert.That(serializablePropertyNames, Does.Not.Contain(nameof(ConnectionInfo.LinkedConnectionId)));
        }

        [Test]
        public void CloneCorrectlySetsParentOfInheritanceObject()
        {
			var originalConnection = new ConnectionInfo();
            var clonedConnection = originalConnection.Clone();
            Assert.That(clonedConnection.Inheritance.Parent, Is.EqualTo(clonedConnection));
        }

        [Test]
        public void CopyFromCopiesProperties()
        {
            var secondConnection = new ConnectionInfo {Domain = TestDomain};
            _connectionInfo.CopyFrom(secondConnection);
            Assert.That(_connectionInfo.Domain, Is.EqualTo(secondConnection.Domain));
        }

        [Test]
        public void CopyingAConnectionInfoAlsoCopiesItsInheritance()
        {
            _connectionInfo.Inheritance.Username = true;
            var secondConnection = new ConnectionInfo {Inheritance = {Username = false}};
            secondConnection.CopyFrom(_connectionInfo);
            Assert.That(secondConnection.Inheritance.Username, Is.True);
        }

        [Test]
        public void PropertyChangedEventRaisedWhenOpenConnectionsChanges()
        {
            var eventWasCalled = false;
            _connectionInfo.PropertyChanged += (sender, args) => eventWasCalled = true;
            _connectionInfo.OpenConnections.Add(new ProtocolSSH2());
            Assert.That(eventWasCalled);
        }

        [Test]
        public void PropertyChangedEventArgsAreCorrectWhenOpenConnectionsChanges()
        {
            var nameOfModifiedProperty = "";
            _connectionInfo.PropertyChanged += (sender, args) => nameOfModifiedProperty = args.PropertyName;
            _connectionInfo.OpenConnections.Add(new ProtocolSSH2());
            Assert.That(nameOfModifiedProperty, Is.EqualTo("OpenConnections"));
        }

	    [TestCaseSource(typeof(InheritancePropertyProvider), nameof(InheritancePropertyProvider.GetProperties))]
	    public void MovingAConnectionFromUnderRootNodeToUnderADifferentNodeEnablesInheritance(PropertyInfo property)
	    {
		    var rootNode = new RootNodeInfo(RootNodeType.Connection);
			var otherContainer = new ContainerInfo();
		    _connectionInfo.Inheritance.EverythingInherited = true;
		    _connectionInfo.SetParent(rootNode);
			_connectionInfo.SetParent(otherContainer);
		    var propertyValue = property.GetValue(_connectionInfo.Inheritance);
		    Assert.That(propertyValue, Is.True);
	    }

        [Test]
        public void LinkedConnectionResolvesSourcePropertiesAtRuntime()
        {
            var sourceConnection = new ConnectionInfo { Hostname = "first-host" };
            var linkedConnection = new ConnectionInfo
            {
                LinkedConnectionId = sourceConnection.ConstantID,
                Hostname = "stale-host"
            };

            var rootNode = new RootNodeInfo(RootNodeType.Connection);
            rootNode.AddChild(sourceConnection);
            rootNode.AddChild(linkedConnection);
            var connectionTreeModel = new ConnectionTreeModel();
            connectionTreeModel.AddRootNode(rootNode);

            var originalModel = Runtime.ConnectionsService.ConnectionTreeModel;
            SetRuntimeConnectionTreeModel(connectionTreeModel);
            try
            {
                Assert.That(linkedConnection.Hostname, Is.EqualTo("first-host"));
                sourceConnection.Hostname = "second-host";
                Assert.That(linkedConnection.Hostname, Is.EqualTo("second-host"));
            }
            finally
            {
                SetRuntimeConnectionTreeModel(originalModel);
            }
        }

		[TestCase(ProtocolType.HTTP, ExpectedResult = 80)]
        [TestCase(ProtocolType.HTTPS, ExpectedResult = 443)]
        [TestCase(ProtocolType.IntApp, ExpectedResult = 0)]
        [TestCase(ProtocolType.RAW, ExpectedResult = 23)]
        [TestCase(ProtocolType.RDP, ExpectedResult = 3389)]
        [TestCase(ProtocolType.Rlogin, ExpectedResult = 513)]
        [TestCase(ProtocolType.SSH1, ExpectedResult = 22)]
        [TestCase(ProtocolType.SSH2, ExpectedResult = 22)]
        [TestCase(ProtocolType.Telnet, ExpectedResult = 23)]
        [TestCase(ProtocolType.VNC, ExpectedResult = 5900)]
        [TestCase(ProtocolType.ARD, ExpectedResult = 5900)]
        public int GetDefaultPortReturnsCorrectPortForProtocol(ProtocolType protocolType)
        {
            _connectionInfo.Protocol = protocolType;
            return _connectionInfo.GetDefaultPort();
        }

        private static void SetRuntimeConnectionTreeModel(ConnectionTreeModel? connectionTreeModel)
        {
            var backingField = typeof(ConnectionsService)
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault(f => f.Name.Contains("<ConnectionTreeModel>k__BackingField"));

            if (backingField != null)
            {
                backingField.SetValue(Runtime.ConnectionsService, connectionTreeModel);
                return;
            }

            var propertyInfo = typeof(ConnectionsService).GetProperty(nameof(ConnectionsService.ConnectionTreeModel));
            propertyInfo?.GetSetMethod(true)?.Invoke(Runtime.ConnectionsService, new object[] { connectionTreeModel });
        }

        [Test]
        public void DomainExpandsNameToken()
        {
            _connectionInfo.Name = "CONTOSO";
            _connectionInfo.Domain = "%Name%";
            Assert.That(_connectionInfo.Domain, Is.EqualTo("CONTOSO"));
        }

        [Test]
        public void DomainExpandsNameTokenCaseInsensitive()
        {
            _connectionInfo.Name = "contoso";
            _connectionInfo.Domain = "%NAME%";
            Assert.That(_connectionInfo.Domain, Is.EqualTo("contoso"));
        }

        [Test]
        public void DomainWithNoTokensIsUnchanged()
        {
            _connectionInfo.Name = "ignored";
            _connectionInfo.Domain = "static.domain.com";
            Assert.That(_connectionInfo.Domain, Is.EqualTo("static.domain.com"));
        }

        [Test]
        public void HostnameExpandsNameToken()
        {
            _connectionInfo.Name = "myserver.example.com";
            _connectionInfo.Hostname = "%Name%";
            Assert.That(_connectionInfo.Hostname, Is.EqualTo("myserver.example.com"));
        }

        [Test]
        public void HostnameExpandsNameTokenCaseInsensitive()
        {
            _connectionInfo.Name = "host1";
            _connectionInfo.Hostname = "%NAME%";
            Assert.That(_connectionInfo.Hostname, Is.EqualTo("host1"));
        }

        [Test]
        public void HostnameWithNoTokensIsUnchanged()
        {
            _connectionInfo.Name = "ignored";
            _connectionInfo.Hostname = "static-host.example.com";
            Assert.That(_connectionInfo.Hostname, Is.EqualTo("static-host.example.com"));
        }

        [Test]
        public void HostnameExpandsNameTokenMixedWithLiteral()
        {
            _connectionInfo.Name = "myserver";
            _connectionInfo.Hostname = "%name%.example.com";
            Assert.That(_connectionInfo.Hostname, Is.EqualTo("myserver.example.com"));
        }

	    private class InheritancePropertyProvider
	    {
		    public static IEnumerable<PropertyInfo> GetProperties()
		    {
			    return ConnectionInfoInheritance.GetProperties();
		    }
	    }
    }
}