using System.Security;
using System.Threading.Tasks;

namespace mRemoteNG.Security.KeyManagement
{
    /// <summary>
    /// Provides a master key that can be used to unwrap a Data Encryption Key (DEK).
    /// Implementations may prompt the user, call an external service (KeePass, 1Password),
    /// or use OS-level secrets (DPAPI / Windows Hello).
    /// </summary>
    public interface IMasterKeyProvider
    {
        /// <summary>
        /// Unique identifier for this provider (e.g. "password", "keepass", "dpapi").
        /// Persisted in the keystore so we know which provider wrapped a DEK.
        /// </summary>
        string ProviderId { get; }

        /// <summary>
        /// Human-readable name for display in the UI.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Returns true if this provider is available on the current machine
        /// (e.g. KeePass installed, Windows Hello enrolled).
        /// </summary>
        bool IsAvailable { get; }

        /// <summary>
        /// Obtains the master key material from the user or external system.
        /// Returns null if the user cancels or the provider cannot deliver a key.
        /// </summary>
        Task<SecureString> GetKeyAsync();

        /// <summary>
        /// Returns true if this provider supports persisting a wrapped DEK
        /// (i.e. it can act as a "remember me" mechanism).
        /// </summary>
        bool CanPersistWrappedKey { get; }
    }
}
