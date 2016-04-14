

namespace mRemoteNG.Forms
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
            this.PagePanel = new System.Windows.Forms.Panel();
            this.OkButton = new System.Windows.Forms.Button();
            this.CancelButtonControl = new System.Windows.Forms.Button();
            this.PageListView = new mRemoteNG.Controls.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // PagePanel
            // 
            resources.ApplyResources(this.PagePanel, "PagePanel");
            this.PagePanel.Name = "PagePanel";
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
            // PageListView
            // 
            resources.ApplyResources(this.PageListView, "PageListView");
            this.PageListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
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
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.OkButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelButtonControl;
            this.Controls.Add(this.CancelButtonControl);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.PagePanel);
            this.Controls.Add(this.PageListView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OptionsForm_FormClosing);
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            this.ResumeLayout(false);

		}
		internal mRemoteNG.Controls.ListView PageListView;
		internal System.Windows.Forms.Panel PagePanel;
		internal System.Windows.Forms.Button OkButton;
		internal System.Windows.Forms.Button CancelButtonControl;
        private System.Windows.Forms.ColumnHeader columnHeader1;
	}
}