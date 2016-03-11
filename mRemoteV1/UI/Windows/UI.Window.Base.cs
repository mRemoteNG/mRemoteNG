using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;


namespace mRemoteNG.UI.Window
{
	public class Base : DockContent
    {
        #region Private Variables
        private Type _WindowType;
        private DockContent _DockPnl;
        #endregion

        #region Constructors
        public Base()
		{
			//InitializeComponent();
		}
        #endregion

        #region Public Properties
        public Type WindowType
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
		private void Base_Load(System.Object sender, System.EventArgs e)
		{
			frmMain.Default.ShowHidePanelTabs();
		}
				
		private void Base_FormClosed(System.Object sender, System.Windows.Forms.FormClosedEventArgs e)
		{
			frmMain.Default.ShowHidePanelTabs(this);
		}
        #endregion
	}
}