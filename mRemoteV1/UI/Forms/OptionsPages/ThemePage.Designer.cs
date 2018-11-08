

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
            this.btnThemeDelete = new mRemoteNG.UI.Controls.Base.NGButton();
            this.btnThemeNew = new mRemoteNG.UI.Controls.Base.NGButton();
            this.cboTheme = new mRemoteNG.UI.Controls.Base.NGComboBox();
            this.themeEnableCombo = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.listPalette = new mRemoteNG.UI.Controls.Base.NGListView();
            this.keyCol = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ColorCol = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ColorNameCol = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.labelRestart = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.listPalette)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnThemeDelete
            // 
            this.btnThemeDelete._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.btnThemeDelete.Location = new System.Drawing.Point(535, 3);
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
            this.btnThemeNew.Location = new System.Drawing.Point(454, 3);
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
            this.cboTheme.Location = new System.Drawing.Point(3, 3);
            this.cboTheme.Name = "cboTheme";
            this.cboTheme.Size = new System.Drawing.Size(445, 21);
            this.cboTheme.TabIndex = 0;
            this.cboTheme.SelectionChangeCommitted += new System.EventHandler(this.cboTheme_SelectionChangeCommitted);
            // 
            // themeEnableCombo
            // 
            this.themeEnableCombo._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.HOVER;
            this.themeEnableCombo.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.themeEnableCombo, 2);
            this.themeEnableCombo.Location = new System.Drawing.Point(454, 469);
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
            this.tableLayoutPanel1.SetColumnSpan(this.listPalette, 3);
            this.listPalette.Cursor = System.Windows.Forms.Cursors.Default;
            this.listPalette.DecorateLines = true;
            this.listPalette.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listPalette.Location = new System.Drawing.Point(3, 32);
            this.listPalette.Name = "listPalette";
            this.listPalette.ShowGroups = false;
            this.listPalette.Size = new System.Drawing.Size(607, 431);
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
            this.labelRestart.Location = new System.Drawing.Point(3, 466);
            this.labelRestart.Name = "labelRestart";
            this.labelRestart.Size = new System.Drawing.Size(399, 13);
            this.labelRestart.TabIndex = 4;
            this.labelRestart.Text = "Warning: Restart is required to disable the themes or to completely apply a new o" +
    "ne";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.cboTheme, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.themeEnableCombo, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelRestart, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnThemeNew, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.listPalette, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnThemeDelete, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(610, 489);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // ThemePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ThemePage";
            this.Size = new System.Drawing.Size(610, 489);
            ((System.ComponentModel.ISupportInitialize)(this.listPalette)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
