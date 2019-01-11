using System.Windows.Forms;
using mRemoteNG.Themes;

namespace mRemoteNG.UI.Forms.Input
{
    public sealed partial class FrmInputBox : Form
    {
        internal string returnValue;

        public FrmInputBox(string title, string promptText, string value)
        {
            InitializeComponent();

            Text = title;
            label.Text = promptText;
            textBox.Text = value;
            ApplyLanguage();
            ApplyTheme();
        }

        private void ApplyLanguage()
        {
            buttonOk.Text = Language.strButtonOK;
            buttonCancel.Text = Language.strButtonCancel;
        }

        private void ApplyTheme()
        {
            var activeTheme = ThemeManager.getInstance().ActiveTheme;
            if (!activeTheme.IsExtendable) return;
            BackColor = activeTheme.ExtendedPalette.getColor("Dialog_Background");
            ForeColor = activeTheme.ExtendedPalette.getColor("Dialog_Foreground");
        }

        private void buttonOk_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.OK;
            returnValue = textBox.Text;
            Close();
        }

        private void buttonCancel_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
