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
        public ICryptographyProvider Build()
        {
            // DPAPI is a distinct, explicit opt-in (Options > Security > "Protect with Windows user
            // account (DPAPI)") — it bypasses the engine/mode/KDF settings entirely, since it needs
            // none of them. Everyone who hasn't turned it on keeps today's exact behaviour.
            if (Properties.OptionsSecurityPage.Default.UseDpapiProtection)
                return new DpapiCryptographyProvider();

            return new CryptoProviderFactoryFromSettings().Build();
        }
    }
}
