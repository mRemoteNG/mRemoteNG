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
            var cipherEngine = ChooseBlockCipherEngine(engine);
            _aeadBlockCipher = ChooseBlockCipherMode(mode, cipherEngine);
        }

        public ICryptographyProvider Build()
        {
            return new AeadCryptographyProvider(_aeadBlockCipher);
        }

        private IBlockCipher ChooseBlockCipherEngine(BlockCipherEngines engine)
        {
            switch (engine)
            {
                case BlockCipherEngines.AES:
                    return new AesEngine();
                case BlockCipherEngines.Twofish:
                    return new TwofishEngine();
                case BlockCipherEngines.Serpent:
                    return new SerpentEngine();
                default:
                    throw new ArgumentOutOfRangeException(nameof(engine), engine, null);
            }
        }

        private IAeadBlockCipher ChooseBlockCipherMode(BlockCipherModes mode, IBlockCipher blockCipher)
        {
            switch (mode)
            {
                case BlockCipherModes.GCM:
                    return new GcmBlockCipher(blockCipher);
                case BlockCipherModes.CCM:
                    return new CcmBlockCipher(blockCipher);
                case BlockCipherModes.EAX:
                    return new EaxBlockCipher(blockCipher);
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }
    }
}