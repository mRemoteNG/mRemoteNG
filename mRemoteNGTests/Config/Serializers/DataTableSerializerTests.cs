using mRemoteNG.Config.Serializers;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Security;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers
{
    public class DataTableSerializerTests
    {
        private DataTableSerializer _dataTableSerializer;
        private SaveFilter _saveFilter;

        [SetUp]
        public void Setup()
        {
            _saveFilter = new SaveFilter();
            _dataTableSerializer = new DataTableSerializer(_saveFilter);
        }

        [TearDown]
        public void Teardown()
        {
            _saveFilter = null;
            _dataTableSerializer = null;
        }

        [Test]
        public void AllItemsSerialized()
        {
            var model = CreateConnectionTreeModel();
            var dataTable = _dataTableSerializer.Serialize(model);
            Assert.That(dataTable.Rows.Count, Is.EqualTo(3));
        }

        [Test]
        public void UsernameSerializedWhenSaveSecurityAllowsIt()
        {
            var model = CreateConnectionTreeModel();
            _saveFilter.SaveUsername = true;
            var dataTable = _dataTableSerializer.Serialize(model);
            Assert.That(dataTable.Rows[0]["Username"], Is.Not.EqualTo(""));
        }

        [Test]
        public void DomainSerializedWhenSaveSecurityAllowsIt()
        {
            var model = CreateConnectionTreeModel();
            _saveFilter.SaveDomain = true;
            var dataTable = _dataTableSerializer.Serialize(model);
            Assert.That(dataTable.Rows[0]["DomainName"], Is.Not.EqualTo(""));
        }

        [Test]
        public void PasswordSerializedWhenSaveSecurityAllowsIt()
        {
            var model = CreateConnectionTreeModel();
            _saveFilter.SavePassword = true;
            var dataTable = _dataTableSerializer.Serialize(model);
            Assert.That(dataTable.Rows[0]["Password"], Is.Not.EqualTo(""));
        }

        [Test]
        public void InheritanceSerializedWhenSaveSecurityAllowsIt()
        {
            var model = CreateConnectionTreeModel();
            _saveFilter.SaveInheritance = true;
            var dataTable = _dataTableSerializer.Serialize(model);
            Assert.That(dataTable.Rows[0]["InheritUsername"], Is.Not.EqualTo(""));
        }



        [Test]
        public void UsernameNotSerializedWhenSaveSecurityDisabled()
        {
            var model = CreateConnectionTreeModel();
            _saveFilter.SaveUsername = false;
            var dataTable = _dataTableSerializer.Serialize(model);
            Assert.That(dataTable.Rows[0]["Username"], Is.EqualTo(""));
        }

        [Test]
        public void DomainNotSerializedWhenSaveSecurityDisabled()
        {
            var model = CreateConnectionTreeModel();
            _saveFilter.SaveDomain = false;
            var dataTable = _dataTableSerializer.Serialize(model);
            Assert.That(dataTable.Rows[0]["DomainName"], Is.EqualTo(""));
        }

        [Test]
        public void PasswordNotSerializedWhenSaveSecurityDisabled()
        {
            var model = CreateConnectionTreeModel();
            _saveFilter.SavePassword = false;
            var dataTable = _dataTableSerializer.Serialize(model);
            Assert.That(dataTable.Rows[0]["Password"], Is.EqualTo(""));
        }

        [Test]
        public void InheritanceNotSerializedWhenSaveSecurityDisabled()
        {
            var model = CreateConnectionTreeModel();
            _saveFilter.SaveInheritance = false;
            var dataTable = _dataTableSerializer.Serialize(model);
            Assert.That(dataTable.Rows[0]["InheritUsername"], Is.False);
        }


        private ConnectionTreeModel CreateConnectionTreeModel()
        {
            var model = new ConnectionTreeModel();
            var root = new RootNodeInfo(RootNodeType.Connection);
            var folder1 = new ContainerInfo {Name = "folder1", Username = "user1", Domain = "domain1", Password = "password1"};
            var con1 = new ConnectionInfo {Name = "Con1", Username = "user1", Domain = "domain1", Password = "password1" };
            var con2 = new ConnectionInfo {Name = "Con2", Username = "user2", Domain = "domain2", Password = "password2" };

            root.AddChild(folder1);
            root.AddChild(con2);
            folder1.AddChild(con1);
            model.AddRootNode(root);
            return model;
        }
    }
}