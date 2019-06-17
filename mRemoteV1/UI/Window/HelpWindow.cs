using mRemoteNG.App.Info;
using System;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;


namespace mRemoteNG.UI.Window
{
    public class HelpWindow : BaseWindow
    {
        #region Form Init
        private WebBrowser _wbHelp;

        private void InitializeComponent()
        {
            this._wbHelp = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // wbHelp
            // 
            this._wbHelp.Dock = System.Windows.Forms.DockStyle.Fill;
            this._wbHelp.Location = new System.Drawing.Point(0, 0);
            this._wbHelp.MinimumSize = new System.Drawing.Size(20, 20);
            this._wbHelp.Name = "_wbHelp";
            this._wbHelp.ScriptErrorsSuppressed = true;
            this._wbHelp.Size = new System.Drawing.Size(542, 323);
            this._wbHelp.TabIndex = 1;
            // 
            // HelpWindow
            // 
            this.ClientSize = new System.Drawing.Size(542, 323);
            this.Controls.Add(this._wbHelp);
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
            InitializeComponent();
        }

        #endregion

        #region Private Methods

        private void HelpWindow_Load(object sender, EventArgs e)
        {
            _wbHelp.Navigate(GeneralAppInfo.HomePath + @"\Help\index.html");
        }

        #endregion
    }
}