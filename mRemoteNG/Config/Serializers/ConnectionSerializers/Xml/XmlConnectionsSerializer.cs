using System;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Security;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Serializers.ConnectionSerializers.Xml
{
    [SupportedOSPlatform("windows")]
    public class XmlConnectionsSerializer(ICryptographyProvider cryptographyProvider,
                                    ISerializer<ConnectionInfo, XElement> connectionNodeSerializer) : ISerializer<ConnectionTreeModel, string>,
                                            ISerializer<ConnectionInfo, string>
    {
        private readonly ICryptographyProvider _cryptographyProvider = cryptographyProvider;
        private readonly ISerializer<ConnectionInfo, XElement> _connectionNodeSerializer = connectionNodeSerializer;

        public Version Version => _connectionNodeSerializer.Version;
        public bool UseFullEncryption { get; set; }

        public string Serialize(ConnectionTreeModel connectionTreeModel)
        {
            RootNodeInfo rootNode = (RootNodeInfo)connectionTreeModel.RootNodes.First(node => node is RootNodeInfo);
            return SerializeConnectionsData(rootNode);
        }

        public string Serialize(ConnectionInfo serializationTarget)
        {
            return SerializeConnectionsData(serializationTarget);
        }

        private string SerializeConnectionsData(ConnectionInfo serializationTarget)
        {
            try
            {
                XmlConnectionsDocumentCompiler documentCompiler =
                    new(_cryptographyProvider, _connectionNodeSerializer);
                XDocument xmlDocument = documentCompiler.CompileDocument(serializationTarget, UseFullEncryption);
                return WriteXmlToString(xmlDocument);
            }
            catch (Exception ex)
            {
                // Do not swallow the failure and return an empty string: that empty
                // string would be written over the connection file, destroying it while
                // reporting a successful save. Surface the error so callers can abort.
                Runtime.MessageCollector.AddExceptionStackTrace("SaveToXml failed", ex);
                throw;
            }
        }

        private static string WriteXmlToString(XNode xmlDocument)
        {
            string xmlString;
            XmlWriterSettings xmlWriterSettings = new() { Indent = true, IndentChars = "    ", Encoding = Encoding.UTF8};
            MemoryStream memoryStream = new();
            using (XmlWriter xmlTextWriter = XmlWriter.Create(memoryStream, xmlWriterSettings))
            {
                xmlDocument.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                StreamReader streamReader = new(memoryStream, Encoding.UTF8, true);
                memoryStream.Seek(0, SeekOrigin.Begin);
                xmlString = streamReader.ReadToEnd();
            }

            return xmlString;
        }
    }
}