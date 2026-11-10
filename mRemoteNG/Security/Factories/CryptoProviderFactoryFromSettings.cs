using mRemoteNG.Properties;
using mRemoteNG.Security.SymmetricEncryption;

namespace mRemoteNG.Security.Factories
{
    public class CryptoProviderFactoryFromSettings : ICryptoProviderFactory
    {
        public ICryptographyProvider Build()
        {
            // DPAPI is a distinct, explicit opt-in (Options > Security > "Protect with Windows user
            // account (DPAPI)") — it bypasses the engine/mode/KDF settings entirely, since it needs
            // none of them. Everyone who hasn't turned it on keeps today's exact behaviour.
            if (Properties.OptionsSecurityPage.Default.UseDpapiProtection)
                return new DpapiCryptographyProvider();

            ICryptographyProvider provider =
                new CryptoProviderFactory(Properties.OptionsSecurityPage.Default.EncryptionEngine, Properties.OptionsSecurityPage.Default.EncryptionBlockCipherMode).Build();
            provider.KeyDerivationIterations = Properties.OptionsSecurityPage.Default.EncryptionKeyDerivationIterations;
            return provider;
        }
    }
}