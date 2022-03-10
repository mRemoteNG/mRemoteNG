using mRemoteNG.Properties;

namespace mRemoteNG.Security.Factories
{
    public class CryptoProviderFactoryFromSettings : ICryptoProviderFactory
    {
        public ICryptographyProvider Build()
        {
            var provider =
                new CryptoProviderFactory(Properties.OptionsSecurityPage.Default.EncryptionEngine, Properties.OptionsSecurityPage.Default.EncryptionBlockCipherMode).Build();
            provider.KeyDerivationIterations = Properties.OptionsSecurityPage.Default.EncryptionKeyDerivationIterations;
            return provider;
        }
    }
}