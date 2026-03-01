using System;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Themes;
using mRemoteNG.UI.Panels;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Forms
{
    [SupportedOSPlatform("windows")]
    public partial class FrmChoosePanel
    {


        public FrmChoosePanel()
        {
            InitializeComponent();
            Icon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.Panel_16x);
        }

        public string Panel
        {
            get => cbPanels.SelectedItem?.ToString() ?? string.Empty;
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
            btnOK.Text = Language._Ok;
            lblDescription.Text = Language.SelectPanel;
            btnNew.Text = Language._New;
            Text = Language.TitleSelectPanel;
        }

        private void ApplyTheme()
        {
            if (!ThemeManager.getInstance().ActiveAndExtended) return;
            var palette = ThemeManager.getInstance().ActiveTheme.ExtendedPalette;
            if (palette == null) return;
            BackColor = palette.getColor("Dialog_Background");
            ForeColor = palette.getColor("Dialog_Foreground");
            lblDescription.BackColor = palette.getColor("Dialog_Background");
            lblDescription.ForeColor = palette.getColor("Dialog_Foreground");
        }

        private void AddAvailablePanels()
        {
            cbPanels.Items.Clear();

            for (int i = 0; i <= Runtime.WindowList.Count - 1; i++)
            {
                var window = Runtime.WindowList[i];
                if (window is null) continue;
                cbPanels.Items.Add(window.Text.Replace("&&", "&", StringComparison.Ordinal));
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
            using (FrmInputBox frmInputBox =
                new(Language.NewPanel, Language.PanelName + ":", Language.NewPanel))
            {
                DialogResult dr = frmInputBox.ShowDialog();
                if (dr != DialogResult.OK || string.IsNullOrEmpty(frmInputBox.returnValue)) return;
                PanelAdder.AddPanel(frmInputBox.returnValue);
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