using System;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Tools;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public partial class AppearancePage
    {
        public AppearancePage()
        {
            InitializeComponent();
        }

        public override string PageName
        {
            get { return Language.strTabAppearance; }
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            lblLanguage.Text = Language.strLanguage;
            lblLanguageRestartRequired.Text = string.Format(Language.strLanguageRestartRequired, Application.ProductName);
            chkShowDescriptionTooltipsInTree.Text = Language.strShowDescriptionTooltips;
            chkShowFullConnectionsFilePathInTitle.Text = Language.strShowFullConsFilePath;
            chkShowSystemTrayIcon.Text = Language.strAlwaysShowSysTrayIcon;
            chkMinimizeToSystemTray.Text = Language.strMinimizeToSysTray;
        }

        public override void LoadSettings()
        {
            base.SaveSettings();

            cboLanguage.Items.Clear();
            cboLanguage.Items.Add(Language.strLanguageDefault);

            foreach (var nativeName in SupportedCultures.CultureNativeNames)
            {
                cboLanguage.Items.Add(nativeName);
            }
            if (!string.IsNullOrEmpty(Settings.Default.OverrideUICulture) &&
                SupportedCultures.IsNameSupported(Settings.Default.OverrideUICulture))
            {
                cboLanguage.SelectedItem = SupportedCultures.get_CultureNativeName(Settings.Default.OverrideUICulture);
            }
            if (cboLanguage.SelectedIndex == -1)
            {
                cboLanguage.SelectedIndex = 0;
            }

            chkShowDescriptionTooltipsInTree.Checked = Settings.Default.ShowDescriptionTooltipsInTree;
            chkShowFullConnectionsFilePathInTitle.Checked = Settings.Default.ShowCompleteConsPathInTitle;
            chkShowSystemTrayIcon.Checked = Settings.Default.ShowSystemTrayIcon;
            chkMinimizeToSystemTray.Checked = Settings.Default.MinimizeToTray;
        }

        public override void SaveSettings()
        {

            if (cboLanguage.SelectedIndex > 0 &&
                SupportedCultures.IsNativeNameSupported(Convert.ToString(cboLanguage.SelectedItem)))
            {
                Settings.Default.OverrideUICulture =
                    SupportedCultures.get_CultureName(Convert.ToString(cboLanguage.SelectedItem));
            }
            else
            {
                Settings.Default.OverrideUICulture = string.Empty;
            }

            Settings.Default.ShowDescriptionTooltipsInTree = chkShowDescriptionTooltipsInTree.Checked;
            Settings.Default.ShowCompleteConsPathInTitle = chkShowFullConnectionsFilePathInTitle.Checked;
            FrmMain.Default.ShowFullPathInTitle = chkShowFullConnectionsFilePathInTitle.Checked;

            Settings.Default.ShowSystemTrayIcon = chkShowSystemTrayIcon.Checked;
            if (Settings.Default.ShowSystemTrayIcon)
            {
                if (Runtime.NotificationAreaIcon == null)
                {
                    Runtime.NotificationAreaIcon = new NotificationAreaIcon();
                }
            }
            else
            {
                if (Runtime.NotificationAreaIcon != null)
                {
                    Runtime.NotificationAreaIcon.Dispose();
                    Runtime.NotificationAreaIcon = null;
                }
            }

            Settings.Default.MinimizeToTray = chkMinimizeToSystemTray.Checked;

            Settings.Default.Save();
        }
    }
}