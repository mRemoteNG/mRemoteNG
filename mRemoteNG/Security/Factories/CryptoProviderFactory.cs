using System;
using mRemoteNG.Security.SymmetricEncryption;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;

namespace mRemoteNG.Security.Factories
{
    public class CryptoProviderFactory : ICryptoProviderFactory
    {
        private readonly IAeadBlockCipher _aeadBlockCipher;

        public CryptoProviderFactory(BlockCipherEngines engine, BlockCipherModes mode)
        {
            IBlockCipher cipherEngine = ChooseBlockCipherEngine(engine);
            _aeadBlockCipher = ChooseBlockCipherMode(mode, cipherEngine);
        }

        public ICryptographyProvider Build()
        {
            return new AeadCryptographyProvider(_aeadBlockCipher);
        }

        private static IBlockCipher ChooseBlockCipherEngine(BlockCipherEngines engine)
        {
            return engine switch
            {
                BlockCipherEngines.AES => new AesEngine(),
                BlockCipherEngines.Twofish => new TwofishEngine(),
                BlockCipherEngines.Serpent => new SerpentEngine(),
                _ => throw new ArgumentOutOfRangeException(nameof(engine), engine, null),
            };
        }

        private static IAeadBlockCipher ChooseBlockCipherMode(BlockCipherModes mode, IBlockCipher blockCipher)
        {
            return mode switch
            {
                BlockCipherModes.GCM => new GcmBlockCipher(blockCipher),
                BlockCipherModes.CCM => new CcmBlockCipher(blockCipher),
                BlockCipherModes.EAX => new EaxBlockCipher(blockCipher),
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null),
            };
        }
    }
}