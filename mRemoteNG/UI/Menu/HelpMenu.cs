using System;
using System.Diagnostics;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Resources.Language;
using mRemoteNG.UI.Forms;

namespace mRemoteNG.UI.Menu
{
    public class HelpMenu : ToolStripMenuItem
    {
        private ToolStripMenuItem _mMenInfoHelp;
        private ToolStripMenuItem _mMenInfoWebsite;
        private ToolStripSeparator _mMenInfoSep1;
        private ToolStripMenuItem _mMenInfoAbout;
        private ToolStripMenuItem _mMenInfoDonate;
        private ToolStripMenuItem _mMenToolsUpdate;
        private ToolStripSeparator _mMenInfoSep2;
        private ToolStripMenuItem _mMenInfoBugReport;
        private ToolStripSeparator _toolStripSeparator2;
        private ToolStripMenuItem _mMenInfoForum;

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
            _mMenInfoBugReport = new ToolStripMenuItem();
            _toolStripSeparator2 = new ToolStripSeparator();
            _mMenInfoSep2 = new ToolStripSeparator();
            _mMenToolsUpdate = new ToolStripMenuItem();
            _mMenInfoAbout = new ToolStripMenuItem();

            // 
            // mMenInfo
            // 
            DropDownItems.AddRange(new ToolStripItem[]
            {
                _mMenInfoHelp,
                _mMenInfoSep1,
                _mMenInfoWebsite,
                _mMenInfoDonate,
                _mMenInfoForum,
                _mMenInfoBugReport,
                _toolStripSeparator2,
                _mMenToolsUpdate,
                _mMenInfoSep2,
                _mMenInfoAbout
            });
            Name = "mMenInfo";
            Size = new System.Drawing.Size(44, 20);
            Text = Language._Help;
            TextDirection = ToolStripTextDirection.Horizontal;
            // 
            // mMenInfoHelp
            // 
            _mMenInfoHelp.Image = Properties.Resources.Help;
            _mMenInfoHelp.Name = "mMenInfoHelp";
            _mMenInfoHelp.ShortcutKeys = Keys.F1;
            _mMenInfoHelp.Size = new System.Drawing.Size(190, 22);
            _mMenInfoHelp.Text = Language.HelpContents;
            _mMenInfoHelp.Click += mMenInfoHelp_Click;
            // 
            // mMenInfoSep1
            // 
            _mMenInfoSep1.Name = "mMenInfoSep1";
            _mMenInfoSep1.Size = new System.Drawing.Size(187, 6);
            // 
            // mMenInfoWebsite
            // 
            _mMenInfoWebsite.Image = Properties.Resources.Website;
            _mMenInfoWebsite.Name = "mMenInfoWebsite";
            _mMenInfoWebsite.Size = new System.Drawing.Size(190, 22);
            _mMenInfoWebsite.Text = Language.Website;
            _mMenInfoWebsite.Click += mMenInfoWebsite_Click;
            // 
            // mMenInfoDonate
            // 
            _mMenInfoDonate.Image = Properties.Resources.Donate;
            _mMenInfoDonate.Name = "mMenInfoDonate";
            _mMenInfoDonate.Size = new System.Drawing.Size(190, 22);
            _mMenInfoDonate.Text = Language.Donate;
            _mMenInfoDonate.Click += mMenInfoDonate_Click;
            // 
            // mMenInfoForum
            // 
            _mMenInfoForum.Image = Properties.Resources.user_comment;
            _mMenInfoForum.Name = "mMenInfoForum";
            _mMenInfoForum.Size = new System.Drawing.Size(190, 22);
            _mMenInfoForum.Text = Language.SupportForum;
            _mMenInfoForum.Click += mMenInfoForum_Click;
            // 
            // mMenInfoBugReport
            // 
            _mMenInfoBugReport.Image = Properties.Resources.Bug;
            _mMenInfoBugReport.Name = "mMenInfoBugReport";
            _mMenInfoBugReport.Size = new System.Drawing.Size(190, 22);
            _mMenInfoBugReport.Text = Language.ReportBug;
            _mMenInfoBugReport.Click += mMenInfoBugReport_Click;
            // 
            // ToolStripSeparator2
            // 
            _toolStripSeparator2.Name = "ToolStripSeparator2";
            _toolStripSeparator2.Size = new System.Drawing.Size(187, 6);
            // 
            // mMenToolsUpdate
            // 
            _mMenToolsUpdate.Image = Properties.Resources.Update;
            _mMenToolsUpdate.Name = "mMenToolsUpdate";
            _mMenToolsUpdate.Size = new System.Drawing.Size(190, 22);
            _mMenToolsUpdate.Text = Language.CheckForUpdates;
            _mMenToolsUpdate.Click += mMenToolsUpdate_Click;
            // 
            // mMenInfoSep2
            // 
            _mMenInfoSep2.Name = "mMenInfoSep2";
            _mMenInfoSep2.Size = new System.Drawing.Size(187, 6);
            // 
            // mMenInfoAbout
            // 
            _mMenInfoAbout.Image = Properties.Resources.mRemoteNG;
            _mMenInfoAbout.Name = "mMenInfoAbout";
            _mMenInfoAbout.Size = new System.Drawing.Size(190, 22);
            _mMenInfoAbout.Text = Language.About;
            _mMenInfoAbout.Click += mMenInfoAbout_Click;
        }

        public void ApplyLanguage()
        {
            Text = Language._Help;
            _mMenInfoHelp.Text = Language.HelpContents;
            _mMenInfoWebsite.Text = Language.Website;
            _mMenInfoDonate.Text = Language.Donate;
            _mMenInfoForum.Text = Language.SupportForum;
            _mMenInfoBugReport.Text = Language.ReportBug;
            _mMenToolsUpdate.Text = Language.CheckForUpdates;
            _mMenInfoAbout.Text = Language.About;
        }

        #region Info

        private void mMenToolsUpdate_Click(object sender, EventArgs e) => Windows.Show(WindowType.Update);

        private void mMenInfoHelp_Click(object sender, EventArgs e) => Process.Start(GeneralAppInfo.UrlDocumentation);

        private void mMenInfoForum_Click(object sender, EventArgs e) => Process.Start(GeneralAppInfo.UrlForum);

        private void mMenInfoBugReport_Click(object sender, EventArgs e) => Process.Start(GeneralAppInfo.UrlBugs);

        private void mMenInfoWebsite_Click(object sender, EventArgs e) => Process.Start(GeneralAppInfo.UrlHome);

        private void mMenInfoDonate_Click(object sender, EventArgs e) => Process.Start(GeneralAppInfo.UrlDonate);

        private void mMenInfoAbout_Click(object sender, EventArgs e) => FrmAbout.Instance.Show();

        #endregion
    }
}