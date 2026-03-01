using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace mRemoteNG.Config.Serializers.ConnectionSerializers.Sql
{
    public class LocalConnectionPropertiesXmlSerializer :
        ISerializer<IEnumerable<LocalConnectionPropertiesModel>, string>,
        IDeserializer<string, IEnumerable<LocalConnectionPropertiesModel>>
    {
        public Version Version { get; } = new Version(1, 0);

        public string Serialize(IEnumerable<LocalConnectionPropertiesModel> models)
        {
            IEnumerable<XElement> localConnections = models
                .Select(m => new XElement("Node",
                                          new XAttribute("ConnectionId", m.ConnectionId),
                                          new XAttribute("Connected", m.Connected),
                                          new XAttribute("Expanded", m.Expanded),
                                          new XAttribute("Favorite", m.Favorite)));

            XElement root = new("LocalConnections", localConnections);
            XDocument xdoc = new(new XDeclaration("1.0", "utf-8", null), root);
            return WriteXmlToString(xdoc);
        }

        public IEnumerable<LocalConnectionPropertiesModel> Deserialize(string serializedData)
        {
            if (string.IsNullOrWhiteSpace(serializedData))
                return Enumerable.Empty<LocalConnectionPropertiesModel>();

            XDocument xdoc = XDocument.Parse(serializedData);
            return xdoc
                   .Descendants("Node")
                   .Where(e => e.Attribute("ConnectionId") != null)
                   .Select(e => new LocalConnectionPropertiesModel
                   {
                       ConnectionId = e.Attribute("ConnectionId")?.Value ?? string.Empty,
                       Connected = bool.Parse(e.Attribute("Connected")?.Value ?? "False"),
                       Expanded = bool.Parse(e.Attribute("Expanded")?.Value ?? "False"),
                       Favorite = bool.Parse(e.Attribute("Favorite")?.Value ?? "False")
                   });
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