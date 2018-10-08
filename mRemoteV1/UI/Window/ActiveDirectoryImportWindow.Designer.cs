

using mRemoteNG.Themes;

namespace mRemoteNG.UI.Window
{
	public partial class ActiveDirectoryImportWindow : BaseWindow
	{
        #region  Windows Form Designer generated code
		private void InitializeComponent()
		{
            this.btnImport = new mRemoteNG.UI.Controls.Base.NGButton();
            this.txtDomain = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.lblDomain = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.btnChangeDomain = new mRemoteNG.UI.Controls.Base.NGButton();
            this.ActiveDirectoryTree = new ADTree.ADtree();
            this.btnClose = new mRemoteNG.UI.Controls.Base.NGButton();
            this.chkSubOU = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.SuspendLayout();
            // 
            // btnImport
            // 
            this.btnImport._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnImport.Location = new System.Drawing.Point(12, 346);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.TabIndex = 4;
            this.btnImport.Text = "&Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // txtDomain
            // 
            this.txtDomain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDomain.Location = new System.Drawing.Point(12, 25);
            this.txtDomain.Name = "txtDomain";
            this.txtDomain.Size = new System.Drawing.Size(406, 22);
            this.txtDomain.TabIndex = 1;
            this.txtDomain.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDomain_KeyDown);
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
            this.btnChangeDomain._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.btnChangeDomain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChangeDomain.Location = new System.Drawing.Point(424, 25);
            this.btnChangeDomain.Name = "btnChangeDomain";
            this.btnChangeDomain.Size = new System.Drawing.Size(99, 23);
            this.btnChangeDomain.TabIndex = 2;
            this.btnChangeDomain.Text = "Change";
            this.btnChangeDomain.UseVisualStyleBackColor = true;
            this.btnChangeDomain.Click += new System.EventHandler(this.btnChangeDomain_Click);
            // 
            // ActiveDirectoryTree
            // 
            this.ActiveDirectoryTree.ADPath = null;
            this.ActiveDirectoryTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ActiveDirectoryTree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ActiveDirectoryTree.Domain = "DOMAIN";
            this.ActiveDirectoryTree.Location = new System.Drawing.Point(12, 52);
            this.ActiveDirectoryTree.Margin = new System.Windows.Forms.Padding(4);
            this.ActiveDirectoryTree.Name = "ActiveDirectoryTree";
            this.ActiveDirectoryTree.SelectedNode = null;
            this.ActiveDirectoryTree.Size = new System.Drawing.Size(510, 271);
            this.ActiveDirectoryTree.TabIndex = 3;
            this.ActiveDirectoryTree.ADPathChanged += new ADTree.ADtree.ADPathChangedEventHandler(this.ActiveDirectoryTree_ADPathChanged);
            // 
            // btnClose
            // 
            this.btnClose._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(447, 330);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 39);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // chkSubOU
            // 
            this.chkSubOU._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.HOVER;
            this.chkSubOU.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkSubOU.AutoSize = true;
            this.chkSubOU.Location = new System.Drawing.Point(12, 330);
            this.chkSubOU.Name = "chkSubOU";
            this.chkSubOU.Size = new System.Drawing.Size(108, 17);
            this.chkSubOU.TabIndex = 6;
            this.chkSubOU.Text = "Import Sub OUs";
            this.chkSubOU.UseVisualStyleBackColor = true;
            // 
            // ActiveDirectoryImportWindow
            // 
            this.AcceptButton = this.btnImport;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(534, 381);
            this.Controls.Add(this.chkSubOU);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.ActiveDirectoryTree);
            this.Controls.Add(this.lblDomain);
            this.Controls.Add(this.txtDomain);
            this.Controls.Add(this.btnChangeDomain);
            this.Controls.Add(this.btnImport);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::mRemoteNG.Resources.ActiveDirectory_Icon;
            this.Name = "ActiveDirectoryImportWindow";
            this.TabText = "Active Directory Import";
            this.Text = "Active Directory Import";
            this.Load += new System.EventHandler(this.ADImport_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private Controls.Base.NGButton btnImport;
		private Controls.Base.NGTextBox txtDomain;
		private Controls.Base.NGLabel lblDomain;
		private Controls.Base.NGButton btnChangeDomain;
		private ADTree.ADtree ActiveDirectoryTree; 
        #endregion

       private Controls.Base.NGButton btnClose;
       private Controls.Base.NGCheckBox chkSubOU;
    }
}
