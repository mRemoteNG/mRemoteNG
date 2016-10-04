using System;
using System.Collections.Generic;
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
			private ToolStripSeparator _cMenSep1;
			private ToolStripMenuItem _cMenExit;
			
			private bool _Disposed;
            public bool Disposed
			{
				get
				{
					return _Disposed;
				}
				set
				{
					_Disposed = value;
				}
			}
			
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
					_cMen.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
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
					_Disposed = true;
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
				if (frmMain.Default.Visible == true)
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
			
			private void ConMenItem_MouseUp(Object sender, MouseEventArgs e)
			{
				if (e.Button == MouseButtons.Left)
				{
					if (((Control)sender).Tag is ConnectionInfo)
					{
						if (frmMain.Default.Visible == false)
						{
							ShowForm();
						}
                        ConnectionInitiator.OpenConnection((ConnectionInfo)((Control)sender).Tag);
					}
				}
			}
			
			private void cMenExit_Click(Object sender, EventArgs e)
			{
				Shutdown.Quit();
			}
		}
		
		public static SaveFileDialog ConnectionsSaveAsDialog()
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.CheckPathExists = true;
			saveFileDialog.InitialDirectory = App.Info.ConnectionsFileInfo.DefaultConnectionsPath;
			saveFileDialog.FileName = App.Info.ConnectionsFileInfo.DefaultConnectionsFile;
			saveFileDialog.OverwritePrompt = true;
				
			saveFileDialog.Filter = Language.strFiltermRemoteXML + "|*.xml|" + Language.strFilterAll + "|*.*";
				
			return saveFileDialog;
		}
		
		public static OpenFileDialog ConnectionsLoadDialog()
		{
			OpenFileDialog lDlg = new OpenFileDialog();
			lDlg.CheckFileExists = true;
			lDlg.InitialDirectory = App.Info.ConnectionsFileInfo.DefaultConnectionsPath;
			lDlg.Filter = Language.strFiltermRemoteXML + "|*.xml|" + Language.strFilterAll + "|*.*";
				
			return lDlg;
		}
	}
}