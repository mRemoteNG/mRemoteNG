// VBConversions Note: VB project level imports
using System.Collections.Generic;
using System;
using AxWFICALib;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using AxMSTSCLib;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
// End of VB project level imports

using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.App;


namespace mRemoteNG.UI.Window
{
	public partial class ActiveDirectoryImport : Base
			{
#region Constructors
				public ActiveDirectoryImport(DockContent panel)
				{
					InitializeComponent();
					
					Runtime.FontOverride(this);
					
					WindowType = Type.ActiveDirectoryImport;
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
				
				public void btnImport_Click(System.Object sender, EventArgs e)
				{
					Import.ImportFromActiveDirectory(ActiveDirectoryTree.ADPath);
					DialogResult = DialogResult.OK;
					Close();
				}
				
				static public void txtDomain_PreviewKeyDown(System.Object sender, PreviewKeyDownEventArgs e)
				{
					if (e.KeyCode == Keys.Enter)
					{
						e.IsInputKey = true;
					}
				}
				
				public void txtDomain_KeyDown(System.Object sender, KeyEventArgs e)
				{
					if (e.KeyCode == Keys.Enter)
					{
						ChangeDomain();
						e.SuppressKeyPress = true;
					}
				}
				
				public void btnChangeDomain_Click(System.Object sender, EventArgs e)
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
					btnImport.Text = My.Language.strButtonImport;
					lblDomain.Text = My.Language.strLabelDomain;
					btnChangeDomain.Text = My.Language.strButtonChange;
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
