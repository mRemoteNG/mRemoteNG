namespace mRemoteNG.UI.Forms
{
    partial class CredentialImportForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.olvCredentials = new BrightIdeasSoftware.ObjectListView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.colUsername = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.colDomain = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.colRepo = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.buttonAccept = new mRemoteNG.UI.Controls.Base.NGButton();
            ((System.ComponentModel.ISupportInitialize)(this.olvCredentials)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // olvCredentials
            // 
            this.olvCredentials.AllColumns.Add(this.colUsername);
            this.olvCredentials.AllColumns.Add(this.colDomain);
            this.olvCredentials.AllColumns.Add(this.colRepo);
            this.olvCredentials.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colUsername,
            this.colDomain,
            this.colRepo});
            this.tableLayoutPanel1.SetColumnSpan(this.olvCredentials, 2);
            this.olvCredentials.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvCredentials.Dock = System.Windows.Forms.DockStyle.Fill;
            this.olvCredentials.FullRowSelect = true;
            this.olvCredentials.Location = new System.Drawing.Point(3, 3);
            this.olvCredentials.Name = "olvCredentials";
            this.olvCredentials.ShowGroups = false;
            this.olvCredentials.Size = new System.Drawing.Size(794, 399);
            this.olvCredentials.TabIndex = 0;
            this.olvCredentials.UseCompatibleStateImageBehavior = false;
            this.olvCredentials.View = System.Windows.Forms.View.Details;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Controls.Add(this.olvCredentials, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonAccept, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 450);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // colUsername
            // 
            this.colUsername.Text = "Username";
            // 
            // colDomain
            // 
            this.colDomain.Text = "Domain";
            // 
            // colRepo
            // 
            this.colRepo.FillsFreeSpace = true;
            this.colRepo.Text = "Repository";
            // 
            // buttonAccept
            // 
            this.buttonAccept._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.buttonAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAccept.Location = new System.Drawing.Point(722, 424);
            this.buttonAccept.Name = "buttonAccept";
            this.buttonAccept.Size = new System.Drawing.Size(75, 23);
            this.buttonAccept.TabIndex = 1;
            this.buttonAccept.Text = "Accept";
            this.buttonAccept.UseVisualStyleBackColor = true;
            this.buttonAccept.Click += new System.EventHandler(this.buttonAccept_Click);
            // 
            // CredentialImportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "CredentialImportForm";
            this.Text = "CredentialImportForm";
            ((System.ComponentModel.ISupportInitialize)(this.olvCredentials)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView olvCredentials;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private BrightIdeasSoftware.OLVColumn colUsername;
        private BrightIdeasSoftware.OLVColumn colDomain;
        private BrightIdeasSoftware.OLVColumn colRepo;
        private Controls.Base.NGButton buttonAccept;
    }
}