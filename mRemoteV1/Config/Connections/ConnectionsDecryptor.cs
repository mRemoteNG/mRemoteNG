using System;
using mRemoteNG.App;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tree.Root;


namespace mRemoteNG.Config.Connections
{
    public class ConnectionsDecryptor
    {
        private readonly ICryptographyProvider _cryptographyProvider;

        public ConnectionsDecryptor()
        {
            _cryptographyProvider = new LegacyRijndaelCryptographyProvider();
        }

        public ConnectionsDecryptor(BlockCipherEngines blockCipherEngine, BlockCipherModes blockCipherMode)
        {
            _cryptographyProvider = new CryptographyProviderFactory().CreateAeadCryptographyProvider(blockCipherEngine, blockCipherMode);
        }

        public string Decrypt(string plainText)
        {
            return _cryptographyProvider.Decrypt(plainText, Runtime.EncryptionKey);
        }

        public string DecryptConnections(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return "";
            if (xml.Contains("<?xml version=\"1.0\" encoding=\"utf-8\"?>")) return xml;

            var decryptedContent = "";
            bool notDecr;

            try
            {
                decryptedContent = _cryptographyProvider.Decrypt(xml, Runtime.EncryptionKey);
                notDecr = decryptedContent == xml;
            }
            catch (Exception)
            {
                notDecr = true;
            }

            if (notDecr)
            {
                if (Authenticate(xml, true))
                {
                    decryptedContent = _cryptographyProvider.Decrypt(xml, Runtime.EncryptionKey);
                    notDecr = false;
                }

                if (notDecr == false)
                    return decryptedContent;
            }
            else
            {
                return decryptedContent;
            }

            return "";
        }

        public bool ConnectionsFileIsAuthentic(string protectedString, RootNodeInfo rootInfo)
        {
            var connectionsFileIsNotEncrypted = _cryptographyProvider.Decrypt(protectedString, Runtime.EncryptionKey) == "ThisIsNotProtected";
            return connectionsFileIsNotEncrypted || Authenticate(protectedString, false, rootInfo);
        }

        private bool Authenticate(string value, bool compareToOriginalValue, RootNodeInfo rootInfo = null)
        {
            var passwordName = "";
            //passwordName = Path.GetFileName(ConnectionFileName);

            if (compareToOriginalValue)
            {
                while (_cryptographyProvider.Decrypt(value, Runtime.EncryptionKey) == value)
                {
                    Runtime.EncryptionKey = Tools.MiscTools.PasswordDialog(passwordName, false);
                    if (Runtime.EncryptionKey.Length == 0)
                        return false;
                }
            }
            else
            {
                while (_cryptographyProvider.Decrypt(value, Runtime.EncryptionKey) != "ThisIsProtected")
                {
                    Runtime.EncryptionKey = Tools.MiscTools.PasswordDialog(passwordName, false);
                    if (Runtime.EncryptionKey.Length == 0)
                        return false;
                }

                if (rootInfo == null) return true;
                rootInfo.Password = true;
                rootInfo.PasswordString = Runtime.EncryptionKey.ConvertToUnsecureString();
            }

            return true;
        }
    }
}