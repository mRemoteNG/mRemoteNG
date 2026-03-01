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

        public Pkcs5S2KeyGenerator(int keyBitSize = 256, int iterations = 1000)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(iterations, 1000);
            ArgumentOutOfRangeException.ThrowIfNegative(keyBitSize);
            _keyBitSize = keyBitSize;
            _iterations = iterations;
        }

        public byte[] DeriveKey(string password, byte[] salt)
        {
            byte[] passwordInBytes = Encoding.UTF8.GetBytes(password);
            try
            {
                Pkcs5S2ParametersGenerator keyGenerator = new();
                keyGenerator.Init(passwordInBytes, salt, _iterations);

                KeyParameter keyParameter = (KeyParameter)keyGenerator.GenerateDerivedMacParameters(_keyBitSize);
                byte[] keyBytes = keyParameter.GetKey();
                return keyBytes;
            }
            finally
            {
                CryptographicOperations.ZeroMemory(passwordInBytes);
            }
        }
    }
}