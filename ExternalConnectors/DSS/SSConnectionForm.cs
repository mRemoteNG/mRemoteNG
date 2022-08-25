namespace ExternalConnectors.DSS
{
    public partial class SSConnectionForm : Form
    {
        public SSConnectionForm()
        {
            InitializeComponent();
        }

        private void SSConnectionForm_Activated(object sender, EventArgs e)
        {
            SetVisibility();
            if (cbUseSSO.Checked)
                btnOK.Focus();
            else
            {
                if (tbPassword.Text.Length == 0)
                    tbPassword.Focus(); 
                else
                    tbOTP.Focus();
            }
        }

        private void cbUseSSO_CheckedChanged(object sender, EventArgs e)
        {
            SetVisibility();
        }
        private void SetVisibility()
        {
            bool ch = cbUseSSO.Checked;
            tbPassword.Enabled = !ch;
            tbUsername.Enabled = !ch;
        }
    }
}
