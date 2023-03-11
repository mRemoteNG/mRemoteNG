using mRemoteNG.Properties;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    [SupportedOSPlatform("windows")]
    public sealed partial class TabsPanelsPage
    {
        public TabsPanelsPage()
        {
            InitializeComponent();
            ApplyTheme();
            PageIcon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.Tab_16x);
        }

        public override string PageName
        {
            get => Language.TabsAndPanels.Replace("&&", "&");
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            chkAlwaysShowPanelTabs.Text = Language.AlwaysShowPanelTabs;
            chkAlwaysShowConnectionTabs.Text = Language.AlwaysShowConnectionTabs;
            chkOpenNewTabRightOfSelected.Text = Language.OpenNewTabRight;
            chkShowLogonInfoOnTabs.Text = Language.ShowLogonInfoOnTabs;
            chkShowProtocolOnTabs.Text = Language.ShowProtocolOnTabs;
            chkIdentifyQuickConnectTabs.Text = Language.IdentifyQuickConnectTabs;
            chkDoubleClickClosesTab.Text = Language.DoubleClickTabClosesIt;
            chkAlwaysShowPanelSelectionDlg.Text = Language.AlwaysShowPanelSelection;
            chkCreateEmptyPanelOnStart.Text = Language.CreateEmptyPanelOnStartUp;
            lblPanelName.Text = $@"{Language.PanelName}:";
        }

        public override void LoadSettings()
        {
            chkAlwaysShowPanelTabs.Checked = Properties.OptionsTabsPanelsPage.Default.AlwaysShowPanelTabs;
            chkAlwaysShowConnectionTabs.Checked = Properties.OptionsTabsPanelsPage.Default.AlwaysShowConnectionTabs;
            chkOpenNewTabRightOfSelected.Checked = Properties.OptionsTabsPanelsPage.Default.OpenTabsRightOfSelected;
            chkShowLogonInfoOnTabs.Checked = Properties.OptionsTabsPanelsPage.Default.ShowLogonInfoOnTabs;
            chkShowProtocolOnTabs.Checked = Properties.OptionsTabsPanelsPage.Default.ShowProtocolOnTabs;
            chkIdentifyQuickConnectTabs.Checked = Properties.OptionsTabsPanelsPage.Default.IdentifyQuickConnectTabs;
            chkDoubleClickClosesTab.Checked = Properties.OptionsTabsPanelsPage.Default.DoubleClickOnTabClosesIt;
            chkAlwaysShowPanelSelectionDlg.Checked = Properties.OptionsTabsPanelsPage.Default.AlwaysShowPanelSelectionDlg;
            chkCreateEmptyPanelOnStart.Checked = Properties.OptionsTabsPanelsPage.Default.CreateEmptyPanelOnStartUp;
            txtBoxPanelName.Text = Properties.OptionsTabsPanelsPage.Default.StartUpPanelName;
            UpdatePanelNameTextBox();
        }

        public override void SaveSettings()
        {
            base.SaveSettings();

            Properties.OptionsTabsPanelsPage.Default.AlwaysShowPanelTabs = chkAlwaysShowPanelTabs.Checked;
            Properties.OptionsTabsPanelsPage.Default.AlwaysShowConnectionTabs = chkAlwaysShowConnectionTabs.Checked;
            FrmMain.Default.ShowHidePanelTabs();

            Properties.OptionsTabsPanelsPage.Default.OpenTabsRightOfSelected = chkOpenNewTabRightOfSelected.Checked;
            Properties.OptionsTabsPanelsPage.Default.ShowLogonInfoOnTabs = chkShowLogonInfoOnTabs.Checked;
            Properties.OptionsTabsPanelsPage.Default.ShowProtocolOnTabs = chkShowProtocolOnTabs.Checked;
            Properties.OptionsTabsPanelsPage.Default.IdentifyQuickConnectTabs = chkIdentifyQuickConnectTabs.Checked;
            Properties.OptionsTabsPanelsPage.Default.DoubleClickOnTabClosesIt = chkDoubleClickClosesTab.Checked;
            Properties.OptionsTabsPanelsPage.Default.AlwaysShowPanelSelectionDlg = chkAlwaysShowPanelSelectionDlg.Checked;
            Properties.OptionsTabsPanelsPage.Default.CreateEmptyPanelOnStartUp = chkCreateEmptyPanelOnStart.Checked;
            Properties.OptionsTabsPanelsPage.Default.StartUpPanelName = txtBoxPanelName.Text;
        }

        private void UpdatePanelNameTextBox()
        {
            txtBoxPanelName.Enabled = chkCreateEmptyPanelOnStart.Checked;
        }

        private void chkCreateEmptyPanelOnStart_CheckedChanged(object sender, System.EventArgs e)
        {
            UpdatePanelNameTextBox();
        }
    }
}