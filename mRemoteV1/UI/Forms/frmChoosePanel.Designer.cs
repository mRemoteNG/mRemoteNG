

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
            this.cbPanels = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblDescription = new System.Windows.Forms.Label();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbPanels
            // 
            this.cbPanels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPanels.FormattingEnabled = true;
            this.cbPanels.Location = new System.Drawing.Point(79, 42);
            this.cbPanels.Name = "cbPanels";
            this.cbPanels.Size = new System.Drawing.Size(157, 21);
            this.cbPanels.TabIndex = 10;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(92, 73);
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
            this.btnNew.Image = global::mRemoteNG.Resources.Panel_Add;
            this.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNew.Location = new System.Drawing.Point(10, 38);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(60, 27);
            this.btnNew.TabIndex = 40;
            this.btnNew.Text = global::mRemoteNG.Language.strButtonNew;
            this.btnNew.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(167, 73);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(69, 23);
            this.btnCancel.TabIndex = 30;
            this.btnCancel.Text = global::mRemoteNG.Language.strButtonCancel;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmChoosePanel
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(245, 107);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnCancel);
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
		internal System.Windows.Forms.ComboBox cbPanels;
		internal System.Windows.Forms.Button btnOK;
		internal System.Windows.Forms.Label lblDescription;
		internal System.Windows.Forms.Button btnNew;
		internal System.Windows.Forms.Button btnCancel;
	}
}
