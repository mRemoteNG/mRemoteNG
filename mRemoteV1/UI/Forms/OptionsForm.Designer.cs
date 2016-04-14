

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
			mRemoteNG.Controls.Alignment Alignment2 = new mRemoteNG.Controls.Alignment();
			this.PagePanel = new System.Windows.Forms.Panel();
			base.Load += new System.EventHandler(OptionsForm_Load);
			base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(OptionsForm_FormClosing);
			this.OkButton = new System.Windows.Forms.Button();
			this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
			this.CancelButtonControl = new System.Windows.Forms.Button();
			this.CancelButtonControl.Click += new System.EventHandler(this.CancelButtonControl_Click);
			this.PageListView = new mRemoteNG.Controls.ListView();
			this.PageListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.PageListView_ItemSelectionChanged);
			this.PageListView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PageListView_MouseUp);
			this.SuspendLayout();
			//
			//PagePanel
			//
			resources.ApplyResources(this.PagePanel, "PagePanel");
			this.PagePanel.Name = "PagePanel";
			//
			//OkButton
			//
			resources.ApplyResources(this.OkButton, "OkButton");
			this.OkButton.Name = "OkButton";
			this.OkButton.UseVisualStyleBackColor = true;
			//
			//CancelButtonControl
			//
			this.CancelButtonControl.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(this.CancelButtonControl, "CancelButtonControl");
			this.CancelButtonControl.Name = "CancelButtonControl";
			this.CancelButtonControl.UseVisualStyleBackColor = true;
			//
			//PageListView
			//
			this.PageListView.InactiveHighlightBackColor = System.Drawing.SystemColors.Highlight;
			this.PageListView.InactiveHighlightBorderColor = System.Drawing.SystemColors.HotTrack;
			this.PageListView.InactiveHighlightForeColor = System.Drawing.SystemColors.HighlightText;
			Alignment2.Horizontal = mRemoteNG.Controls.HorizontalAlignment.Left;
			Alignment2.Vertical = mRemoteNG.Controls.VerticalAlignment.Middle;
			this.PageListView.LabelAlignment = Alignment2;
			resources.ApplyResources(this.PageListView, "PageListView");
			this.PageListView.MultiSelect = false;
			this.PageListView.Name = "PageListView";
			this.PageListView.OwnerDraw = true;
			this.PageListView.ShowFocusCues = false;
			this.PageListView.TileSize = new System.Drawing.Size(150, 30);
			this.PageListView.UseCompatibleStateImageBehavior = false;
			this.PageListView.View = System.Windows.Forms.View.Tile;
			//
			//OptionsForm
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
			this.ResumeLayout(false);
				
		}
		internal mRemoteNG.Controls.ListView PageListView;
		internal System.Windows.Forms.Panel PagePanel;
		internal System.Windows.Forms.Button OkButton;
		internal System.Windows.Forms.Button CancelButtonControl;
	}
}