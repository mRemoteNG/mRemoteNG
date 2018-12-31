namespace mRemoteNG.UI.Forms.CredentialManager
{
    partial class CredentialManagerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CredentialManagerForm));
            this.panelMain = new System.Windows.Forms.Panel();
            this.olvPageList = new mRemoteNG.UI.Controls.Base.NGListView();
            this.olvColumnPage = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.buttonClose = new mRemoteNG.UI.Controls.Base.NGButton();
            ((System.ComponentModel.ISupportInitialize)(this.olvPageList)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMain.Location = new System.Drawing.Point(152, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(555, 375);
            this.panelMain.TabIndex = 4;
            // 
            // olvPageList
            // 
            this.olvPageList.AllColumns.Add(this.olvColumnPage);
            this.olvPageList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.olvPageList.CellEditUseWholeCell = false;
            this.olvPageList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnPage});
            this.olvPageList.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvPageList.DecorateLines = true;
            this.olvPageList.FullRowSelect = true;
            this.olvPageList.HasCollapsibleGroups = false;
            this.olvPageList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.olvPageList.HideSelection = false;
            this.olvPageList.IsSearchOnSortColumn = false;
            this.olvPageList.Location = new System.Drawing.Point(0, 0);
            this.olvPageList.MultiSelect = false;
            this.olvPageList.Name = "olvPageList";
            this.olvPageList.RowHeight = 30;
            this.olvPageList.SelectAllOnControlA = false;
            this.olvPageList.ShowFilterMenuOnRightClick = false;
            this.olvPageList.ShowGroups = false;
            this.olvPageList.ShowHeaderInAllViews = false;
            this.olvPageList.Size = new System.Drawing.Size(154, 424);
            this.olvPageList.TabIndex = 5;
            this.olvPageList.UseCompatibleStateImageBehavior = false;
            this.olvPageList.View = System.Windows.Forms.View.Details;
            // 
            // olvColumnPage
            // 
            this.olvColumnPage.AspectName = "PageName";
            this.olvColumnPage.CellVerticalAlignment = System.Drawing.StringAlignment.Center;
            this.olvColumnPage.FillsFreeSpace = true;
            this.olvColumnPage.Groupable = false;
            this.olvColumnPage.Hideable = false;
            this.olvColumnPage.IsEditable = false;
            this.olvColumnPage.Searchable = false;
            this.olvColumnPage.Sortable = false;
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.buttonClose);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 374);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(707, 50);
            this.pnlBottom.TabIndex = 6;
            // 
            // buttonClose
            // 
            this.buttonClose._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Location = new System.Drawing.Point(620, 15);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // CredentialManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(707, 424);
            this.Controls.Add(this.olvPageList);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.pnlBottom);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CredentialManagerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Credential Manager";
            ((System.ComponentModel.ISupportInitialize)(this.olvPageList)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panelMain;
        private Controls.Base.NGListView olvPageList;
        private System.Windows.Forms.Panel pnlBottom;
        private Controls.Base.NGButton buttonClose;
        private BrightIdeasSoftware.OLVColumn olvColumnPage;
    }
}