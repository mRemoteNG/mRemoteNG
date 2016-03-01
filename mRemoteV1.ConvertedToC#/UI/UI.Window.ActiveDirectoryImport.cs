using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.App;

namespace mRemoteNG.UI
{
	namespace Window
	{
		public partial class ActiveDirectoryImport : Base
		{
			#region "Constructors"
			public ActiveDirectoryImport(DockContent panel)
			{
				Load += ADImport_Load;
				InitializeComponent();

				Runtime.FontOverride(ref this);

				WindowType = Type.ActiveDirectoryImport;
				DockPnl = panel;
			}
			#endregion

			#region "Private Methods"
			#region "Event Handlers"
			private void ADImport_Load(object sender, EventArgs e)
			{
				ApplyLanguage();

				txtDomain.Text = ActiveDirectoryTree.Domain;
				EnableDisableImportButton();
			}

			private void btnImport_Click(System.Object sender, EventArgs e)
			{
				Import.ImportFromActiveDirectory(ActiveDirectoryTree.ADPath);
				DialogResult = DialogResult.OK;
				Close();
			}

			private static void txtDomain_PreviewKeyDown(System.Object sender, PreviewKeyDownEventArgs e)
			{
				if (e.KeyCode == Keys.Enter)
					e.IsInputKey = true;
			}

			private void txtDomain_KeyDown(System.Object sender, KeyEventArgs e)
			{
				if (e.KeyCode == Keys.Enter) {
					ChangeDomain();
					e.SuppressKeyPress = true;
				}
			}

			private void btnChangeDomain_Click(System.Object sender, EventArgs e)
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
				btnImport.Text = mRemoteNG.My.Language.strButtonImport;
				lblDomain.Text = mRemoteNG.My.Language.strLabelDomain;
				btnChangeDomain.Text = mRemoteNG.My.Language.strButtonChange;
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
}
