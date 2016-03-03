// VBConversions Note: VB project level imports
using System.Collections.Generic;
using System;
using AxWFICALib;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using AxMSTSCLib;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
// End of VB project level imports


namespace mRemoteNG
{
	[global::Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]public 
	partial class frmChoosePanel : System.Windows.Forms.Form
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
			base.Load += new System.EventHandler(frmChoosePanel_Load);
			this.btnOK = new System.Windows.Forms.Button();
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			this.lblDescription = new System.Windows.Forms.Label();
			this.btnNew = new System.Windows.Forms.Button();
			this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			this.SuspendLayout();
			//
			//cbPanels
			//
			this.cbPanels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbPanels.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.cbPanels.FormattingEnabled = true;
			this.cbPanels.Location = new System.Drawing.Point(79, 45);
			this.cbPanels.Name = "cbPanels";
			this.cbPanels.Size = new System.Drawing.Size(157, 21);
			this.cbPanels.TabIndex = 10;
			//
			//btnOK
			//
			this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnOK.Location = new System.Drawing.Point(167, 74);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(69, 23);
			this.btnOK.TabIndex = 20;
			this.btnOK.Text = My.Language.strButtonOK;
			this.btnOK.UseVisualStyleBackColor = true;
			//
			//lblDescription
			//
			this.lblDescription.Location = new System.Drawing.Point(7, 8);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Size = new System.Drawing.Size(229, 29);
			this.lblDescription.TabIndex = 0;
			this.lblDescription.Text = My.Language.strLabelSelectPanel;
			//
			//btnNew
			//
			this.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnNew.Image = global::My.Resources.Resources.Panel_Add;
			this.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnNew.Location = new System.Drawing.Point(10, 44);
			this.btnNew.Name = "btnNew";
			this.btnNew.Size = new System.Drawing.Size(57, 23);
			this.btnNew.TabIndex = 40;
			this.btnNew.Text = My.Language.strButtonNew;
			this.btnNew.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnNew.UseVisualStyleBackColor = true;
			//
			//btnCancel
			//
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCancel.Location = new System.Drawing.Point(92, 74);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(69, 23);
			this.btnCancel.TabIndex = 30;
			this.btnCancel.Text = My.Language.strButtonCancel;
			this.btnCancel.UseVisualStyleBackColor = true;
			//
			//frmChoosePanel
			//
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF((float) (6.0F), (float) (13.0F));
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(245, 107);
			this.Controls.Add(this.lblDescription);
			this.Controls.Add(this.btnNew);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.cbPanels);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = global::My.Resources.Resources.Panels_Icon;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmChoosePanel";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = My.Language.strTitleSelectPanel;
			this.ResumeLayout(false);
			
		}
		internal System.Windows.Forms.ComboBox cbPanels;
		internal System.Windows.Forms.Button btnOK;
		internal System.Windows.Forms.Label lblDescription;
		internal System.Windows.Forms.Button btnNew;
		internal System.Windows.Forms.Button btnCancel;
	}
}
