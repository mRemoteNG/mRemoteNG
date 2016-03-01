using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using mRemoteNG.App.Runtime;
namespace mRemoteNG
{

	public partial class frmChoosePanel
	{
		public string Panel {
			get { return cbPanels.SelectedItem.ToString(); }
			set { cbPanels.SelectedItem = value; }
		}

		private void frmChoosePanel_Load(System.Object sender, System.EventArgs e)
		{
			ApplyLanguage();

			AddAvailablePanels();
		}

		private void ApplyLanguage()
		{
			btnOK.Text = mRemoteNG.My.Language.strButtonOK;
			lblDescription.Text = mRemoteNG.My.Language.strLabelSelectPanel;
			btnNew.Text = mRemoteNG.My.Language.strButtonNew;
			btnCancel.Text = mRemoteNG.My.Language.strButtonCancel;
			Text = mRemoteNG.My.Language.strTitleSelectPanel;
		}

		private void AddAvailablePanels()
		{
			cbPanels.Items.Clear();

			for (int i = 0; i <= mRemoteNG.App.Runtime.WindowList.Count - 1; i++) {
				cbPanels.Items.Add(mRemoteNG.App.Runtime.WindowList[i].Text.Replace("&&", "&"));
			}

			if (cbPanels.Items.Count > 0) {
				cbPanels.SelectedItem = cbPanels.Items[0];
				cbPanels.Enabled = true;
				btnOK.Enabled = true;
			} else {
				cbPanels.Enabled = false;
				btnOK.Enabled = false;
			}
		}

		private void btnNew_Click(System.Object sender, System.EventArgs e)
		{
			string pnlName = Interaction.InputBox(mRemoteNG.My.Language.strPanelName + ":", mRemoteNG.My.Language.strNewPanel, mRemoteNG.My.Language.strNewPanel);

			if (!string.IsNullOrEmpty(pnlName)) {
				mRemoteNG.App.Runtime.AddPanel(pnlName);
				AddAvailablePanels();
				cbPanels.SelectedItem = pnlName;
				cbPanels.Focus();
			}
		}

		private void btnOK_Click(System.Object sender, System.EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
		}

		private void btnCancel_Click(System.Object sender, System.EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		}
		public frmChoosePanel()
		{
			Load += frmChoosePanel_Load;
			InitializeComponent();
		}
	}
}
