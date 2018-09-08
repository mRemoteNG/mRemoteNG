using System.Windows.Forms;
using mRemoteNG.Themes;

namespace mRemoteNG.UI.Forms.Input
{
    public partial class InputBox : Form
    {
        private readonly DisplayProperties _display = new DisplayProperties();

        public InputBox()
        {
            InitializeComponent();
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
            BackColor = activeTheme.ExtendedPalette.getColor("Dialog_Background");
            ForeColor = activeTheme.ExtendedPalette.getColor("Dialog_Foreground");
        }

        public DialogResult ShowAsDialog(string title, string promptText, ref string value)
        {
            Text = title;
            label.Text = promptText;
            textBox.Text = value;
            var dialogResult = ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }
    }
}
