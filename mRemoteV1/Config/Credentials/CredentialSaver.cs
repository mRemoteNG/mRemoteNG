using System.Collections;
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

        private XmlDocument BuildXmlDocument(IEnumerable credentialList)
        {
            var xmlDoc = new XmlDocument();
            var xmlSerializer = new XmlSerializer(typeof(CredentialInfo));
            using (var xmlStream = new MemoryStream())
            {
                foreach (var credential in credentialList)
                {
                    xmlSerializer.Serialize(xmlStream, credential);
                }
                
                xmlStream.Position = 0;
                xmlDoc.Load(xmlStream);
            }
            return xmlDoc;
        }
    }
}