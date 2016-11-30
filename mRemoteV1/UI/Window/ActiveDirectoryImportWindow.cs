using System;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.App;
using mRemoteNG.Container;


namespace mRemoteNG.UI.Window
{
	public partial class ActiveDirectoryImportWindow
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

	    private void ADImport_Load(object sender, EventArgs e)
		{
			ApplyLanguage();
			txtDomain.Text = ActiveDirectoryTree.Domain;
			EnableDisableImportButton();
		}

	    private void btnImport_Click(object sender, EventArgs e)
		{
		    var selectedNode = Windows.TreeForm.SelectedNode;
		    ContainerInfo importDestination;
		    if (selectedNode != null)
		        importDestination = selectedNode as ContainerInfo ?? selectedNode.Parent;
		    else
		        importDestination = Runtime.ConnectionTreeModel.RootNodes.First();

		    Import.ImportFromActiveDirectory(ActiveDirectoryTree.ADPath, importDestination, chkSubOU.Checked);
		}

	    private static void txtDomain_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				e.IsInputKey = true;
		}

	    private void txtDomain_KeyDown(object sender, KeyEventArgs e)
		{
		    if (e.KeyCode != Keys.Enter) return;
		    ChangeDomain();
		    e.SuppressKeyPress = true;
		}

	    private void btnChangeDomain_Click(object sender, EventArgs e)
		{
			ChangeDomain();
		}

	    private void ActiveDirectoryTree_ADPathChanged(object sender)
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}