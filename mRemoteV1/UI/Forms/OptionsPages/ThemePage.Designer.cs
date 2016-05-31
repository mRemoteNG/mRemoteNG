

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
			this.btnThemeDelete = new System.Windows.Forms.Button();
			this.btnThemeDelete.Click += new System.EventHandler(this.btnThemeDelete_Click);
			this.btnThemeNew = new System.Windows.Forms.Button();
			this.btnThemeNew.Click += new System.EventHandler(this.btnThemeNew_Click);
			this.cboTheme = new System.Windows.Forms.ComboBox();
			this.cboTheme.DropDown += new System.EventHandler(this.cboTheme_DropDown);
			this.cboTheme.SelectionChangeCommitted += new System.EventHandler(this.cboTheme_SelectionChangeCommitted);
			this.ThemePropertyGrid = new System.Windows.Forms.PropertyGrid();
			this.SuspendLayout();
			//
			//btnThemeDelete
			//
			this.btnThemeDelete.Location = new System.Drawing.Point(535, 0);
			this.btnThemeDelete.Name = "btnThemeDelete";
			this.btnThemeDelete.Size = new System.Drawing.Size(75, 23);
			this.btnThemeDelete.TabIndex = 6;
			this.btnThemeDelete.Text = "&Delete";
			this.btnThemeDelete.UseVisualStyleBackColor = true;
			//
			//btnThemeNew
			//
			this.btnThemeNew.Location = new System.Drawing.Point(454, 0);
			this.btnThemeNew.Name = "btnThemeNew";
			this.btnThemeNew.Size = new System.Drawing.Size(75, 23);
			this.btnThemeNew.TabIndex = 5;
			this.btnThemeNew.Text = "&New";
			this.btnThemeNew.UseVisualStyleBackColor = true;
			//
			//cboTheme
			//
			this.cboTheme.FormattingEnabled = true;
			this.cboTheme.Location = new System.Drawing.Point(3, 1);
			this.cboTheme.Name = "cboTheme";
			this.cboTheme.Size = new System.Drawing.Size(445, 21);
			this.cboTheme.TabIndex = 4;
			//
			//ThemePropertyGrid
			//
			this.ThemePropertyGrid.Anchor = (System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.ThemePropertyGrid.Location = new System.Drawing.Point(3, 29);
			this.ThemePropertyGrid.Name = "ThemePropertyGrid";
			this.ThemePropertyGrid.Size = new System.Drawing.Size(607, 460);
			this.ThemePropertyGrid.TabIndex = 7;
			this.ThemePropertyGrid.UseCompatibleTextRendering = true;
			//
			//ThemePage
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF((float) (6.0F), (float) (13.0F));
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnThemeDelete);
			this.Controls.Add(this.btnThemeNew);
			this.Controls.Add(this.cboTheme);
			this.Controls.Add(this.ThemePropertyGrid);
			this.Name = "ThemePage";
			this.PageIcon = (System.Drawing.Icon) (resources.GetObject("$this.PageIcon"));
			this.Size = new System.Drawing.Size(610, 489);
			this.ResumeLayout(false);
				
		}
		internal System.Windows.Forms.Button btnThemeDelete;
		internal System.Windows.Forms.Button btnThemeNew;
		internal System.Windows.Forms.ComboBox cboTheme;
		internal System.Windows.Forms.PropertyGrid ThemePropertyGrid;
			
	}
}
