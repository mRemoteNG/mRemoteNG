using System.Security;

namespace mRemoteNG.Security
{
    public interface ICryptographyProvider
    {
        int BlockSizeInBytes { get; }

        BlockCipherEngines CipherEngine { get; }

        BlockCipherModes CipherMode { get; }

        int KeyDerivationIterations { get; set; }

        string Encrypt(string plainText, SecureString encryptionKey);

        string Decrypt(string cipherText, SecureString decryptionKey);
    }
}