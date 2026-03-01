using System.Windows.Forms;
using mRemoteNG.Themes;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Forms
{
    [SupportedOSPlatform("windows")]
    public sealed partial class FrmInputBox : Form
    {
        internal string? returnValue;

        public FrmInputBox(string title, string promptText, string value)
        {
            InitializeComponent();

            Text = title;
            label.Text = promptText;
            textBox.Text = value;
            ApplyLanguage();
            ApplyTheme();
        }

        public FrmInputBox(string title, string promptText, string value, bool isPassword)
            : this(title, promptText, value)
        {
            if (isPassword)
            {
                textBox.UseSystemPasswordChar = true;
            }
        }

        private void ApplyLanguage()
        {
            _Ok.Text = Language._Ok;
            buttonCancel.Text = Language._Cancel;
        }

        private void ApplyTheme()
        {
            ThemeManager _themeManager = ThemeManager.getInstance();
            if (!_themeManager.ActiveAndExtended) return;
            BackColor = _themeManager.ActiveTheme.ExtendedPalette?.getColor("Dialog_Background") ?? BackColor;
            ForeColor = _themeManager.ActiveTheme.ExtendedPalette?.getColor("Dialog_Foreground") ?? ForeColor;
        }

        private void _Ok_Click(object sender, System.EventArgs e)
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