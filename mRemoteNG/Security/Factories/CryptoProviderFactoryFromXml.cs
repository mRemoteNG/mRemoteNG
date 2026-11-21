using System;
using System.Runtime.Versioning;
using System.Xml.Linq;
using mRemoteNG.Security.SymmetricEncryption;

namespace mRemoteNG.Security.Factories
{
    [SupportedOSPlatform("windows")]
    public class CryptoProviderFactoryFromXml : ICryptoProviderFactory
    {
        private readonly XElement _element;

        public CryptoProviderFactoryFromXml(XElement element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            _element = element;
        }

        public ICryptographyProvider Build()
        {
            ICryptographyProvider cryptoProvider;
            try
            {
                BlockCipherEngines engine = (BlockCipherEngines)Enum.Parse(typeof(BlockCipherEngines),
                                                            _element?.Attribute("EncryptionEngine")?.Value ?? "");
                BlockCipherModes mode = (BlockCipherModes)Enum.Parse(typeof(BlockCipherModes),
                                                        _element?.Attribute("BlockCipherMode")?.Value ?? "");
                int keyDerivationIterations = int.Parse(_element?.Attribute("KdfIterations")?.Value ?? "");

                // A connections file from format 3.0 on has one salt for the whole file.
                string salt = _element?.Attribute("KdfSalt")?.Value;
                if (!string.IsNullOrEmpty(salt))
                    return new CryptoProviderFactory(engine, mode).BuildWithKeyDerivedOnce(Convert.FromBase64String(salt), keyDerivationIterations);

                cryptoProvider = new CryptoProviderFactory(engine, mode).Build();
                cryptoProvider.KeyDerivationIterations = keyDerivationIterations;
            }
            catch (Exception)
            {
                return new LegacyRijndaelCryptographyProvider();
            }

            return cryptoProvider;
        }
    }
}