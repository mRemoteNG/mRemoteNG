using mRemoteNG.Themes;
using WeifenLuo.WinFormsUI.Docking;
using System.Runtime.Versioning;
using System.Windows.Forms;
using System.ComponentModel;

namespace mRemoteNG.UI.Window
{
    [SupportedOSPlatform("windows")]
    [DesignerCategory("Form")]
    public partial class BaseWindow
    {
        #region Private Variables

        private ThemeManager _themeManager;

        #endregion

        #region Public Properties

        protected WindowType WindowType { get; set; }

        protected DockContent DockPnl { get; set; }

        #endregion

        #region Public Methods

        public BaseWindow()
        {
            InitializeComponent();
        }

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

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion

        internal void ApplyTheme()
        {
            _themeManager = ThemeManager.getInstance();
            if (!_themeManager.ActiveAndExtended) return;
            BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
        }

    }
}