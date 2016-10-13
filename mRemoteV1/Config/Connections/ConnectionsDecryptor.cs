using System;
using mRemoteNG.App;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tree.Root;
using Org.BouncyCastle.Security;


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
            return plainText == "" ? "" : _cryptographyProvider.Decrypt(plainText, Runtime.EncryptionKey);
        }

        public string LegacyFullFileDecrypt(string xml)
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
            var connectionsFileIsNotEncrypted = false;
            try
            {
                connectionsFileIsNotEncrypted = _cryptographyProvider.Decrypt(protectedString, Runtime.EncryptionKey) == "ThisIsNotProtected";
            }
            catch (EncryptionException)
            {
            }
            return connectionsFileIsNotEncrypted || Authenticate(protectedString, false, rootInfo);
        }

        private bool Authenticate(string value, bool compareToOriginalValue, RootNodeInfo rootInfo = null)
        {
            var passwordName = "";
            //passwordName = Path.GetFileName(ConnectionFileName);

            var stillEncrypted = true;
            if (compareToOriginalValue)
            {
                while (stillEncrypted)
                {
                    Runtime.EncryptionKey = Tools.MiscTools.PasswordDialog(passwordName, false);
                    if (Runtime.EncryptionKey.Length == 0)
                        return false;

                    try
                    {
                        stillEncrypted = _cryptographyProvider.Decrypt(value, Runtime.EncryptionKey) == value;
                    }
                    catch(EncryptionException) { }
                }
            }
            else
            {
                while (stillEncrypted)
                {
                    Runtime.EncryptionKey = Tools.MiscTools.PasswordDialog(passwordName, false);
                    if (Runtime.EncryptionKey.Length == 0)
                        return false;

                    try
                    {
                        stillEncrypted = _cryptographyProvider.Decrypt(value, Runtime.EncryptionKey) != "ThisIsProtected";
                    }
                    catch (EncryptionException) { }
                }

                if (rootInfo == null) return true;
                rootInfo.Password = true;
                rootInfo.PasswordString = Runtime.EncryptionKey.ConvertToUnsecureString();
            }

            return true;
        }
    }
}