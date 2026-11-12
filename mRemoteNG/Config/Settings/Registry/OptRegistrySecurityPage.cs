using System.Runtime.Versioning;
using Microsoft.Win32;
using mRemoteNG.App.Info;
using mRemoteNG.Security;
using mRemoteNG.Tools.WindowsRegistry;

namespace mRemoteNG.Config.Settings.Registry
{
    [SupportedOSPlatform("windows")]
    public sealed partial class OptRegistrySecurityPage
    {
        /// <summary>
        /// Specifies whether the complete connections file is encrypted.
        /// </summary>
        public WinRegistryEntry<bool> EncryptCompleteConnectionsFile { get; private set; }

        /// <summary>
        /// Specifies whether the connections file is protected with Windows DPAPI
        /// (<see cref="mRemoteNG.Security.SymmetricEncryption.DpapiCryptographyProvider"/>) instead of a
        /// password-derived key. When set, the Encryption Engine / Block Cipher Mode / KDF Iterations
        /// settings below have no effect, since DPAPI uses none of them.
        /// </summary>
        public WinRegistryEntry<bool> UseDpapiProtection { get; private set; }

        /// <summary>
        /// Specifies the encryption engine used for encryption.
        /// </summary>
        public WinRegistryEntry<string> EncryptionEngine { get; private set; }

        /// <summary>
        /// Specifies the block cipher mode used for encryption.
        /// </summary>
        public WinRegistryEntry<string> EncryptionBlockCipherMode { get; private set; }

        /// <summary>
        /// Specifies the number of iterations used in the encryption key derivation process.
        /// </summary>
        public WinRegistryEntry<int> EncryptionKeyDerivationIterations { get; private set; }

        public OptRegistrySecurityPage() : this(WindowsRegistryInfo.Hive, WindowsRegistryInfo.SecurityOptions)
        {
        }

        /// <summary>
        /// Initializes a new instance reading from an explicit hive/subkey instead of the production
        /// <see cref="WindowsRegistryInfo.Hive"/>/<see cref="WindowsRegistryInfo.SecurityOptions"/>
        /// location. Internal so tests (<c>InternalsVisibleTo("mRemoteNGTests")</c>) can exercise this
        /// class against a throwaway <see cref="RegistryHive.CurrentUser"/> path instead of
        /// <see cref="RegistryHive.LocalMachine"/>, which would require administrator rights to write.
        /// </summary>
        /// <param name="hive">The registry hive to read settings from.</param>
        /// <param name="subKey">The registry subkey to read settings from.</param>
        internal OptRegistrySecurityPage(RegistryHive hive, string subKey)
        {
            EncryptCompleteConnectionsFile = new WinRegistryEntry<bool>(hive, subKey, nameof(EncryptCompleteConnectionsFile)).Read();
            UseDpapiProtection = new WinRegistryEntry<bool>(hive, subKey, nameof(UseDpapiProtection)).Read();
            EncryptionEngine = new WinRegistryEntry<string>(hive, subKey, nameof(EncryptionEngine)).Read();
            EncryptionBlockCipherMode = new WinRegistryEntry<string>(hive, subKey, nameof(EncryptionBlockCipherMode)).Read();
            EncryptionKeyDerivationIterations = new WinRegistryEntry<int>(hive, subKey, nameof(EncryptionKeyDerivationIterations)).Read();

            SetupValidation();
            Apply();
        }

        /// <summary>
        /// Configures validation settings for various parameters
        /// </summary>
        private void SetupValidation()
        {
            var SecurityPage = new UI.Forms.OptionsPages.SecurityPage();

            EncryptionEngine.SetValidation<BlockCipherEngines>();
            EncryptionBlockCipherMode.SetValidation<BlockCipherModes>();

            int EncryptionKeyDerivIteraMin = (int)SecurityPage.numberBoxKdfIterations.Minimum;
            int EncryptionKeyDerivIteraMax = (int)SecurityPage.numberBoxKdfIterations.Maximum;
            EncryptionKeyDerivationIterations.SetValidation(EncryptionKeyDerivIteraMin, EncryptionKeyDerivIteraMax);
        }

        /// <summary>
        /// Applies registry settings and overrides various properties.
        /// </summary>
        private void Apply()
        {
            ApplyEncryptCompleteConnectionsFile();
            ApplyUseDpapiProtection();
            ApplyEncryptionEngine();
            ApplyEncryptionBlockCipherMode();
            ApplyEncryptionKeyDerivationIterations();
        }

        private void ApplyEncryptCompleteConnectionsFile()
        {
            if (EncryptCompleteConnectionsFile.IsSet)
                Properties.OptionsSecurityPage.Default.EncryptCompleteConnectionsFile = EncryptCompleteConnectionsFile.Value;
        }

        private void ApplyUseDpapiProtection()
        {
            if (UseDpapiProtection.IsSet)
                Properties.OptionsSecurityPage.Default.UseDpapiProtection = UseDpapiProtection.Value;
        }

        private void ApplyEncryptionEngine()
        {
            if (EncryptionEngine.IsValid)
            {
                BlockCipherEngines blockCipherEngines = (BlockCipherEngines)System.Enum.Parse(typeof(BlockCipherEngines), EncryptionEngine.Value);
                Properties.OptionsSecurityPage.Default.EncryptionEngine = blockCipherEngines;
            }
        }

        private void ApplyEncryptionBlockCipherMode()
        {
            if (EncryptionBlockCipherMode.IsValid)
            {
                BlockCipherModes blockCipherModes = (BlockCipherModes)System.Enum.Parse(typeof(BlockCipherModes), EncryptionBlockCipherMode.Value);
                Properties.OptionsSecurityPage.Default.EncryptionBlockCipherMode = blockCipherModes;
            }
        }

        private void ApplyEncryptionKeyDerivationIterations()
        {
            if (EncryptionKeyDerivationIterations.IsValid)
                Properties.OptionsSecurityPage.Default.EncryptionKeyDerivationIterations = EncryptionKeyDerivationIterations.Value;
        }
    }
}