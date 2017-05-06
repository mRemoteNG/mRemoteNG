using System.Xml.Linq;
using mRemoteNG.Security;
using mRemoteNG.Tree.Root;


namespace mRemoteNG.Config.Serializers
{
    public class XmlRootNodeSerializer
    {
        public XElement SerializeRootNodeInfo(RootNodeInfo rootNodeInfo, ICryptographyProvider cryptographyProvider, bool fullFileEncryption = false)
        {
            var element = new XElement("Connections");
            element.Add(new XAttribute(XName.Get("Name"), rootNodeInfo.Name));
            element.Add(new XAttribute(XName.Get("EncryptionEngine"), cryptographyProvider.CipherEngine));
            element.Add(new XAttribute(XName.Get("BlockCipherMode"), cryptographyProvider.CipherMode));
            element.Add(new XAttribute(XName.Get("KdfIterations"), cryptographyProvider.KeyDerivationIterations));
            element.Add(new XAttribute(XName.Get("FullFileEncryption"), fullFileEncryption.ToString()));
            element.Add(CreateProtectedAttribute(rootNodeInfo, cryptographyProvider));
            element.Add(new XAttribute(XName.Get("ConfVersion"), "2.8"));
            return element;
        }

        private XAttribute CreateProtectedAttribute(RootNodeInfo rootNodeInfo, ICryptographyProvider cryptographyProvider)
        {
            var attribute = new XAttribute(XName.Get("Protected"), "");
            var plainText = rootNodeInfo.Password ? "ThisIsProtected" : "ThisIsNotProtected";
            var encryptionPassword = rootNodeInfo.PasswordString.ConvertToSecureString();
            attribute.Value = cryptographyProvider.Encrypt(plainText, encryptionPassword);
            return attribute;
        }
    }
}