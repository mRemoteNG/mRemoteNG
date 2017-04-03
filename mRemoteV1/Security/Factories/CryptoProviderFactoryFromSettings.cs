namespace mRemoteNG.Security.Factories
{
    public class CryptoProviderFactoryFromSettings : ICryptoProviderFactory
    {
        public ICryptographyProvider Build()
        {
            var provider = new CryptoProviderFactory(Settings.Default.EncryptionEngine, Settings.Default.EncryptionBlockCipherMode).Build();
            provider.KeyDerivationIterations = Settings.Default.EncryptionKeyDerivationIterations;
            return provider;
        }
    }
}