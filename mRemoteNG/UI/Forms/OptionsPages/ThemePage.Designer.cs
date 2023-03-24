

using mRemoteNG.UI.Controls;

namespace mRemoteNG.UI.Forms.OptionsPages
{

    public sealed partial class ThemePage : OptionsPage
    {

        //UserControl overrides dispose to clean up the component list.
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

        //Required by the Windows Form Designer
        private System.ComponentModel.Container components = null;

        //NOTE: The following procedure is required by the Windows Form Designer
        //It can be modified using the Windows Form Designer.
        //Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            btnThemeDelete = new MrngButton();
            btnThemeNew = new MrngButton();
            cboTheme = new MrngComboBox();
            listPalette = new MrngListView();
            keyCol = new BrightIdeasSoftware.OLVColumn();
            ColorCol = new BrightIdeasSoftware.OLVColumn();
            ColorNameCol = new BrightIdeasSoftware.OLVColumn();
            labelRestart = new MrngLabel();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            tlpMain = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)listPalette).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tlpMain.SuspendLayout();
            SuspendLayout();
            // 
            // btnThemeDelete
            // 
            btnThemeDelete._mice = MrngButton.MouseState.OUT;
            btnThemeDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            btnThemeDelete.Location = new System.Drawing.Point(507, 3);
            btnThemeDelete.Name = "btnThemeDelete";
            btnThemeDelete.Size = new System.Drawing.Size(94, 23);
            btnThemeDelete.TabIndex = 2;
            btnThemeDelete.Text = "&Delete";
            btnThemeDelete.UseVisualStyleBackColor = true;
            btnThemeDelete.Click += btnThemeDelete_Click;
            // 
            // btnThemeNew
            // 
            btnThemeNew._mice = MrngButton.MouseState.OUT;
            btnThemeNew.Dock = System.Windows.Forms.DockStyle.Fill;
            btnThemeNew.Location = new System.Drawing.Point(407, 3);
            btnThemeNew.Name = "btnThemeNew";
            btnThemeNew.Size = new System.Drawing.Size(94, 23);
            btnThemeNew.TabIndex = 1;
            btnThemeNew.Text = "&New";
            btnThemeNew.UseVisualStyleBackColor = true;
            btnThemeNew.Click += btnThemeNew_Click;
            // 
            // cboTheme
            // 
            cboTheme._mice = MrngComboBox.MouseState.HOVER;
            cboTheme.Dock = System.Windows.Forms.DockStyle.Fill;
            cboTheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboTheme.FormattingEnabled = true;
            cboTheme.Location = new System.Drawing.Point(3, 3);
            cboTheme.Name = "cboTheme";
            cboTheme.Size = new System.Drawing.Size(398, 21);
            cboTheme.TabIndex = 0;
            cboTheme.SelectionChangeCommitted += cboTheme_SelectionChangeCommitted;
            // 
            // listPalette
            // 
            listPalette.AllColumns.Add(keyCol);
            listPalette.AllColumns.Add(ColorCol);
            listPalette.AllColumns.Add(ColorNameCol);
            listPalette.CellEditUseWholeCell = false;
            listPalette.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { keyCol, ColorCol, ColorNameCol });
            listPalette.DecorateLines = true;
            listPalette.Dock = System.Windows.Forms.DockStyle.Fill;
            listPalette.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            listPalette.Location = new System.Drawing.Point(3, 37);
            listPalette.Name = "listPalette";
            listPalette.ShowGroups = false;
            listPalette.Size = new System.Drawing.Size(604, 416);
            listPalette.TabIndex = 3;
            listPalette.UseCellFormatEvents = true;
            listPalette.UseCompatibleStateImageBehavior = false;
            listPalette.View = System.Windows.Forms.View.Details;
            // 
            // keyCol
            // 
            keyCol.AspectName = "Key";
            keyCol.IsEditable = false;
            keyCol.Text = "Element";
            keyCol.Width = 262;
            // 
            // ColorCol
            // 
            ColorCol.Sortable = false;
            ColorCol.Text = "Color";
            ColorCol.Width = 45;
            // 
            // ColorNameCol
            // 
            ColorNameCol.AspectName = "Value";
            ColorNameCol.Sortable = false;
            ColorNameCol.Text = "Color Name";
            ColorNameCol.Width = 265;
            // 
            // labelRestart
            // 
            labelRestart.AutoSize = true;
            labelRestart.Dock = System.Windows.Forms.DockStyle.Fill;
            labelRestart.Location = new System.Drawing.Point(3, 0);
            labelRestart.Name = "labelRestart";
            labelRestart.Size = new System.Drawing.Size(598, 28);
            labelRestart.TabIndex = 4;
            labelRestart.Text = "Warning: Restart is required...";
            labelRestart.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            tableLayoutPanel1.Controls.Add(cboTheme, 0, 0);
            tableLayoutPanel1.Controls.Add(btnThemeNew, 1, 0);
            tableLayoutPanel1.Controls.Add(btnThemeDelete, 2, 0);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.Size = new System.Drawing.Size(604, 28);
            tableLayoutPanel1.TabIndex = 6;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel2.Controls.Add(labelRestart, 0, 0);
            tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel2.Location = new System.Drawing.Point(3, 459);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new System.Drawing.Size(604, 28);
            tableLayoutPanel2.TabIndex = 7;
            // 
            // tlpMain
            // 
            tlpMain.ColumnCount = 1;
            tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tlpMain.Controls.Add(tableLayoutPanel2, 0, 2);
            tlpMain.Controls.Add(tableLayoutPanel1, 0, 0);
            tlpMain.Controls.Add(listPalette, 0, 1);
            tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            tlpMain.Location = new System.Drawing.Point(0, 0);
            tlpMain.Name = "tlpMain";
            tlpMain.RowCount = 3;
            tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            tlpMain.Size = new System.Drawing.Size(610, 490);
            tlpMain.TabIndex = 8;
            // 
            // ThemePage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            Controls.Add(tlpMain);
            Name = "ThemePage";
            Size = new System.Drawing.Size(610, 490);
            ((System.ComponentModel.ISupportInitialize)listPalette).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tlpMain.ResumeLayout(false);
            ResumeLayout(false);
        }

        internal MrngButton btnThemeDelete;
        internal MrngButton btnThemeNew;
        internal MrngComboBox cboTheme;
        private Controls.MrngListView listPalette;
        private Controls.MrngLabel labelRestart;
        private BrightIdeasSoftware.OLVColumn keyCol;
        private BrightIdeasSoftware.OLVColumn ColorCol;
        private BrightIdeasSoftware.OLVColumn ColorNameCol;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
    }
}
