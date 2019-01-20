using System.Linq;
using System.Security;
using mRemoteNG.Config.Serializers.MsSql;
using mRemoteNG.Connection;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNGTests.TestHelpers;
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
            _dataTableSerializer = new DataTableSerializer(
                _saveFilter, 
                new LegacyRijndaelCryptographyProvider(), 
                new SecureString());
        }

        [Test]
        public void AllItemsSerialized()
        {
            var model = CreateConnectionTreeModel();
            var dataTable = _dataTableSerializer.Serialize(model);
            Assert.That(dataTable.Rows.Count, Is.EqualTo(model.GetRecursiveChildList().Count()));
        }

        [Test]
        public void ReturnsEmptyDataTableWhenGivenEmptyConnectionTreeModel()
        {
            var model = new ConnectionTreeModel();
            var dataTable = _dataTableSerializer.Serialize(model);
            Assert.That(dataTable.Rows.Count, Is.EqualTo(0));
        }

        [Test]
        public void InheritanceSerializedWhenSaveSecurityAllowsIt()
        {
            var model = CreateConnectionTreeModel();
            model.GetRecursiveChildList().ForEach(c => c.Inheritance.UserField = true);
            _saveFilter.SaveInheritance = true;
            var dataTable = _dataTableSerializer.Serialize(model);
            Assert.That(dataTable.Rows[0]["InheritUserField"], Is.EqualTo(true));
        }

        [Test]
        public void InheritanceNotSerializedWhenSaveSecurityDisabled()
        {
            var model = CreateConnectionTreeModel();
            model.GetRecursiveChildList().ForEach(c => c.Inheritance.UserField = true);
            _saveFilter.SaveInheritance = false;
            var dataTable = _dataTableSerializer.Serialize(model);
            Assert.That(dataTable.Rows[0]["InheritUserField"], Is.False);
        }

        [Test]
        public void CanSerializeEmptyConnectionInfo()
        {
            var dataTable = _dataTableSerializer.Serialize(new ConnectionInfo());
            Assert.That(dataTable.Rows.Count, Is.EqualTo(1));
        }


        private ConnectionTreeModel CreateConnectionTreeModel()
        {
            var builder = new ConnectionTreeModelBuilder();
            return builder.Build();
        }
    }
}