using System.ComponentModel;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public partial class SecurityPage : OptionsPage
    {
        public SecurityPage()
        {
            InitializeComponent();
        }

        [Browsable(false)]
        public override string PageName
        {
            get { return Language.strTabSecurity; }
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();
            chkEncryptCompleteFile.Text = Language.strEncryptCompleteConnectionFile;
        }

        public override void LoadSettings()
        {
            chkEncryptCompleteFile.Checked = Settings.Default.EncryptCompleteConnectionsFile;
        }

        public override void SaveSettings()
        {
            Settings.Default.EncryptCompleteConnectionsFile = chkEncryptCompleteFile.Checked;

            Settings.Default.Save();
        }

        public override void RevertSettings()
        {

        }
    }
}