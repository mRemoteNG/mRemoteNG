using WeifenLuo.WinFormsUI.Docking;
// ReSharper disable UnusedAutoPropertyAccessor.Global


namespace mRemoteNG.UI.Window
{
	public class BaseWindow : DockContent
    {
        #region Private Variables

        #endregion

        #region Constructors
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
        /*
		private void Base_Load(object sender, EventArgs e)
		{
			FrmMain.Default.ShowHidePanelTabs();
		}
        
		private void Base_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
		{
			FrmMain.Default.ShowHidePanelTabs(this);
		}
        */
        #endregion
    }
}