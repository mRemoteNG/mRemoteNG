using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Security;
using System.Windows.Forms;

using mRemoteNG.Security;
using mRemoteNG.Tools;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.App
{
    [SupportedOSPlatform("windows")]
    internal static class StartupUnlockService
    {
        public static bool EnsureStartupUnlocked(IWin32Window owner)
        {
            if (!EnsureMasterPasswordUnlocked(owner))
                return false;

            if (Properties.OptionsDBsPage.Default.UseSQLServer)
                return true;

            string startupConnectionFile = Runtime.ConnectionsService.GetStartupConnectionFileName();
            if (string.IsNullOrWhiteSpace(startupConnectionFile) || !File.Exists(startupConnectionFile))
            {
                Runtime.ResetEncryptionKey();
                return true;
            }

            if (!XmlKeyValidator.ConnectionFileRequiresPassword(startupConnectionFile))
            {
                Runtime.ResetEncryptionKey();
                return true;
            }

            if (XmlKeyValidator.ConnectionFileUsesKey(startupConnectionFile, Runtime.EncryptionKey))
                return true;

            while (true)
            {
                Optional<SecureString> password = MiscTools.PasswordDialog(owner, Path.GetFileName(startupConnectionFile), verify: false);
                if (!password.Any())
                    return false;

                if (XmlKeyValidator.ConnectionFileUsesKey(startupConnectionFile, password.First()))
                {
                    if (!Runtime.HasActiveMasterPasswordSession)
                        Runtime.SetEncryptionKey(password.First());
                    return true;
                }

                MessageBox.Show(owner, "The unlock password is invalid.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static bool EnsureMasterPasswordUnlocked(IWin32Window owner)
        {
            if (!MasterPasswordService.IsConfigured)
            {
                Runtime.ClearMasterPasswordSession();
                return true;
            }

            string hint = MasterPasswordService.Hint;
            string promptName = string.IsNullOrWhiteSpace(hint)
                ? Language.MasterPassword
                : $"{Language.MasterPassword} ({Language.MasterPasswordHint}: {hint})";

            while (true)
            {
                Optional<SecureString> password = MiscTools.PasswordDialog(owner, promptName, verify: false);
                if (!password.Any())
                    return false;

                if (MasterPasswordService.TryUnlock(password.First()))
                    return true;

                MessageBox.Show(owner, Language.MasterPasswordInvalid, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
