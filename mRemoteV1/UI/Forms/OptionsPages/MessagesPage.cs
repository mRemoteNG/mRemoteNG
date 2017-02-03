using System.Windows.Forms;
using mRemoteNG.App;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public partial class MessagesPage
    {
        public MessagesPage()
        {
            InitializeComponent();
        }

        public override string PageName
        {
            get { return "Messages"; }
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            // notifications panel
            lblSwitchToErrorsAndInfos.Text = Language.strSwitchToErrorsAndInfos;
            chkMCInformation.Text = Language.strInformations;
            chkMCWarnings.Text = Language.strWarnings;
            chkMCErrors.Text = Language.strErrors;
        }

        public override void LoadSettings()
        {
            base.SaveSettings();
            // notifications panel
            chkMCInformation.Checked = Settings.Default.SwitchToMCOnInformation;
            chkMCWarnings.Checked = Settings.Default.SwitchToMCOnWarning;
            chkMCErrors.Checked = Settings.Default.SwitchToMCOnError;

            // log file
            textBoxLogPath.Text = Settings.Default.LogFilePath;
        }

        public override void SaveSettings()
        {
            Settings.Default.SwitchToMCOnInformation = chkMCInformation.Checked;
            Settings.Default.SwitchToMCOnWarning = chkMCWarnings.Checked;
            Settings.Default.SwitchToMCOnError = chkMCErrors.Checked;

            Settings.Default.Save();
        }
    }
}