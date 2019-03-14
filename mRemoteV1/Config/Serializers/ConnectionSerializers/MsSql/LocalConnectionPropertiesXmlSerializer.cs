using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace mRemoteNG.Config.Serializers.MsSql
{
    public class LocalConnectionPropertiesXmlSerializer : 
        ISerializer<IEnumerable<LocalConnectionPropertiesModel>, string>, 
        IDeserializer<string, IEnumerable<LocalConnectionPropertiesModel>>
    {
        public string Serialize(IEnumerable<LocalConnectionPropertiesModel> models)
        {
            var localConnections = models
                .Select(m => new XElement("Node", 
                    new XAttribute("ConnectionId", m.ConnectionId),
                    new XAttribute("Connected", m.Connected),
                    new XAttribute("Expanded", m.Expanded)));

            var root = new XElement("LocalConnections", localConnections);
            var xdoc = new XDocument(new XDeclaration("1.0", "utf-8", null), root);
            return WriteXmlToString(xdoc);
        }

        public IEnumerable<LocalConnectionPropertiesModel> Deserialize(string serializedData)
        {
            if (string.IsNullOrWhiteSpace(serializedData))
                return Enumerable.Empty<LocalConnectionPropertiesModel>();

            var xdoc = XDocument.Parse(serializedData);
            return xdoc
                .Descendants("Node")
                .Where(e => e.Attribute("ConnectionId") != null)
                .Select(e => new LocalConnectionPropertiesModel
                {
                    ConnectionId = e.Attribute("ConnectionId")?.Value,
                    Connected = bool.Parse(e.Attribute("Connected")?.Value ?? "False"),
                    Expanded = bool.Parse(e.Attribute("Expanded")?.Value ?? "False")
                });
        }

        private static string WriteXmlToString(XNode xmlDocument)
        {
            string xmlString;
            var xmlWriterSettings = new XmlWriterSettings { Indent = true, IndentChars = "    ", Encoding = Encoding.UTF8 };
            var memoryStream = new MemoryStream();
            using (var xmlTextWriter = XmlWriter.Create(memoryStream, xmlWriterSettings))
            {
                xmlDocument.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                var streamReader = new StreamReader(memoryStream, Encoding.UTF8, true);
                memoryStream.Seek(0, SeekOrigin.Begin);
                xmlString = streamReader.ReadToEnd();
            }
            return xmlString;
        }
    }
}
