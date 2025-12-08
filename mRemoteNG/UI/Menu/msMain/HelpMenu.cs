using System;
using System.Diagnostics;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.UI.Forms;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;
using mRemoteNG.Config.Settings.Registry;

namespace mRemoteNG.UI.Menu
{
    [SupportedOSPlatform("windows")]
    public class HelpMenu : ToolStripMenuItem
    {
        private ToolStripMenuItem _mMenInfoHelp = null!;
        private ToolStripMenuItem _mMenInfoWebsite = null!;
        private ToolStripSeparator _mMenInfoSep1 = null!;
        private ToolStripMenuItem _mMenInfoAbout = null!;
        private ToolStripMenuItem _mMenInfoDonate = null!;
        private ToolStripSeparator _mMenInfoSep2 = null!;
        private ToolStripSeparator _mMenInfoSep3 = null!;
        private ToolStripSeparator _mMenInfoSep4 = null!;
        private ToolStripMenuItem _mMenInfoForum = null!;
        private ToolStripMenuItem _mMenInfoChat = null!;
        private ToolStripMenuItem _mMenInfoCommunity = null!;
        private ToolStripMenuItem _mMenInfoBug = null!;
        private ToolStripMenuItem _mMenToolsUpdate = null!;

        public HelpMenu()
        {
            Initialize();
        }

        private void Initialize()
        {
            _mMenInfoHelp = new ToolStripMenuItem();
            _mMenInfoSep1 = new ToolStripSeparator();
            _mMenInfoWebsite = new ToolStripMenuItem();
            _mMenInfoDonate = new ToolStripMenuItem();
            _mMenInfoForum = new ToolStripMenuItem();
            _mMenInfoChat = new ToolStripMenuItem();
            _mMenInfoCommunity = new ToolStripMenuItem();
            _mMenInfoBug = new ToolStripMenuItem();
            _mMenToolsUpdate = new ToolStripMenuItem();
            _mMenInfoSep2 = new ToolStripSeparator();
            _mMenInfoSep3 = new ToolStripSeparator();
            _mMenInfoSep4 = new ToolStripSeparator();
            _mMenInfoAbout = new ToolStripMenuItem();

            // 
            // mMenInfo
            // 
            DropDownItems.AddRange(new ToolStripItem[]
            {
                _mMenInfoHelp,
                _mMenInfoSep1,
                _mMenInfoWebsite,
                _mMenInfoForum,
                _mMenInfoChat,
                _mMenInfoCommunity,
                _mMenInfoBug,
                _mMenInfoSep2,
                _mMenToolsUpdate,
                _mMenInfoSep3,
                _mMenInfoDonate,
                _mMenInfoSep4,
                _mMenInfoAbout
            });
            Name = "mMenInfo";
            Size = new System.Drawing.Size(44, 20);
            Text = Language._Help;
            TextDirection = ToolStripTextDirection.Horizontal;
            // 
            // mMenInfoHelp
            // 
            _mMenInfoHelp.Image = Properties.Resources.F1Help_16x;
            _mMenInfoHelp.Name = "mMenInfoHelp";
            _mMenInfoHelp.ShortcutKeys = Keys.F1;
            _mMenInfoHelp.Size = new System.Drawing.Size(190, 22);
            _mMenInfoHelp.Text = Language.MenuItem_HelpContents;
            _mMenInfoHelp.Click += mMenInfoHelp_Click;
            // 
            // mMenToolsUpdate
            // 
            _mMenToolsUpdate.Image = Properties.Resources.RunUpdate_16x;
            _mMenToolsUpdate.Name = "mMenToolsUpdate";
            _mMenToolsUpdate.Size = new System.Drawing.Size(190, 22);
            _mMenToolsUpdate.Text = Language.MenuItem_CheckForUpdates;
            _mMenToolsUpdate.Click += mMenToolsUpdate_Click;
            _mMenToolsUpdate.Enabled = CommonRegistrySettings.AllowCheckForUpdates
                && CommonRegistrySettings.AllowCheckForUpdatesManual;
            // 
            // mMenInfoSep1
            // 
            _mMenInfoSep1.Name = "mMenInfoSep1";
            _mMenInfoSep1.Size = new System.Drawing.Size(187, 6);
            // 
            // mMenInfoWebsite
            // 
            _mMenInfoWebsite.Name = "mMenInfoWebsite";
            _mMenInfoWebsite.Size = new System.Drawing.Size(190, 22);
            _mMenInfoWebsite.Text = Language.MenuItem_Website;
            _mMenInfoWebsite.Click += mMenInfoWebsite_Click;
            // 
            // mMenInfoDonate
            // 
            _mMenInfoDonate.Name = "mMenInfoDonate";
            _mMenInfoDonate.Size = new System.Drawing.Size(190, 22);
            _mMenInfoDonate.Text = Language.MenuItem_Donate;
            _mMenInfoDonate.Click += mMenInfoDonate_Click;
            // 
            // mMenInfoForum
            // 
            _mMenInfoForum.Name = "mMenInfoForum";
            _mMenInfoForum.Size = new System.Drawing.Size(190, 22);
            _mMenInfoForum.Text = Language.MenuItem_SupportForum;
            _mMenInfoForum.Click += mMenInfoForum_Click;
            // 
            // mMenInfoChat
            // 
            _mMenInfoChat.Name = "mMenInfoChat";
            _mMenInfoChat.Size = new System.Drawing.Size(190, 22);
            _mMenInfoChat.Text = Language.MenuItem_Chat;
            _mMenInfoChat.Click += mMenInfoChat_Click;
            // 
            // mMenInfoCommunity
            // 
            _mMenInfoCommunity.Name = "mMenInfoCommunity";
            _mMenInfoCommunity.Size = new System.Drawing.Size(190, 22);
            _mMenInfoCommunity.Text = Language.MenuItem_Community;
            _mMenInfoCommunity.Click += mMenInfoCommunity_Click;
            // 
            // mMenInfoBug
            // 
            _mMenInfoBug.Name = "mMenInfoBug";
            _mMenInfoBug.Size = new System.Drawing.Size(190, 22);
            _mMenInfoBug.Text = Language.MenuItem_ReportIssue;
            _mMenInfoBug.Click += mMenInfoBug_Click;
            // 
            // mMenInfoSep2
            // 
            _mMenInfoSep2.Name = "mMenInfoSep2";
            _mMenInfoSep2.Size = new System.Drawing.Size(187, 6);
            // 
            // mMenInfoSep3
            // 
            _mMenInfoSep3.Name = "mMenInfoSep2";
            _mMenInfoSep3.Size = new System.Drawing.Size(187, 6);
            // 
            // mMenInfoSep4
            // 
            _mMenInfoSep4.Name = "mMenInfoSep2";
            _mMenInfoSep4.Size = new System.Drawing.Size(187, 6);
            // 
            // mMenInfoAbout
            // 
            _mMenInfoAbout.Image = Properties.Resources.UIAboutBox_16x;
            _mMenInfoAbout.Name = "mMenInfoAbout";
            _mMenInfoAbout.Size = new System.Drawing.Size(190, 22);
            _mMenInfoAbout.Text = Language.MenuItem_About;
            _mMenInfoAbout.Click += mMenInfoAbout_Click;
        }

        public void ApplyLanguage()
        {
            Text = Language._Help;
            _mMenInfoHelp.Text = Language.MenuItem_HelpContents;
            _mMenInfoWebsite.Text = Language.MenuItem_Website;
            _mMenInfoDonate.Text = Language.MenuItem_Donate;
            _mMenInfoForum.Text = Language.MenuItem_SupportForum;
            _mMenInfoChat.Text = Language.MenuItem_Chat;
            _mMenInfoCommunity.Text = Language.MenuItem_Community;
            _mMenInfoBug.Text = Language.MenuItem_ReportIssue;
            _mMenInfoAbout.Text = Language.MenuItem_About;
            _mMenToolsUpdate.Text = Language.MenuItem_CheckForUpdates;
        }

        #region Info

        private async void mMenToolsUpdate_Click(object sender, EventArgs e)
        {
            AppWindows.Show(WindowType.Update);
            var updateWindow = AppWindows.UpdateForm;
            if (updateWindow != null && !updateWindow.IsDisposed)
            {
                await updateWindow.PerformUpdateCheckAsync();
            }
        }

        private void mMenInfoHelp_Click(object? sender, EventArgs e) => OpenUrl(GeneralAppInfo.UrlDocumentation);

        private void mMenInfoForum_Click(object? sender, EventArgs e) => OpenUrl(GeneralAppInfo.UrlForum);

        private void mMenInfoChat_Click(object? sender, EventArgs e) => OpenUrl(GeneralAppInfo.UrlChat);

        private void mMenInfoCommunity_Click(object? sender, EventArgs e) => OpenUrl(GeneralAppInfo.UrlCommunity);

        private void mMenInfoBug_Click(object? sender, EventArgs e) => OpenUrl(GeneralAppInfo.UrlBugs);

        private void mMenInfoWebsite_Click(object? sender, EventArgs e) => OpenUrl(GeneralAppInfo.UrlHome);

        private void mMenInfoDonate_Click(object? sender, EventArgs e) => OpenUrl(GeneralAppInfo.UrlDonate);
        
        private static void OpenUrl(string url)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };
            Process.Start(startInfo);
        }

        private void mMenInfoAbout_Click(object? sender, EventArgs e)
        {
            if (frmAbout.Instance == null || frmAbout.Instance.IsDisposed)
                frmAbout.Instance = new frmAbout();
            frmAbout.Instance.Show(FrmMain.Default.pnlDock);
        }

        #endregion
    }
}