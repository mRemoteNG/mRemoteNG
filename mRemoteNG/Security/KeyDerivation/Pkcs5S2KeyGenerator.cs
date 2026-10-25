using System;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;


namespace mRemoteNG.Security.KeyDerivation
{
    public class Pkcs5S2KeyGenerator : IKeyDerivationFunction
    {
        private readonly int _iterations;
        private readonly int _keyBitSize;
        private readonly KeyDerivationAlgorithms _algorithm;

        public Pkcs5S2KeyGenerator(int keyBitSize = 256, int iterations = 1000, KeyDerivationAlgorithms algorithm = KeyDerivationAlgorithms.Pbkdf2Sha1Legacy)
        {
            if (iterations < 1000)
                throw new ArgumentOutOfRangeException($"Minimum value of {nameof(iterations)} is 1000");
            if (keyBitSize < 0)
                throw new ArgumentOutOfRangeException($"{nameof(keyBitSize)} must be positive");
            _keyBitSize = keyBitSize;
            _iterations = iterations;
            _algorithm = algorithm;
        }

        public byte[] DeriveKey(string password, byte[] salt)
        {
            return _algorithm switch
            {
                KeyDerivationAlgorithms.Pbkdf2Sha256 => DeriveSha256(password, salt),
                _ => DeriveSha1Legacy(password, salt),
            };
        }

        private byte[] DeriveSha256(string password, byte[] salt)
        {
            byte[] passwordInBytes = Encoding.UTF8.GetBytes(password);
            try
            {
                return Rfc2898DeriveBytes.Pbkdf2(passwordInBytes, salt, _iterations, HashAlgorithmName.SHA256, _keyBitSize / 8);
            }
            finally
            {
                CryptographicOperations.ZeroMemory(passwordInBytes);
            }
        }

        // Kept byte-for-byte compatible so files written before KdfAlgorithm existed still decrypt
        private byte[] DeriveSha1Legacy(string password, byte[] salt)
        {
            byte[] passwordInBytes = PbeParametersGenerator.Pkcs5PasswordToBytes(password.ToCharArray());

            Pkcs5S2ParametersGenerator keyGenerator = new();
            keyGenerator.Init(passwordInBytes, salt, _iterations);

            KeyParameter keyParameter = (KeyParameter)keyGenerator.GenerateDerivedMacParameters(_keyBitSize);
            byte[] keyBytes = keyParameter.GetKey();
            return keyBytes;
        }
    }
}
