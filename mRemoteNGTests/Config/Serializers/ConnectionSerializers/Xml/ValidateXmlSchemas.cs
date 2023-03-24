using mRemoteNG.Connection;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Xml;

namespace mRemoteNGTests.Config.Serializers.ConnectionSerializers.Xml;

public class ValidateXmlSchemas
{
    private XmlConnectionsSerializer _serializer;
    private ConnectionTreeModel _connectionTreeModel;
    private ICryptographyProvider _cryptographyProvider;
    private XmlReaderSettings _xmlReaderSettings;

    [SetUp]
    public void Setup()
    {
        _connectionTreeModel = new ConnectionTreeModel();
        var root = new RootNodeInfo(RootNodeType.Connection);
        root.AddChild(new ConnectionInfo());
        _connectionTreeModel.AddRootNode(root);

        _cryptographyProvider = new AeadCryptographyProvider();
        var connectionNodeSerializer = new XmlConnectionNodeSerializer28(
            _cryptographyProvider,
            _connectionTreeModel.RootNodes.OfType<RootNodeInfo>().First().PasswordString.ConvertToSecureString(),
            new SaveFilter());
        _serializer = new XmlConnectionsSerializer(_cryptographyProvider, connectionNodeSerializer);
        _xmlReaderSettings = new XmlReaderSettings
        {
            ValidationType = ValidationType.Schema,
            ValidationFlags = XmlSchemaValidationFlags.ProcessInlineSchema |
                              XmlSchemaValidationFlags.ProcessSchemaLocation |
                              XmlSchemaValidationFlags.ReportValidationWarnings
        };
    }

    [Test]
    public void ValidateSchema()
    {
        var sb = new StringBuilder();
        var xml = _serializer.Serialize(_connectionTreeModel);
        var schemaFileName = $"mremoteng_confcons_v{_serializer.Version.Major}_{_serializer.Version.Minor}.xsd";
        var schemaFile = GetTargetPath(schemaFileName);
        _xmlReaderSettings.Schemas.Add("http://mremoteng.org", schemaFile);
        _xmlReaderSettings.ValidationEventHandler += (sender, args) =>
        {
            sb.AppendLine($"{args.Severity}: {args.Message}");
        };

        using (var stream = GenerateStreamFromString(xml))
        {
            var reader = XmlReader.Create(stream, _xmlReaderSettings);
            while (reader.Read()) ;
        }

        Assert.That(sb.ToString(), Is.Empty);
    }

    public string GetTargetPath(string fileName, [CallerFilePath] string sourceFilePath = "")
    {
        var path = Path.GetDirectoryName(sourceFilePath);
        var filePath = $@"{path}\..\..\..\..\..\mRemoteNG\Schemas\{fileName}";

        return filePath;
    }

    private Stream GenerateStreamFromString(string s)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }
}