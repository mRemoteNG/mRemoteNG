using System;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text;
using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Resources.Language;
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
    /// ones an administrator put in the registry before it could take them protected with DPAPI
    /// for the machine. Portable builds keep the old form, since their settings travel with the
    /// executable to other machines and users.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static class OptionPasswords
    {
        private const string DpapiPrefix = "dpapi:";

        /// <summary>Ties a protected value to this use, so DPAPI data made for anything else does not open here.</summary>
        private static readonly byte[] Entropy = Encoding.UTF8.GetBytes("mRemoteNG option password");

        private const string ComputerDpapiPrefix = "dpapi-machine:";

        /// <summary>
        /// The same for a password in the registry. The script in the documentation of the registry
        /// settings passes these bytes too, so changing them breaks the values made with it.
        /// </summary>
        private static readonly byte[] RegistryEntropy = Encoding.UTF8.GetBytes("mRemoteNG registry password");

        /// <summary>The registry values already reported in this session, so each is reported once.</summary>
        private static readonly HashSet<string> ReportedRegistryValues = new(StringComparer.Ordinal);

        public static string Protect(string plainText) => Protect(plainText, Runtime.IsPortableEdition);

        internal static string Protect(string plainText, bool portable)
        {
            if (string.IsNullOrEmpty(plainText)) return "";
            if (portable) return OldForm(plainText);

            try
            {
                byte[] protectedBytes = ProtectedData.Protect(Encoding.UTF8.GetBytes(plainText), Entropy, DataProtectionScope.CurrentUser);
                return DpapiPrefix + Convert.ToBase64String(protectedBytes);
            }
            catch (CryptographicException)
            {
                // DPAPI is not available in every logon session (a key-based SSH session, for one).
                // Saving the options must not fail on that: the old form, as before.
                return OldForm(plainText);
            }
        }

        private static string OldForm(string plainText) =>
            new LegacyRijndaelCryptographyProvider().Encrypt(plainText, Runtime.EncryptionKey);

        /// <summary>
        /// The password in clear. A value protected for another user or on another machine reads
        /// as no password at all; a value in the old form that cannot be decrypted throws, as before.
        /// </summary>
        public static string Unprotect(string stored)
        {
            if (string.IsNullOrEmpty(stored)) return "";
            if (IsProtectedForAComputer(stored))
                return UnprotectDpapi(stored.Substring(ComputerDpapiPrefix.Length), RegistryEntropy, DataProtectionScope.LocalMachine);
            if (!IsProtected(stored)) return new LegacyRijndaelCryptographyProvider().Decrypt(stored, Runtime.EncryptionKey);

            return UnprotectDpapi(stored.Substring(DpapiPrefix.Length), Entropy, DataProtectionScope.CurrentUser);
        }

        private static string UnprotectDpapi(string base64, byte[] entropy, DataProtectionScope scope)
        {
            try
            {
                return Encoding.UTF8.GetString(ProtectedData.Unprotect(Convert.FromBase64String(base64), entropy, scope));
            }
            catch (Exception ex) when (ex is CryptographicException or FormatException)
            {
                return "";
            }
        }

        internal static bool IsProtected(string stored) => stored.StartsWith(DpapiPrefix, StringComparison.Ordinal);

        internal static bool IsProtectedForAComputer(string stored) => stored.StartsWith(ComputerDpapiPrefix, StringComparison.Ordinal);

        /// <summary>
        /// A password for the registry settings of this computer, protected with DPAPI for the
        /// machine: any user of this computer can read it, but the value copied to another
        /// computer, in a registry export for one, does not open there. The default key of the old
        /// form is public, so that form protected nothing.
        /// </summary>
        public static string ProtectForThisComputer(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return "";

            byte[] protectedBytes = ProtectedData.Protect(Encoding.UTF8.GetBytes(plainText), RegistryEntropy, DataProtectionScope.LocalMachine);
            return ComputerDpapiPrefix + Convert.ToBase64String(protectedBytes);
        }

        /// <summary>
        /// The form a password set in the registry is kept in the options, or null when it cannot
        /// be used: a value protected on another computer, or one that is not encrypted at all, as
        /// plain-text passwords never were taken. A value in the old form is still taken, with a
        /// warning, so the policies already deployed keep working until they are generated again.
        /// </summary>
        public static string FromRegistry(string stored, string valueName, MessageCollector messageCollector)
        {
            if (string.IsNullOrEmpty(stored)) return null;

            if (IsProtectedForAComputer(stored))
            {
                if (Unprotect(stored) != "") return stored;

                ReportOnce(messageCollector, valueName, string.Format(Language.RegistryPasswordOtherComputer, valueName));
                return null;
            }

            if (!HasTheLegacyForm(stored)) return null;

            try
            {
                new LegacyRijndaelCryptographyProvider().Decrypt(stored, Runtime.EncryptionKey);
            }
            catch (EncryptionException)
            {
                return null;
            }

            ReportOnce(messageCollector, valueName, string.Format(Language.RegistryPasswordOldForm, valueName));
            return stored;
        }

        /// <summary>
        /// The old form is the base64 of a 16-byte IV followed by whole AES blocks. Base64 decoding
        /// skips spaces, so without this check a short value typed in clear can decode, and decrypt
        /// to nothing, as if it were one.
        /// </summary>
        private static bool HasTheLegacyForm(string stored)
        {
            byte[] bytes;
            try
            {
                bytes = Convert.FromBase64String(stored);
            }
            catch (FormatException)
            {
                return false;
            }

            return bytes.Length >= 32 && bytes.Length % 16 == 0;
        }

        private static void ReportOnce(MessageCollector messageCollector, string valueName, string message)
        {
            lock (ReportedRegistryValues)
            {
                if (!ReportedRegistryValues.Add(valueName)) return;
            }

            messageCollector.AddMessage(MessageClass.WarningMsg, message);
        }

        internal static void ForgetReportedRegistryValues()
        {
            lock (ReportedRegistryValues)
                ReportedRegistryValues.Clear();
        }

        /// <summary>
        /// Moves the passwords an earlier version saved in the old form over to DPAPI, and saves
        /// them. Where DPAPI is not available they stay in the old form, and the move is tried again
        /// at the next start. A value the registry sets is put back later in the start, in the old
        /// form the registry holds it in. Never throws: any other failure is reported, and the
        /// passwords stay readable.
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
            // A value protected for this computer comes from the registry, which keeps setting it.
            if (string.IsNullOrEmpty(stored) || IsProtected(stored) || IsProtectedForAComputer(stored)) return stored;

            try
            {
                // Where DPAPI is not available the value stays as it is, rather than saved anew in the old form.
                string moved = Protect(Unprotect(stored), portable: false);
                return IsProtected(moved) ? moved : stored;
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
