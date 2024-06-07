namespace ExternalConnectors.CPS
{
    public partial class CPSConnectionForm : Form
    {
        public CPSConnectionForm()
        {
            InitializeComponent();
        }

        private void CPSConnectionForm_Activated(object sender, EventArgs e)
        {
            SetVisibility();
            if (cbUseSSO.Checked)
                btnOK.Focus();
            else
            {
                if (tbAPIKey.Text.Length == 0)
                    tbAPIKey.Focus(); 
                else
                    tbOTP.Focus();
            }

            tbAPIKey.Focus();
            if (!string.IsNullOrEmpty(tbAPIKey.Text) || cbUseSSO.Checked == true)
                tbOTP.Focus();


        }

        private void cbUseSSO_CheckedChanged(object sender, EventArgs e)
        {
            SetVisibility();
        }
        private void SetVisibility()
        {
            bool ch = cbUseSSO.Checked;
            tbAPIKey.Enabled = !ch;
            //tbUsername.Enabled = !ch;
        }
    }
}
