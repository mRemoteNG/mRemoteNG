using System;
using System.IO;
using System.Runtime.Versioning;
using System.Security;
using System.Xml.Linq;

using mRemoteNG.Security.Factories;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Security
{
    [SupportedOSPlatform("windows")]
    public static class XmlKeyValidator
    {
        public static bool ConnectionFileRequiresPassword(string filePath)
        {
            if (!File.Exists(filePath))
                return false;

            string xml = File.ReadAllText(filePath);
            if (string.IsNullOrWhiteSpace(xml))
                return false;

            if (!LooksLikeXml(xml) || !TryLoadRoot(xml, out XElement? root))
                return true;

            string? protectedValue = root.Attribute("Protected")?.Value;
            if (string.IsNullOrWhiteSpace(protectedValue))
                return false;

            try
            {
                ICryptographyProvider cryptoProvider = new CryptoProviderFactoryFromXml(root).Build();
                string plainText = cryptoProvider.Decrypt(protectedValue, CreateDefaultConnectionKey());
                return plainText != "ThisIsNotProtected";
            }
            catch
            {
                return true;
            }
        }

        public static bool ConnectionFileUsesKey(string filePath, SecureString key)
        {
            if (!File.Exists(filePath))
                return false;

            string xml = File.ReadAllText(filePath);
            if (string.IsNullOrWhiteSpace(xml) || !LooksLikeXml(xml) || !TryLoadRoot(xml, out XElement? root))
                return false;

            string? protectedValue = root.Attribute("Protected")?.Value;
            if (string.IsNullOrWhiteSpace(protectedValue))
                return true;

            try
            {
                ICryptographyProvider cryptoProvider = new CryptoProviderFactoryFromXml(root).Build();
                string plainText = cryptoProvider.Decrypt(protectedValue, key);
                return plainText == "ThisIsProtected" || plainText == "ThisIsNotProtected";
            }
            catch
            {
                return false;
            }
        }

        public static bool CredentialsFileUsesKey(string filePath, SecureString key)
        {
            if (!File.Exists(filePath))
                return true;

            string xml = File.ReadAllText(filePath);
            if (string.IsNullOrWhiteSpace(xml))
                return true;

            if (!LooksLikeXml(xml) || !TryLoadRoot(xml, out XElement? root))
                return false;

            string? authValue = root.Attribute("Auth")?.Value;
            if (string.IsNullOrWhiteSpace(authValue))
                return true;

            try
            {
                ICryptographyProvider cryptoProvider = new CryptoProviderFactoryFromXml(root).Build();
                _ = cryptoProvider.Decrypt(authValue, key);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool LooksLikeXml(string xml)
        {
            return xml.TrimStart().StartsWith("<", StringComparison.Ordinal);
        }

        private static bool TryLoadRoot(string xml, out XElement? root)
        {
            root = null;

            try
            {
                root = XElement.Parse(xml, LoadOptions.None);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static SecureString CreateDefaultConnectionKey()
        {
            return new RootNodeInfo(RootNodeType.Connection).PasswordString.ConvertToSecureString();
        }
    }
}
