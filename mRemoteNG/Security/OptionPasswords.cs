using System;
using System.Configuration;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text;
using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Security.SymmetricEncryption;

namespace mRemoteNG.Security
{
    /// <summary>
    /// The passwords kept in the options: the SQL server's, the update proxy's and the default
    /// credentials'. They used to be encrypted with <see cref="LegacyRijndaelCryptographyProvider"/>
    /// under <see cref="Runtime.EncryptionKey"/>, which is always the public default key, so
    /// anyone who could read user.config could decrypt them. Installed builds now protect them
    /// with DPAPI for the current Windows user, which suits user.config, kept per user and per
    /// machine anyway. The old form is still read: values saved by an earlier version, and the
    /// ones an administrator puts in the registry. Portable builds keep the old form, since
    /// their settings travel with the executable to other machines and users.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static class OptionPasswords
    {
        private const string DpapiPrefix = "dpapi:";

        /// <summary>Ties a protected value to this use, so DPAPI data made for anything else does not open here.</summary>
        private static readonly byte[] Entropy = Encoding.UTF8.GetBytes("mRemoteNG option password");

        public static string Protect(string plainText) => Protect(plainText, Runtime.IsPortableEdition);

        internal static string Protect(string plainText, bool portable)
        {
            if (string.IsNullOrEmpty(plainText)) return "";
            if (portable) return new LegacyRijndaelCryptographyProvider().Encrypt(plainText, Runtime.EncryptionKey);

            byte[] protectedBytes = ProtectedData.Protect(Encoding.UTF8.GetBytes(plainText), Entropy, DataProtectionScope.CurrentUser);
            return DpapiPrefix + Convert.ToBase64String(protectedBytes);
        }

        /// <summary>
        /// The password in clear. A value protected for another user or on another machine reads
        /// as no password at all; a value in the old form that cannot be decrypted throws, as before.
        /// </summary>
        public static string Unprotect(string stored)
        {
            if (string.IsNullOrEmpty(stored)) return "";
            if (!IsProtected(stored)) return new LegacyRijndaelCryptographyProvider().Decrypt(stored, Runtime.EncryptionKey);

            try
            {
                byte[] protectedBytes = Convert.FromBase64String(stored.Substring(DpapiPrefix.Length));
                return Encoding.UTF8.GetString(ProtectedData.Unprotect(protectedBytes, Entropy, DataProtectionScope.CurrentUser));
            }
            catch (Exception ex) when (ex is CryptographicException or FormatException)
            {
                return "";
            }
        }

        internal static bool IsProtected(string stored) => stored.StartsWith(DpapiPrefix, StringComparison.Ordinal);

        /// <summary>
        /// Moves the passwords an earlier version saved in the old form over to DPAPI, and saves
        /// them. A value the registry sets is moved too, and set again from the registry at the
        /// next start. Never throws: a failure is reported, and the passwords stay readable.
        /// </summary>
        public static void ProtectStoredPasswords(MessageCollector messageCollector)
        {
            if (Runtime.IsPortableEdition) return;

            try
            {
                string sql = Reprotect(Properties.OptionsDBsPage.Default.SQLPass);
                SaveIfChanged(Properties.OptionsDBsPage.Default, nameof(Properties.OptionsDBsPage.SQLPass), sql);

                string proxy = Reprotect(Properties.OptionsUpdatesPage.Default.UpdateProxyAuthPass);
                SaveIfChanged(Properties.OptionsUpdatesPage.Default, nameof(Properties.OptionsUpdatesPage.UpdateProxyAuthPass), proxy);

                string credentials = Reprotect(Properties.OptionsCredentialsPage.Default.DefaultPassword);
                SaveIfChanged(Properties.OptionsCredentialsPage.Default, nameof(Properties.OptionsCredentialsPage.DefaultPassword), credentials);
            }
            catch (Exception ex)
            {
                messageCollector.AddExceptionMessage("Protecting the passwords kept in the options failed", ex);
            }
        }

        /// <summary>The DPAPI form of a value in the old form; anything else as it is.</summary>
        internal static string Reprotect(string stored)
        {
            if (string.IsNullOrEmpty(stored) || IsProtected(stored)) return stored;

            try
            {
                return Protect(Unprotect(stored), portable: false);
            }
            catch (EncryptionException)
            {
                // Not readable as the old form either: left for whoever reads it to report.
                return stored;
            }
        }

        private static void SaveIfChanged(ApplicationSettingsBase settings, string name, string value)
        {
            if ((string)settings[name] == value) return;
            settings[name] = value;
            settings.Save();
        }
    }
}
