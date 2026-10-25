using System;
using mRemoteNG.Properties;
using mRemoteNG.Security.KeyDerivation;

namespace mRemoteNG.Security.Factories
{
    public class CryptoProviderFactoryFromSettings : ICryptoProviderFactory
    {
        public ICryptographyProvider Build()
        {
            ICryptographyProvider provider =
                new CryptoProviderFactory(Properties.OptionsSecurityPage.Default.EncryptionEngine, Properties.OptionsSecurityPage.Default.EncryptionBlockCipherMode).Build();
            // Everything saved from now on uses the stronger KDF. Older settings may still hold a low
            // iteration count, so it is raised to the minimum; files re-encrypt on their next save.
            provider.KeyDerivationAlgorithm = KeyDerivationDefaults.Algorithm;
            provider.KeyDerivationIterations = Math.Max(Properties.OptionsSecurityPage.Default.EncryptionKeyDerivationIterations,
                                                        KeyDerivationDefaults.MinimumIterations);
            return provider;
        }
    }
}
