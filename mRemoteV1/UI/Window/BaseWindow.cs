using mRemoteNG.Themes;
using mRemoteNG.UI.Forms;
using System.Drawing;
using WeifenLuo.WinFormsUI.Docking;
// ReSharper disable UnusedAutoPropertyAccessor.Global


namespace mRemoteNG.UI.Window
{
	public class BaseWindow : DockContent
    {
        #region Private Variables
        private WindowType _WindowType;
        private DockContent _DockPnl;
        private ThemeManager _themeManager;
        #endregion

        #region Constructors
        public BaseWindow()
		{
            //InitializeComponent();
        }
        #endregion

        #region Public Properties

        protected WindowType WindowType { get; set; }

        protected DockContent DockPnl { get; set; }

        #endregion
				
        #region Public Methods
		public void SetFormText(string t)
		{
			this.Text = t;
			this.TabText = t;
		}
        #endregion

        #region Private Methods 
        public new void ApplyTheme()
        {

            if (!Tools.DesignModeTest.IsInDesignMode(this))
            {
                _themeManager = ThemeManager.getInstance();
                if (_themeManager.ThemingActive)
                {
                    this.BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
                    this.ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
                }
            }
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
	}
}