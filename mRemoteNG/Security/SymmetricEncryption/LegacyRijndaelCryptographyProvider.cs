using System;
using System.IO;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Resources.Language;


namespace mRemoteNG.Security.SymmetricEncryption
{
    [SupportedOSPlatform("windows")]
    public class LegacyRijndaelCryptographyProvider : ICryptographyProvider
    {
        public int BlockSizeInBytes { get; }

        public BlockCipherEngines CipherEngine { get; }

        public BlockCipherModes CipherMode { get; }
        public int KeyDerivationIterations { get; set; }

        public LegacyRijndaelCryptographyProvider()
        {
            BlockSizeInBytes = 16;
        }

        public string Encrypt(string strToEncrypt, SecureString strSecret)
        {
            if (string.IsNullOrWhiteSpace(strToEncrypt) || strSecret.Length == 0)
                return strToEncrypt;

            try
            {
                using var aes = Aes.Create();
                aes.BlockSize = BlockSizeInBytes * 8;

                using (var md5 = MD5.Create())
                {
                    var key = md5.ComputeHash(Encoding.UTF8.GetBytes(strSecret.ConvertToUnsecureString()));
                    aes.Key = key;
                    aes.GenerateIV();
                }

                using var ms = new MemoryStream();
                ms.Write(aes.IV, 0, BlockSizeInBytes);

                using var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
                var data = Encoding.UTF8.GetBytes(strToEncrypt);

                cs.Write(data, 0, data.Length);
                cs.FlushFinalBlock();

                var encdata = ms.ToArray();

                return Convert.ToBase64String(encdata);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, string.Format(Language.ErrorEncryptionFailed, ex.Message));
            }

            return strToEncrypt;
        }

        public string Decrypt(string ciphertextBase64, SecureString password)
        {
            if (string.IsNullOrEmpty(ciphertextBase64) || password.Length == 0)
                return ciphertextBase64;

            try
            {
                using var aes = Aes.Create();
                aes.BlockSize = BlockSizeInBytes * 8;

                using (var md5 = MD5.Create())
                {
                    var key = md5.ComputeHash(Encoding.UTF8.GetBytes(password.ConvertToUnsecureString()));
                    aes.Key = key;
                }

                var ciphertext = Convert.FromBase64String(ciphertextBase64);

                using var ms = new MemoryStream(ciphertext);

                var iv = new byte[BlockSizeInBytes];
                ms.Read(iv, 0, iv.Length);
                aes.IV = iv;

                using var cryptoStream = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
                using var streamReader = new StreamReader(cryptoStream, Encoding.UTF8, true);
                var plaintext = streamReader.ReadToEnd();

                return plaintext;
            }
            catch (Exception ex)
            {
                //Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, string.Format(Language.ErrorDecryptionFailed, ex.Message));
                throw new EncryptionException(Language.ErrorDecryptionFailed, ex);
            }
        }
    }
}