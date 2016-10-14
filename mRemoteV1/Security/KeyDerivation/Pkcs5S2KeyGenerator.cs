using System;
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
            if (iterations < 1000)
                throw new ArgumentOutOfRangeException($"Minimum value of {nameof(iterations)} is 1000");
            if (keyBitSize < 0)
                throw new ArgumentOutOfRangeException($"{nameof(keyBitSize)} must be positive");
            _keyBitSize = keyBitSize;
            _iterations = iterations;
        }

        public byte[] DeriveKey(string password, byte[] salt)
        {
            var passwordInBytes = PbeParametersGenerator.Pkcs5PasswordToBytes(password.ToCharArray());

            var keyGenerator = new Pkcs5S2ParametersGenerator();
            keyGenerator.Init(passwordInBytes, salt, _iterations);

            var keyParameter = (KeyParameter) keyGenerator.GenerateDerivedMacParameters(_keyBitSize);
            var keyBytes = keyParameter.GetKey();
            return keyBytes;
        }
    }
}