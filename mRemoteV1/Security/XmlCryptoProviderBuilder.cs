using System;
using System.Xml.Linq;


namespace mRemoteNG.Security
{
    public class XmlCryptoProviderBuilder
    {
        private readonly XElement _element;

        public XmlCryptoProviderBuilder(XElement element)
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
                var engine = (BlockCipherEngines)Enum.Parse(typeof(BlockCipherEngines), _element?.Attribute("EncryptionEngine")?.Value ?? "");
                var mode = (BlockCipherModes)Enum.Parse(typeof(BlockCipherModes), _element?.Attribute("BlockCipherMode")?.Value ?? "");
                cryptoProvider = new CryptographyProviderFactory().CreateAeadCryptographyProvider(engine, mode);

                var keyDerivationIterations = int.Parse(_element?.Attribute("KdfIterations")?.Value ?? "");
                cryptoProvider.KeyDerivationIterations = keyDerivationIterations;
            }
            catch (Exception)
            {
                return new CryptographyProviderFactory().CreateLegacyRijndaelCryptographyProvider();
            }

            return cryptoProvider;
        }
    }
}