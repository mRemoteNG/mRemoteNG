using System;
using System.Windows.Forms;
using mRemoteNG.App;

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
            if (!string.IsNullOrEmpty(mRemoteNG.Settings.Default.OverrideUICulture) &&
                SupportedCultures.IsNameSupported(mRemoteNG.Settings.Default.OverrideUICulture))
            {
                cboLanguage.SelectedItem = SupportedCultures.get_CultureNativeName(mRemoteNG.Settings.Default.OverrideUICulture);
            }
            if (cboLanguage.SelectedIndex == -1)
            {
                cboLanguage.SelectedIndex = 0;
            }

            chkShowDescriptionTooltipsInTree.Checked = mRemoteNG.Settings.Default.ShowDescriptionTooltipsInTree;
            chkShowFullConnectionsFilePathInTitle.Checked = mRemoteNG.Settings.Default.ShowCompleteConsPathInTitle;
            chkShowSystemTrayIcon.Checked = mRemoteNG.Settings.Default.ShowSystemTrayIcon;
            chkMinimizeToSystemTray.Checked = mRemoteNG.Settings.Default.MinimizeToTray;
        }

        public override void SaveSettings()
        {

            if (cboLanguage.SelectedIndex > 0 &&
                SupportedCultures.IsNativeNameSupported(Convert.ToString(cboLanguage.SelectedItem)))
            {
                mRemoteNG.Settings.Default.OverrideUICulture =
                    SupportedCultures.get_CultureName(Convert.ToString(cboLanguage.SelectedItem));
            }
            else
            {
                mRemoteNG.Settings.Default.OverrideUICulture = string.Empty;
            }

            mRemoteNG.Settings.Default.ShowDescriptionTooltipsInTree = chkShowDescriptionTooltipsInTree.Checked;
            mRemoteNG.Settings.Default.ShowCompleteConsPathInTitle = chkShowFullConnectionsFilePathInTitle.Checked;
            frmMain.Default.ShowFullPathInTitle = chkShowFullConnectionsFilePathInTitle.Checked;

            mRemoteNG.Settings.Default.ShowSystemTrayIcon = chkShowSystemTrayIcon.Checked;
            if (mRemoteNG.Settings.Default.ShowSystemTrayIcon)
            {
                if (Runtime.NotificationAreaIcon == null)
                {
                    Runtime.NotificationAreaIcon = new Tools.Controls.NotificationAreaIcon();
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

            mRemoteNG.Settings.Default.MinimizeToTray = chkMinimizeToSystemTray.Checked;
        }
    }
}