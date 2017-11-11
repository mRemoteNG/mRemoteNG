namespace mRemoteNG.UI.Forms
{
    partial class frmOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOptions));
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.lstOptionPages = new mRemoteNG.UI.Controls.Base.NGListView();
            this.PageName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.btnCancel = new mRemoteNG.UI.Controls.Base.NGButton();
            this.btnOK = new mRemoteNG.UI.Controls.Base.NGButton();
            this.pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lstOptionPages)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Controls.Add(this.btnOK);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 492);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(764, 35);
            this.pnlBottom.TabIndex = 0;
            // 
            // splitter1
            // 
            this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 491);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(764, 1);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // splitter2
            // 
            this.splitter2.Location = new System.Drawing.Point(151, 0);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(1, 491);
            this.splitter2.TabIndex = 3;
            this.splitter2.TabStop = false;
            // 
            // pnlMain
            // 
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(152, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(612, 491);
            this.pnlMain.TabIndex = 4;
            // 
            // lstOptionPages
            // 
            this.lstOptionPages.AllColumns.Add(this.PageName);
            this.lstOptionPages.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstOptionPages.CellEditUseWholeCell = false;
            this.lstOptionPages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.PageName});
            this.lstOptionPages.Cursor = System.Windows.Forms.Cursors.Default;
            this.lstOptionPages.DecorateLines = true;
            this.lstOptionPages.Dock = System.Windows.Forms.DockStyle.Left;
            this.lstOptionPages.FullRowSelect = true;
            this.lstOptionPages.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstOptionPages.HideSelection = false;
            this.lstOptionPages.LabelWrap = false;
            this.lstOptionPages.Location = new System.Drawing.Point(0, 0);
            this.lstOptionPages.MultiSelect = false;
            this.lstOptionPages.Name = "lstOptionPages";
            this.lstOptionPages.RowHeight = 25;
            this.lstOptionPages.ShowGroups = false;
            this.lstOptionPages.ShowImagesOnSubItems = true;
            this.lstOptionPages.Size = new System.Drawing.Size(151, 491);
            this.lstOptionPages.TabIndex = 2;
            this.lstOptionPages.TileSize = new System.Drawing.Size(168, 40);
            this.lstOptionPages.UseCompatibleStateImageBehavior = false;
            this.lstOptionPages.View = System.Windows.Forms.View.Details;
            // 
            // PageName
            // 
            this.PageName.AspectName = "PageName";
            this.PageName.FillsFreeSpace = true;
            this.PageName.Groupable = false;
            this.PageName.ImageAspectName = "IconImage";
            this.PageName.IsEditable = false;
            // 
            // btnCancel
            // 
            this.btnCancel._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(681, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(600, 6);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // frmOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 527);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.lstOptionPages);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.pnlBottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "mRemoteNG Options";
            this.Load += new System.EventHandler(this.frmOptions_Load);
            this.pnlBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lstOptionPages)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Splitter splitter1;
        private Controls.Base.NGListView lstOptionPages;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Panel pnlMain;
        private Controls.Base.NGButton btnOK;
        private Controls.Base.NGButton btnCancel;
        private BrightIdeasSoftware.OLVColumn PageName;
    }
}