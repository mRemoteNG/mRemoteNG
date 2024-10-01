using System.Runtime.Versioning;
using Microsoft.Win32;
using mRemoteNG.App.Info;
using mRemoteNG.Tools.WindowsRegistry;

namespace mRemoteNG.Config.Settings.Registry
{
    [SupportedOSPlatform("windows")]
    public sealed partial class OptRegistryConnectionsPage
    {
        /// <summary>
        /// Specifies whether to single click to connection opens/establishes connection
        /// </summary>
        public WinRegistryEntry<bool> SingleClickOnConnectionOpensIt { get; private set; }

        /// <summary>
        /// Specifies whether a single click on an open connection switches the focused tab to that connection.
        /// </summary>
        public WinRegistryEntry<bool> SingleClickSwitchesToOpenConnection { get; private set; }

        /// <summary>
        /// Specifies whether to track open connections in the connection tree.
        /// </summary>
        public WinRegistryEntry<bool> TrackActiveConnectionInConnectionTree { get; private set; }

        /// <summary>
        /// Specifies whether to set the hostname like the display name when creating or renaming a connection.
        /// </summary>
        public WinRegistryEntry<bool> SetHostnameLikeDisplayName { get; private set; }

        /// <summary>
        /// Specifies whether filter matches in the search are applied in the connection tree.
        /// </summary>
        public WinRegistryEntry<bool> UseFilterSearch { get; private set; }

        /// <summary>
        /// Specifies whether the search bar is placed above the connection tree.
        /// </summary>
        public WinRegistryEntry<bool> PlaceSearchBarAboveConnectionTree { get; private set; }

        /// <summary>
        /// Specifies whether the username trimming is disabled.
        /// </summary>
        public WinRegistryEntry<bool> DoNotTrimUsername { get; private set; }

        /// <summary>
        /// Specifies the number of RDP reconnections.
        /// </summary>
        public WinRegistryEntry<int> RdpReconnectionCount { get; private set; }

        /// <summary>
        /// Specifies the overall connection timeout for RDP connections.
        /// </summary>
        public WinRegistryEntry<int> ConRDPOverallConnectionTimeout { get; private set; }

        /// <summary>
        /// Specifies the autosave interval in minutes. 
        /// </summary>
        public WinRegistryEntry<int> AutoSaveEveryMinutes { get; private set; }

        public OptRegistryConnectionsPage()
        {
            RegistryHive hive = WindowsRegistryInfo.Hive;
            string subKey = WindowsRegistryInfo.ConnectionOptions;

            SingleClickOnConnectionOpensIt = new WinRegistryEntry<bool>(hive, subKey, nameof(SingleClickOnConnectionOpensIt)).Read();
            SingleClickSwitchesToOpenConnection = new WinRegistryEntry<bool>(hive, subKey, nameof(SingleClickSwitchesToOpenConnection)).Read();
            TrackActiveConnectionInConnectionTree = new WinRegistryEntry<bool>(hive, subKey, nameof(TrackActiveConnectionInConnectionTree)).Read();
            SetHostnameLikeDisplayName = new WinRegistryEntry<bool>(hive, subKey, nameof(SetHostnameLikeDisplayName)).Read();
            UseFilterSearch = new WinRegistryEntry<bool>(hive, subKey, nameof(UseFilterSearch)).Read();
            PlaceSearchBarAboveConnectionTree = new WinRegistryEntry<bool>(hive, subKey, nameof(PlaceSearchBarAboveConnectionTree)).Read();
            DoNotTrimUsername = new WinRegistryEntry<bool>(hive, subKey, nameof(DoNotTrimUsername)).Read();
            RdpReconnectionCount = new WinRegistryEntry<int>(hive, subKey, nameof(RdpReconnectionCount)).Read();
            ConRDPOverallConnectionTimeout = new WinRegistryEntry<int>(hive, subKey, nameof(ConRDPOverallConnectionTimeout)).Read();
            AutoSaveEveryMinutes = new WinRegistryEntry<int>(hive, subKey, nameof(AutoSaveEveryMinutes)).Read();

            SetupValidation();
            Apply();
        }

        /// <summary>
        /// Configures validation settings for various parameters
        /// </summary>
        private void SetupValidation()
        {
            var connectionsPage = new UI.Forms.OptionsPages.ConnectionsPage();

            int ConRDPOverallConnectionTimeoutMin = (int)connectionsPage.numRDPConTimeout.Minimum;
            int ConRDPOverallConnectionTimeoutMax = (int)connectionsPage.numRDPConTimeout.Maximum;
            ConRDPOverallConnectionTimeout.SetValidation(ConRDPOverallConnectionTimeoutMin, ConRDPOverallConnectionTimeoutMax);

            int numAutoSaveMin = (int)connectionsPage.numAutoSave.Minimum;
            int numAutoSaveMax = (int)connectionsPage.numAutoSave.Maximum;
            AutoSaveEveryMinutes.SetValidation(numAutoSaveMin, numAutoSaveMax);

            int RdpReconnectionCountMin = (int)connectionsPage.numRdpReconnectionCount.Minimum;
            int RdpReconnectionCountMax = (int)connectionsPage.numRdpReconnectionCount.Maximum;
            RdpReconnectionCount.SetValidation(RdpReconnectionCountMin, RdpReconnectionCountMax);
        }

        /// <summary>
        /// Applies registry settings and overrides various properties.
        /// </summary>
        private void Apply()
        {
            ApplySingleClickOnConnectionOpensIt();
            ApplySingleClickSwitchesToOpenConnection();
            ApplyTrackActiveConnectionInConnectionTree();
            ApplySetHostnameLikeDisplayName();
            ApplyUseFilterSearch();
            ApplyPlaceSearchBarAboveConnectionTree();
            ApplyDoNotTrimUsername();
            ApplyRdpReconnectionCount();
            ApplyConRDPOverallConnectionTimeout();
            ApplyAutoSaveEveryMinutes();
        }

        private void ApplySingleClickOnConnectionOpensIt()
        {
            if (SingleClickOnConnectionOpensIt.IsSet)
                Properties.Settings.Default.SingleClickOnConnectionOpensIt = SingleClickOnConnectionOpensIt.Value;
        }

        private void ApplySingleClickSwitchesToOpenConnection()
        {
            if (SingleClickSwitchesToOpenConnection.IsSet)
                 Properties.Settings.Default.SingleClickSwitchesToOpenConnection = SingleClickSwitchesToOpenConnection.Value;
        }

        private void ApplyTrackActiveConnectionInConnectionTree()
        {
            if (TrackActiveConnectionInConnectionTree.IsSet)
                Properties.Settings.Default.TrackActiveConnectionInConnectionTree = TrackActiveConnectionInConnectionTree.Value;
        }

        private void ApplySetHostnameLikeDisplayName()
        {
            if (SetHostnameLikeDisplayName.IsSet)
                Properties.Settings.Default.SetHostnameLikeDisplayName = SetHostnameLikeDisplayName.Value;
        }

        private void ApplyUseFilterSearch()
        {
            if (UseFilterSearch.IsSet)
                Properties.Settings.Default.UseFilterSearch = UseFilterSearch.Value;
        }

        private void ApplyPlaceSearchBarAboveConnectionTree()
        {
            if (PlaceSearchBarAboveConnectionTree.IsSet)
                Properties.Settings.Default.PlaceSearchBarAboveConnectionTree = PlaceSearchBarAboveConnectionTree.Value;
        }

        private void ApplyDoNotTrimUsername()
        {
            if (DoNotTrimUsername.IsSet)
                Properties.Settings.Default.DoNotTrimUsername = DoNotTrimUsername.Value;
        }

        private void ApplyRdpReconnectionCount()
        {
            if (RdpReconnectionCount.IsValid)
                Properties.Settings.Default.RdpReconnectionCount = RdpReconnectionCount.Value;
        }

        private void ApplyConRDPOverallConnectionTimeout()
        {
            if (ConRDPOverallConnectionTimeout.IsValid)
                Properties.Settings.Default.ConRDPOverallConnectionTimeout = ConRDPOverallConnectionTimeout.Value;
        }

        private void ApplyAutoSaveEveryMinutes()
        {
            if (AutoSaveEveryMinutes.IsValid)
                Properties.OptionsBackupPage.Default.AutoSaveEveryMinutes = AutoSaveEveryMinutes.Value;
        }
    }
}