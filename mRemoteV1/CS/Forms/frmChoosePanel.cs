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

//using mRemoteNG.App.Runtime;

namespace mRemoteNG
{
	public partial class frmChoosePanel
	{
		public frmChoosePanel()
		{
			InitializeComponent();
		}
public string Panel
		{
			get
			{
				return cbPanels.SelectedItem.ToString();
			}
			set
			{
				cbPanels.SelectedItem = value;
			}
		}
		
		public void frmChoosePanel_Load(System.Object sender, System.EventArgs e)
		{
			ApplyLanguage();
			
			AddAvailablePanels();
		}
		
		private void ApplyLanguage()
		{
			btnOK.Text = My.Language.strButtonOK;
			lblDescription.Text = My.Language.strLabelSelectPanel;
			btnNew.Text = My.Language.strButtonNew;
			btnCancel.Text = My.Language.strButtonCancel;
			Text = My.Language.strTitleSelectPanel;
		}
		
		private void AddAvailablePanels()
		{
			cbPanels.Items.Clear();
			
			for (int i = 0; i <= WindowList.Count - 1; i++)
			{
				cbPanels.Items.Add(WindowList[i].Text.Replace("&&", "&"));
			}
			
			if (cbPanels.Items.Count > 0)
			{
				cbPanels.SelectedItem = cbPanels.Items[0];
				cbPanels.Enabled = true;
				btnOK.Enabled = true;
			}
			else
			{
				cbPanels.Enabled = false;
				btnOK.Enabled = false;
			}
		}
		
		public void btnNew_Click(System.Object sender, System.EventArgs e)
		{
			string pnlName = Interaction.InputBox(My.Language.strPanelName + ":", My.Language.strNewPanel, My.Language.strNewPanel);
			
			if (!string.IsNullOrEmpty(pnlName))
			{
				AddPanel(pnlName);
				AddAvailablePanels();
				cbPanels.SelectedItem = pnlName;
				cbPanels.Focus();
			}
		}
		
		public void btnOK_Click(System.Object sender, System.EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
		}
		
		public void btnCancel_Click(System.Object sender, System.EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		}
	}
}
