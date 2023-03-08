using System;
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
        private readonly NotifyIcon _nI;
        private readonly ContextMenuStrip _cMen;
        private readonly ToolStripMenuItem _cMenCons;
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

                var cMenSep1 = new ToolStripSeparator();

                var cMenExit = new ToolStripMenuItem {Text = Language.Exit};
                cMenExit.Click += cMenExit_Click;

                _cMen = new ContextMenuStrip
                {
                    Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular,
                                                   System.Drawing.GraphicsUnit.Point, Convert.ToByte(0)),
                    RenderMode = ToolStripRenderMode.Professional
                };
                _cMen.Items.AddRange(new ToolStripItem[] {_cMenCons, cMenSep1, cMenExit});

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
                _nI.Visible = false;
                _nI.Dispose();
                _cMen.Dispose();
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
            _cMenCons.DropDownItems.Clear();
            var menuItemsConverter = new ConnectionsTreeToMenuItemsConverter
            {
                MouseUpEventHandler = ConMenItem_MouseUp
            };

            // ReSharper disable once CoVariantArrayConversion
            ToolStripItem[] rootMenuItems = menuItemsConverter
                                            .CreateToolStripDropDownItems(Runtime.ConnectionsService
                                                                                 .ConnectionTreeModel).ToArray();
            _cMenCons.DropDownItems.AddRange(rootMenuItems);
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
                ShowForm();
                FrmMain.ShowInTaskbar = true;
            }
        }

        private static void ShowForm()
        {
            FrmMain.Show();
            FrmMain.WindowState = FrmMain.PreviousWindowState;

            if (Properties.OptionsAppearancePage.Default.ShowSystemTrayIcon) return;
            Runtime.NotificationAreaIcon.Dispose();
            Runtime.NotificationAreaIcon = null;
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
            if (FrmMain.Visible == false)
                ShowForm();
            Runtime.ConnectionInitiator.OpenConnection((ConnectionInfo)((ToolStripMenuItem)sender).Tag);
        }

        private static void cMenExit_Click(object sender, EventArgs e)
        {
            Shutdown.Quit();
        }
    }
}