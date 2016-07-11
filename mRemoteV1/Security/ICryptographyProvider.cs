using System.Security;

namespace mRemoteNG.Security
{
    public interface ICryptographyProvider
    {
        string Encrypt(string plainText, SecureString encryptionKey);

        string Decrypt(string cipherText, SecureString decryptionKey);
    }
}