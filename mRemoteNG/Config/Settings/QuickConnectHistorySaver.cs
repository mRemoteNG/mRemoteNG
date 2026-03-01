using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Xml;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Messages;
using mRemoteNG.UI.Controls;
using mRemoteNG.UI.Tabs;
using mRemoteNG.UI.Window;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.Config.Settings
{
    [SupportedOSPlatform("windows")]
    public static class QuickConnectHistorySaver
    {
        private static HashSet<QuickConnectSessionKey>? _capturedOpenQuickConnectSessions;
        private static List<QuickConnectComboBox.HistoryItemData>? _capturedHistoryItems;

        private readonly struct QuickConnectSessionKey : IEquatable<QuickConnectSessionKey>
        {
            public QuickConnectSessionKey(string hostname, int port, ProtocolType protocol)
            {
                Hostname = hostname?.Trim() ?? string.Empty;
                Port = port;
                Protocol = protocol;
            }

            public string Hostname { get; }
            public int Port { get; }
            public ProtocolType Protocol { get; }

            public bool Equals(QuickConnectSessionKey other)
            {
                return Port == other.Port
                       && Protocol == other.Protocol
                       && string.Equals(Hostname, other.Hostname, StringComparison.OrdinalIgnoreCase);
            }

            public override bool Equals(object? obj)
            {
                return obj is QuickConnectSessionKey other && Equals(other);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Hostname.ToUpperInvariant(), Port, Protocol);
            }
        }

        public static void CaptureOpenQuickConnectSessionsForShutdown(QuickConnectComboBox? comboBox = null)
        {
            _capturedOpenQuickConnectSessions = GetOpenQuickConnectSessions();
            _capturedHistoryItems = CaptureHistoryItems(comboBox);
        }

        public static void Save(QuickConnectComboBox comboBox)
        {
            try
            {
                string settingsPath = SettingsFileInfo.SettingsPath;
                if (string.IsNullOrWhiteSpace(settingsPath))
                {
                    Runtime.MessageCollector.AddMessage(
                        MessageClass.WarningMsg,
                        "SaveQuickConnectHistory skipped because the settings path is empty.",
                        true);
                    _capturedOpenQuickConnectSessions = null;
                    _capturedHistoryItems = null;
                    return;
                }

                if (!Directory.Exists(settingsPath))
                    Directory.CreateDirectory(settingsPath);

                string filePath = Path.Combine(settingsPath, SettingsFileInfo.QuickConnectHistoryFileName);
                HashSet<QuickConnectSessionKey> connectedQuickSessions = _capturedOpenQuickConnectSessions ?? GetOpenQuickConnectSessions();
                List<QuickConnectComboBox.HistoryItemData> historyItems = _capturedHistoryItems ?? comboBox.GetHistoryItems().ToList();
                _capturedOpenQuickConnectSessions = null;
                _capturedHistoryItems = null;

                Runtime.MessageCollector.AddMessage(
                    MessageClass.InformationMsg,
                    $"Saving quick connect history to '{filePath}' with {historyItems.Count} item(s).",
                    true);

                using XmlTextWriter writer = new(filePath, Encoding.UTF8)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4
                };

                writer.WriteStartDocument();
                writer.WriteStartElement("History");

                foreach (QuickConnectComboBox.HistoryItemData item in historyItems)
                {
                    writer.WriteStartElement("Item");
                    writer.WriteAttributeString("Hostname", item.Hostname);
                    writer.WriteAttributeString("Port", Convert.ToString(item.Port, CultureInfo.InvariantCulture));
                    writer.WriteAttributeString("Protocol", item.Protocol.ToString());
                    bool connected = connectedQuickSessions.Contains(new QuickConnectSessionKey(item.Hostname, item.Port, item.Protocol));
                    writer.WriteAttributeString("Connected", connected.ToString().ToLowerInvariant());
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();

                Runtime.MessageCollector.AddMessage(
                    MessageClass.InformationMsg,
                    $"Saved quick connect history file '{filePath}'.",
                    true);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("SaveQuickConnectHistory failed", ex);
            }
        }

        private static List<QuickConnectComboBox.HistoryItemData>? CaptureHistoryItems(QuickConnectComboBox? comboBox)
        {
            if (comboBox == null)
            {
                Runtime.MessageCollector.AddMessage(
                    MessageClass.WarningMsg,
                    "Quick Connect history capture skipped because the combo box was null.",
                    true);
                return null;
            }

            try
            {
                List<QuickConnectComboBox.HistoryItemData> historyItems = comboBox.GetHistoryItems().ToList();
                Runtime.MessageCollector.AddMessage(
                    MessageClass.InformationMsg,
                    $"Captured {historyItems.Count} quick connect history item(s) for shutdown.",
                    true);
                return historyItems;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(
                    "CaptureQuickConnectHistory failed",
                    ex,
                    MessageClass.WarningMsg,
                    true);
                return null;
            }
        }

        private static HashSet<QuickConnectSessionKey> GetOpenQuickConnectSessions()
        {
            HashSet<QuickConnectSessionKey> sessions = [];

            if (Runtime.WindowList == null || Runtime.WindowList.Count == 0)
                return sessions;

            foreach (object window in Runtime.WindowList)
            {
                if (window is not ConnectionWindow connectionWindow)
                    continue;
                if (connectionWindow.Controls.Count < 1)
                    continue;
                if (connectionWindow.Controls[0] is not DockPanel dockPanel)
                    continue;

                foreach (IDockContent dockContent in dockPanel.DocumentsToArray())
                {
                    if (dockContent is not ConnectionTab connectionTab)
                        continue;

                    InterfaceControl? interfaceControl = InterfaceControl.FindInterfaceControl(connectionTab);
                    ConnectionInfo? connectionInfo = interfaceControl?.OriginalInfo ?? interfaceControl?.Info;
                    if (connectionInfo == null || !connectionInfo.IsQuickConnect)
                        continue;
                    if (string.IsNullOrWhiteSpace(connectionInfo.Hostname))
                        continue;

                    sessions.Add(new QuickConnectSessionKey(connectionInfo.Hostname, connectionInfo.Port, connectionInfo.Protocol));
                }
            }

            return sessions;
        }
    }
}
