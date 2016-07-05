using System.IO;
using System.Xml;
using System.Xml.Serialization;
using mRemoteNG.Credential;

namespace mRemoteNG.Config.Credentials
{
    public class CredentialSaver
    {
        private string _credentialFilePath;

        public CredentialSaver(string credentialFilePath)
        {
            _credentialFilePath = credentialFilePath;
        }

        public void Save(CredentialList credentialList)
        {
            var xmlDoc = BuildXmlDocument(credentialList);
            xmlDoc.Save(_credentialFilePath);
        }

        private XmlDocument BuildXmlDocument(CredentialList credentialList)
        {
            var xmlDoc = new XmlDocument();
            var xmlSerializer = new XmlSerializer(typeof(CredentialList));
            using (var memoryStream = new MemoryStream())
            {
                xmlSerializer.Serialize(memoryStream, credentialList);
                memoryStream.Position = 0;
                xmlDoc.Load(memoryStream);
                return xmlDoc;
            }
        }
    }
}