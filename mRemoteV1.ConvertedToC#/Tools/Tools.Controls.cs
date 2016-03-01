using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using mRemoteNG.App.Runtime;

namespace mRemoteNG.Tools
{
	public class Controls
	{
		public class ComboBoxItem
		{
			private string _Text;
			public string Text {
				get { return this._Text; }
				set { this._Text = value; }
			}

			private object _Tag;
			public object Tag {
				get { return this._Tag; }
				set { this._Tag = value; }
			}

			public ComboBoxItem(string Text, object Tag = null)
			{
				this._Text = Text;
				if (Tag != null) {
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
			public bool Disposed {
				get { return _Disposed; }
				set { _Disposed = value; }
			}


			//Public Event MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
			//Public Event MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)


			public NotificationAreaIcon()
			{
				try {
					this._cMenCons = new ToolStripMenuItem();
					this._cMenCons.Text = mRemoteNG.My.Language.strConnections;
					this._cMenCons.Image = mRemoteNG.My.Resources.Root;

					this._cMenSep1 = new ToolStripSeparator();

					this._cMenExit = new ToolStripMenuItem();
					this._cMenExit.Text = mRemoteNG.My.Language.strMenuExit;
					this._cMenExit.Click += cMenExit_Click;

					this._cMen = new ContextMenuStrip();
					this._cMen.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
					this._cMen.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
					this._cMen.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
						this._cMenCons,
						this._cMenSep1,
						this._cMenExit
					});

					this._nI = new NotifyIcon();
					this._nI.Text = "mRemote";
					this._nI.BalloonTipText = "mRemote";
					this._nI.Icon = mRemoteNG.My.Resources.mRemote_Icon;
					this._nI.ContextMenuStrip = this._cMen;
					this._nI.Visible = true;

					this._nI.MouseClick += nI_MouseClick;
					this._nI.MouseDoubleClick += nI_MouseDoubleClick;
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "Creating new SysTrayIcon failed" + Constants.vbNewLine + ex.Message, true);
				}
			}

			public void Dispose()
			{
				try {
					this._nI.Visible = false;
					this._nI.Dispose();
					this._cMen.Dispose();
					this._Disposed = true;
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "Disposing SysTrayIcon failed" + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void nI_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
			{
				if (e.Button == MouseButtons.Right) {
					this._cMenCons.DropDownItems.Clear();

					foreach (TreeNode tNode in mRemoteNG.App.Runtime.Windows.treeForm.tvConnections.Nodes) {
						AddNodeToMenu(tNode.Nodes, this._cMenCons);
					}
				}
			}

			private void AddNodeToMenu(TreeNodeCollection tnc, ToolStripMenuItem menToolStrip)
			{
				try {
					foreach (TreeNode tNode in tnc) {
						ToolStripMenuItem tMenItem = new ToolStripMenuItem();
						tMenItem.Text = tNode.Text;
						tMenItem.Tag = tNode;

						if (mRemoteNG.Tree.Node.GetNodeType(tNode) == mRemoteNG.Tree.Node.Type.Container) {
							tMenItem.Image = mRemoteNG.My.Resources.Folder;
							tMenItem.Tag = tNode.Tag;

							menToolStrip.DropDownItems.Add(tMenItem);
							AddNodeToMenu(tNode.Nodes, tMenItem);
						} else if (mRemoteNG.Tree.Node.GetNodeType(tNode) == mRemoteNG.Tree.Node.Type.Connection | mRemoteNG.Tree.Node.GetNodeType(tNode) == mRemoteNG.Tree.Node.Type.PuttySession) {
							tMenItem.Image = Windows.treeForm.imgListTree.Images[tNode.ImageIndex];
							tMenItem.Tag = tNode.Tag;

							menToolStrip.DropDownItems.Add(tMenItem);
						}

						tMenItem.MouseUp += ConMenItem_MouseUp;
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "AddNodeToMenu failed" + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void nI_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
			{
				if (My.MyProject.Forms.frmMain.Visible == true) {
					HideForm();
				} else {
					ShowForm();
				}
			}

			private void ShowForm()
			{
				My.MyProject.Forms.frmMain.Show();
				My.MyProject.Forms.frmMain.WindowState = My.MyProject.Forms.frmMain.PreviousWindowState;

				if (mRemoteNG.My.Settings.ShowSystemTrayIcon == false) {
					mRemoteNG.App.Runtime.NotificationAreaIcon.Dispose();
					mRemoteNG.App.Runtime.NotificationAreaIcon = null;
				}
			}

			private void HideForm()
			{
				My.MyProject.Forms.frmMain.Hide();
				My.MyProject.Forms.frmMain.PreviousWindowState = My.MyProject.Forms.frmMain.WindowState;
			}

			private void ConMenItem_MouseUp(System.Object sender, System.Windows.Forms.MouseEventArgs e)
			{
				if (e.Button == MouseButtons.Left) {
					if (sender.Tag is Connection.Info) {
						if (My.MyProject.Forms.frmMain.Visible == false)
							ShowForm();
						mRemoteNG.App.Runtime.OpenConnection(sender.Tag);
					}
				}
			}

			private void cMenExit_Click(System.Object sender, System.EventArgs e)
			{
				mRemoteNG.App.Runtime.Shutdown.Quit();
			}
		}

		public static SaveFileDialog ConnectionsSaveAsDialog()
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.CheckPathExists = true;
			saveFileDialog.InitialDirectory = mRemoteNG.App.Info.Connections.DefaultConnectionsPath;
			saveFileDialog.FileName = mRemoteNG.App.Info.Connections.DefaultConnectionsFile;
			saveFileDialog.OverwritePrompt = true;

			saveFileDialog.Filter = mRemoteNG.My.Language.strFiltermRemoteXML + "|*.xml|" + mRemoteNG.My.Language.strFilterAll + "|*.*";

			return saveFileDialog;
		}

		public static SaveFileDialog ConnectionsExportDialog()
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.CheckPathExists = true;
			saveFileDialog.InitialDirectory = mRemoteNG.App.Info.Connections.DefaultConnectionsPath;
			saveFileDialog.FileName = mRemoteNG.App.Info.Connections.DefaultConnectionsFile;
			saveFileDialog.OverwritePrompt = true;

			saveFileDialog.Filter = mRemoteNG.My.Language.strFiltermRemoteXML + "|*.xml|" + mRemoteNG.My.Language.strFiltermRemoteCSV + "|*.csv|" + mRemoteNG.My.Language.strFiltervRD2008CSV + "|*.csv|" + mRemoteNG.My.Language.strFilterAll + "|*.*";

			return saveFileDialog;
		}

		public static OpenFileDialog ConnectionsLoadDialog()
		{
			OpenFileDialog lDlg = new OpenFileDialog();
			lDlg.CheckFileExists = true;
			lDlg.InitialDirectory = mRemoteNG.App.Info.Connections.DefaultConnectionsPath;
			lDlg.Filter = mRemoteNG.My.Language.strFiltermRemoteXML + "|*.xml|" + mRemoteNG.My.Language.strFilterAll + "|*.*";

			return lDlg;
		}

		public static OpenFileDialog ImportConnectionsRdpFileDialog()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.CheckFileExists = true;
			openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			openFileDialog.Filter = string.Join("|", {
				mRemoteNG.My.Language.strFilterRDP,
				"*.rdp",
				mRemoteNG.My.Language.strFilterAll,
				"*.*"
			});
			openFileDialog.Multiselect = true;
			return openFileDialog;
		}

		public class TreeNodeSorter : IComparer
		{

			public System.Windows.Forms.SortOrder Sorting { get; set; }

			public TreeNodeSorter(SortOrder sortOrder = SortOrder.None) : base()
			{
				Sorting = sortOrder;
			}

			public int Compare(object x, object y)
			{
				TreeNode tx = (TreeNode)x;
				TreeNode ty = (TreeNode)y;

				switch (Sorting) {
					case SortOrder.Ascending:
						return string.Compare(tx.Text, ty.Text);
					case SortOrder.Descending:
						return string.Compare(ty.Text, tx.Text);
					default:
						return 0;
				}
			}
		}
	}
}
