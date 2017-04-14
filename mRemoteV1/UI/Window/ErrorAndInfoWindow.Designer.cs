
namespace mRemoteNG.UI.Window
{
	public partial class ErrorAndInfoWindow
	{
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
			this.lblMsgDate.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, System.Convert.ToByte(0));
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
			this.cMenMC.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, System.Convert.ToByte(0));
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
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, System.Convert.ToByte(0));
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
	}
}