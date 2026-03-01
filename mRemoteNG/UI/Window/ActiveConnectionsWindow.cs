using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Themes;
using mRemoteNG.UI.Tabs;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.UI.Window
{
    [SupportedOSPlatform("windows")]
    public class ActiveConnectionsWindow : BaseWindow
    {
        private ListView _listView = null!;
        private readonly ThemeManager _themeManager;

        public ActiveConnectionsWindow()
        {
            WindowType = WindowType.ActiveConnections;
            DockPnl = this;
            InitializeComponent();
            _themeManager = ThemeManager.getInstance();
            _themeManager.ThemeChanged += ApplyThemeHandler;
            ApplyTheme();

            Runtime.ConnectionInitiator.ConnectionOpened += OnConnectionChanged;
            Runtime.ConnectionInitiator.ConnectionClosed += OnConnectionChanged;
            Load += (_, _) => RefreshList();
        }

        private void OnConnectionChanged(string hostname, string protocol)
        {
            if (IsDisposed) return;
            if (InvokeRequired)
            {
                try { BeginInvoke(new MethodInvoker(RefreshList)); }
                catch (ObjectDisposedException)

                {

                    _ = 0; // Intentionally empty

                }
                catch (InvalidOperationException)

                {

                    _ = 0; // Intentionally empty

                }
            }
            else
            {
                RefreshList();
            }
        }

        public void RefreshList()
        {
            if (IsDisposed) return;
            _listView.BeginUpdate();
            _listView.Items.Clear();

            foreach (ConnectionInfo entry in GetOpenConnections())
            {
                var item = new ListViewItem(entry.Name);
                item.SubItems.Add(entry.Hostname);
                item.SubItems.Add(entry.Protocol.ToString());
                item.Tag = entry;
                _listView.Items.Add(item);
            }

            _listView.EndUpdate();
        }

        private static IEnumerable<ConnectionInfo> GetOpenConnections()
        {
            var seen = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < Runtime.WindowList.Count; i++)
            {
                if (Runtime.WindowList[i] is not ConnectionWindow connectionWindow) continue;
                if (connectionWindow.Controls.Count < 1) continue;
                if (connectionWindow.Controls[0] is not DockPanel dockPanel) continue;

                foreach (IDockContent dockContent in dockPanel.Documents)
                {
                    if (dockContent is not ConnectionTab tab) continue;
                    InterfaceControl? ic = InterfaceControl.FindInterfaceControl(tab);
                    if (ic?.OriginalInfo == null) continue;
                    if (seen.Add(ic.OriginalInfo.ConstantID))
                        yield return ic.OriginalInfo;
                }
            }
        }

        private void ListView_DoubleClick(object sender, EventArgs e)
        {
            SwitchToSelected();
        }

        private void ListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SwitchToSelected();
        }

        private void SwitchToSelected()
        {
            if (_listView.SelectedItems.Count == 0) return;
            if (_listView.SelectedItems[0].Tag is ConnectionInfo connectionInfo)
                Runtime.ConnectionInitiator.SwitchToOpenConnection(connectionInfo);
        }

        private void DisconnectSelected()
        {
            if (_listView.SelectedItems.Count == 0) return;
            if (_listView.SelectedItems[0].Tag is not ConnectionInfo connectionInfo) return;
            foreach (ProtocolBase protocol in connectionInfo.OpenConnections)
                protocol.Close();
        }

        private void ContextMenu_SwitchTo_Click(object sender, EventArgs e) => SwitchToSelected();
        private void ContextMenu_Disconnect_Click(object sender, EventArgs e) => DisconnectSelected();

        private void ApplyThemeHandler()
        {
            ApplyTheme();
            if (!_themeManager.ActiveAndExtended) return;
            _listView.BackColor = _themeManager.ActiveTheme.ExtendedPalette?.getColor("List_Background") ?? SystemColors.Window;
            _listView.ForeColor = _themeManager.ActiveTheme.ExtendedPalette?.getColor("List_Foreground") ?? SystemColors.WindowText;
        }

        private void InitializeComponent()
        {
            _listView = new ListView();
            SuspendLayout();

            // context menu
            var contextMenu = new ContextMenuStrip();
            var menuSwitchTo = new ToolStripMenuItem { Text = "Switch To" };
            menuSwitchTo.Click += ContextMenu_SwitchTo_Click;
            var menuDisconnect = new ToolStripMenuItem { Text = "Disconnect" };
            menuDisconnect.Click += ContextMenu_Disconnect_Click;
            contextMenu.Items.AddRange(new ToolStripItem[] { menuSwitchTo, menuDisconnect });

            // listView
            _listView.AccessibleName = "Active Connections";
            _listView.AccessibleDescription = "List of currently open connections";
            _listView.Columns.AddRange(new[]
            {
                new ColumnHeader { Text = "Name", Width = 160 },
                new ColumnHeader { Text = "Host", Width = 180 },
                new ColumnHeader { Text = "Protocol", Width = 90 },
            });
            _listView.ContextMenuStrip = contextMenu;
            _listView.Dock = DockStyle.Fill;
            _listView.FullRowSelect = true;
            _listView.GridLines = true;
            _listView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            _listView.MultiSelect = false;
            _listView.Name = "lvActiveConnections";
            _listView.View = View.Details;
            _listView.DoubleClick += ListView_DoubleClick;
            _listView.KeyDown += ListView_KeyDown;

            // ActiveConnectionsWindow
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(460, 300);
            Controls.Add(_listView);
            Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            HideOnClose = true;
            Name = "ActiveConnectionsWindow";
            TabText = "Active Connections";
            Text = "Active Connections";
            ResumeLayout(false);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _themeManager.ThemeChanged -= ApplyThemeHandler;
                Runtime.ConnectionInitiator.ConnectionOpened -= OnConnectionChanged;
                Runtime.ConnectionInitiator.ConnectionClosed -= OnConnectionChanged;
            }
            base.Dispose(disposing);
        }
    }
}
