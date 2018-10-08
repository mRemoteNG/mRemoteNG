using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using mRemoteNG.Config.Serializers.Xml;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers.ConnectionSerializers.Xml
{
    public class ValidateXmlSchemas
    {
        private XmlConnectionsSerializer _serializer;
        private ConnectionTreeModel _connectionTreeModel;
        private ICryptographyProvider _cryptographyProvider;
        private XmlReaderSettings _xmlReaderSettings;

        [SetUp]
        public void Setup()
        {
            _connectionTreeModel = new ConnectionTreeModelBuilder().Build();
            _cryptographyProvider = new AeadCryptographyProvider();
            var connectionNodeSerializer = new XmlConnectionNodeSerializer26(
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

            var schemaFile = GetTargetPath("mremoteng_confcons_v2_6.xsd");
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
            const string debugOrRelease =
#if DEBUG
                "Debug";
#else
				"Release";
#endif

            const string normalOrPortable =
#if PORTABLE
                " Portable";
#else
            "";
#endif
            var path = Path.GetDirectoryName(sourceFilePath);
            var filePath = $@"{path}\..\..\..\..\bin\{debugOrRelease}{normalOrPortable}\Schemas\{fileName}";
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
}
