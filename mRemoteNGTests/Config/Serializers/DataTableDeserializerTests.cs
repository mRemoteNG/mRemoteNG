using System.Data;
using System.Linq;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Connection;
using mRemoteNG.Security;
using mRemoteNG.Tree;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers
{
    public class DataTableDeserializerTests
    {
        private DataTableDeserializer _deserializer;

        [SetUp]
        public void Setup()
        {
            
        }


        [Test]
        public void WeCanDeserializeATree()
        {
            var model = CreateConnectionTreeModel();
            var dataTable = CreateDataTable(model.RootNodes[0]);
            _deserializer = new DataTableDeserializer(dataTable);
            var output = _deserializer.Deserialize();
            Assert.That(output.GetRecursiveChildList().Count(), Is.EqualTo(model.GetRecursiveChildList().Count()));
        }

        [Test]
        public void WeCanDeserializeASingleEntry()
        {
            var dataTable = CreateDataTable(new ConnectionInfo());
            _deserializer = new DataTableDeserializer(dataTable);
            var output = _deserializer.Deserialize();
            Assert.That(output.GetRecursiveChildList().Count(), Is.EqualTo(1));
        }


        private DataTable CreateDataTable(ConnectionInfo tableContent)
        {
            var serializer = new DataTableSerializer(new SaveFilter());
            return serializer.Serialize(tableContent);
        }

        private ConnectionTreeModel CreateConnectionTreeModel()
        {
            var builder = new ConnectionTreeModelBuilder();
            return builder.Build();
        }
    }
}