using System;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.UI.Forms;


namespace mRemoteNG.Tools
{
	public class Controls
	{
		public class NotificationAreaIcon
		{
			private NotifyIcon _nI;
			private ContextMenuStrip _cMen;
			private ToolStripMenuItem _cMenCons;

		    public bool Disposed { get; set; }

		    public NotificationAreaIcon()
			{
			    try
				{
				    _cMenCons = new ToolStripMenuItem
				    {
				        Text = Language.strConnections,
				        Image = Resources.Root
				    };

				    var cMenSep1 = new ToolStripSeparator();

				    var cMenExit = new ToolStripMenuItem {Text = Language.strMenuExit};
				    cMenExit.Click += cMenExit_Click;

				    _cMen = new ContextMenuStrip
				    {
				        Font =
				            new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular,
				                System.Drawing.GraphicsUnit.Point, Convert.ToByte(0)),
				        RenderMode = ToolStripRenderMode.Professional
				    };
				    _cMen.Items.AddRange(new ToolStripItem[] {_cMenCons, cMenSep1, cMenExit});

				    _nI = new NotifyIcon
				    {
				        Text = "mRemote",
				        BalloonTipText = "mRemote",
				        Icon = Resources.mRemote_Icon,
				        ContextMenuStrip = _cMen,
				        Visible = true
				    };

				    _nI.MouseClick += nI_MouseClick;
					_nI.MouseDoubleClick += nI_MouseDoubleClick;
				}
				catch (Exception ex)
				{
					Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Creating new SysTrayIcon failed" + Environment.NewLine + ex.Message, true);
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
					Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Disposing SysTrayIcon failed" + Environment.NewLine + ex.Message, true);
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

                ToolStripItem[] rootMenuItems = menuItemsConverter.CreateToolStripDropDownItems(Runtime.ConnectionTreeModel).ToArray();
                _cMenCons.DropDownItems.AddRange(rootMenuItems);
            }
			
			private void nI_MouseDoubleClick(object sender, MouseEventArgs e)
			{
				if (frmMain.Default.Visible)
				{
					HideForm();
				}
				else
				{
					ShowForm();
				}
			}
			
			private void ShowForm()
			{
				frmMain.Default.Show();
				frmMain.Default.WindowState = frmMain.Default.PreviousWindowState;

			    if (Settings.Default.ShowSystemTrayIcon) return;
			    Runtime.NotificationAreaIcon.Dispose();
			    Runtime.NotificationAreaIcon = null;
			}
			
			private void HideForm()
			{
				frmMain.Default.Hide();
				frmMain.Default.PreviousWindowState = frmMain.Default.WindowState;
			}
			
			private void ConMenItem_MouseUp(Object sender, MouseEventArgs e)
			{
			    if (e.Button != MouseButtons.Left) return;
			    if (!(((Control) sender).Tag is ConnectionInfo)) return;
			    if (frmMain.Default.Visible == false)
			    {
			        ShowForm();
			    }
			    ConnectionInitiator.OpenConnection((ConnectionInfo)((Control)sender).Tag);
			}
			
			private void cMenExit_Click(Object sender, EventArgs e)
			{
				Shutdown.Quit();
			}
		}
		
		public static SaveFileDialog ConnectionsSaveAsDialog()
		{
			var saveFileDialog = new SaveFileDialog();
			saveFileDialog.CheckPathExists = true;
			saveFileDialog.InitialDirectory = App.Info.ConnectionsFileInfo.DefaultConnectionsPath;
			saveFileDialog.FileName = App.Info.ConnectionsFileInfo.DefaultConnectionsFile;
			saveFileDialog.OverwritePrompt = true;
				
			saveFileDialog.Filter = Language.strFiltermRemoteXML + "|*.xml|" + Language.strFilterAll + "|*.*";
				
			return saveFileDialog;
		}
		
		public static OpenFileDialog ConnectionsLoadDialog()
		{
		    var lDlg = new OpenFileDialog
		    {
		        CheckFileExists = true,
		        InitialDirectory = App.Info.ConnectionsFileInfo.DefaultConnectionsPath,
		        Filter = Language.strFiltermRemoteXML + "|*.xml|" + Language.strFilterAll + "|*.*"
		    };

		    return lDlg;
		}
	}
}