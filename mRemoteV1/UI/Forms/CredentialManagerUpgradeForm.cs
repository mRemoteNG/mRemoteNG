using System.Windows.Forms;
using mRemoteNG.App;

namespace mRemoteNG.UI.Forms
{
    public partial class CredentialManagerUpgradeForm : Form
    {
        public string ConnectionFilePath { get; set; }

        public CredentialManagerUpgradeForm()
        {
            InitializeComponent();
            ApplyLanguage();
            SetValues();
        }

        private void ApplyLanguage()
        {
            labelConfConsPathHeader.Text = "Connection file";
        }

        private void SetValues()
        {
            labelConfConsPath.Text = ConnectionFilePath;
        }

        private void buttonPerformUpgrade_Click(object sender, System.EventArgs e)
        {
            tabControl.SelectedTab = tabPageUpgradeOptions;
        }

        private void buttonBack_Click(object sender, System.EventArgs e)
        {
            tabControl.SelectedTab = tabPageWelcome;
        }

        private void buttonExit_Click(object sender, System.EventArgs e)
        {
            Shutdown.Quit();
        }
    }
}