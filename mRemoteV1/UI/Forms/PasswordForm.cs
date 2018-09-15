using System;
using System.Security;
using System.Windows.Forms;
using mRemoteNG.Security;
using mRemoteNG.Tools;

namespace mRemoteNG.UI.Forms
{
    public partial class PasswordForm : IKeyProvider
	{
        private readonly string _passwordName;
        private SecureString _password = new SecureString();

        /// <summary>
        /// Puts the dialog into the New Password mode. An extra
        /// password box is shown which must match the first password
        /// to continue.
        /// </summary>
	    private bool NewPasswordMode { get; }

        /// <summary>
        /// Creates a new password form for entering or setting a password.
        /// </summary>
        /// <param name="passwordName"></param>
        /// <param name="newPasswordMode">
        /// Puts the dialog into the New Password mode. An extra
        /// password box is shown which must match the first password
        /// to continue.
        /// </param>
		public PasswordForm(string passwordName = null, bool newPasswordMode = true)
		{
			InitializeComponent();
			_passwordName = passwordName;
			NewPasswordMode = newPasswordMode;
		}

        /// <summary>
        /// Dispaly a dialog box requesting that the user 
        /// enter their password.
        /// </summary>
        /// <returns></returns>
        public Optional<SecureString> GetKey()
        {
            var dialog = ShowDialog();
            return dialog == DialogResult.OK
                ? _password
                : Optional<SecureString>.Empty;
        }

        #region Event Handlers
        private void frmPassword_Load(object sender, EventArgs e)
		{
			ApplyLanguage();
            var display = new DisplayProperties();
		    pbLock.Image = display.ScaleImage(pbLock.Image);
		    Height = tableLayoutPanel1.Height;

            if (NewPasswordMode) return;
            lblVerify.Visible = false;
		    txtVerify.Visible = false;
		    tableLayoutPanel1.RowStyles[tableLayoutPanel1.GetRow(lblVerify)].SizeType = SizeType.Absolute;
		    tableLayoutPanel1.RowStyles[tableLayoutPanel1.GetRow(lblVerify)].Height = 0;
		    tableLayoutPanel1.RowStyles[tableLayoutPanel1.GetRow(txtVerify)].SizeType = SizeType.Absolute;
            tableLayoutPanel1.RowStyles[tableLayoutPanel1.GetRow(txtVerify)].Height = 0;
		}

        private void PasswordForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _password = txtPassword.Text.ConvertToSecureString();
            txtPassword.Text = "";
            txtVerify.Text = "";
        }

        private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
            Close();
		}

	    private void btnOK_Click(object sender, EventArgs e)
	    {
            if (NewPasswordMode)
	            VerifyNewPassword();

	        DialogResult = DialogResult.OK;
	    }

	    private void txtPassword_TextChanged(object sender, EventArgs e)
		{
			HideStatus();
		}
        #endregion
		
        #region Private Methods
		private void ApplyLanguage()
		{
			Text = string.IsNullOrEmpty(_passwordName) ? Language.strTitlePassword : string.Format(Language.strTitlePasswordWithName, _passwordName);
				
			lblPassword.Text = Language.strLabelPassword;
			lblVerify.Text = Language.strLabelVerify;
			btnCancel.Text = Language.strButtonCancel;
			btnOK.Text = Language.strButtonOK;
		}
			
		private bool VerifyNewPassword()
		{
			if (txtPassword.Text.Length >= 3)
			{
				if (txtPassword.Text == txtVerify.Text)
					return true;
			    ShowStatus(Language.strPasswordStatusMustMatch);
			    return false;
			}
		    ShowStatus(Language.strPasswordStatusTooShort);
		    return false;
		}
			
		private void ShowStatus(string status)
		{
			lblStatus.Visible = true;
			lblStatus.Text = status;
		}
			
		private void HideStatus()
		{
			lblStatus.Visible = false;
		}
        #endregion
	}
}