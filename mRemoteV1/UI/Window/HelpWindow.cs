using mRemoteNG.App.Info;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using Gecko;
using WeifenLuo.WinFormsUI.Docking;


namespace mRemoteNG.UI.Window
{
    public class HelpWindow : BaseWindow
    {
        private GeckoWebBrowser geckoWebBrowser;
        #region Form Init

        private void InitializeComponent()
        {
            this.geckoWebBrowser = new Gecko.GeckoWebBrowser();
            this.SuspendLayout();
            // 
            // geckoWebBrowser
            // 
            this.geckoWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.geckoWebBrowser.FrameEventsPropagateToMainWindow = false;
            this.geckoWebBrowser.Location = new System.Drawing.Point(0, 0);
            this.geckoWebBrowser.Name = "geckoWebBrowser";
            this.geckoWebBrowser.Size = new System.Drawing.Size(542, 323);
            this.geckoWebBrowser.TabIndex = 0;
            this.geckoWebBrowser.UseHttpActivityObserver = false;
            this.geckoWebBrowser.DomClick += new System.EventHandler<Gecko.DomMouseEventArgs>(this.LinkClicked);
            // 
            // HelpWindow
            // 
            this.ClientSize = new System.Drawing.Size(542, 323);
            this.Controls.Add(this.geckoWebBrowser);
            this.Icon = global::mRemoteNG.Resources.Help_Icon;
            this.Name = "HelpWindow";
            this.TabText = "Help";
            this.Text = "Help";
            this.Load += new System.EventHandler(this.HelpWindow_Load);
            this.ResumeLayout(false);

        }

        #endregion

        #region Public Methods

        public HelpWindow()
        {
            WindowType = WindowType.Help;
            DockPnl = new DockContent();
            if (!Xpcom.IsInitialized)
                Xpcom.Initialize("Firefox");
            InitializeComponent();
        }

        #endregion

        #region Private Methods

        private void HelpWindow_Load(object sender, EventArgs e) => geckoWebBrowser.Navigate(GeneralAppInfo.HomePath + @"\Help\index.html");

        private void LinkClicked(object sender, DomMouseEventArgs e)
        {
            var url = ((GeckoWebBrowser) sender).StatusText;
            if (url.StartsWith("file://"))
            {
                geckoWebBrowser.Navigate(url);
                e.Handled = true;
                return;
            }
            Process.Start(url);
            e.Handled = true;
        }

        #endregion
    }
}