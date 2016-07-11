using System.Security;

namespace mRemoteNG.Security
{
    public interface ICryptographyProvider
    {
        int BlockSizeInBytes { get; }

        string Encrypt(string plainText, SecureString encryptionKey);

        string Decrypt(string cipherText, SecureString decryptionKey);
    }
}