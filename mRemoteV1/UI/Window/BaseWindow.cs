using System;
using System.Windows.Forms;
using mRemoteNG.UI.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.UI.Window
{
    public class BaseWindow : DockContent
    {
        #region Constructors

        #endregion

        #region Public Methods

        public void SetFormText(string Text)
        {
            this.Text = Text;
            TabText = Text;
        }

        #endregion

        #region Private Variables

        #endregion

        #region Public Properties

        public WindowType WindowType { get; set; }

        public DockContent DockPnl { get; set; }

        #endregion

        #region Private Methods

        private void Base_Load(object sender, EventArgs e)
        {
            frmMain.Default.ShowHidePanelTabs();
        }

        private void Base_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmMain.Default.ShowHidePanelTabs(this);
        }

        #endregion
    }
}