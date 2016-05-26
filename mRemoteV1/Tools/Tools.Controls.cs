using System;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.My;
using mRemoteNG.UI.Forms;


namespace mRemoteNG.Tools
{
	public class Controls
	{
		public class ComboBoxItem
		{
			private string _Text;
            public string Text
			{
				get { return this._Text; }
				set { this._Text = value; }
			}
				
			private object _Tag;
            public object Tag
			{
				get
				{
					return this._Tag;
				}
				set
				{
					this._Tag = value;
				}
			}
				
			public ComboBoxItem(string Text, object Tag = null)
			{
				this._Text = Text;
				if (Tag != null)
				{
					this._Tag = Tag;
				}
			}
				
			public override string ToString()
			{
				return this._Text;
			}
		}
		
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
				
				
			//Public Event MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
			//Public Event MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
				
				
			public NotificationAreaIcon()
			{
				try
				{
					this._cMenCons = new ToolStripMenuItem();
					this._cMenCons.Text = Language.strConnections;
					this._cMenCons.Image = Resources.Root;
						
					this._cMenSep1 = new ToolStripSeparator();
						
					this._cMenExit = new ToolStripMenuItem();
					this._cMenExit.Text = Language.strMenuExit;
					this._cMenExit.Click += cMenExit_Click;
						
					this._cMen = new ContextMenuStrip();
					this._cMen.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
					this._cMen.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
					this._cMen.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {this._cMenCons, this._cMenSep1, this._cMenExit});
						
					this._nI = new NotifyIcon();
					this._nI.Text = "mRemote";
					this._nI.BalloonTipText = "mRemote";
					this._nI.Icon = Resources.mRemote_Icon;
					this._nI.ContextMenuStrip = this._cMen;
					this._nI.Visible = true;
						
					this._nI.MouseClick += nI_MouseClick;
					this._nI.MouseDoubleClick += nI_MouseDoubleClick;
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
					this._nI.Visible = false;
					this._nI.Dispose();
					this._cMen.Dispose();
					this._Disposed = true;
				}
				catch (Exception ex)
				{
					Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Disposing SysTrayIcon failed" + Environment.NewLine + ex.Message, true);
				}
			}
				
			private void nI_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
			{
				if (e.Button == MouseButtons.Right)
				{
					this._cMenCons.DropDownItems.Clear();
						
					foreach (TreeNode tNode in App.Windows.treeForm.tvConnections.Nodes)
					{
						AddNodeToMenu(tNode.Nodes, this._cMenCons);
					}
				}
			}
				
			private void AddNodeToMenu(TreeNodeCollection tnc, ToolStripMenuItem menToolStrip)
			{
				try
				{
					foreach (TreeNode tNode in tnc)
					{
						ToolStripMenuItem tMenItem = new ToolStripMenuItem();
						tMenItem.Text = tNode.Text;
						tMenItem.Tag = tNode;
							
						if (Tree.ConnectionTreeNode.GetNodeType(tNode) == Tree.TreeNodeType.Container)
						{
							tMenItem.Image = Resources.Folder;
							tMenItem.Tag = tNode.Tag;
								
							menToolStrip.DropDownItems.Add(tMenItem);
							AddNodeToMenu(tNode.Nodes, tMenItem);
						}
						else if (Tree.ConnectionTreeNode.GetNodeType(tNode) == Tree.TreeNodeType.Connection | Tree.ConnectionTreeNode.GetNodeType(tNode) == Tree.TreeNodeType.PuttySession)
						{
							tMenItem.Image = Windows.treeForm.imgListTree.Images[tNode.ImageIndex];
							tMenItem.Tag = tNode.Tag;
								
							menToolStrip.DropDownItems.Add(tMenItem);
						}
							
						tMenItem.MouseUp += ConMenItem_MouseUp;
					}
				}
				catch (Exception ex)
				{
					Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "AddNodeToMenu failed" + Environment.NewLine + ex.Message, true);
				}
			}
				
			private void nI_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
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
					
				if (mRemoteNG.Settings.Default.ShowSystemTrayIcon == false)
				{
					App.Runtime.NotificationAreaIcon.Dispose();
					App.Runtime.NotificationAreaIcon = null;
				}
			}
				
			private void HideForm()
			{
				frmMain.Default.Hide();
				frmMain.Default.PreviousWindowState = frmMain.Default.WindowState;
			}
				
			private void ConMenItem_MouseUp(System.Object sender, System.Windows.Forms.MouseEventArgs e)
			{
				if (e.Button == MouseButtons.Left)
				{
					if (((System.Windows.Forms.Control)sender).Tag is Connection.ConnectionInfo)
					{
						if (frmMain.Default.Visible == false)
						{
							ShowForm();
						}
                        App.Runtime.OpenConnection((mRemoteNG.Connection.ConnectionInfo)((System.Windows.Forms.Control)sender).Tag);
					}
				}
			}
				
			private void cMenExit_Click(System.Object sender, System.EventArgs e)
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
		
		public static SaveFileDialog ConnectionsExportDialog()
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.CheckPathExists = true;
			saveFileDialog.InitialDirectory = App.Info.ConnectionsFileInfo.DefaultConnectionsPath;
			saveFileDialog.FileName = App.Info.ConnectionsFileInfo.DefaultConnectionsFile;
			saveFileDialog.OverwritePrompt = true;
				
			saveFileDialog.Filter = Language.strFiltermRemoteXML + "|*.xml|" + Language.strFiltermRemoteCSV + "|*.csv|" + Language.strFiltervRD2008CSV + "|*.csv|" + Language.strFilterAll + "|*.*";
				
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
		
		public static OpenFileDialog ImportConnectionsRdpFileDialog()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.CheckFileExists = true;
			openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			openFileDialog.Filter = string.Join("|", new[] {Language.strFilterRDP, "*.rdp", Language.strFilterAll, "*.*"});
			openFileDialog.Multiselect = true;
			return openFileDialog;
		}
	}
}