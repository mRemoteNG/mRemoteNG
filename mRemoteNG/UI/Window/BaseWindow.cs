using mRemoteNG.Themes;
using WeifenLuo.WinFormsUI.Docking;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.Messages;
using mRemoteNG.UI.Window;

namespace mRemoteNG.UI.Window
{
    [SupportedOSPlatform("windows")]
    public class BaseWindow : DockContent
    {
        #region Private Variables

        //private WindowType _WindowType;
        //private DockContent _DockPnl;
        private ThemeManager? _themeManager;

        #endregion

        #region Public Properties

        protected WindowType WindowType { get; set; }

        protected DockContent? DockPnl { get; set; }

        #endregion

        #region Public Methods

        public void SetFormText(string t)
        {
            Text = t;
            TabText = t;
        }

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            // Handle Ctrl+Tab and Ctrl+PgDn to navigate to next tab
            if (keyData == (Keys.Control | Keys.Tab) || keyData == (Keys.Control | Keys.PageDown))
            {
                if (this is ConnectionWindow connectionWindow)
                {
                    connectionWindow.NavigateToNextTab();
                    return true;
                }
            }

            // Handle Ctrl+Shift+Tab and Ctrl+PgUp to navigate to previous tab
            if (keyData == (Keys.Control | Keys.Shift | Keys.Tab) || keyData == (Keys.Control | Keys.PageUp))
            {
                if (this is ConnectionWindow connectionWindow)
                {
                    connectionWindow.NavigateToPreviousTab();
                    return true;
                }
            }

            // Handle Ctrl+F to find in session output (SSH/Telnet)
            if (keyData == (Keys.Control | Keys.F))
            {
                if (this is ConnectionWindow connectionWindow)
                {
                    connectionWindow.FindInSession();
                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion

        internal void ApplyTheme()
        {
            _themeManager = ThemeManager.getInstance();
            if (!_themeManager.ActiveAndExtended) return;
            BackColor = _themeManager.ActiveTheme.ExtendedPalette?.getColor("Dialog_Background") ?? BackColor;
            ForeColor = _themeManager.ActiveTheme.ExtendedPalette?.getColor("Dialog_Foreground") ?? ForeColor;
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
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "BaseWindow";
            this.ResumeLayout(false);
        }
    }
}