using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Properties;
using mRemoteNG.UI.Forms;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;


namespace mRemoteNG.Tools
{
    [SupportedOSPlatform("windows")]
    public class NotificationAreaIcon
    {
        private readonly NotifyIcon? _nI;
        private readonly ContextMenuStrip? _cMen;
        private readonly ToolStripMenuItem? _cMenCons;
        private readonly ToolStripMenuItem? _cMenWol;
        private static readonly FrmMain FrmMain = FrmMain.Default;

        public bool Disposed { get; private set; }

        public NotificationAreaIcon()
        {
            try
            {
                _cMenCons = new ToolStripMenuItem
                {
                    Text = Language.Connections,
                    Image = Properties.Resources.ASPWebSite_16x
                };

                _cMenWol = new ToolStripMenuItem
                {
                    Text = Language.ResourceManager.GetString("WakeOnLan", Language.Culture) ?? "Wake On LAN"
                };

                ToolStripSeparator cMenSep1 = new();

                ToolStripMenuItem cMenExit = new() { Text = Language.Exit};
                cMenExit.Click += cMenExit_Click;

                _cMen = new ContextMenuStrip
                {
                    Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular,
                                                   System.Drawing.GraphicsUnit.Point, Convert.ToByte(0)),
                    RenderMode = ToolStripRenderMode.Professional
                };
                _cMen.Items.AddRange(new ToolStripItem[] {_cMenCons, _cMenWol, cMenSep1, cMenExit});

                _nI = new NotifyIcon
                {
                    Text = @"mRemoteNG",
                    BalloonTipText = @"mRemoteNG",
                    Icon = Properties.Resources.mRemoteNG_Icon,
                    ContextMenuStrip = _cMen,
                    Visible = true
                };

                _nI.MouseClick += nI_MouseClick;
                _nI.MouseDoubleClick += nI_MouseDoubleClick;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("Creating new SysTrayIcon failed", ex);
            }
        }

        public void Dispose()
        {
            try
            {
                if (_nI != null)
                {
                    _nI.Visible = false;
                    _nI.Dispose();
                }

                _cMen?.Dispose();
                Disposed = true;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("Disposing SysTrayIcon failed", ex);
            }
        }

        private void nI_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            if (_cMenCons == null || _cMenWol == null) return;

            _cMenCons.DropDownItems.Clear();
            _cMenWol.DropDownItems.Clear();

            var connectionTreeModel = Runtime.ConnectionsService.ConnectionTreeModel;
            if (connectionTreeModel == null) return;

            ConnectionsTreeToMenuItemsConverter connectMenuItemsConverter = new()
            {
                MouseUpEventHandler = ConMenItem_MouseUp
            };

            // ReSharper disable once CoVariantArrayConversion
            ToolStripItem[] rootConnectionMenuItems = connectMenuItemsConverter
                                                      .CreateToolStripDropDownItems(connectionTreeModel).ToArray();
            _cMenCons.DropDownItems.AddRange(rootConnectionMenuItems);

            ConnectionsTreeToMenuItemsConverter wakeOnLanMenuItemsConverter = new()
            {
                MouseUpEventHandler = WakeOnLanMenuItem_MouseUp
            };

            // ReSharper disable once CoVariantArrayConversion
            ToolStripItem[] rootWakeOnLanMenuItems = wakeOnLanMenuItemsConverter
                                                     .CreateToolStripDropDownItems(connectionTreeModel).ToArray();
            ConfigureWakeOnLanMenuItems(rootWakeOnLanMenuItems);
            _cMenWol.DropDownItems.AddRange(rootWakeOnLanMenuItems);
            _cMenWol.Enabled = _cMenWol.DropDownItems.Cast<ToolStripItem>().Any(item => item.Enabled);
        }

        private static void nI_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (FrmMain.Visible)
            {
                HideForm();
                FrmMain.ShowInTaskbar = false;
            }
            else
            {
                if (ShowForm())
                    FrmMain.ShowInTaskbar = true;
            }
        }

        private static bool ShowForm()
        {
            if (!FrmMain.TryUnlockIfNeeded())
                return false;

            FrmMain.Show();
            FrmMain.WindowState = FrmMain.PreviousWindowState;

            if (Properties.OptionsAppearancePage.Default.ShowSystemTrayIcon) return true;
            Runtime.NotificationAreaIcon?.Dispose();
            Runtime.NotificationAreaIcon = null;
            return true;
        }

        private static void HideForm()
        {
            FrmMain.Hide();
            FrmMain.PreviousWindowState = FrmMain.WindowState;
        }

        private void ConMenItem_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (((ToolStripMenuItem)sender).Tag is ContainerInfo) return;
            if (FrmMain.Visible == false && !ShowForm())
                return;
            if (((ToolStripMenuItem)sender).Tag is ConnectionInfo connectionInfo)
                Runtime.ConnectionInitiator.OpenConnection(connectionInfo);
        }

        private static void ConfigureWakeOnLanMenuItems(IEnumerable<ToolStripItem> menuItems)
        {
            foreach (ToolStripItem menuItem in menuItems)
            {
                if (menuItem is not ToolStripMenuItem toolStripMenuItem)
                    continue;

                ConfigureWakeOnLanMenuItem(toolStripMenuItem);
            }
        }

        private static bool ConfigureWakeOnLanMenuItem(ToolStripMenuItem menuItem)
        {
            if (menuItem.Tag is ConnectionInfo connectionInfo && connectionInfo is not ContainerInfo)
            {
                bool canWake = WakeOnLan.IsValidMacAddress(connectionInfo.MacAddress);
                menuItem.Enabled = canWake;
                return canWake;
            }

            bool hasWakeableChild = false;
            foreach (ToolStripItem childItem in menuItem.DropDownItems)
            {
                if (childItem is not ToolStripMenuItem childMenuItem)
                    continue;

                hasWakeableChild |= ConfigureWakeOnLanMenuItem(childMenuItem);
            }

            menuItem.Enabled = hasWakeableChild;
            return hasWakeableChild;
        }

        private static void WakeOnLanMenuItem_MouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (sender is not ToolStripMenuItem menuItem) return;
            if (menuItem.Tag is not ConnectionInfo connectionInfo || connectionInfo is ContainerInfo) return;
            if (!WakeOnLan.IsValidMacAddress(connectionInfo.MacAddress)) return;

            WakeOnLan.TrySendMagicPacket(connectionInfo.MacAddress);
        }

        private static void cMenExit_Click(object sender, EventArgs e)
        {
            Shutdown.Quit();
        }
    }
}
