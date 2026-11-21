using mRemoteNG.Properties;
using mRemoteNG.Security.SymmetricEncryption;

namespace mRemoteNG.Security.Factories
{
    /// <summary>
    /// Builds the cryptography provider used specifically for <c>confCons.xml</c> (the connections
    /// file) — the only place the "Protect connections file with this Windows user account (DPAPI)"
    /// opt-in (<see cref="OptionsSecurityPage.UseDpapiProtection"/>) applies. Deliberately separate
    /// from <see cref="CryptoProviderFactoryFromSettings"/>, which several unrelated subsystems
    /// (the SQLite user profile store's outer encryption, the credential repository list) also
    /// depend on and which must keep using the password-derived engine/mode/KDF settings regardless
    /// of this checkbox.
    /// </summary>
    public class ConnectionsFileCryptoProviderFactory : ICryptoProviderFactory
    {
        /// <summary>
        /// One salt for the files written in this session. Keeping it for the session lets the key
        /// derived from it be reused at every save instead of derived again; it is still random,
        /// and written in each file.
        /// </summary>
        private static readonly byte[] SessionSalt = DerivedKeyCryptographyProvider.NewSalt();

        public ICryptographyProvider Build()
        {
            // DPAPI is a distinct, explicit opt-in (Options > Security > "Protect with Windows user
            // account (DPAPI)") — it bypasses the engine/mode/KDF settings entirely, since it needs
            // none of them.
            if (Properties.OptionsSecurityPage.Default.UseDpapiProtection)
                return new DpapiCryptographyProvider();

            // The engine and mode still come from the settings. The key is derived once for the
            // file (CVE-2026-87868), so the iteration count is not the per-value one from the
            // settings, which has to stay low enough to derive a key for every value.
            return new CryptoProviderFactory(Properties.OptionsSecurityPage.Default.EncryptionEngine, Properties.OptionsSecurityPage.Default.EncryptionBlockCipherMode)
                .BuildWithKeyDerivedOnce(SessionSalt, DerivedKeyCryptographyProvider.DefaultIterations);
        }
    }
}
