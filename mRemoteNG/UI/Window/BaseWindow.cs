using mRemoteNG.Themes;
using WeifenLuo.WinFormsUI.Docking;
using System.Runtime.Versioning;

// ReSharper disable UnusedAutoPropertyAccessor.Global


namespace mRemoteNG.UI.Window
{
    [SupportedOSPlatform("windows")]
    public class BaseWindow : DockContent
    {
        #region Private Variables

        //private WindowType _WindowType;
        //private DockContent _DockPnl;
        private ThemeManager _themeManager;

        #endregion

        #region Public Properties

        protected WindowType WindowType { get; set; }

        protected DockContent DockPnl { get; set; }

        #endregion

        #region Public Methods

        public void SetFormText(string t)
        {
            Text = t;
            TabText = t;
        }

        #endregion

        internal void ApplyTheme()
        {
            _themeManager = ThemeManager.getInstance();
            if (!_themeManager.ActiveAndExtended) return;
            BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
        }


        #region Private Methods

        /*
                private void Base_Load(object sender, EventArgs e)
                {
                    FrmMain.Default.ShowHidePanelTabs();
                }
        */

        /*
                private void Base_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
                {
                    FrmMain.Default.ShowHidePanelTabs(this);
                }
        */

        #endregion

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // BaseWindow
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular,
                                                System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "BaseWindow";
            this.ResumeLayout(false);
        }
    }
}