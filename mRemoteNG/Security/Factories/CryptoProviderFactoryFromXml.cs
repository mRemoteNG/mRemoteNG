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
                var engine = (BlockCipherEngines)Enum.Parse(typeof(BlockCipherEngines),
                                                            _element?.Attribute("EncryptionEngine")?.Value ?? "");
                var mode = (BlockCipherModes)Enum.Parse(typeof(BlockCipherModes),
                                                        _element?.Attribute("BlockCipherMode")?.Value ?? "");
                cryptoProvider = new CryptoProviderFactory(engine, mode).Build();

                var keyDerivationIterations = int.Parse(_element?.Attribute("KdfIterations")?.Value ?? "");
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