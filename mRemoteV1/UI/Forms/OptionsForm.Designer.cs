

namespace mRemoteNG.UI.Forms
{
    public partial class OptionsForm : System.Windows.Forms.Form
	{
			
		//Form overrides dispose to clean up the component list.
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            mRemoteNG.Controls.Alignment alignment1 = new mRemoteNG.Controls.Alignment();
            this.OkButton = new System.Windows.Forms.Button();
            this.CancelButtonControl = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel2 = new System.Windows.Forms.Panel();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.PagePanel = new System.Windows.Forms.Panel();
            this.PageListView = new mRemoteNG.Controls.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // OkButton
            // 
            resources.ApplyResources(this.OkButton, "OkButton");
            this.OkButton.Name = "OkButton";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // CancelButtonControl
            // 
            this.CancelButtonControl.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.CancelButtonControl, "CancelButtonControl");
            this.CancelButtonControl.Name = "CancelButtonControl";
            this.CancelButtonControl.UseVisualStyleBackColor = true;
            this.CancelButtonControl.Click += new System.EventHandler(this.CancelButtonControl_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.OkButton);
            this.panel1.Controls.Add(this.CancelButtonControl);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // splitter1
            // 
            resources.ApplyResources(this.splitter1, "splitter1");
            this.splitter1.Name = "splitter1";
            this.splitter1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.PageListView);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // splitter2
            // 
            resources.ApplyResources(this.splitter2, "splitter2");
            this.splitter2.Name = "splitter2";
            this.splitter2.TabStop = false;
            // 
            // PagePanel
            // 
            resources.ApplyResources(this.PagePanel, "PagePanel");
            this.PagePanel.Name = "PagePanel";
            // 
            // PageListView
            // 
            this.PageListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            resources.ApplyResources(this.PageListView, "PageListView");
            this.PageListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.PageListView.InactiveHighlightBackColor = System.Drawing.SystemColors.Highlight;
            this.PageListView.InactiveHighlightBorderColor = System.Drawing.SystemColors.HotTrack;
            this.PageListView.InactiveHighlightForeColor = System.Drawing.SystemColors.HighlightText;
            alignment1.Horizontal = mRemoteNG.Controls.HorizontalAlignment.Left;
            alignment1.Vertical = mRemoteNG.Controls.VerticalAlignment.Middle;
            this.PageListView.LabelAlignment = alignment1;
            this.PageListView.MultiSelect = false;
            this.PageListView.Name = "PageListView";
            this.PageListView.OwnerDraw = true;
            this.PageListView.ShowFocusCues = false;
            this.PageListView.TileSize = new System.Drawing.Size(100, 30);
            this.PageListView.UseCompatibleStateImageBehavior = false;
            this.PageListView.View = System.Windows.Forms.View.Tile;
            this.PageListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.PageListView_ItemSelectionChanged);
            this.PageListView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PageListView_MouseUp);
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.OkButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelButtonControl;
            this.Controls.Add(this.PagePanel);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OptionsForm_FormClosing);
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		internal System.Windows.Forms.Button OkButton;
		internal System.Windows.Forms.Button CancelButtonControl;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Panel PagePanel;
        internal Controls.ListView PageListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}