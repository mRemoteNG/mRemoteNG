using mRemoteNG.UI.Controls;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Forms
{

    public partial class FrmChooseCredential : System.Windows.Forms.Form
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
            this.cbCredentials = new MrngComboBox();
            this.btnOK = new MrngButton();
            this.lblDescription = new mRemoteNG.UI.Controls.MrngLabel();
            this.SuspendLayout();
            //
            // cbCredentials
            //
            this.cbCredentials._mice = MrngComboBox.MouseState.HOVER;
            this.cbCredentials.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCredentials.FormattingEnabled = true;
            this.cbCredentials.Location = new System.Drawing.Point(12, 42);
            this.cbCredentials.Name = "cbCredentials";
            this.cbCredentials.Size = new System.Drawing.Size(224, 21);
            this.cbCredentials.TabIndex = 10;
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
            this.lblDescription.Text = "Select a credential from the list below. Click OK to connect.";
            //
            // FrmChooseCredential
            //
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(245, 107);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cbCredentials);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmChooseCredential";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connect With Credentials";
            this.Load += new System.EventHandler(this.FrmChooseCredential_Load);
            this.ResumeLayout(false);

        }
        internal MrngComboBox cbCredentials;
        internal MrngButton btnOK;
        internal Controls.MrngLabel lblDescription;
    }
}
