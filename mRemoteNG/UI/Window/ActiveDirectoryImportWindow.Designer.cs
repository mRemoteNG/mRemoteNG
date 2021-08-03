
using mRemoteNG.UI.Controls;

namespace mRemoteNG.UI.Window
{
	public partial class ActiveDirectoryImportWindow
	{
        #region  Windows Form Designer generated code
		private void InitializeComponent()
		{
            this.btnImport = new MrngButton();
            this.txtDomain = new mRemoteNG.UI.Controls.MrngTextBox();
            this.lblDomain = new mRemoteNG.UI.Controls.MrngLabel();
            this.btnChangeDomain = new MrngButton();
            this.activeDirectoryTree = new mRemoteNG.UI.Controls.MrngAdTree();
            this.btnClose = new MrngButton();
            this.chkSubOU = new MrngCheckBox();
            this.SuspendLayout();
            // 
            // btnImport
            // 
            this.btnImport._mice = MrngButton.MouseState.HOVER;
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnImport.Location = new System.Drawing.Point(126, 345);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.TabIndex = 4;
            this.btnImport.Text = "&Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // txtDomain
            // 
            this.txtDomain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDomain.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDomain.Location = new System.Drawing.Point(12, 25);
            this.txtDomain.Name = "txtDomain";
            this.txtDomain.Size = new System.Drawing.Size(406, 22);
            this.txtDomain.TabIndex = 1;
            this.txtDomain.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtDomain_KeyDown);
            // 
            // lblDomain
            // 
            this.lblDomain.AutoSize = true;
            this.lblDomain.Location = new System.Drawing.Point(9, 9);
            this.lblDomain.Name = "lblDomain";
            this.lblDomain.Size = new System.Drawing.Size(50, 13);
            this.lblDomain.TabIndex = 0;
            this.lblDomain.Text = "Domain:";
            // 
            // btnChangeDomain
            // 
            this.btnChangeDomain._mice = MrngButton.MouseState.HOVER;
            this.btnChangeDomain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChangeDomain.Location = new System.Drawing.Point(422, 23);
            this.btnChangeDomain.Name = "btnChangeDomain";
            this.btnChangeDomain.Size = new System.Drawing.Size(100, 24);
            this.btnChangeDomain.TabIndex = 2;
            this.btnChangeDomain.Text = "Change";
            this.btnChangeDomain.UseVisualStyleBackColor = true;
            this.btnChangeDomain.Click += new System.EventHandler(this.BtnChangeDomain_Click);
            // 
            // ActiveDirectoryTree
            // 
            this.activeDirectoryTree.AdPath = null;
            this.activeDirectoryTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.activeDirectoryTree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.activeDirectoryTree.Location = new System.Drawing.Point(12, 52);
            this.activeDirectoryTree.Margin = new System.Windows.Forms.Padding(4);
            this.activeDirectoryTree.Name = "ActiveDirectoryTree";
            this.activeDirectoryTree.SelectedNode = null;
            this.activeDirectoryTree.Size = new System.Drawing.Size(510, 285);
            this.activeDirectoryTree.TabIndex = 3;
            this.activeDirectoryTree.AdPathChanged += new mRemoteNG.UI.Controls.MrngAdTree.AdPathChangedEventHandler(this.ActiveDirectoryTree_ADPathChanged);
            // 
            // btnClose
            // 
            this.btnClose._mice = MrngButton.MouseState.HOVER;
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(422, 344);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 24);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // chkSubOU
            // 
            this.chkSubOU._mice = MrngCheckBox.MouseState.HOVER;
            this.chkSubOU.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkSubOU.AutoSize = true;
            this.chkSubOU.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSubOU.Location = new System.Drawing.Point(12, 349);
            this.chkSubOU.Name = "chkSubOU";
            this.chkSubOU.Size = new System.Drawing.Size(108, 17);
            this.chkSubOU.TabIndex = 6;
            this.chkSubOU.Text = "Import Sub OUs";
            this.chkSubOU.UseVisualStyleBackColor = true;
            // 
            // ActiveDirectoryImportWindow
            // 
            this.AcceptButton = this.btnImport;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(534, 381);
            this.Controls.Add(this.chkSubOU);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.activeDirectoryTree);
            this.Controls.Add(this.lblDomain);
            this.Controls.Add(this.txtDomain);
            this.Controls.Add(this.btnChangeDomain);
            this.Controls.Add(this.btnImport);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ActiveDirectoryImportWindow";
            this.TabText = "Active Directory Import";
            this.Text = "Active Directory Import";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private MrngButton btnImport;
		private Controls.MrngTextBox txtDomain;
		private Controls.MrngLabel lblDomain;
		private MrngButton btnChangeDomain;
		private mRemoteNG.UI.Controls.MrngAdTree activeDirectoryTree; 
        #endregion

       private MrngButton btnClose;
       private MrngCheckBox chkSubOU;
    }
}
