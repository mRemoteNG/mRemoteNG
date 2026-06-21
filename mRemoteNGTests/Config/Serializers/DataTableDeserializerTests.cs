using System.Data;
using System.Linq;
using System.Security;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Sql;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tree;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers;

public class DataTableDeserializerTests
{
    private DataTableDeserializer _deserializer;
    private ICryptographyProvider _cryptographyProvider;

    [SetUp]
    public void Setup()
    {
        _cryptographyProvider = new LegacyRijndaelCryptographyProvider();
    }

    [Test]
    public void WeCanDeserializeATree()
    {
        var model = CreateConnectionTreeModel();
        var dataTable = CreateDataTable(model.RootNodes[0]);
        _deserializer = new DataTableDeserializer(_cryptographyProvider, new SecureString());
        var output = _deserializer.Deserialize(dataTable);
        Assert.That(output.GetRecursiveChildList().Count, Is.EqualTo(model.GetRecursiveChildList().Count));
    }

    [Test]
    public void WeCanDeserializeASingleEntry()
    {
        var dataTable = CreateDataTable(new ConnectionInfo());
        _deserializer = new DataTableDeserializer(_cryptographyProvider, new SecureString());
        var output = _deserializer.Deserialize(dataTable);
        Assert.That(output.GetRecursiveChildList().Count, Is.EqualTo(1));
    }

    [Test]
    public void UnknownLegacyResolutionFallsBackToSmartSize()
    {
        var dataTable = CreateDataTable(new ConnectionInfo());
        dataTable.Rows[0]["Resolution"] = "Res1920x1080";
        _deserializer = new DataTableDeserializer(_cryptographyProvider, new SecureString());
        var output = _deserializer.Deserialize(dataTable);
        var connection = (ConnectionInfo)output.GetRecursiveChildList().First();
        Assert.That(connection.Resolution, Is.EqualTo(RDPResolutions.SmartSize));
    }


    private DataTable CreateDataTable(ConnectionInfo tableContent)
    {
        var serializer = new DataTableSerializer(new SaveFilter(), _cryptographyProvider, new SecureString());
        return serializer.Serialize(tableContent);
    }

    private ConnectionTreeModel CreateConnectionTreeModel()
    {
        var builder = new ConnectionTreeModelBuilder();
        return builder.Build();
    }
}