using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Versioning;
using System.Xml;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Messages;
using mRemoteNG.Properties;
using mRemoteNG.Resources.Language;
using mRemoteNG.UI.Controls;

namespace mRemoteNG.Config.Settings
{
    [SupportedOSPlatform("windows")]
    public class QuickConnectHistoryLoader
    {
        private readonly QuickConnectComboBox _comboBox;

        private readonly struct QuickConnectHistoryItem(string hostname, int port, ProtocolType protocol, bool connected)
        {
            public string Hostname { get; } = hostname;
            public int Port { get; } = port;
            public ProtocolType Protocol { get; } = protocol;
            public bool Connected { get; } = connected;
        }

        public QuickConnectHistoryLoader(QuickConnectComboBox comboBox)
        {
            _comboBox = comboBox ?? throw new ArgumentNullException(nameof(comboBox));
        }

        public void Load()
        {
            try
            {
                List<QuickConnectHistoryItem> historyItems = LoadHistoryItems();
                Runtime.MessageCollector.AddMessage(
                    MessageClass.InformationMsg,
                    $"Loading {historyItems.Count} quick connect history item(s) into the toolbar.",
                    true);

                foreach (QuickConnectHistoryItem item in historyItems)
                {
                    ConnectionInfo connectionInfo = BuildConnectionInfo(item);
                    _comboBox.Add(connectionInfo);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("LoadQuickConnectHistory failed", ex);
            }
        }

        public static IEnumerable<ConnectionInfo> LoadPreviouslyConnectedQuickConnectSessions()
        {
            try
            {
                List<ConnectionInfo> reconnectSessions = [];
                foreach (QuickConnectHistoryItem item in LoadHistoryItems())
                {
                    if (!item.Connected)
                        continue;

                    ConnectionInfo connectionInfo = BuildConnectionInfo(item);
                    connectionInfo.PleaseConnect = true;
                    reconnectSessions.Add(connectionInfo);
                }

                return reconnectSessions;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("LoadQuickConnectHistory failed", ex);
                return [];
            }
        }

        private static List<QuickConnectHistoryItem> LoadHistoryItems()
        {
            string settingsPath = SettingsFileInfo.SettingsPath;
            if (string.IsNullOrWhiteSpace(settingsPath))
            {
                Runtime.MessageCollector.AddMessage(
                    MessageClass.WarningMsg,
                    "LoadQuickConnectHistory skipped because the settings path is empty.",
                    true);
                return [];
            }

            string filePath = Path.Combine(settingsPath, SettingsFileInfo.QuickConnectHistoryFileName);
            if (!File.Exists(filePath))
            {
                Runtime.MessageCollector.AddMessage(
                    MessageClass.InformationMsg,
                    $"Quick connect history file not found at '{filePath}'.",
                    true);
                return [];
            }

            XmlDocument doc = new();
            doc.Load(filePath);

            XmlNodeList? items = doc.SelectNodes("//Item");
            if (items == null)
                return [];

            List<QuickConnectHistoryItem> historyItems = [];
            foreach (XmlNode item in items)
            {
                string hostname = item.Attributes?["Hostname"]?.Value ?? string.Empty;
                if (string.IsNullOrEmpty(hostname))
                    continue;

                _ = int.TryParse(item.Attributes?["Port"]?.Value, out int port);
                _ = Enum.TryParse(item.Attributes?["Protocol"]?.Value, out ProtocolType protocol);
                _ = bool.TryParse(item.Attributes?["Connected"]?.Value, out bool connected);

                historyItems.Add(new QuickConnectHistoryItem(hostname, port, protocol, connected));
            }

            Runtime.MessageCollector.AddMessage(
                MessageClass.InformationMsg,
                $"Loaded {historyItems.Count} quick connect history item(s) from '{filePath}'.",
                true);

            return historyItems;
        }

        private static ConnectionInfo BuildConnectionInfo(QuickConnectHistoryItem item)
        {
            // Copy default connection settings (including RedirectClipboard, credentials, etc.)
            // so that history-restored sessions behave the same as connections created via the
            // Quick Connect toolbar button (#1577).
            ConnectionInfo connectionInfo = new();
            connectionInfo.CopyFrom(DefaultConnectionInfo.Instance);

            connectionInfo.Hostname = item.Hostname;
            connectionInfo.Protocol = item.Protocol;
            connectionInfo.IsQuickConnect = true;
            connectionInfo.PleaseConnect = item.Connected;

            connectionInfo.Name = OptionsTabsPanelsPage.Default.IdentifyQuickConnectTabs
                ? string.Format(CultureInfo.InvariantCulture, Language.Quick, item.Hostname)
                : item.Hostname;

            connectionInfo.Port = item.Port > 0
                ? item.Port
                : connectionInfo.GetDefaultPort();

            return connectionInfo;
        }
    }
}
