using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Xml;
using System.Xml.Linq;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Connection;
using mRemoteNG.Messages;
using mRemoteNG.Security;
using mRemoteNG.UI.Tabs;
using mRemoteNG.UI.Window;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.UI.Panels
{
    /// <summary>
    /// Remembers which panel each open connection was in when mRemoteNG closed, so that the connections
    /// of the previous session reopen in the same panels, which are created again as they are needed.
    /// </summary>
    /// <remarks>
    /// This is the state of a session on this computer, like the panel layout, so it is kept in a file of
    /// its own in the settings folder. The connections file is not changed, and the Panel property of
    /// the connections stays as the user set it.
    /// </remarks>
    [SupportedOSPlatform("windows")]
    public class SessionPanelStore
    {
        public const string DefaultFileName = "sessionPanels.xml";
        private const string RootElementName = "SessionPanels";
        private const string ConnectionElementName = "Connection";
        private const string IdAttributeName = "Id";
        private const string PanelAttributeName = "Panel";

        private readonly string _filePath;

        public SessionPanelStore(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("A file path is required.", nameof(filePath));
            _filePath = filePath;
        }

        /// <summary>The store in the settings folder, next to the panel layout.</summary>
        public static SessionPanelStore ForSettingsFolder()
        {
            return new SessionPanelStore(Path.Combine(SettingsFileInfo.SettingsPath, DefaultFileName));
        }

        /// <summary>
        /// The panel each connection was in, by connection Id. Empty when there is no file or it cannot be read.
        /// </summary>
        public IReadOnlyDictionary<string, string> Load()
        {
            Dictionary<string, string> panels = new(StringComparer.Ordinal);
            try
            {
                if (!File.Exists(_filePath))
                    return panels;

                XmlDocument document = SecureXmlHelper.LoadXmlFromFile(_filePath);
                if (document.DocumentElement?.Name != RootElementName)
                    return panels;

                foreach (XmlElement element in document.DocumentElement.ChildNodes.OfType<XmlElement>())
                {
                    if (element.Name != ConnectionElementName)
                        continue;

                    string id = element.GetAttribute(IdAttributeName);
                    string panel = element.GetAttribute(PanelAttributeName);
                    if (id.Length > 0 && panel.Length > 0)
                        panels.TryAdd(id, panel);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector?.AddExceptionMessage(
                    "Could not read the panels of the previous session from " + _filePath, ex, MessageClass.WarningMsg);
                panels.Clear();
            }

            return panels;
        }

        public void Save(IReadOnlyDictionary<string, string> panels)
        {
            try
            {
                XDocument document = new(
                    new XElement(RootElementName,
                        panels.Select(panel => new XElement(ConnectionElementName,
                            new XAttribute(IdAttributeName, panel.Key),
                            new XAttribute(PanelAttributeName, panel.Value)))));

                string? folder = Path.GetDirectoryName(_filePath);
                if (!string.IsNullOrEmpty(folder))
                    Directory.CreateDirectory(folder);
                document.Save(_filePath);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector?.AddExceptionMessage(
                    "Could not save the panels of this session to " + _filePath, ex, MessageClass.WarningMsg);
            }
        }

        /// <summary>
        /// The panel each open connection is in, by connection Id. A connection open in more than one panel
        /// keeps the first.
        /// </summary>
        public static Dictionary<string, string> Collect(IEnumerable<ConnectionWindow> connectionPanels)
        {
            Dictionary<string, string> panels = new(StringComparer.Ordinal);
            foreach (ConnectionWindow connectionPanel in connectionPanels)
            {
                if (connectionPanel == null || connectionPanel.IsDisposed)
                    continue;

                string panelName = connectionPanel.PanelName;
                // Contents rather than Documents, so tabs minimized to the bottom of the panel count too
                foreach (IDockContent content in connectionPanel.connDock.Contents.ToArray())
                {
                    string? id = GetConnectionId(content);
                    if (!string.IsNullOrEmpty(id))
                        panels.TryAdd(id, panelName);
                }
            }

            return panels;
        }

        private static string? GetConnectionId(IDockContent content)
        {
            if (content is not ConnectionTab tab)
                return null;

            // The tab holds its ConnectionInfo until the protocol attaches its InterfaceControl. For a
            // connection through an SSH tunnel, Info is a copy and OriginalInfo the connection in the tree.
            ConnectionInfo? connectionInfo = tab.Tag switch
            {
                InterfaceControl interfaceControl => interfaceControl.OriginalInfo ?? interfaceControl.Info,
                ConnectionInfo info => info,
                _ => null
            };

            return connectionInfo?.ConstantID;
        }
    }
}
