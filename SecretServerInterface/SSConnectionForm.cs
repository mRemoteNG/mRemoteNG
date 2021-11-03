using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SecretServerInterface
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
                tbPassword.Focus();
        }

        private void cbUseSSO_CheckedChanged(object sender, EventArgs e)
        {
            SetVisibility();
        }
        private void SetVisibility()
        {
            bool ch = cbUseSSO.Checked;
            tbDomain.Enabled = !ch;
            tbPassword.Enabled = !ch;
            tbUsername.Enabled = !ch;
            tbSSURL.Enabled = !ch;

            tbWinAuthURL.Enabled = ch;
        }
    }
}
