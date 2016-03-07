using System.Collections.Generic;
using System;
using AxWFICALib;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using AxMSTSCLib;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.My;


namespace mRemoteNG.Forms.OptionsPages
{
	public partial class AppearancePage
	{
		public AppearancePage()
		{
			InitializeComponent();
		}
        public override string PageName
		{
			get
			{
				return Language.strTabAppearance;
			}
			set
			{
			}
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
				
			foreach (string nativeName in SupportedCultures.CultureNativeNames)
			{
				cboLanguage.Items.Add(nativeName);
			}
            if (!string.IsNullOrEmpty(My.Settings.Default.OverrideUICulture) && SupportedCultures.IsNameSupported(My.Settings.Default.OverrideUICulture))
			{
                cboLanguage.SelectedItem = SupportedCultures.get_CultureNativeName(My.Settings.Default.OverrideUICulture);
			}
			if (cboLanguage.SelectedIndex == -1)
			{
				cboLanguage.SelectedIndex = 0;
			}

            chkShowDescriptionTooltipsInTree.Checked = My.Settings.Default.ShowDescriptionTooltipsInTree;
            chkShowFullConnectionsFilePathInTitle.Checked = My.Settings.Default.ShowCompleteConsPathInTitle;
            chkShowSystemTrayIcon.Checked = My.Settings.Default.ShowSystemTrayIcon;
            chkMinimizeToSystemTray.Checked = My.Settings.Default.MinimizeToTray;
		}
			
		public override void SaveSettings()
		{
			base.SaveSettings();
				
			if (cboLanguage.SelectedIndex > 0 && SupportedCultures.IsNativeNameSupported(System.Convert.ToString(cboLanguage.SelectedItem)))
			{
                My.Settings.Default.OverrideUICulture = SupportedCultures.get_CultureName(System.Convert.ToString(cboLanguage.SelectedItem));
			}
			else
			{
                My.Settings.Default.OverrideUICulture = string.Empty;
			}

            My.Settings.Default.ShowDescriptionTooltipsInTree = chkShowDescriptionTooltipsInTree.Checked;
            My.Settings.Default.ShowCompleteConsPathInTitle = chkShowFullConnectionsFilePathInTitle.Checked;
			frmMain.Default.ShowFullPathInTitle = chkShowFullConnectionsFilePathInTitle.Checked;

            My.Settings.Default.ShowSystemTrayIcon = chkShowSystemTrayIcon.Checked;
            if (My.Settings.Default.ShowSystemTrayIcon)
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
				
			My.Settings.Default.MinimizeToTray = chkMinimizeToSystemTray.Checked;
		}
	}
}
