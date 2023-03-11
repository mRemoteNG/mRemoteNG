using System;
using System.Diagnostics;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.UI.Forms;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Menu
{
    [SupportedOSPlatform("windows")]
    public class HelpMenu : ToolStripMenuItem
    {
        private ToolStripMenuItem _mMenInfoHelp;
        private ToolStripMenuItem _mMenInfoWebsite;
        private ToolStripSeparator _mMenInfoSep1;
        private ToolStripMenuItem _mMenInfoAbout;
        private ToolStripMenuItem _mMenInfoDonate;
        private ToolStripSeparator _mMenInfoSep2;
        private ToolStripSeparator _mMenInfoSep3;
        private ToolStripSeparator _mMenInfoSep4;
        private ToolStripMenuItem _mMenInfoForum;
        private ToolStripMenuItem _mMenToolsUpdate;

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
            _mMenInfoHelp.Text = Language.HelpContents;
            _mMenInfoHelp.Click += mMenInfoHelp_Click;
            // 
            // mMenToolsUpdate
            // 
            _mMenToolsUpdate.Image = Properties.Resources.RunUpdate_16x;
            _mMenToolsUpdate.Name = "mMenToolsUpdate";
            _mMenToolsUpdate.Size = new System.Drawing.Size(190, 22);
            _mMenToolsUpdate.Text = Language.CheckForUpdates;
            _mMenToolsUpdate.Click += mMenToolsUpdate_Click;
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
            _mMenInfoWebsite.Text = Language.Website;
            _mMenInfoWebsite.Click += mMenInfoWebsite_Click;
            // 
            // mMenInfoDonate
            // 
            _mMenInfoDonate.Name = "mMenInfoDonate";
            _mMenInfoDonate.Size = new System.Drawing.Size(190, 22);
            _mMenInfoDonate.Text = Language.Donate;
            _mMenInfoDonate.Click += mMenInfoDonate_Click;
            // 
            // mMenInfoForum
            // 
            _mMenInfoForum.Name = "mMenInfoForum";
            _mMenInfoForum.Size = new System.Drawing.Size(190, 22);
            _mMenInfoForum.Text = Language.SupportForum;
            _mMenInfoForum.Click += mMenInfoForum_Click;
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
            _mMenInfoAbout.Text = Language.About;
            _mMenToolsUpdate.Text = Language.CheckForUpdates;
        }

        #region Info

        private void mMenToolsUpdate_Click(object sender, EventArgs e) => Windows.Show(WindowType.Update);

        private void mMenInfoHelp_Click(object sender, EventArgs e) => Process.Start("explorer.exe", GeneralAppInfo.UrlDocumentation);

        private void mMenInfoForum_Click(object sender, EventArgs e) => Process.Start("explorer.exe", GeneralAppInfo.UrlForum);

        private void mMenInfoWebsite_Click(object sender, EventArgs e) => Process.Start("explorer.exe", GeneralAppInfo.UrlHome);

        private void mMenInfoDonate_Click(object sender, EventArgs e) => Process.Start("explorer.exe", GeneralAppInfo.UrlDonate);

        private void mMenInfoAbout_Click(object sender, EventArgs e) => frmAbout.Instance.Show();

        #endregion
    }
}