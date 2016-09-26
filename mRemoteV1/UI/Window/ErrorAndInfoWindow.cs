using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Text;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.App;
using mRemoteNG.UI.Forms;


namespace mRemoteNG.UI.Window
{
	public class ErrorAndInfoWindow : BaseWindow
	{
        #region Form Init
		internal System.Windows.Forms.PictureBox pbError;
		internal System.Windows.Forms.Label lblMsgDate;
		internal System.Windows.Forms.ListView lvErrorCollector;
		internal System.Windows.Forms.ColumnHeader clmMessage;
		internal System.Windows.Forms.TextBox txtMsgText;
		internal System.Windows.Forms.ImageList imgListMC;
		private System.ComponentModel.Container components = null;
		internal System.Windows.Forms.ContextMenuStrip cMenMC;
		internal System.Windows.Forms.ToolStripMenuItem cMenMCCopy;
		internal System.Windows.Forms.ToolStripMenuItem cMenMCDelete;
		internal System.Windows.Forms.Panel pnlErrorMsg;
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.Load += new System.EventHandler(ErrorsAndInfos_Load);
			this.Resize += new System.EventHandler(ErrorsAndInfos_Resize);
			this.pnlErrorMsg = new System.Windows.Forms.Panel();
			this.txtMsgText = new System.Windows.Forms.TextBox();
			this.txtMsgText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MC_KeyDown);
			this.lblMsgDate = new System.Windows.Forms.Label();
			this.pbError = new System.Windows.Forms.PictureBox();
			this.lvErrorCollector = new System.Windows.Forms.ListView();
			this.lvErrorCollector.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MC_KeyDown);
			this.lvErrorCollector.SelectedIndexChanged += new System.EventHandler(this.lvErrorCollector_SelectedIndexChanged);
			this.clmMessage = (System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader());
			this.cMenMC = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cMenMC.Opening += new System.ComponentModel.CancelEventHandler(this.cMenMC_Opening);
			this.cMenMCCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.cMenMCCopy.Click += new System.EventHandler(this.cMenMCCopy_Click);
			this.cMenMCDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.cMenMCDelete.Click += new System.EventHandler(this.cMenMCDelete_Click);
			this.imgListMC = new System.Windows.Forms.ImageList(this.components);
			this.pnlErrorMsg.SuspendLayout();
			((System.ComponentModel.ISupportInitialize) this.pbError).BeginInit();
			this.cMenMC.SuspendLayout();
			this.SuspendLayout();
			//
			//pnlErrorMsg
			//
			this.pnlErrorMsg.Anchor = (System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.pnlErrorMsg.BackColor = System.Drawing.SystemColors.Control;
			this.pnlErrorMsg.Controls.Add(this.txtMsgText);
			this.pnlErrorMsg.Controls.Add(this.lblMsgDate);
			this.pnlErrorMsg.Controls.Add(this.pbError);
			this.pnlErrorMsg.Location = new System.Drawing.Point(0, 1);
			this.pnlErrorMsg.Name = "pnlErrorMsg";
			this.pnlErrorMsg.Size = new System.Drawing.Size(198, 232);
			this.pnlErrorMsg.TabIndex = 20;
			//
			//txtMsgText
			//
			this.txtMsgText.Anchor = (System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.txtMsgText.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtMsgText.Location = new System.Drawing.Point(40, 20);
			this.txtMsgText.Multiline = true;
			this.txtMsgText.Name = "txtMsgText";
			this.txtMsgText.ReadOnly = true;
			this.txtMsgText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtMsgText.Size = new System.Drawing.Size(158, 211);
			this.txtMsgText.TabIndex = 30;
			//
			//lblMsgDate
			//
			this.lblMsgDate.Anchor = (System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.lblMsgDate.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.lblMsgDate.Location = new System.Drawing.Point(40, 5);
			this.lblMsgDate.Name = "lblMsgDate";
			this.lblMsgDate.Size = new System.Drawing.Size(155, 13);
			this.lblMsgDate.TabIndex = 40;
			//
			//pbError
			//
			this.pbError.InitialImage = null;
			this.pbError.Location = new System.Drawing.Point(2, 5);
			this.pbError.Name = "pbError";
			this.pbError.Size = new System.Drawing.Size(32, 32);
			this.pbError.TabIndex = 0;
			this.pbError.TabStop = false;
			//
			//lvErrorCollector
			//
			this.lvErrorCollector.Anchor = (System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.lvErrorCollector.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lvErrorCollector.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {this.clmMessage});
			this.lvErrorCollector.ContextMenuStrip = this.cMenMC;
			this.lvErrorCollector.FullRowSelect = true;
			this.lvErrorCollector.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lvErrorCollector.HideSelection = false;
			this.lvErrorCollector.Location = new System.Drawing.Point(203, 1);
			this.lvErrorCollector.Name = "lvErrorCollector";
			this.lvErrorCollector.ShowGroups = false;
			this.lvErrorCollector.Size = new System.Drawing.Size(413, 232);
			this.lvErrorCollector.SmallImageList = this.imgListMC;
			this.lvErrorCollector.TabIndex = 10;
			this.lvErrorCollector.UseCompatibleStateImageBehavior = false;
			this.lvErrorCollector.View = System.Windows.Forms.View.Details;
			//
			//clmMessage
			//
			this.clmMessage.Text = Language.strColumnMessage;
			this.clmMessage.Width = 184;
			//
			//cMenMC
			//
			this.cMenMC.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.cMenMC.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {this.cMenMCCopy, this.cMenMCDelete});
			this.cMenMC.Name = "cMenMC";
			this.cMenMC.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.cMenMC.Size = new System.Drawing.Size(153, 70);
			//
			//cMenMCCopy
			//
			this.cMenMCCopy.Image = Resources.Copy;
			this.cMenMCCopy.Name = "cMenMCCopy";
			this.cMenMCCopy.ShortcutKeys = (System.Windows.Forms.Keys) (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C);
			this.cMenMCCopy.Size = new System.Drawing.Size(152, 22);
			this.cMenMCCopy.Text = Language.strMenuCopy;
			//
			//cMenMCDelete
			//
			this.cMenMCDelete.Image = Resources.Delete;
			this.cMenMCDelete.Name = "cMenMCDelete";
			this.cMenMCDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.cMenMCDelete.Size = new System.Drawing.Size(152, 22);
			this.cMenMCDelete.Text = Language.strMenuDelete;
			//
			//imgListMC
			//
			this.imgListMC.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.imgListMC.ImageSize = new System.Drawing.Size(16, 16);
			this.imgListMC.TransparentColor = System.Drawing.Color.Transparent;
			//
			//ErrorsAndInfos
			//
			this.ClientSize = new System.Drawing.Size(617, 233);
			this.Controls.Add(this.lvErrorCollector);
			this.Controls.Add(this.pnlErrorMsg);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.HideOnClose = true;
			this.Icon = Resources.Info_Icon;
			this.Name = "ErrorsAndInfos";
			this.TabText = Language.strMenuNotifications;
			this.Text = "Notifications";
			this.pnlErrorMsg.ResumeLayout(false);
			this.pnlErrorMsg.PerformLayout();
			((System.ComponentModel.ISupportInitialize) this.pbError).EndInit();
			this.cMenMC.ResumeLayout(false);
			this.ResumeLayout(false);
					
		}
        #endregion
				
        #region Public Properties
		private DockContent _PreviousActiveForm;
        public DockContent PreviousActiveForm
		{
			get
			{
				return this._PreviousActiveForm;
			}
			set
			{
				this._PreviousActiveForm = value;
			}
		}
        #endregion
				
        #region Form Stuff
		private void ErrorsAndInfos_Load(object sender, System.EventArgs e)
		{
			ApplyLanguage();
		}
				
		private void ApplyLanguage()
		{
			clmMessage.Text = Language.strColumnMessage;
			cMenMCCopy.Text = Language.strMenuNotificationsCopyAll;
			cMenMCDelete.Text = Language.strMenuNotificationsDeleteAll;
			TabText = Language.strMenuNotifications;
			Text = Language.strMenuNotifications;
		}
        #endregion
				
        #region Public Methods
		public ErrorAndInfoWindow(DockContent Panel)
		{
			this.WindowType = WindowType.ErrorsAndInfos;
			this.DockPnl = Panel;
			this.InitializeComponent();
			this.LayoutVertical();
			this.FillImageList();
		}
        #endregion
				
        #region Private Methods
		private void FillImageList()
		{
			this.imgListMC.Images.Add(Resources.InformationSmall);
			this.imgListMC.Images.Add(Resources.WarningSmall);
			this.imgListMC.Images.Add(Resources.ErrorSmall);
		}
				
				
		private ControlLayout _Layout = ControlLayout.Vertical;
				
		private void LayoutVertical()
		{
			try
			{
				this.pnlErrorMsg.Location = new Point(0, this.Height - 200);
				this.pnlErrorMsg.Size = new Size(this.Width, this.Height - this.pnlErrorMsg.Top);
				this.pnlErrorMsg.Anchor = (System.Windows.Forms.AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
				this.txtMsgText.Size = new Size(this.pnlErrorMsg.Width - this.pbError.Width - 8, this.pnlErrorMsg.Height - 20);
				this.lvErrorCollector.Location = new Point(0, 0);
				this.lvErrorCollector.Size = new Size(this.Width, this.Height - this.pnlErrorMsg.Height - 5);
				this.lvErrorCollector.Anchor = (System.Windows.Forms.AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);
						
				this._Layout = ControlLayout.Vertical;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "LayoutVertical (UI.Window.ErrorsAndInfos) failed" + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void LayoutHorizontal()
		{
			try
			{
				this.pnlErrorMsg.Location = new Point(0, 0);
				this.pnlErrorMsg.Size = new Size(200, this.Height);
				this.pnlErrorMsg.Anchor = (System.Windows.Forms.AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Top);
				this.txtMsgText.Size = new Size(this.pnlErrorMsg.Width - this.pbError.Width - 8, this.pnlErrorMsg.Height - 20);
				this.lvErrorCollector.Location = new Point(this.pnlErrorMsg.Width + 5, 0);
				this.lvErrorCollector.Size = new Size(this.Width - this.pnlErrorMsg.Width - 5, this.Height);
				this.lvErrorCollector.Anchor = (System.Windows.Forms.AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);
						
				this._Layout = ControlLayout.Horizontal;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "LayoutHorizontal (UI.Window.ErrorsAndInfos) failed" + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void ErrorsAndInfos_Resize(object sender, System.EventArgs e)
		{
			try
			{
				if (this.Width > this.Height)
				{
					if (this._Layout == ControlLayout.Vertical)
					{
						this.LayoutHorizontal();
					}
				}
				else
				{
					if (this._Layout == ControlLayout.Horizontal)
					{
						this.LayoutVertical();
					}
				}
						
				this.lvErrorCollector.Columns[0].Width = this.lvErrorCollector.Width - 20;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "ErrorsAndInfos_Resize (UI.Window.ErrorsAndInfos) failed" + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void pnlErrorMsg_ResetDefaultStyle()
		{
			try
			{
				this.pnlErrorMsg.BackColor = Color.FromKnownColor(KnownColor.Control);
				this.pbError.Image = null;
				this.txtMsgText.Text = "";
				this.txtMsgText.BackColor = Color.FromKnownColor(KnownColor.Control);
				this.lblMsgDate.Text = "";
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "pnlErrorMsg_ResetDefaultStyle (UI.Window.ErrorsAndInfos) failed" + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void MC_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Escape)
				{
					try
					{
						if (this._PreviousActiveForm != null)
						{
							this._PreviousActiveForm.Show(frmMain.Default.pnlDock);
						}
						else
						{
							Windows.TreeForm.Show(frmMain.Default.pnlDock);
						}
					}
					catch (Exception)
					{
					    // ignored
					}
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "MC_KeyDown (UI.Window.ErrorsAndInfos) failed" + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void lvErrorCollector_SelectedIndexChanged(System.Object sender, System.EventArgs e)
		{
			try
			{
				if (this.lvErrorCollector.SelectedItems.Count == 0 | this.lvErrorCollector.SelectedItems.Count > 1)
				{
					this.pnlErrorMsg_ResetDefaultStyle();
					return;
				}
						
				ListViewItem sItem = this.lvErrorCollector.SelectedItems[0];
                Messages.Message eMsg = (Messages.Message)sItem.Tag;
				switch (eMsg.MsgClass)
				{
					case Messages.MessageClass.InformationMsg:
						this.pbError.Image = Resources.Information;
						this.pnlErrorMsg.BackColor = Color.LightSteelBlue;
						this.txtMsgText.BackColor = Color.LightSteelBlue;
						break;
					case Messages.MessageClass.WarningMsg:
						this.pbError.Image = Resources.Warning;
						this.pnlErrorMsg.BackColor = Color.Gold;
						this.txtMsgText.BackColor = Color.Gold;
						break;
					case Messages.MessageClass.ErrorMsg:
						this.pbError.Image = Resources._Error;
						this.pnlErrorMsg.BackColor = Color.IndianRed;
						this.txtMsgText.BackColor = Color.IndianRed;
						break;
				}
						
				this.lblMsgDate.Text = eMsg.MsgDate.ToString();
				this.txtMsgText.Text = eMsg.MsgText;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "lvErrorCollector_SelectedIndexChanged (UI.Window.ErrorsAndInfos) failed" + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void cMenMC_Opening(System.Object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (lvErrorCollector.Items.Count > 0)
			{
				cMenMCCopy.Enabled = true;
				cMenMCDelete.Enabled = true;
			}
			else
			{
				cMenMCCopy.Enabled = false;
				cMenMCDelete.Enabled = false;
			}
					
			if (lvErrorCollector.SelectedItems.Count > 0)
			{
				cMenMCCopy.Text = Language.strMenuCopy;
				cMenMCDelete.Text = Language.strMenuNotificationsDelete;
			}
			else
			{
				cMenMCCopy.Text = Language.strMenuNotificationsCopyAll;
				cMenMCDelete.Text = Language.strMenuNotificationsDeleteAll;
			}
		}
				
		private void cMenMCCopy_Click(System.Object sender, System.EventArgs e)
		{
			CopyMessagesToClipboard();
		}
				
		private void CopyMessagesToClipboard()
		{
			try
			{
				IEnumerable items = default(IEnumerable);
				if (lvErrorCollector.SelectedItems.Count > 0)
				{
					items = lvErrorCollector.SelectedItems;
				}
				else
				{
					items = lvErrorCollector.Items;
				}
						
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("----------");
						
				lvErrorCollector.BeginUpdate();
						
				Messages.Message message = default(Messages.Message);
				foreach (ListViewItem item in items)
				{
					message = item.Tag as Messages.Message;
					if (message == null)
					{
						continue;
					}
							
					stringBuilder.AppendLine(message.MsgClass.ToString());
					stringBuilder.AppendLine(message.MsgDate.ToString());
					stringBuilder.AppendLine(message.MsgText);
					stringBuilder.AppendLine("----------");
				}
						
				Clipboard.SetText(stringBuilder.ToString());
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "UI.Window.ErrorsAndInfos.CopyMessagesToClipboard() failed." + Environment.NewLine + ex.Message, true);
			}
			finally
			{
				lvErrorCollector.EndUpdate();
			}
		}
				
		private void cMenMCDelete_Click(System.Object sender, System.EventArgs e)
		{
			DeleteMessages();
		}
				
		private void DeleteMessages()
		{
			try
			{
				lvErrorCollector.BeginUpdate();
						
				if (lvErrorCollector.SelectedItems.Count > 0)
				{
					foreach (ListViewItem item in lvErrorCollector.SelectedItems)
					{
						item.Remove();
					}
				}
				else
				{
					lvErrorCollector.Items.Clear();
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "UI.Window.ErrorsAndInfos.DeleteMessages() failed" + Environment.NewLine + ex.Message, true);
			}
			finally
			{
				lvErrorCollector.EndUpdate();
			}
		}
        #endregion
				
		public enum ControlLayout
		{
			Vertical = 0,
			Horizontal = 1
		}
	}
}
