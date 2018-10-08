

namespace mRemoteNG.UI.Forms
{
	
    public partial class frmChoosePanel : System.Windows.Forms.Form
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
            this.cbPanels = new mRemoteNG.UI.Controls.Base.NGComboBox();
            this.btnOK = new mRemoteNG.UI.Controls.Base.NGButton();
            this.lblDescription = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.btnNew = new mRemoteNG.UI.Controls.Base.NGButton();
            this.SuspendLayout();
            // 
            // cbPanels
            // 
            this.cbPanels._mice = mRemoteNG.UI.Controls.Base.NGComboBox.MouseState.HOVER;
            this.cbPanels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPanels.FormattingEnabled = true;
            this.cbPanels.Location = new System.Drawing.Point(12, 42);
            this.cbPanels.Name = "cbPanels";
            this.cbPanels.Size = new System.Drawing.Size(224, 21);
            this.cbPanels.TabIndex = 10;
            // 
            // btnOK
            // 
            this.btnOK._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.btnOK.Location = new System.Drawing.Point(167, 72);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(69, 23);
            this.btnOK.TabIndex = 20;
            this.btnOK.Text = global::mRemoteNG.Language.strButtonOK;
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
            this.btnNew._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.btnNew.Image = global::mRemoteNG.Resources.Panel_Add;
            this.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNew.Location = new System.Drawing.Point(101, 70);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(60, 27);
            this.btnNew.TabIndex = 40;
            this.btnNew.Text = global::mRemoteNG.Language.strButtonNew;
            this.btnNew.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // frmChoosePanel
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(245, 107);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cbPanels);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = global::mRemoteNG.Resources.Panels_Icon;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmChoosePanel";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Panel";
            this.Load += new System.EventHandler(this.frmChoosePanel_Load);
            this.ResumeLayout(false);

		}
		internal Controls.Base.NGComboBox cbPanels;
		internal Controls.Base.NGButton btnOK;
		internal Controls.Base.NGLabel lblDescription;
		internal Controls.Base.NGButton btnNew;
	}
}
