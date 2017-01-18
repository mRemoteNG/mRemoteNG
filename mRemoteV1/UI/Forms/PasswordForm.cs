using System;
using System.Windows.Forms;
using mRemoteNG.UI.Controls;

namespace mRemoteNG.UI.Forms
{
    public partial class PasswordForm
	{
        private string _passwordName;

	    #region Public Properties

	    private bool Verify { get; set; }

	    public string Password => Verify ? txtVerify.Text : txtPassword.Text;

	    #endregion
		
        #region Constructors
		public PasswordForm(string passwordName = null, bool verify = true)
		{
			// This call is required by the designer.
			InitializeComponent();
				
			// Add any initialization after the InitializeComponent() call.
			_passwordName = passwordName;
			Verify = verify;
		}
        #endregion
		
        #region Event Handlers

	    private void frmPassword_Load(object sender, EventArgs e)
		{
			ApplyLanguage();

		    if (Verify) return;
		    Height = Height - (txtVerify.Top - txtPassword.Top);
		    lblVerify.Visible = false;
		    txtVerify.Visible = false;
		}

	    private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
            Close();
		}

	    private void btnOK_Click(object sender, EventArgs e)
		{
			if (Verify)
			{
				if (VerifyPassword())
					DialogResult = DialogResult.OK;
			}
			else
			{
				DialogResult = DialogResult.OK;
			}
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
				{
					return true;
				}
				else
				{
					ShowStatus(Language.strPasswordStatusMustMatch);
					return false;
				}
			}
			else
			{
				ShowStatus(Language.strPasswordStatusTooShort);
				return false;
			}
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