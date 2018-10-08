namespace mRemoteNG.UI.Forms.OptionsPages
{
    public sealed partial class TabsPanelsPage
    {
        public TabsPanelsPage()
        {
            InitializeComponent();
            ApplyTheme();
        }

        public override string PageName
        {
            get => Language.strTabsAndPanels.Replace("&&", "&");
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            chkAlwaysShowPanelTabs.Text = Language.strAlwaysShowPanelTabs;
            chkOpenNewTabRightOfSelected.Text = Language.strOpenNewTabRight;
            chkShowLogonInfoOnTabs.Text = Language.strShowLogonInfoOnTabs;
            chkShowProtocolOnTabs.Text = Language.strShowProtocolOnTabs;
            chkIdentifyQuickConnectTabs.Text = Language.strIdentifyQuickConnectTabs;
            chkDoubleClickClosesTab.Text = Language.strDoubleClickTabClosesIt;
            chkAlwaysShowPanelSelectionDlg.Text = Language.strAlwaysShowPanelSelection;
            chkCreateEmptyPanelOnStart.Text = Language.strCreateEmptyPanelOnStartUp;
            lblPanelName.Text = $@"{Language.strPanelName}:";
        }

        public override void LoadSettings()
        {
            base.SaveSettings();

            chkAlwaysShowPanelTabs.Checked = Settings.Default.AlwaysShowPanelTabs;
            chkOpenNewTabRightOfSelected.Checked = Settings.Default.OpenTabsRightOfSelected;
            chkShowLogonInfoOnTabs.Checked = Settings.Default.ShowLogonInfoOnTabs;
            chkShowProtocolOnTabs.Checked = Settings.Default.ShowProtocolOnTabs;
            chkIdentifyQuickConnectTabs.Checked = Settings.Default.IdentifyQuickConnectTabs;
            chkDoubleClickClosesTab.Checked = Settings.Default.DoubleClickOnTabClosesIt;
            chkAlwaysShowPanelSelectionDlg.Checked = Settings.Default.AlwaysShowPanelSelectionDlg;
            chkCreateEmptyPanelOnStart.Checked = Settings.Default.CreateEmptyPanelOnStartUp;
            txtBoxPanelName.Text = Settings.Default.StartUpPanelName;
            UpdatePanelNameTextBox();
        }

        public override void SaveSettings()
        {
            base.SaveSettings();

            Settings.Default.AlwaysShowPanelTabs = chkAlwaysShowPanelTabs.Checked;
            FrmMain.Default.ShowHidePanelTabs();

            Settings.Default.OpenTabsRightOfSelected = chkOpenNewTabRightOfSelected.Checked;
            Settings.Default.ShowLogonInfoOnTabs = chkShowLogonInfoOnTabs.Checked;
            Settings.Default.ShowProtocolOnTabs = chkShowProtocolOnTabs.Checked;
            Settings.Default.IdentifyQuickConnectTabs = chkIdentifyQuickConnectTabs.Checked;
            Settings.Default.DoubleClickOnTabClosesIt = chkDoubleClickClosesTab.Checked;
            Settings.Default.AlwaysShowPanelSelectionDlg = chkAlwaysShowPanelSelectionDlg.Checked;
            Settings.Default.CreateEmptyPanelOnStartUp = chkCreateEmptyPanelOnStart.Checked;
            Settings.Default.StartUpPanelName = txtBoxPanelName.Text;

            Settings.Default.Save();
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