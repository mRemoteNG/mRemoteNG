using System;
using System.Diagnostics;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Connection;
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
        private ToolStripMenuItem _mMenKeyboardShortcuts = null!;
        private ToolStripSeparator _mMenInfoSep1 = null!;
        private ToolStripMenuItem _mMenForkGitHub = null!;
        private ToolStripMenuItem _mMenForkReleases = null!;
        private ToolStripMenuItem _mMenForkReportIssue = null!;
        private ToolStripSeparator _mMenInfoSep2 = null!;
        private ToolStripMenuItem _mMenInfoWebsite = null!;
        private ToolStripMenuItem _mMenInfoForum = null!;
        private ToolStripMenuItem _mMenInfoChat = null!;
        private ToolStripMenuItem _mMenInfoCommunity = null!;
        private ToolStripSeparator _mMenInfoSep3 = null!;
        private ToolStripMenuItem _mMenToolsDebugDump = null!;
        private ToolStripMenuItem _mMenToolsUpdate = null!;
        private ToolStripSeparator _mMenInfoSep4 = null!;
        private ToolStripMenuItem _mMenInfoDonate = null!;
        private ToolStripSeparator _mMenInfoSep5 = null!;
        private ToolStripMenuItem _mMenInfoAbout = null!;

        public HelpMenu()
        {
            Initialize();
        }

        private void Initialize()
        {
            _mMenInfoHelp = new ToolStripMenuItem();
            _mMenKeyboardShortcuts = new ToolStripMenuItem();
            _mMenInfoSep1 = new ToolStripSeparator();
            _mMenForkGitHub = new ToolStripMenuItem();
            _mMenForkReleases = new ToolStripMenuItem();
            _mMenForkReportIssue = new ToolStripMenuItem();
            _mMenInfoSep2 = new ToolStripSeparator();
            _mMenInfoWebsite = new ToolStripMenuItem();
            _mMenInfoForum = new ToolStripMenuItem();
            _mMenInfoChat = new ToolStripMenuItem();
            _mMenInfoCommunity = new ToolStripMenuItem();
            _mMenInfoSep3 = new ToolStripSeparator();
            _mMenToolsDebugDump = new ToolStripMenuItem();
            _mMenToolsUpdate = new ToolStripMenuItem();
            _mMenInfoSep4 = new ToolStripSeparator();
            _mMenInfoDonate = new ToolStripMenuItem();
            _mMenInfoSep5 = new ToolStripSeparator();
            _mMenInfoAbout = new ToolStripMenuItem();

            //
            // mMenInfo
            //
            DropDownItems.AddRange(new ToolStripItem[]
            {
                _mMenInfoHelp,
                _mMenKeyboardShortcuts,
                _mMenInfoSep1,
                _mMenForkGitHub,
                _mMenForkReleases,
                _mMenForkReportIssue,
                _mMenInfoSep2,
                _mMenInfoWebsite,
                _mMenInfoForum,
                _mMenInfoChat,
                _mMenInfoCommunity,
                _mMenInfoSep3,
                _mMenToolsDebugDump,
                _mMenToolsUpdate,
                _mMenInfoSep4,
                _mMenInfoDonate,
                _mMenInfoSep5,
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
            // mMenKeyboardShortcuts
            //
            _mMenKeyboardShortcuts.Name = "mMenKeyboardShortcuts";
            _mMenKeyboardShortcuts.Size = new System.Drawing.Size(190, 22);
            _mMenKeyboardShortcuts.Text = "Keyboard Shortcuts...";
            _mMenKeyboardShortcuts.Click += mMenKeyboardShortcuts_Click;
            //
            // mMenForkGitHub
            //
            _mMenForkGitHub.Name = "mMenForkGitHub";
            _mMenForkGitHub.Size = new System.Drawing.Size(190, 22);
            _mMenForkGitHub.Text = Language.MenuItem_ForkGitHub;
            _mMenForkGitHub.Click += mMenForkGitHub_Click;
            //
            // mMenForkReleases
            //
            _mMenForkReleases.Name = "mMenForkReleases";
            _mMenForkReleases.Size = new System.Drawing.Size(190, 22);
            _mMenForkReleases.Text = Language.MenuItem_ForkReleases;
            _mMenForkReleases.Click += mMenForkReleases_Click;
            //
            // mMenForkReportIssue
            //
            _mMenForkReportIssue.Name = "mMenForkReportIssue";
            _mMenForkReportIssue.Size = new System.Drawing.Size(190, 22);
            _mMenForkReportIssue.Text = Language.MenuItem_ForkReportIssue;
            _mMenForkReportIssue.Click += mMenForkReportIssue_Click;
            //
            // mMenToolsDebugDump
            //
            _mMenToolsDebugDump.Name = "mMenToolsDebugDump";
            _mMenToolsDebugDump.Size = new System.Drawing.Size(190, 22);
            _mMenToolsDebugDump.Text = "Generate Debug Bundle";
            _mMenToolsDebugDump.Click += mMenToolsDebugDump_Click;
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
            _mMenInfoWebsite.Text = Language.MenuItem_OriginalWebsite;
            _mMenInfoWebsite.Click += mMenInfoWebsite_Click;
            //
            // mMenInfoDonate
            //
            _mMenInfoDonate.Name = "mMenInfoDonate";
            _mMenInfoDonate.Size = new System.Drawing.Size(190, 22);
            _mMenInfoDonate.Text = Language.MenuItem_OriginalDonate;
            _mMenInfoDonate.Click += mMenInfoDonate_Click;
            //
            // mMenInfoForum
            //
            _mMenInfoForum.Name = "mMenInfoForum";
            _mMenInfoForum.Size = new System.Drawing.Size(190, 22);
            _mMenInfoForum.Text = Language.MenuItem_OriginalForum;
            _mMenInfoForum.Click += mMenInfoForum_Click;
            //
            // mMenInfoChat
            //
            _mMenInfoChat.Name = "mMenInfoChat";
            _mMenInfoChat.Size = new System.Drawing.Size(190, 22);
            _mMenInfoChat.Text = Language.MenuItem_OriginalChat;
            _mMenInfoChat.Click += mMenInfoChat_Click;
            //
            // mMenInfoCommunity
            //
            _mMenInfoCommunity.Name = "mMenInfoCommunity";
            _mMenInfoCommunity.Size = new System.Drawing.Size(190, 22);
            _mMenInfoCommunity.Text = Language.MenuItem_OriginalCommunity;
            _mMenInfoCommunity.Click += mMenInfoCommunity_Click;
            //
            // mMenInfoSep2
            //
            _mMenInfoSep2.Name = "mMenInfoSep2";
            _mMenInfoSep2.Size = new System.Drawing.Size(187, 6);
            //
            // mMenInfoSep3
            //
            _mMenInfoSep3.Name = "mMenInfoSep3";
            _mMenInfoSep3.Size = new System.Drawing.Size(187, 6);
            //
            // mMenInfoSep4
            //
            _mMenInfoSep4.Name = "mMenInfoSep4";
            _mMenInfoSep4.Size = new System.Drawing.Size(187, 6);
            //
            // mMenInfoSep5
            //
            _mMenInfoSep5.Name = "mMenInfoSep5";
            _mMenInfoSep5.Size = new System.Drawing.Size(187, 6);
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
            _mMenForkGitHub.Text = Language.MenuItem_ForkGitHub;
            _mMenForkReleases.Text = Language.MenuItem_ForkReleases;
            _mMenForkReportIssue.Text = Language.MenuItem_ForkReportIssue;
            _mMenInfoWebsite.Text = Language.MenuItem_OriginalWebsite;
            _mMenInfoDonate.Text = Language.MenuItem_OriginalDonate;
            _mMenInfoForum.Text = Language.MenuItem_OriginalForum;
            _mMenInfoChat.Text = Language.MenuItem_OriginalChat;
            _mMenInfoCommunity.Text = Language.MenuItem_OriginalCommunity;
            _mMenToolsDebugDump.Text = "Generate Debug Bundle";
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

        private void mMenToolsDebugDump_Click(object? sender, EventArgs e)
        {
            mRemoteNG.Tools.DebugDumper.CreateDebugBundle();
        }

        private void mMenKeyboardShortcuts_Click(object sender, EventArgs e)
        {
            AppWindows.Show(WindowType.KeyboardShortcuts);
        }

        private void mMenInfoHelp_Click(object? sender, EventArgs e) => WebHelper.GoToUrl(GeneralAppInfo.UrlDocumentation);

        private void mMenForkGitHub_Click(object? sender, EventArgs e) => OpenUrl(GeneralAppInfo.UrlForkHome);

        private void mMenForkReleases_Click(object? sender, EventArgs e) => OpenUrl(GeneralAppInfo.UrlForkReleases);

        private void mMenForkReportIssue_Click(object? sender, EventArgs e) => OpenUrl(GeneralAppInfo.UrlBugs);

        private void mMenInfoForum_Click(object? sender, EventArgs e) => OpenUrl(GeneralAppInfo.UrlForum);

        private void mMenInfoChat_Click(object? sender, EventArgs e) => OpenUrl(GeneralAppInfo.UrlChat);

        private void mMenInfoCommunity_Click(object? sender, EventArgs e) => OpenUrl(GeneralAppInfo.UrlCommunity);

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
