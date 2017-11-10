

namespace mRemoteNG.UI.Forms.OptionsPages
{
	
    public partial class ThemePage : OptionsPage
	{
			
		//UserControl overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]protected override void Dispose(bool disposing)
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
			
		//Required by the Windows Form Designer
		private System.ComponentModel.Container components = null;
			
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ThemePage));
            this.btnThemeDelete = new mRemoteNG.UI.Controls.Base.NGButton();
            this.btnThemeNew = new mRemoteNG.UI.Controls.Base.NGButton();
            this.cboTheme = new mRemoteNG.UI.Controls.Base.NGComboBox();
            this.themeEnableCombo = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.listPalette = new mRemoteNG.UI.Controls.Base.NGListView();
            this.keyCol = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ColorCol = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ColorNameCol = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.labelRestart = new mRemoteNG.UI.Controls.Base.NGLabel();
            ((System.ComponentModel.ISupportInitialize)(this.listPalette)).BeginInit();
            this.SuspendLayout();
            // 
            // btnThemeDelete
            // 
            this.btnThemeDelete._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.btnThemeDelete.Location = new System.Drawing.Point(535, 1);
            this.btnThemeDelete.Name = "btnThemeDelete";
            this.btnThemeDelete.Size = new System.Drawing.Size(75, 23);
            this.btnThemeDelete.TabIndex = 2;
            this.btnThemeDelete.Text = "&Delete";
            this.btnThemeDelete.UseVisualStyleBackColor = true;
            this.btnThemeDelete.Click += new System.EventHandler(this.btnThemeDelete_Click);
            // 
            // btnThemeNew
            // 
            this.btnThemeNew._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.btnThemeNew.Location = new System.Drawing.Point(454, 1);
            this.btnThemeNew.Name = "btnThemeNew";
            this.btnThemeNew.Size = new System.Drawing.Size(75, 23);
            this.btnThemeNew.TabIndex = 1;
            this.btnThemeNew.Text = "&New";
            this.btnThemeNew.UseVisualStyleBackColor = true;
            this.btnThemeNew.Click += new System.EventHandler(this.btnThemeNew_Click);
            // 
            // cboTheme
            // 
            this.cboTheme._mice = mRemoteNG.UI.Controls.Base.NGComboBox.MouseState.HOVER;
            this.cboTheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTheme.FormattingEnabled = true;
            this.cboTheme.Location = new System.Drawing.Point(3, 2);
            this.cboTheme.Name = "cboTheme";
            this.cboTheme.Size = new System.Drawing.Size(445, 21);
            this.cboTheme.TabIndex = 0;
            this.cboTheme.SelectionChangeCommitted += new System.EventHandler(this.cboTheme_SelectionChangeCommitted);
            // 
            // themeEnableCombo
            // 
            this.themeEnableCombo._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.HOVER;
            this.themeEnableCombo.AutoSize = true;
            this.themeEnableCombo.Location = new System.Drawing.Point(487, 457);
            this.themeEnableCombo.Name = "themeEnableCombo";
            this.themeEnableCombo.Size = new System.Drawing.Size(100, 17);
            this.themeEnableCombo.TabIndex = 5;
            this.themeEnableCombo.Text = "Enable Themes";
            this.themeEnableCombo.UseVisualStyleBackColor = true;
            this.themeEnableCombo.CheckedChanged += new System.EventHandler(this.themeEnableCombo_CheckedChanged);
            // 
            // listPalette
            // 
            this.listPalette.AllColumns.Add(this.keyCol);
            this.listPalette.AllColumns.Add(this.ColorCol);
            this.listPalette.AllColumns.Add(this.ColorNameCol);
            this.listPalette.CellEditUseWholeCell = false;
            this.listPalette.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.keyCol,
            this.ColorCol,
            this.ColorNameCol});
            this.listPalette.Cursor = System.Windows.Forms.Cursors.Default;
            this.listPalette.DecorateLines = true;
            this.listPalette.Location = new System.Drawing.Point(3, 29);
            this.listPalette.Name = "listPalette";
            this.listPalette.ShowGroups = false;
            this.listPalette.Size = new System.Drawing.Size(604, 413);
            this.listPalette.TabIndex = 3;
            this.listPalette.UseCellFormatEvents = true;
            this.listPalette.UseCompatibleStateImageBehavior = false;
            this.listPalette.View = System.Windows.Forms.View.Details;
            // 
            // keyCol
            // 
            this.keyCol.AspectName = "Key";
            this.keyCol.IsEditable = false;
            this.keyCol.Text = "Element";
            this.keyCol.Width = 262;
            // 
            // ColorCol
            // 
            this.ColorCol.Sortable = false;
            this.ColorCol.Text = "Color";
            this.ColorCol.Width = 45;
            // 
            // ColorNameCol
            // 
            this.ColorNameCol.AspectName = "Value";
            this.ColorNameCol.Sortable = false;
            this.ColorNameCol.Text = "Color Name";
            this.ColorNameCol.Width = 265;
            // 
            // labelRestart
            // 
            this.labelRestart.AutoSize = true;
            this.labelRestart.Location = new System.Drawing.Point(23, 457);
            this.labelRestart.Name = "labelRestart";
            this.labelRestart.Size = new System.Drawing.Size(399, 13);
            this.labelRestart.TabIndex = 4;
            this.labelRestart.Text = "Warning: Restart is required to disable the themes or to completely apply a new o" +
    "ne";
            // 
            // ThemePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelRestart);
            this.Controls.Add(this.listPalette);
            this.Controls.Add(this.themeEnableCombo);
            this.Controls.Add(this.btnThemeDelete);
            this.Controls.Add(this.btnThemeNew);
            this.Controls.Add(this.cboTheme);
            this.Name = "ThemePage";
            this.PageIcon = ((System.Drawing.Icon)(resources.GetObject("$this.PageIcon")));
            this.Size = new System.Drawing.Size(610, 489);
            ((System.ComponentModel.ISupportInitialize)(this.listPalette)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		internal Controls.Base.NGButton btnThemeDelete;
		internal Controls.Base.NGButton btnThemeNew;
		internal Controls.Base.NGComboBox cboTheme;
        private Controls.Base.NGCheckBox themeEnableCombo;
        private Controls.Base.NGListView listPalette;
        private Controls.Base.NGLabel labelRestart;
        private BrightIdeasSoftware.OLVColumn keyCol;
        private BrightIdeasSoftware.OLVColumn ColorCol;
        private BrightIdeasSoftware.OLVColumn ColorNameCol;
    }
}
