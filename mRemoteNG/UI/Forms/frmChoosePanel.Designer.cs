using mRemoteNG.UI.Controls;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Forms
{
	
    public partial class FrmChoosePanel : System.Windows.Forms.Form
	{
		//Form overrides dispose to clean up the component list.
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
            this.cbPanels = new MrngComboBox();
            this.btnOK = new MrngButton();
            this.lblDescription = new mRemoteNG.UI.Controls.MrngLabel();
            this.btnNew = new MrngButton();
            this.SuspendLayout();
            // 
            // cbPanels
            // 
            this.cbPanels._mice = MrngComboBox.MouseState.HOVER;
            this.cbPanels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPanels.FormattingEnabled = true;
            this.cbPanels.Location = new System.Drawing.Point(12, 42);
            this.cbPanels.Name = "cbPanels";
            this.cbPanels.Size = new System.Drawing.Size(224, 21);
            this.cbPanels.TabIndex = 10;
            // 
            // btnOK
            // 
            this.btnOK._mice = MrngButton.MouseState.HOVER;
            this.btnOK.Location = new System.Drawing.Point(167, 72);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 24);
            this.btnOK.TabIndex = 20;
            this.btnOK.Text = Language._Ok;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(7, 8);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(229, 29);
            this.lblDescription.TabIndex = 0;
            this.lblDescription.Text = "Select a panel from the list below or click New to add a new one. Click OK to con" +
    "tinue.";
            // 
            // btnNew
            // 
            this.btnNew._mice = MrngButton.MouseState.HOVER;
            this.btnNew.Image = global::mRemoteNG.Properties.Resources.InsertPanel_16x;
            this.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNew.Location = new System.Drawing.Point(86, 72);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 24);
            this.btnNew.TabIndex = 40;
            this.btnNew.Text = Language._New;
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // FrmChoosePanel
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(245, 107);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cbPanels);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmChoosePanel";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Panel";
            this.Load += new System.EventHandler(this.frmChoosePanel_Load);
            this.ResumeLayout(false);

		}
		internal MrngComboBox cbPanels;
		internal MrngButton btnOK;
		internal Controls.MrngLabel lblDescription;
		internal MrngButton btnNew;
	}
}
