using System;
using System.Linq;
using System.Runtime.Versioning;
using System.Security;
using System.Threading.Tasks;
using mRemoteNG.Resources.Language;
using mRemoteNG.Tools;

namespace mRemoteNG.Security.KeyManagement
{
    /// <summary>
    /// Obtains a master key by prompting the user for a password.
    /// Uses the existing <see cref="IKeyProvider"/> (typically <c>FrmPassword</c>).
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class PasswordMasterKeyProvider : IMasterKeyProvider
    {
        private readonly Func<IKeyProvider> _keyProviderFactory;

        public string ProviderId => "password";
        public string DisplayName => Language.MasterKeyProviderPassword;
        public bool IsAvailable => true;
        public bool CanPersistWrappedKey => false;

        /// <param name="keyProviderFactory">
        /// Factory that creates a new <see cref="IKeyProvider"/> (e.g. <c>() => new FrmPassword("Master Password", true)</c>).
        /// </param>
        public PasswordMasterKeyProvider(Func<IKeyProvider> keyProviderFactory)
        {
            _keyProviderFactory = keyProviderFactory ?? throw new ArgumentNullException(nameof(keyProviderFactory));
        }

        public Task<SecureString> GetKeyAsync()
        {
            IKeyProvider provider = _keyProviderFactory();
            Optional<SecureString> result = provider.GetKey();
            SecureString key = result.Any() ? result.First() : null;
            return Task.FromResult(key);
        }
    }
}
