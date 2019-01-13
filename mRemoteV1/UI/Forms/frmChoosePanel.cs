using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.UI.Forms.Input;
using mRemoteNG.UI.Panels;

namespace mRemoteNG.UI.Forms
{
	public partial class FrmChoosePanel
	{
	    private readonly PanelAdder _panelAdder;

		public FrmChoosePanel()
		{
			InitializeComponent();
            _panelAdder = new PanelAdder();
		}
        public string Panel
		{
			get => cbPanels.SelectedItem.ToString();
            set => cbPanels.SelectedItem = value;
        }

	    private void frmChoosePanel_Load(object sender, System.EventArgs e)
		{
			ApplyLanguage();
		    ApplyTheme();
		    AddAvailablePanels();
		}
		
		private void ApplyLanguage()
		{
			btnOK.Text = Language.strButtonOK;
			lblDescription.Text = Language.strLabelSelectPanel;
			btnNew.Text = Language.strButtonNew;
			Text = Language.strTitleSelectPanel;
		}

	    private void ApplyTheme()
	    {
            if (!Themes.ThemeManager.getInstance().ThemingActive) return;
            BackColor = Themes.ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            ForeColor = Themes.ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
            lblDescription.BackColor = Themes.ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            lblDescription.ForeColor = Themes.ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
        }

        private void AddAvailablePanels()
		{
			cbPanels.Items.Clear();
			
			for (var i = 0; i <= Runtime.WindowList.Count - 1; i++)
			{
                cbPanels.Items.Add(Runtime.WindowList[i].Text.Replace("&&", "&"));
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

	    private void btnNew_Click(object sender, System.EventArgs e)
		{
		    var pnlName = Language.strNewPanel;
            using (var frmInputBox = new FrmInputBox(Language.strNewPanel, Language.strPanelName + ":", ref pnlName))
            {
                var dr = frmInputBox.ShowDialog();
                if (dr != DialogResult.OK || string.IsNullOrEmpty(frmInputBox.returnValue)) return;
                _panelAdder.AddPanel(frmInputBox.returnValue);
                AddAvailablePanels();
                cbPanels.SelectedItem = frmInputBox.returnValue;
                cbPanels.Focus();
            }
		}

	    private void btnOK_Click(object sender, System.EventArgs e)
		{
            DialogResult = DialogResult.OK;
		}
	}
}
