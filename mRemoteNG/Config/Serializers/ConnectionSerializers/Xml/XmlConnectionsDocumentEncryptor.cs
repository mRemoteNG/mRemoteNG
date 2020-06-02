using System.Security;
using System.Xml.Linq;
using mRemoteNG.Security;

namespace mRemoteNG.Config.Serializers.Xml
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
            var contentToEncrypt = GetContentToEncrypt(documentToEncrypt.Root);
            var encryptedContent = _cryptographyProvider.Encrypt(contentToEncrypt, encryptionKey);
            var encryptedDocument = ReplaceInnerXml(documentToEncrypt, encryptedContent);
            return encryptedDocument;
        }

        private string GetContentToEncrypt(XNode element)
        {
            var reader = element.CreateReader();
            reader.MoveToContent();
            return reader.ReadInnerXml();
        }

        private XDocument ReplaceInnerXml(XDocument originalDocument, string newContent)
        {
            var newRootElement = ShallowCloneRootNode(originalDocument.Root);
            newRootElement.SetValue(newContent);
            return new XDocument(newRootElement);
        }

        private XElement ShallowCloneRootNode(XElement originalElement)
        {
            var newElement = new XElement(originalElement.Name);
            foreach (var attribute in originalElement.Attributes())
                newElement.Add(new XAttribute(attribute));
            return newElement;
        }
    }
}