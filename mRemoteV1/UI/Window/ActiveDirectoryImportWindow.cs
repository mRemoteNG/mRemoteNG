using System;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.App;
using mRemoteNG.My;


namespace mRemoteNG.UI.Window
{
	public partial class ActiveDirectoryImportWindow : BaseWindow
	{
        #region Constructors
		public ActiveDirectoryImportWindow(DockContent panel)
		{
			InitializeComponent();
			Runtime.FontOverride(this);
			WindowType = WindowType.ActiveDirectoryImport;
			DockPnl = panel;
		}
        #endregion
				
        #region Private Methods
        #region Event Handlers
		public void ADImport_Load(object sender, EventArgs e)
		{
			ApplyLanguage();
			txtDomain.Text = ActiveDirectoryTree.Domain;
			EnableDisableImportButton();
		}
				
		public void btnImport_Click(Object sender, EventArgs e)
		{
			Import.ImportFromActiveDirectory(ActiveDirectoryTree.ADPath);
			DialogResult = DialogResult.OK;
			Close();
		}
				
		static public void txtDomain_PreviewKeyDown(Object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				e.IsInputKey = true;
			}
		}
				
		public void txtDomain_KeyDown(Object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				ChangeDomain();
				e.SuppressKeyPress = true;
			}
		}
				
		public void btnChangeDomain_Click(Object sender, EventArgs e)
		{
			ChangeDomain();
		}
				
		public void ActiveDirectoryTree_ADPathChanged(object sender)
		{
			EnableDisableImportButton();
		}
        #endregion
				
		private void ApplyLanguage()
		{
			btnImport.Text = Language.strButtonImport;
			lblDomain.Text = Language.strLabelDomain;
			btnChangeDomain.Text = Language.strButtonChange;
		}
				
		private void ChangeDomain()
		{
			ActiveDirectoryTree.Domain = txtDomain.Text;
			ActiveDirectoryTree.Refresh();
		}
				
		private void EnableDisableImportButton()
		{
			btnImport.Enabled = !string.IsNullOrEmpty(ActiveDirectoryTree.ADPath);
		}
        #endregion
	}
}