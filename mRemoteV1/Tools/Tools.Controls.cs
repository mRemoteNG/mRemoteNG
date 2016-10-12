using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Connection;
using mRemoteNG.Messages;
using mRemoteNG.UI.Forms;

namespace mRemoteNG.Tools
{
    public class Controls
    {
        public static SaveFileDialog ConnectionsSaveAsDialog()
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.InitialDirectory = ConnectionsFileInfo.DefaultConnectionsPath;
            saveFileDialog.FileName = ConnectionsFileInfo.DefaultConnectionsFile;
            saveFileDialog.OverwritePrompt = true;

            saveFileDialog.Filter = Language.strFiltermRemoteXML + "|*.xml|" + Language.strFilterAll + "|*.*";

            return saveFileDialog;
        }

        public static OpenFileDialog ConnectionsLoadDialog()
        {
            var lDlg = new OpenFileDialog();
            lDlg.CheckFileExists = true;
            lDlg.InitialDirectory = ConnectionsFileInfo.DefaultConnectionsPath;
            lDlg.Filter = Language.strFiltermRemoteXML + "|*.xml|" + Language.strFilterAll + "|*.*";

            return lDlg;
        }

        public class NotificationAreaIcon
        {
            private readonly ContextMenuStrip _cMen;
            private readonly ToolStripMenuItem _cMenCons;
            private readonly ToolStripMenuItem _cMenExit;
            private readonly ToolStripSeparator _cMenSep1;

            private readonly NotifyIcon _nI;

            public NotificationAreaIcon()
            {
                try
                {
                    _cMenCons = new ToolStripMenuItem();
                    _cMenCons.Text = Language.strConnections;
                    _cMenCons.Image = Resources.Root;

                    _cMenSep1 = new ToolStripSeparator();

                    _cMenExit = new ToolStripMenuItem();
                    _cMenExit.Text = Language.strMenuExit;
                    _cMenExit.Click += cMenExit_Click;

                    _cMen = new ContextMenuStrip();
                    _cMen.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, Convert.ToByte(0));
                    _cMen.RenderMode = ToolStripRenderMode.Professional;
                    _cMen.Items.AddRange(new ToolStripItem[] {_cMenCons, _cMenSep1, _cMenExit});

                    _nI = new NotifyIcon();
                    _nI.Text = "mRemote";
                    _nI.BalloonTipText = "mRemote";
                    _nI.Icon = Resources.mRemote_Icon;
                    _nI.ContextMenuStrip = _cMen;
                    _nI.Visible = true;

                    _nI.MouseClick += nI_MouseClick;
                    _nI.MouseDoubleClick += nI_MouseDoubleClick;
                }
                catch (Exception ex)
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                        "Creating new SysTrayIcon failed" + Environment.NewLine + ex.Message, true);
                }
            }

            public bool Disposed { get; set; }

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
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                        "Disposing SysTrayIcon failed" + Environment.NewLine + ex.Message, true);
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

                ToolStripItem[] rootMenuItems =
                    menuItemsConverter.CreateToolStripDropDownItems(Runtime.ConnectionTreeModel).ToArray();
                _cMenCons.DropDownItems.AddRange(rootMenuItems);
            }

            private void nI_MouseDoubleClick(object sender, MouseEventArgs e)
            {
                if (frmMain.Default.Visible)
                    HideForm();
                else
                    ShowForm();
            }

            private void ShowForm()
            {
                frmMain.Default.Show();
                frmMain.Default.WindowState = frmMain.Default.PreviousWindowState;

                if (Settings.Default.ShowSystemTrayIcon == false)
                {
                    Runtime.NotificationAreaIcon.Dispose();
                    Runtime.NotificationAreaIcon = null;
                }
            }

            private void HideForm()
            {
                frmMain.Default.Hide();
                frmMain.Default.PreviousWindowState = frmMain.Default.WindowState;
            }

            private void ConMenItem_MouseUp(object sender, MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Left)
                    if (((Control) sender).Tag is ConnectionInfo)
                    {
                        if (frmMain.Default.Visible == false)
                            ShowForm();
                        ConnectionInitiator.OpenConnection((ConnectionInfo) ((Control) sender).Tag);
                    }
            }

            private void cMenExit_Click(object sender, EventArgs e)
            {
                Shutdown.Quit();
            }
        }
    }
}