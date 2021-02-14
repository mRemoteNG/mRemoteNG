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
            tbPassword.Focus();
        }
    }
}
