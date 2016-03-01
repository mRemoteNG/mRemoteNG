using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.My;

namespace mRemoteNG.Forms.OptionsPages
{
	public partial class AppearancePage
	{
		public override string PageName {
			get { return Language.strTabAppearance; }
			set { }
		}

		public override void ApplyLanguage()
		{
			base.ApplyLanguage();

			lblLanguage.Text = Language.strLanguage;
			lblLanguageRestartRequired.Text = string.Format(Language.strLanguageRestartRequired, Application.Info.ProductName);
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

			foreach (string nativeName in SupportedCultures.CultureNativeNames) {
				cboLanguage.Items.Add(nativeName);
			}
			if (!string.IsNullOrEmpty(Settings.OverrideUICulture) && SupportedCultures.IsNameSupported(Settings.OverrideUICulture)) {
				cboLanguage.SelectedItem = SupportedCultures.CultureNativeName[Settings.OverrideUICulture];
			}
			if (cboLanguage.SelectedIndex == -1) {
				cboLanguage.SelectedIndex = 0;
			}

			chkShowDescriptionTooltipsInTree.Checked = Settings.ShowDescriptionTooltipsInTree;
			chkShowFullConnectionsFilePathInTitle.Checked = Settings.ShowCompleteConsPathInTitle;
			chkShowSystemTrayIcon.Checked = Settings.ShowSystemTrayIcon;
			chkMinimizeToSystemTray.Checked = Settings.MinimizeToTray;
		}

		public override void SaveSettings()
		{
			base.SaveSettings();

			if (cboLanguage.SelectedIndex > 0 & SupportedCultures.IsNativeNameSupported(cboLanguage.SelectedItem)) {
				Settings.OverrideUICulture = SupportedCultures.CultureName[cboLanguage.SelectedItem];
			} else {
				Settings.OverrideUICulture = string.Empty;
			}

			Settings.ShowDescriptionTooltipsInTree = chkShowDescriptionTooltipsInTree.Checked;
			Settings.ShowCompleteConsPathInTitle = chkShowFullConnectionsFilePathInTitle.Checked;
			My.MyProject.Forms.frmMain.ShowFullPathInTitle = chkShowFullConnectionsFilePathInTitle.Checked;

			Settings.ShowSystemTrayIcon = chkShowSystemTrayIcon.Checked;
			if (Settings.ShowSystemTrayIcon) {
				if (Runtime.NotificationAreaIcon == null) {
					Runtime.NotificationAreaIcon = new Tools.Controls.NotificationAreaIcon();
				}
			} else {
				if (Runtime.NotificationAreaIcon != null) {
					Runtime.NotificationAreaIcon.Dispose();
					Runtime.NotificationAreaIcon = null;
				}
			}

			Settings.MinimizeToTray = chkMinimizeToSystemTray.Checked;
		}
	}
}
