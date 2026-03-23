using System;
using System.Runtime.Versioning;
using System.Security;
using System.Xml.Linq;

using mRemoteNG.Security;
using mRemoteNG.Security.Factories;
using mRemoteNG.Security.SymmetricEncryption;

namespace mRemoteNG.App
{
    [SupportedOSPlatform("windows")]
    internal static class MasterPasswordService
    {
        private const string VerifierElementName = "MasterPassword";
        private const string VerifierAttributeName = "Verifier";
        private const string VerifierPlainText = "mRemoteNG.MasterPassword.Verifier.v1";

        public static bool IsConfigured => !string.IsNullOrWhiteSpace(Properties.OptionsSecurityPage.Default.MasterPasswordVerifier);

        public static bool TryUnlock(SecureString password)
        {
            if (!TryCreateVerifierProvider(out ICryptographyProvider? cryptoProvider, out string? verifier))
                return false;

            try
            {
                if (cryptoProvider.Decrypt(verifier, password) != VerifierPlainText)
                    return false;

                Runtime.SetMasterPasswordSession(password);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void SetMasterPassword(SecureString password)
        {
            SecureString oldEncryptionKey = Runtime.EncryptionKey.Copy();

            try
            {
                Properties.OptionsSecurityPage.Default.MasterPasswordVerifier = BuildVerifier(password);
                Properties.OptionsSecurityPage.Default.Save();

                Runtime.SetMasterPasswordSession(password);
                MigrateEncryptedSettings(oldEncryptionKey, Runtime.EncryptionKey);
            }
            finally
            {
                oldEncryptionKey.Dispose();
            }
        }

        public static void RemoveMasterPassword()
        {
            SecureString oldEncryptionKey = Runtime.EncryptionKey.Copy();

            try
            {
                Properties.OptionsSecurityPage.Default.MasterPasswordVerifier = string.Empty;
                Properties.OptionsSecurityPage.Default.Save();

                Runtime.ClearMasterPasswordSession();
                MigrateEncryptedSettings(oldEncryptionKey, Runtime.EncryptionKey);
            }
            finally
            {
                oldEncryptionKey.Dispose();
            }
        }

        private static string BuildVerifier(SecureString password)
        {
            ICryptographyProvider cryptoProvider = new CryptoProviderFactoryFromSettings().Build();
            XElement verifierElement = new(VerifierElementName,
                new XAttribute("EncryptionEngine", cryptoProvider.CipherEngine),
                new XAttribute("BlockCipherMode", cryptoProvider.CipherMode),
                new XAttribute("KdfIterations", cryptoProvider.KeyDerivationIterations),
                new XAttribute(VerifierAttributeName, cryptoProvider.Encrypt(VerifierPlainText, password)));

            return verifierElement.ToString(SaveOptions.DisableFormatting);
        }

        private static bool TryCreateVerifierProvider(out ICryptographyProvider cryptoProvider, out string verifier)
        {
            cryptoProvider = null!;
            verifier = string.Empty;

            if (!IsConfigured)
                return false;

            try
            {
                XElement verifierElement = XElement.Parse(Properties.OptionsSecurityPage.Default.MasterPasswordVerifier, LoadOptions.None);
                verifier = verifierElement.Attribute(VerifierAttributeName)?.Value ?? string.Empty;
                if (string.IsNullOrWhiteSpace(verifier))
                    return false;

                cryptoProvider = new CryptoProviderFactoryFromXml(verifierElement).Build();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void MigrateEncryptedSettings(SecureString oldKey, SecureString newKey)
        {
            if (oldKey.ConvertToUnsecureString() == newKey.ConvertToUnsecureString())
                return;

            LegacyRijndaelCryptographyProvider cryptoProvider = new();
            ReEncryptSetting(
                () => Properties.OptionsCredentialsPage.Default.DefaultPassword,
                value => Properties.OptionsCredentialsPage.Default.DefaultPassword = value,
                "default credentials password",
                cryptoProvider,
                oldKey,
                newKey);
            ReEncryptSetting(
                () => Properties.OptionsDBsPage.Default.SQLPass,
                value => Properties.OptionsDBsPage.Default.SQLPass = value,
                "SQL password",
                cryptoProvider,
                oldKey,
                newKey);
            ReEncryptSetting(
                () => Properties.OptionsUpdatesPage.Default.UpdateProxyAuthPass,
                value => Properties.OptionsUpdatesPage.Default.UpdateProxyAuthPass = value,
                "update proxy password",
                cryptoProvider,
                oldKey,
                newKey);

            Properties.OptionsCredentialsPage.Default.Save();
            Properties.OptionsDBsPage.Default.Save();
            Properties.OptionsUpdatesPage.Default.Save();
        }

        private static void ReEncryptSetting(
            Func<string> getter,
            Action<string> setter,
            string settingName,
            ICryptographyProvider cryptoProvider,
            SecureString oldKey,
            SecureString newKey)
        {
            string existingCipherText = getter();
            if (string.IsNullOrWhiteSpace(existingCipherText))
                return;

            try
            {
                string plainText = cryptoProvider.Decrypt(existingCipherText, oldKey);
                setter(cryptoProvider.Encrypt(plainText, newKey));
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage($"Failed to migrate {settingName}", ex);
            }
        }
    }
}
