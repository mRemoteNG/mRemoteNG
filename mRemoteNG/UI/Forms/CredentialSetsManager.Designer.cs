using mRemoteNG.UI.Controls;

namespace mRemoteNG.UI.Forms
{
    public partial class CredentialSetsManager : System.Windows.Forms.Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components != null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this.listViewCredentials = new BrightIdeasSoftware.ObjectListView();
            this.olvColumnTitle = new BrightIdeasSoftware.OLVColumn();
            this.olvColumnUsername = new BrightIdeasSoftware.OLVColumn();
            this.olvColumnDomain = new BrightIdeasSoftware.OLVColumn();
            this.btnAdd = new MrngButton();
            this.btnEdit = new MrngButton();
            this.btnDelete = new MrngButton();
            this.btnClose = new MrngButton();
            this.grpCredentialDetails = new System.Windows.Forms.GroupBox();
            this.lblTitle = new MrngLabel();
            this.lblUsername = new MrngLabel();
            this.lblPassword = new MrngLabel();
            this.lblDomain = new MrngLabel();
            this.txtTitle = new MrngTextBox();
            this.txtUsername = new MrngTextBox();
            this.txtPassword = new MrngTextBox();
            this.txtDomain = new MrngTextBox();
            this.btnSave = new MrngButton();
            this.btnCancelEdit = new MrngButton();
            ((System.ComponentModel.ISupportInitialize)(this.listViewCredentials)).BeginInit();
            this.grpCredentialDetails.SuspendLayout();
            this.SuspendLayout();
            //
            // listViewCredentials
            //
            this.listViewCredentials.AllColumns.Add(this.olvColumnTitle);
            this.listViewCredentials.AllColumns.Add(this.olvColumnUsername);
            this.listViewCredentials.AllColumns.Add(this.olvColumnDomain);
            this.listViewCredentials.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewCredentials.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnTitle,
            this.olvColumnUsername,
            this.olvColumnDomain});
            this.listViewCredentials.FullRowSelect = true;
            this.listViewCredentials.GridLines = true;
            this.listViewCredentials.HideSelection = false;
            this.listViewCredentials.Location = new System.Drawing.Point(12, 12);
            this.listViewCredentials.MultiSelect = false;
            this.listViewCredentials.Name = "listViewCredentials";
            this.listViewCredentials.ShowGroups = false;
            this.listViewCredentials.Size = new System.Drawing.Size(460, 200);
            this.listViewCredentials.TabIndex = 0;
            this.listViewCredentials.UseCompatibleStateImageBehavior = false;
            this.listViewCredentials.View = System.Windows.Forms.View.Details;
            this.listViewCredentials.SelectedIndexChanged += new System.EventHandler(this.listViewCredentials_SelectedIndexChanged);
            this.listViewCredentials.DoubleClick += new System.EventHandler(this.listViewCredentials_DoubleClick);
            //
            // olvColumnTitle
            //
            this.olvColumnTitle.AspectName = "Title";
            this.olvColumnTitle.Text = "Title";
            this.olvColumnTitle.Width = 150;
            //
            // olvColumnUsername
            //
            this.olvColumnUsername.AspectName = "Username";
            this.olvColumnUsername.Text = "Username";
            this.olvColumnUsername.Width = 150;
            //
            // olvColumnDomain
            //
            this.olvColumnDomain.AspectName = "Domain";
            this.olvColumnDomain.Text = "Domain";
            this.olvColumnDomain.Width = 150;
            //
            // btnAdd
            //
            this.btnAdd._mice = MrngButton.MouseState.HOVER;
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Location = new System.Drawing.Point(12, 218);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            //
            // btnEdit
            //
            this.btnEdit._mice = MrngButton.MouseState.HOVER;
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEdit.Location = new System.Drawing.Point(93, 218);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            //
            // btnDelete
            //
            this.btnDelete._mice = MrngButton.MouseState.HOVER;
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(174, 218);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            //
            // btnClose
            //
            this.btnClose._mice = MrngButton.MouseState.HOVER;
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(397, 218);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            //
            // grpCredentialDetails
            //
            this.grpCredentialDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpCredentialDetails.Controls.Add(this.lblTitle);
            this.grpCredentialDetails.Controls.Add(this.txtTitle);
            this.grpCredentialDetails.Controls.Add(this.lblUsername);
            this.grpCredentialDetails.Controls.Add(this.txtUsername);
            this.grpCredentialDetails.Controls.Add(this.lblPassword);
            this.grpCredentialDetails.Controls.Add(this.txtPassword);
            this.grpCredentialDetails.Controls.Add(this.lblDomain);
            this.grpCredentialDetails.Controls.Add(this.txtDomain);
            this.grpCredentialDetails.Controls.Add(this.btnSave);
            this.grpCredentialDetails.Controls.Add(this.btnCancelEdit);
            this.grpCredentialDetails.Enabled = false;
            this.grpCredentialDetails.Location = new System.Drawing.Point(12, 250);
            this.grpCredentialDetails.Name = "grpCredentialDetails";
            this.grpCredentialDetails.Size = new System.Drawing.Size(460, 160);
            this.grpCredentialDetails.TabIndex = 5;
            this.grpCredentialDetails.TabStop = false;
            this.grpCredentialDetails.Text = "Credential Details";
            //
            // lblTitle
            //
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(10, 25);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(33, 13);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Title:";
            //
            // txtTitle
            //
            this.txtTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTitle.Location = new System.Drawing.Point(100, 22);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(345, 22);
            this.txtTitle.TabIndex = 1;
            //
            // lblUsername
            //
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(10, 53);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(61, 13);
            this.lblUsername.TabIndex = 2;
            this.lblUsername.Text = "Username:";
            //
            // txtUsername
            //
            this.txtUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUsername.Location = new System.Drawing.Point(100, 50);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(345, 22);
            this.txtUsername.TabIndex = 3;
            //
            // lblPassword
            //
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(10, 81);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(59, 13);
            this.lblPassword.TabIndex = 4;
            this.lblPassword.Text = "Password:";
            //
            // txtPassword
            //
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.Location = new System.Drawing.Point(100, 78);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(345, 22);
            this.txtPassword.TabIndex = 5;
            this.txtPassword.UseSystemPasswordChar = true;
            //
            // lblDomain
            //
            this.lblDomain.AutoSize = true;
            this.lblDomain.Location = new System.Drawing.Point(10, 109);
            this.lblDomain.Name = "lblDomain";
            this.lblDomain.Size = new System.Drawing.Size(50, 13);
            this.lblDomain.TabIndex = 6;
            this.lblDomain.Text = "Domain:";
            //
            // txtDomain
            //
            this.txtDomain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDomain.Location = new System.Drawing.Point(100, 106);
            this.txtDomain.Name = "txtDomain";
            this.txtDomain.Size = new System.Drawing.Size(345, 22);
            this.txtDomain.TabIndex = 7;
            //
            // btnSave
            //
            this.btnSave._mice = MrngButton.MouseState.HOVER;
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(289, 131);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //
            // btnCancelEdit
            //
            this.btnCancelEdit._mice = MrngButton.MouseState.HOVER;
            this.btnCancelEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelEdit.Location = new System.Drawing.Point(370, 131);
            this.btnCancelEdit.Name = "btnCancelEdit";
            this.btnCancelEdit.Size = new System.Drawing.Size(75, 23);
            this.btnCancelEdit.TabIndex = 9;
            this.btnCancelEdit.Text = "Cancel";
            this.btnCancelEdit.UseVisualStyleBackColor = true;
            this.btnCancelEdit.Click += new System.EventHandler(this.btnCancelEdit_Click);
            //
            // CredentialSetsManager
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(484, 421);
            this.Controls.Add(this.listViewCredentials);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.grpCredentialDetails);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CredentialSetsManager";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Credential Sets Manager";
            this.Load += new System.EventHandler(this.CredentialSetsManager_Load);
            ((System.ComponentModel.ISupportInitialize)(this.listViewCredentials)).EndInit();
            this.grpCredentialDetails.ResumeLayout(false);
            this.grpCredentialDetails.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private BrightIdeasSoftware.ObjectListView listViewCredentials;
        private BrightIdeasSoftware.OLVColumn olvColumnTitle;
        private BrightIdeasSoftware.OLVColumn olvColumnUsername;
        private BrightIdeasSoftware.OLVColumn olvColumnDomain;
        private MrngButton btnAdd;
        private MrngButton btnEdit;
        private MrngButton btnDelete;
        private MrngButton btnClose;
        private System.Windows.Forms.GroupBox grpCredentialDetails;
        private MrngLabel lblTitle;
        private MrngLabel lblUsername;
        private MrngLabel lblPassword;
        private MrngLabel lblDomain;
        private MrngTextBox txtTitle;
        private MrngTextBox txtUsername;
        private MrngTextBox txtPassword;
        private MrngTextBox txtDomain;
        private MrngButton btnSave;
        private MrngButton btnCancelEdit;
    }
}
