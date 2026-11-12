using System;
using System.Runtime.Versioning;
using System.Xml.Linq;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Serializers.ConnectionSerializers.Xml
{
    [SupportedOSPlatform("windows")]
    public class XmlRootNodeSerializer
    {
        public XElement SerializeRootNodeInfo(RootNodeInfo rootNodeInfo, ICryptographyProvider cryptographyProvider, Version version, bool fullFileEncryption = false)
        {
            XNamespace xmlNamespace = "http://mremoteng.org";
            XElement element = new(xmlNamespace + "Connections");
            element.Add(new XAttribute(XNamespace.Xmlns + "mrng", xmlNamespace));
            element.Add(new XAttribute(XName.Get("Name"), rootNodeInfo.Name));
            element.Add(new XAttribute(XName.Get("Export"), "false"));
            // EncryptionEngine/BlockCipherMode/KdfIterations are meaningless for DPAPI (it has none of
            // these), but kept for XML shape compatibility with readers that expect them to be
            // present; XmlConnectionsDeserializer ignores them when KeyProtection="Dpapi" is set below.
            element.Add(new XAttribute(XName.Get("EncryptionEngine"), cryptographyProvider.CipherEngine));
            element.Add(new XAttribute(XName.Get("BlockCipherMode"), cryptographyProvider.CipherMode));
            element.Add(new XAttribute(XName.Get("KdfIterations"), cryptographyProvider.KeyDerivationIterations));
            element.Add(new XAttribute(XName.Get("FullFileEncryption"), fullFileEncryption.ToString().ToLowerInvariant()));
            if (cryptographyProvider is DpapiCryptographyProvider)
                element.Add(new XAttribute(XName.Get("KeyProtection"), "Dpapi"));
            element.Add(CreateProtectedAttribute(rootNodeInfo, cryptographyProvider));
            element.Add(new XAttribute(XName.Get("ConfVersion"), version.ToString(2)));
            return element;
        }

        private XAttribute CreateProtectedAttribute(RootNodeInfo rootNodeInfo, ICryptographyProvider cryptographyProvider)
        {
            XAttribute attribute = new(XName.Get("Protected"), "");
            string plainText = (rootNodeInfo.PasswordString != rootNodeInfo.DefaultPassword) ? "ThisIsProtected" : "ThisIsNotProtected";
            System.Security.SecureString encryptionPassword = rootNodeInfo.PasswordString.ConvertToSecureString();
            attribute.Value = cryptographyProvider.Encrypt(plainText, encryptionPassword);
            return attribute;
        }
    }
}