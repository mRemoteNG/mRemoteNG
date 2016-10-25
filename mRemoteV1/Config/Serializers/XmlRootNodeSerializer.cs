using System.Xml.Linq;
using mRemoteNG.Security;
using mRemoteNG.Tree.Root;


namespace mRemoteNG.Config.Serializers
{
    public class XmlRootNodeSerializer
    {
        public XElement SerializeRootNodeInfo(RootNodeInfo rootNodeInfo, ICryptographyProvider cryptographyProvider, bool fullFileEncryption = false, bool export = false)
        {
            var element = new XElement("Connections");
            element.Add(new XAttribute(XName.Get("Name"), rootNodeInfo.Name));
            element.Add(new XAttribute(XName.Get("Export"), export.ToString()));
            element.Add(new XAttribute(XName.Get("EncryptionEngine"), cryptographyProvider.CipherEngine));
            element.Add(new XAttribute(XName.Get("BlockCipherMode"), cryptographyProvider.CipherMode));
            element.Add(new XAttribute(XName.Get("KdfIterations"), cryptographyProvider.KeyDerivationIterations));
            element.Add(new XAttribute(XName.Get("FullFileEncryption"), fullFileEncryption.ToString()));
            element.Add(CreateProtectedAttribute(rootNodeInfo, cryptographyProvider, export));
            element.Add(new XAttribute(XName.Get("ConfVersion"), "2.6"));
            return element;
        }

        private XAttribute CreateProtectedAttribute(RootNodeInfo rootNodeInfo, ICryptographyProvider cryptographyProvider, bool export)
        {
            var attribute = new XAttribute(XName.Get("Protected"), "");
            var plainText = rootNodeInfo.Password && !export ? "ThisIsProtected" : "ThisIsNotProtected";
            var encryptionPassword = export
                ? rootNodeInfo.DefaultPassword.ConvertToSecureString()
                : rootNodeInfo.PasswordString.ConvertToSecureString();
            attribute.Value = cryptographyProvider.Encrypt(plainText, encryptionPassword);
            return attribute;
        }
    }
}