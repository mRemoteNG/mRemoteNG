using System.Security;
using System.Xml.Linq;
using mRemoteNG.Security;

namespace mRemoteNG.Config.Serializers.ConnectionSerializers.Xml
{
    public class XmlConnectionsDocumentEncryptor(ICryptographyProvider cryptographyProvider)
    {
        private readonly ICryptographyProvider _cryptographyProvider = cryptographyProvider;

        public XDocument EncryptDocument(XDocument documentToEncrypt, SecureString encryptionKey)
        {
            XElement root = documentToEncrypt.Root
                ?? throw new System.InvalidOperationException("Cannot encrypt a document without a root element.");
            string contentToEncrypt = GetContentToEncrypt(root);
            string encryptedContent = _cryptographyProvider.Encrypt(contentToEncrypt, encryptionKey);
            XDocument encryptedDocument = ReplaceInnerXml(documentToEncrypt, encryptedContent);
            return encryptedDocument;
        }

        private static string GetContentToEncrypt(XNode element)
        {
            System.Xml.XmlReader reader = element.CreateReader();
            reader.MoveToContent();
            return reader.ReadInnerXml();
        }

        private static XDocument ReplaceInnerXml(XDocument originalDocument, string newContent)
        {
            XElement newRootElement = ShallowCloneRootNode(originalDocument.Root
                ?? throw new System.InvalidOperationException("Cannot replace inner XML of a document without a root element."));
            newRootElement.SetValue(newContent);
            return new XDocument(newRootElement);
        }

        private static XElement ShallowCloneRootNode(XElement originalElement)
        {
            XElement newElement = new(originalElement.Name);
            foreach (XAttribute attribute in originalElement.Attributes())
                newElement.Add(new XAttribute(attribute));
            return newElement;
        }
    }
}