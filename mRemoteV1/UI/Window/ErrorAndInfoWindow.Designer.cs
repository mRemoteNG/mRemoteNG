
namespace mRemoteNG.UI.Window
{
	public partial class ErrorAndInfoWindow
	{
		internal System.Windows.Forms.PictureBox pbError;
		internal System.Windows.Forms.ListView lvErrorCollector;
		internal System.Windows.Forms.ColumnHeader clmMessage;
		internal Controls.Base.NGTextBox txtMsgText;
		internal System.Windows.Forms.ImageList imgListMC;
		internal System.Windows.Forms.ContextMenuStrip cMenMC;
		internal System.Windows.Forms.ToolStripMenuItem cMenMCCopy;
		internal System.Windows.Forms.ToolStripMenuItem cMenMCDelete;

        private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.txtMsgText = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.pbError = new System.Windows.Forms.PictureBox();
            this.lvErrorCollector = new System.Windows.Forms.ListView();
            this.clmMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cMenMC = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cMenMCCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenMCDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.imgListMC = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlErrorMsg = new System.Windows.Forms.Panel();
            this.lblMsgDate = new mRemoteNG.UI.Controls.Base.NGLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pbError)).BeginInit();
            this.cMenMC.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlErrorMsg.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtMsgText
            // 
            this.txtMsgText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMsgText.Location = new System.Drawing.Point(40, 19);
            this.txtMsgText.Multiline = true;
            this.txtMsgText.Name = "txtMsgText";
            this.txtMsgText.ReadOnly = true;
            this.txtMsgText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMsgText.Size = new System.Drawing.Size(154, 208);
            this.txtMsgText.TabIndex = 30;
            this.txtMsgText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MC_KeyDown);
            // 
            // pbError
            // 
            this.pbError.InitialImage = null;
            this.pbError.Location = new System.Drawing.Point(5, 5);
            this.pbError.Name = "pbError";
            this.pbError.Size = new System.Drawing.Size(32, 32);
            this.pbError.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbError.TabIndex = 0;
            this.pbError.TabStop = false;
            // 
            // lvErrorCollector
            // 
            this.lvErrorCollector.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvErrorCollector.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvErrorCollector.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmMessage});
            this.lvErrorCollector.ContextMenuStrip = this.cMenMC;
            this.lvErrorCollector.FullRowSelect = true;
            this.lvErrorCollector.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvErrorCollector.HideSelection = false;
            this.lvErrorCollector.Location = new System.Drawing.Point(200, 0);
            this.lvErrorCollector.Margin = new System.Windows.Forms.Padding(0);
            this.lvErrorCollector.Name = "lvErrorCollector";
            this.lvErrorCollector.ShowGroups = false;
            this.lvErrorCollector.Size = new System.Drawing.Size(417, 233);
            this.lvErrorCollector.SmallImageList = this.imgListMC;
            this.lvErrorCollector.TabIndex = 10;
            this.lvErrorCollector.UseCompatibleStateImageBehavior = false;
            this.lvErrorCollector.View = System.Windows.Forms.View.Details;
            this.lvErrorCollector.SelectedIndexChanged += new System.EventHandler(this.lvErrorCollector_SelectedIndexChanged);
            this.lvErrorCollector.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MC_KeyDown);
            // 
            // clmMessage
            // 
            this.clmMessage.Text = global::mRemoteNG.Language.strColumnMessage;
            this.clmMessage.Width = 184;
            // 
            // cMenMC
            // 
            this.cMenMC.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cMenMC.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cMenMCCopy,
            this.cMenMCDelete});
            this.cMenMC.Name = "cMenMC";
            this.cMenMC.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.cMenMC.Size = new System.Drawing.Size(141, 48);
            this.cMenMC.Opening += new System.ComponentModel.CancelEventHandler(this.cMenMC_Opening);
            // 
            // cMenMCCopy
            // 
            this.cMenMCCopy.Image = global::mRemoteNG.Resources.Copy;
            this.cMenMCCopy.Name = "cMenMCCopy";
            this.cMenMCCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.cMenMCCopy.Size = new System.Drawing.Size(140, 22);
            this.cMenMCCopy.Text = global::mRemoteNG.Language.strMenuCopy;
            this.cMenMCCopy.Click += new System.EventHandler(this.cMenMCCopy_Click);
            // 
            // cMenMCDelete
            // 
            this.cMenMCDelete.Image = global::mRemoteNG.Resources.Delete;
            this.cMenMCDelete.Name = "cMenMCDelete";
            this.cMenMCDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.cMenMCDelete.Size = new System.Drawing.Size(140, 22);
            this.cMenMCDelete.Text = global::mRemoteNG.Language.strMenuDelete;
            this.cMenMCDelete.Click += new System.EventHandler(this.cMenMCDelete_Click);
            // 
            // imgListMC
            // 
            this.imgListMC.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imgListMC.ImageSize = new System.Drawing.Size(16, 16);
            this.imgListMC.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.pnlErrorMsg, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lvErrorCollector, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(617, 233);
            this.tableLayoutPanel1.TabIndex = 21;
            // 
            // pnlErrorMsg
            // 
            this.pnlErrorMsg.BackColor = System.Drawing.SystemColors.Control;
            this.pnlErrorMsg.Controls.Add(this.txtMsgText);
            this.pnlErrorMsg.Controls.Add(this.lblMsgDate);
            this.pnlErrorMsg.Controls.Add(this.pbError);
            this.pnlErrorMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlErrorMsg.Location = new System.Drawing.Point(0, 0);
            this.pnlErrorMsg.Margin = new System.Windows.Forms.Padding(0);
            this.pnlErrorMsg.Name = "pnlErrorMsg";
            this.pnlErrorMsg.Size = new System.Drawing.Size(200, 233);
            this.pnlErrorMsg.TabIndex = 20;
            // 
            // lblMsgDate
            // 
            this.lblMsgDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMsgDate.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsgDate.Location = new System.Drawing.Point(37, 3);
            this.lblMsgDate.Name = "lblMsgDate";
            this.lblMsgDate.Size = new System.Drawing.Size(157, 13);
            this.lblMsgDate.TabIndex = 40;
            // 
            // ErrorAndInfoWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(617, 233);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Icon = global::mRemoteNG.Resources.Info_Icon;
            this.Name = "ErrorAndInfoWindow";
            this.TabText = global::mRemoteNG.Language.strMenuNotifications;
            this.Text = "Notifications";
            this.Load += new System.EventHandler(this.ErrorsAndInfos_Load);
            this.Resize += new System.EventHandler(this.ErrorsAndInfos_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pbError)).EndInit();
            this.cMenMC.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlErrorMsg.ResumeLayout(false);
            this.pnlErrorMsg.PerformLayout();
            this.ResumeLayout(false);

		}

        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        internal System.Windows.Forms.Panel pnlErrorMsg;
        internal Controls.Base.NGLabel lblMsgDate;
    }
}