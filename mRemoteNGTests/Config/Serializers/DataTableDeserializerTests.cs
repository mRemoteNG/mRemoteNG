using System.Data;
using System.Linq;
using System.Security;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Sql;
using mRemoteNG.Connection;
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

    // Regression test for issue #2221: DataTableDeserializer was incorrectly accessing
    // "DomainName" column which does not exist in tblCons; the correct column is "Domain".
    [Test]
    public void SerializedTableUsesDomainColumnNotDomainName()
    {
        var saveFilter = new SaveFilter { SaveDomain = true };
        var serializer = new DataTableSerializer(saveFilter, _cryptographyProvider, new SecureString());
        var dataTable = serializer.Serialize(new ConnectionInfo { Domain = "testdomain" });

        Assert.That(dataTable.Columns.Contains("Domain"), Is.True,
            "tblCons must have a 'Domain' column");
        Assert.That(dataTable.Columns.Contains("DomainName"), Is.False,
            "tblCons must not have a 'DomainName' column (regression: #2221)");
    }

    // Regression test for issue #2221: verify Domain value round-trips correctly.
    [Test]
    public void DomainFieldRoundtripsCorrectly()
    {
        var saveFilter = new SaveFilter { SaveDomain = true };
        var serializer = new DataTableSerializer(saveFilter, _cryptographyProvider, new SecureString());
        var dataTable = serializer.Serialize(new ConnectionInfo { Domain = "corp.example.com" });

        _deserializer = new DataTableDeserializer(_cryptographyProvider, new SecureString());
        var output = _deserializer.Deserialize(dataTable);
        var connection = output.GetRecursiveChildList().First();

        Assert.That(connection.Domain, Is.EqualTo("corp.example.com"));
    }

    // Regression test for issue #2221: if the "Domain" column is absent (old schema),
    // deserialization must not throw and must return an empty domain.
    [Test]
    public void DeserializationDoesNotThrowWhenDomainColumnMissing()
    {
        var dataTable = CreateDataTable(new ConnectionInfo { Domain = "somevalue" });
        dataTable.Columns.Remove("Domain");

        _deserializer = new DataTableDeserializer(_cryptographyProvider, new SecureString());
        ConnectionTreeModel output = null;
        Assert.DoesNotThrow(() => output = _deserializer.Deserialize(dataTable));
        Assert.That(output.GetRecursiveChildList().First().Domain, Is.EqualTo(""));
    }


    private DataTable CreateDataTable(ConnectionInfo tableContent)
    {
        var serializer = new DataTableSerializer(new SaveFilter(), _cryptographyProvider, new SecureString());
        return serializer.Serialize(tableContent);
    }

    private static ConnectionTreeModel CreateConnectionTreeModel()
    {
        return ConnectionTreeModelBuilder.Build();
    }
}