using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Windows.Forms;
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
    /// <summary>Where a floating panel's window was: its normal bounds and whether it was maximized.</summary>
    public sealed record FloatingPanelPlacement(Rectangle Bounds, bool Maximized);

    /// <summary>
    /// The panels of a session: the panel each open connection was in, by connection Id, and the placement
    /// of the panels that were floating, by panel name. Panels without a placement were docked.
    /// </summary>
    public sealed class SessionPanels
    {
        public SessionPanels(IReadOnlyDictionary<string, string> connectionPanels,
                             IReadOnlyDictionary<string, FloatingPanelPlacement> floatingPanels)
        {
            ConnectionPanels = connectionPanels ?? throw new ArgumentNullException(nameof(connectionPanels));
            FloatingPanels = floatingPanels ?? throw new ArgumentNullException(nameof(floatingPanels));
        }

        public static SessionPanels Empty { get; } = new(
            new Dictionary<string, string>(StringComparer.Ordinal),
            new Dictionary<string, FloatingPanelPlacement>(StringComparer.Ordinal));

        public IReadOnlyDictionary<string, string> ConnectionPanels { get; }

        public IReadOnlyDictionary<string, FloatingPanelPlacement> FloatingPanels { get; }
    }

    /// <summary>
    /// Remembers which panel each open connection was in when mRemoteNG closed, and where the floating
    /// panels were, so that the connections of the previous session reopen in the same panels, which are
    /// created again as they are needed.
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
        private const string PanelElementName = "Panel";
        private const string IdAttributeName = "Id";
        private const string PanelAttributeName = "Panel";
        private const string NameAttributeName = "Name";
        private const string BoundsAttributeName = "Bounds";
        private const string MaximizedAttributeName = "Maximized";

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

        /// <summary>The saved session panels. Empty when there is no file or it cannot be read.</summary>
        public SessionPanels Load()
        {
            Dictionary<string, string> connectionPanels = new(StringComparer.Ordinal);
            Dictionary<string, FloatingPanelPlacement> floatingPanels = new(StringComparer.Ordinal);
            try
            {
                if (!File.Exists(_filePath))
                    return SessionPanels.Empty;

                XmlDocument document = SecureXmlHelper.LoadXmlFromFile(_filePath);
                if (document.DocumentElement?.Name != RootElementName)
                    return SessionPanels.Empty;

                foreach (XmlElement element in document.DocumentElement.ChildNodes.OfType<XmlElement>())
                {
                    switch (element.Name)
                    {
                        case ConnectionElementName:
                            string id = element.GetAttribute(IdAttributeName);
                            string panel = element.GetAttribute(PanelAttributeName);
                            if (id.Length > 0 && panel.Length > 0)
                                connectionPanels.TryAdd(id, panel);
                            break;
                        case PanelElementName:
                            string name = element.GetAttribute(NameAttributeName);
                            if (name.Length > 0 && TryParseBounds(element.GetAttribute(BoundsAttributeName), out Rectangle bounds))
                            {
                                bool maximized = string.Equals(element.GetAttribute(MaximizedAttributeName), "true",
                                                               StringComparison.OrdinalIgnoreCase);
                                floatingPanels.TryAdd(name, new FloatingPanelPlacement(bounds, maximized));
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector?.AddExceptionMessage(
                    "Could not read the panels of the previous session from " + _filePath, ex, MessageClass.WarningMsg);
                return SessionPanels.Empty;
            }

            return new SessionPanels(connectionPanels, floatingPanels);
        }

        public void Save(SessionPanels sessionPanels)
        {
            if (sessionPanels == null)
                throw new ArgumentNullException(nameof(sessionPanels));

            try
            {
                XDocument document = new(
                    new XElement(RootElementName,
                        sessionPanels.FloatingPanels.Select(panel => new XElement(PanelElementName,
                            new XAttribute(NameAttributeName, panel.Key),
                            new XAttribute(BoundsAttributeName, FormatBounds(panel.Value.Bounds)),
                            new XAttribute(MaximizedAttributeName, panel.Value.Maximized ? "true" : "false"))),
                        sessionPanels.ConnectionPanels.Select(connection => new XElement(ConnectionElementName,
                            new XAttribute(IdAttributeName, connection.Key),
                            new XAttribute(PanelAttributeName, connection.Value)))));

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
        /// The panel each open connection is in, and where the floating panels that have connections are.
        /// A connection open in more than one panel keeps the first.
        /// </summary>
        public static SessionPanels Collect(IEnumerable<ConnectionWindow> connectionPanels)
        {
            Dictionary<string, string> panels = new(StringComparer.Ordinal);
            Dictionary<string, FloatingPanelPlacement> floatingPanels = new(StringComparer.Ordinal);
            foreach (ConnectionWindow connectionPanel in connectionPanels)
            {
                if (connectionPanel == null || connectionPanel.IsDisposed)
                    continue;

                string panelName = connectionPanel.PanelName;
                bool hasConnections = false;
                // Contents rather than Documents, so tabs minimized to the bottom of the panel count too
                foreach (IDockContent content in connectionPanel.connDock.Contents.ToArray())
                {
                    string? id = GetConnectionId(content);
                    if (string.IsNullOrEmpty(id))
                        continue;

                    hasConnections = true;
                    panels.TryAdd(id, panelName);
                }

                if (hasConnections && GetFloatingPlacement(connectionPanel) is { } placement)
                    floatingPanels.TryAdd(panelName, placement);
            }

            return new SessionPanels(panels, floatingPanels);
        }

        private static FloatingPanelPlacement? GetFloatingPlacement(ConnectionWindow connectionPanel)
        {
            if (connectionPanel.DockState != DockState.Float ||
                connectionPanel.DockHandler.FloatPane?.FloatWindow is not { IsDisposed: false } floatWindow)
                return null;

            // RestoreBounds holds the normal bounds of a maximized or minimized window
            Rectangle bounds = floatWindow.WindowState == FormWindowState.Normal
                ? floatWindow.Bounds
                : floatWindow.RestoreBounds;
            if (bounds.Width <= 0 || bounds.Height <= 0)
                return null;

            return new FloatingPanelPlacement(bounds, floatWindow.WindowState == FormWindowState.Maximized);
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

        private static string FormatBounds(Rectangle bounds)
        {
            return string.Join(",", new[] { bounds.X, bounds.Y, bounds.Width, bounds.Height }
                                    .Select(value => value.ToString(CultureInfo.InvariantCulture)));
        }

        private static bool TryParseBounds(string text, out Rectangle bounds)
        {
            bounds = Rectangle.Empty;
            string[] parts = text.Split(',');
            if (parts.Length != 4)
                return false;

            int[] values = new int[4];
            for (int i = 0; i < 4; i++)
            {
                if (!int.TryParse(parts[i].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out values[i]))
                    return false;
            }

            if (values[2] <= 0 || values[3] <= 0)
                return false;

            bounds = new Rectangle(values[0], values[1], values[2], values[3]);
            return true;
        }
    }
}
