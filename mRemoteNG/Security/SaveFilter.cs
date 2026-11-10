using mRemoteNG.Config.Settings.Registry;
using System.Runtime.Versioning;

namespace mRemoteNG.Security
{
    [SupportedOSPlatform("windows")]
    public class SaveFilter
    {
        private readonly bool _allowPasswords;
        private readonly bool _allowUsernames;
        private bool _savePassword;
        private bool _saveUsername;

        public SaveFilter(bool disableEverything = false, bool allowPasswords = true, bool allowUsernames = true)
        {
            _allowPasswords = allowPasswords && CommonRegistrySettings.AllowSavePasswords;
            _allowUsernames = allowUsernames && CommonRegistrySettings.AllowSaveUsernames;
            if (disableEverything) return;
            SaveUsername = CommonRegistrySettings.AllowSaveUsernames;
            SavePassword = CommonRegistrySettings.AllowSavePasswords;
            SaveDomain = true;
            SaveCredentialId = true;
            SaveInheritance = true;
        }

        public bool SaveUsername
        {
            get => _saveUsername && _allowUsernames;
            set => _saveUsername = value;
        }

        public bool SavePassword
        {
            get => _savePassword && _allowPasswords;
            set => _savePassword = value;
        }

        /// <summary>Returns an export filter whose limits cannot be relaxed by a caller's selections.</summary>
        public SaveFilter RestrictForExport()
        {
            return new SaveFilter(true,
                _allowPasswords && CommonRegistrySettings.AllowExportPasswords,
                _allowUsernames && CommonRegistrySettings.AllowExportUsernames)
            {
                SavePassword = SavePassword,
                SaveUsername = SaveUsername,
                SaveDomain = SaveDomain,
                SaveCredentialId = SaveCredentialId,
                SaveInheritance = SaveInheritance
            };
        }

        public bool SaveDomain { get; set; }

        public bool SaveCredentialId { get; set; }

        public bool SaveInheritance { get; set; }
    }
}