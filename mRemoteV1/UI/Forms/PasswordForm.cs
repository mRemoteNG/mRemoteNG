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

	    private bool Verify { get; }

		public PasswordForm(string passwordName = null, bool verify = true)
		{
			InitializeComponent();
			_passwordName = passwordName;
			Verify = verify;
		}

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

		    if (Verify) return;
		    Height = Height - (txtVerify.Top - txtPassword.Top);
		    lblVerify.Visible = false;
		    txtVerify.Visible = false;
		}

        private void PasswordForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _password = txtPassword.Text.ConvertToSecureString();
            txtPassword.Text = "";
            txtVerify.Text = "";
            if (Verify) return;
            Height = Height + (txtVerify.Top - txtPassword.Top);
        }

        private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
            Close();
		}

	    private void btnOK_Click(object sender, EventArgs e)
	    {
	        if (Verify && VerifyPassword())
	            DialogResult = DialogResult.OK;
	        else
	            DialogResult = DialogResult.None;
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
			
		private bool VerifyPassword()
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