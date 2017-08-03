using mRemoteNG.Themes;
using mRemoteNG.UI.Forms;
using System.Drawing;
using WeifenLuo.WinFormsUI.Docking;


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
        public WindowType WindowType
		{
			get { return this._WindowType; }
			set { this._WindowType = value; }
		}
		
        public DockContent DockPnl
		{
			get { return this._DockPnl; }
			set { this._DockPnl = value; }
		}
        #endregion
				
        #region Public Methods
		public void SetFormText(string Text)
		{
			this.Text = Text;
			this.TabText = Text;
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

        private void Base_Load(System.Object sender, System.EventArgs e)
		{ 
            FrmMain.Default.ShowHidePanelTabs();
		}
				
		private void Base_FormClosed(System.Object sender, System.Windows.Forms.FormClosedEventArgs e)
		{
			FrmMain.Default.ShowHidePanelTabs(this);
		}
        #endregion
	}
}