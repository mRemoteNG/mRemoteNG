using System;
using System.Xml.Linq;
using mRemoteNG.Security.SymmetricEncryption;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;

namespace mRemoteNG.Security
{
    public class CryptographyProviderFactory
    {
        public ICryptographyProvider CreateAeadCryptographyProvider(BlockCipherEngines engine, BlockCipherModes mode)
        {
            var cipherEngine = ChooseBlockCipherEngine(engine);
            var cipher = ChooseBlockCipherMode(mode, cipherEngine);
            return new AeadCryptographyProvider(cipher);
        }

        public ICryptographyProvider CreateLegacyRijndaelCryptographyProvider()
        {
            return new LegacyRijndaelCryptographyProvider();
        }

        public static ICryptographyProvider BuildFromXml(XElement element)
        {
            var builder = new XmlCryptoProviderBuilder(element);
            return builder.Build();
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