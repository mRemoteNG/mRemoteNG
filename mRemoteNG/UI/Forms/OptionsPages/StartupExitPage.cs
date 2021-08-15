﻿using System;
using mRemoteNG.Properties;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public sealed partial class StartupExitPage
    {
        public StartupExitPage()
        {
            InitializeComponent();
            ApplyTheme();
            PageIcon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.StartupProject_16x);
        }

        public override string PageName
        {
            get => Language.StartupExit;
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            chkSaveConsOnExit.Text = Language.SaveConsOnExit;
            chkReconnectOnStart.Text = Language.ReconnectAtStartup;
            chkSingleInstance.Text = Language.AllowOnlySingleInstance;
            chkStartMinimized.Text = Language.StartMinimized;
        }

        public override void SaveSettings()
        {
            base.SaveSettings();

            Settings.Default.SaveConsOnExit = chkSaveConsOnExit.Checked;
            Settings.Default.OpenConsFromLastSession = chkReconnectOnStart.Checked;
            Settings.Default.SingleInstance = chkSingleInstance.Checked;
            Settings.Default.StartMinimized = chkStartMinimized.Checked;
            Settings.Default.StartFullScreen = chkStartFullScreen.Checked;
        }

        private void StartupExitPage_Load(object sender, EventArgs e)
        {
            chkSaveConsOnExit.Checked = Settings.Default.SaveConsOnExit;
            chkReconnectOnStart.Checked = Settings.Default.OpenConsFromLastSession;
            chkSingleInstance.Checked = Settings.Default.SingleInstance;
            chkStartMinimized.Checked = Settings.Default.StartMinimized;
            chkStartFullScreen.Checked = Settings.Default.StartFullScreen;
            ;
        }

        private void chkStartFullScreen_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStartFullScreen.Checked && chkStartMinimized.Checked)
            {
                chkStartMinimized.Checked = false;
            } 
        }

        private void chkStartMinimized_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStartMinimized.Checked && chkStartFullScreen.Checked)
            {
                chkStartFullScreen.Checked = false;
            }
        }
    }
}