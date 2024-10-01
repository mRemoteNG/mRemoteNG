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

        public OptRegistrySecurityPage()
        {
            
            RegistryHive hive = WindowsRegistryInfo.Hive;
            string subKey = WindowsRegistryInfo.SecurityOptions;

            EncryptCompleteConnectionsFile = new WinRegistryEntry<bool>(hive, subKey, nameof(EncryptCompleteConnectionsFile)).Read();
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
            ApplyEncryptionEngine();
            ApplyEncryptionBlockCipherMode();
            ApplyEncryptionKeyDerivationIterations();
        }

        private void ApplyEncryptCompleteConnectionsFile()
        {
            if (EncryptCompleteConnectionsFile.IsSet)
                Properties.OptionsSecurityPage.Default.EncryptCompleteConnectionsFile = EncryptCompleteConnectionsFile.Value;
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