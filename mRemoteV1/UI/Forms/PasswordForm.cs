using System;
using System.Windows.Forms;

namespace mRemoteNG.UI.Forms
{
    public partial class PasswordForm
    {
        private readonly string _passwordName;

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

        #region Public Properties

        public bool Verify { get; set; } = true;

        public string Password
        {
            get
            {
                if (Verify)
                    return txtVerify.Text;
                return txtPassword.Text;
            }
        }

        #endregion

        #region Event Handlers

        private void frmPassword_Load(object sender, EventArgs e)
        {
            ApplyLanguage();

            if (!Verify)
            {
                Height = Height - (txtVerify.Top - txtPassword.Top);
                lblVerify.Visible = false;
                txtVerify.Visible = false;
            }
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
            if (string.IsNullOrEmpty(_passwordName))
                Text = Language.strTitlePassword;
            else
                Text = string.Format(Language.strTitlePasswordWithName, _passwordName);

            lblPassword.Text = Language.strLabelPassword;
            lblVerify.Text = Language.strLabelVerify;

            btnCancel.Text = Language.strButtonCancel;
            btnOK.Text = Language.strButtonOK;
        }

        private bool VerifyPassword()
        {
            if (txtPassword.Text.Length >= 3)
                if (txtPassword.Text == txtVerify.Text)
                {
                    return true;
                }
                else
                {
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