using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Credential;
using mRemoteNG.Themes;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Forms
{
    [SupportedOSPlatform("windows")]
    public partial class FrmChooseCredential
    {
        public FrmChooseCredential()
        {
            InitializeComponent();
            Icon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.Key_16x);
        }

        /// <summary>
        /// The credential record selected by the user, or null if none was chosen.
        /// </summary>
        public ICredentialRecord SelectedCredential =>
            cbCredentials.SelectedItem as ICredentialRecord;

        private void FrmChooseCredential_Load(object sender, System.EventArgs e)
        {
            ApplyLanguage();
            ApplyTheme();
            AddAvailableCredentials();
        }

        private void ApplyLanguage()
        {
            btnOK.Text = Language._Ok;
            lblDescription.Text = Language.SelectCredential;
            Text = Language.ConnectWithCredentials;
        }

        // Apply the dark/light title bar before the window is shown to avoid a white flash.
        protected override void OnHandleCreated(System.EventArgs e)
        {
            base.OnHandleCreated(e);
            ThemeManager.getInstance().ApplyThemeToTitleBar(this);
        }

        private void ApplyTheme()
        {
            ThemeManager.getInstance().ApplyThemeToTitleBar(this);
            if (!ThemeManager.getInstance().ActiveAndExtended) return;
            BackColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            ForeColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
            lblDescription.BackColor =
                ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            lblDescription.ForeColor =
                ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
        }

        private void AddAvailableCredentials()
        {
            cbCredentials.Items.Clear();

            List<ICredentialRecord> credentials = Runtime.CredentialProviderCatalog
                .GetCredentialRecords()
                .OrderBy(cred => cred.Title)
                .ToList();

            foreach (ICredentialRecord credential in credentials)
                cbCredentials.Items.Add(credential);

            if (cbCredentials.Items.Count > 0)
            {
                cbCredentials.SelectedItem = cbCredentials.Items[0];
                cbCredentials.Enabled = true;
                btnOK.Enabled = true;
            }
            else
            {
                cbCredentials.Enabled = false;
                btnOK.Enabled = false;
            }
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
