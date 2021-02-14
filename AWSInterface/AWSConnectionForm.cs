using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AWSInterface
{
    public partial class AWSConnectionForm : Form
    {
        public AWSConnectionForm()
        {
            InitializeComponent();

        }

        private void AWSConnectionForm_Activated(object sender, EventArgs e)
        {
            tbAccesKeyID.Focus();
        }
    }
}
