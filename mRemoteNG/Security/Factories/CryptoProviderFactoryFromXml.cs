using System;
using System.Runtime.Versioning;
using System.Xml.Linq;
using mRemoteNG.Security.KeyDerivation;
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
            // Files written before encryption settings were stored have none of these attributes
            // and really do use the legacy provider.
            if (_element.Attribute("EncryptionEngine") == null)
                return new LegacyRijndaelCryptographyProvider();

            // From here on the file says how it was encrypted. If that can't be read, refuse rather than
            // quietly falling back to the legacy provider, which would hide tampering or corruption.
            try
            {
                BlockCipherEngines engine = ParseEnum<BlockCipherEngines>("EncryptionEngine");
                BlockCipherModes mode = ParseEnum<BlockCipherModes>("BlockCipherMode");
                ICryptographyProvider cryptoProvider = new CryptoProviderFactory(engine, mode).Build();

                cryptoProvider.KeyDerivationIterations = int.Parse(_element.Attribute("KdfIterations")?.Value ?? "");
                cryptoProvider.KeyDerivationAlgorithm = _element.Attribute("KdfAlgorithm") == null
                    ? KeyDerivationAlgorithms.Pbkdf2Sha1Legacy
                    : ParseEnum<KeyDerivationAlgorithms>("KdfAlgorithm");
                return cryptoProvider;
            }
            catch (Exception ex) when (ex is FormatException or ArgumentException or OverflowException)
            {
                throw new EncryptionException("The file's encryption settings are missing or invalid.", ex);
            }
        }

        private T ParseEnum<T>(string attributeName) where T : struct, Enum
        {
            string value = _element.Attribute(attributeName)?.Value ?? "";
            if (!Enum.TryParse(value, out T result) || !Enum.IsDefined(result))
                throw new FormatException($"Unknown {attributeName} '{value}'");
            return result;
        }
    }
}
