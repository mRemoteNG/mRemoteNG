namespace mRemoteNG.UI.Forms.OptionsPages
{
    public partial class CredentialsPage : OptionsPage
    {
        public CredentialsPage()
        {
            InitializeComponent();
        }

        public override string PageName {
            get { return Language.Credentials; }
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();
            checkBoxUnlockOnStartup.Text = Language.PromptUnlockCredReposOnStartup;
        }

        public override void LoadSettings()
        {
            base.SaveSettings();
            checkBoxUnlockOnStartup.Checked = Settings.Default.PromptUnlockCredReposOnStartup;
        }

        public override void SaveSettings()
        {
            Settings.Default.PromptUnlockCredReposOnStartup = checkBoxUnlockOnStartup.Checked;
            Settings.Default.Save();
        }
    }
}