using mRemoteNG.Properties;

namespace mRemoteNG.Security.Factories
{
    /// <summary>
    /// Builds a password-derived AES/Twofish/Serpent provider from the Options > Security
    /// engine/mode/KDF settings. This is shared by several unrelated subsystems (the SQLite user
    /// profile store's outer encryption, the credential repository list, a connections profile's
    /// own save path) that must NOT be affected by the connections-file-only DPAPI opt-in — see
    /// <see cref="ConnectionsFileCryptoProviderFactory"/> for the one that is.
    /// </summary>
    public class CryptoProviderFactoryFromSettings : ICryptoProviderFactory
    {
        public ICryptographyProvider Build()
        {
            ICryptographyProvider provider =
                new CryptoProviderFactory(Properties.OptionsSecurityPage.Default.EncryptionEngine, Properties.OptionsSecurityPage.Default.EncryptionBlockCipherMode).Build();
            provider.KeyDerivationIterations = Properties.OptionsSecurityPage.Default.EncryptionKeyDerivationIterations;
            return provider;
        }
    }
}