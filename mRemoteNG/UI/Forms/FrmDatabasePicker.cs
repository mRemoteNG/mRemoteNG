using System;
using System.Windows.Forms;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Forms
{
    public partial class FrmDatabasePicker : Form
    {
        public DatabaseProfile? SelectedProfile { get; private set; }

        public FrmDatabasePicker()
        {
            InitializeComponent();
            LoadProfiles();
            ApplyLanguage();
        }

        private void LoadProfiles()
        {
            var profiles = DatabaseProfileManager.Profiles;
            lstProfiles.DataSource = profiles;
            lstProfiles.DisplayMember = "Name";
            
            if (lstProfiles.Items.Count > 0)
                lstProfiles.SelectedIndex = 0;
        }

        private void ApplyLanguage()
        {
            this.Text = "Select Database Profile"; // TODO: Add to Language resources
            btnOK.Text = Language._Ok;
            btnCancel.Text = Language._Cancel;
            lblSelectProfile.Text = "Select a profile to load:";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (lstProfiles.SelectedItem is DatabaseProfile profile)
            {
                SelectedProfile = profile;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Please select a profile.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void lstProfiles_DoubleClick(object sender, EventArgs e)
        {
            btnOK_Click(sender, e);
        }
    }
}
