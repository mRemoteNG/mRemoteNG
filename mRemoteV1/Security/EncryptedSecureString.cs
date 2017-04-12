using System.Security;
using mRemoteNG.Security.SymmetricEncryption;
using Org.BouncyCastle.Security;
// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.Security
{
    public class EncryptedSecureString
    {
        private static SecureString _machineKey;
        private SecureString _secureString;
        private readonly ICryptographyProvider _cryptographyProvider;

        private static SecureString MachineKey
        {
            get { return _machineKey ?? (_machineKey = GenerateNewMachineKey(32)); }
        }

        public EncryptedSecureString()
        {
            _secureString = new SecureString();
            _cryptographyProvider = new AeadCryptographyProvider();
        }

        public EncryptedSecureString(ICryptographyProvider cryptographyProvider)
        {
            _secureString = new SecureString();
            _cryptographyProvider = cryptographyProvider;
        }

        public string GetClearTextValue()
        {
            var encryptedText = _secureString.ConvertToUnsecureString();
            var clearText = _cryptographyProvider.Decrypt(encryptedText, MachineKey);
            return clearText;
        }

        public void SetValue(string value)
        {
            var cipherText = _cryptographyProvider.Encrypt(value, MachineKey);
            _secureString = cipherText.ConvertToSecureString();
        }

        private static SecureString GenerateNewMachineKey(int keySize)
        {
            var random = new SecureRandom();
            random.SetSeed(random.GenerateSeed(128));

            var machineKeyString = "";
            for (var x = 0; x < keySize; x++)
            {
                machineKeyString += (char)random.Next(33, 126);
            }
            
            return machineKeyString.ConvertToSecureString();
        }
    }
}