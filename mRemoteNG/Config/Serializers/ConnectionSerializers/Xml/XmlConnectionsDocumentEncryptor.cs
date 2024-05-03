using System.Security;
using System.Xml.Linq;
using mRemoteNG.Security;

namespace mRemoteNG.Config.Serializers.ConnectionSerializers.Xml
{
    public class XmlConnectionsDocumentEncryptor
    {
        private readonly ICryptographyProvider _cryptographyProvider;

        public XmlConnectionsDocumentEncryptor(ICryptographyProvider cryptographyProvider)
        {
            _cryptographyProvider = cryptographyProvider;
        }

        public XDocument EncryptDocument(XDocument documentToEncrypt, SecureString encryptionKey)
        {
            string contentToEncrypt = GetContentToEncrypt(documentToEncrypt.Root);
            string encryptedContent = _cryptographyProvider.Encrypt(contentToEncrypt, encryptionKey);
            XDocument encryptedDocument = ReplaceInnerXml(documentToEncrypt, encryptedContent);
            return encryptedDocument;
        }

        private string GetContentToEncrypt(XNode element)
        {
            System.Xml.XmlReader reader = element.CreateReader();
            reader.MoveToContent();
            return reader.ReadInnerXml();
        }

        private XDocument ReplaceInnerXml(XDocument originalDocument, string newContent)
        {
            XElement newRootElement = ShallowCloneRootNode(originalDocument.Root);
            newRootElement.SetValue(newContent);
            return new XDocument(newRootElement);
        }

        private XElement ShallowCloneRootNode(XElement originalElement)
        {
            XElement newElement = new(originalElement.Name);
            foreach (XAttribute attribute in originalElement.Attributes())
                newElement.Add(new XAttribute(attribute));
            return newElement;
        }
    }
}