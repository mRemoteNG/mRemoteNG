using System.Runtime.Versioning;
using Microsoft.Win32;
using mRemoteNG.App.Info;
using mRemoteNG.Tools.WindowsRegistry;

namespace mRemoteNG.Config.Settings.Registry
{
    [SupportedOSPlatform("windows")]
    public sealed partial class OptRegistryStartupExitPage
    {
        /// <summary>
        /// Specifies whether the application should start minimized or fullscreen.
        /// </summary>
        /// /// <remarks>
        /// Default value is null, which has no effect.
        /// </remarks>
        public WinRegistryEntry<string> StartupBehavior { get; private set; }

        /// <summary>
        /// Specifies whether sessions should be automatically reconnected on application startup.
        /// </summary>
        public WinRegistryEntry<bool> OpenConnectionsFromLastSession { get; private set; }

        /// <summary>
        /// Ensures that only a single instance of the application is allowed to run.
        /// </summary>
        public WinRegistryEntry<bool> EnforceSingleApplicationInstance { get; private set; }

        public OptRegistryStartupExitPage()
        {
            RegistryHive hive = WindowsRegistryInfo.Hive;
            string subKey = WindowsRegistryInfo.StartupExitOptions;

            StartupBehavior = new WinRegistryEntry<string>(hive, subKey, nameof(StartupBehavior)).Read();
            OpenConnectionsFromLastSession = new WinRegistryEntry<bool>(hive, subKey, nameof(OpenConnectionsFromLastSession)).Read();
            EnforceSingleApplicationInstance = new WinRegistryEntry<bool>(hive, subKey, nameof(EnforceSingleApplicationInstance)).Read();

            SetupValidation();
            Apply();
        }

        /// <summary>
        /// Configures validation settings for various parameters
        /// </summary>
        private void SetupValidation()
        {
            StartupBehavior.SetValidation(
               new string[] {
                    "None",
                    "Minimized",
                    "FullScreen"
               });
        }

        /// <summary>
        /// Applies registry settings and overrides various properties.
        /// </summary>
        private void Apply()
        {
            ApplyStartupBehavior();
            ApplyOpenConnectionsFromLastSession();
            ApplyEnforceSingleApplicationInstance();
        }

        private void ApplyStartupBehavior()
        {
            if (StartupBehavior.IsSet)
            {
                switch (StartupBehavior.Value)
                {
                    case "None":
                        Properties.OptionsStartupExitPage.Default.StartMinimized = false;
                        Properties.OptionsStartupExitPage.Default.StartFullScreen = false;
                        break;
                    case "Minimized":
                        Properties.OptionsStartupExitPage.Default.StartMinimized = true;
                        break;
                    case "FullScreen":
                        Properties.OptionsStartupExitPage.Default.StartFullScreen = true;
                        break;
                }
            }
        }

        private void ApplyOpenConnectionsFromLastSession()
        {
            if (OpenConnectionsFromLastSession.IsSet)
                Properties.OptionsStartupExitPage.Default.OpenConsFromLastSession = OpenConnectionsFromLastSession.Value;
        }

        private void ApplyEnforceSingleApplicationInstance()
        {
            if (EnforceSingleApplicationInstance.IsSet)
                Properties.OptionsStartupExitPage.Default.SingleInstance = EnforceSingleApplicationInstance.Value;
        }
    }
}