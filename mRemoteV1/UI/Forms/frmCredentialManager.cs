using System;
using mRemoteNG;
using System.Windows.Forms;
using mRemoteNG.App;

namespace mRemoteNG.UI.Forms
{
    public partial class FrmCredentialManager : Form
    {
        public FrmCredentialManager()
        {
            InitializeComponent();
            Load += OnLoad;
        }

        private void OnLoad(object sender, EventArgs eventArgs)
        {
            
        }

        private void btnAddCredential_Click(object sender, EventArgs e)
        {

        }

        private void btnEditCredential_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}