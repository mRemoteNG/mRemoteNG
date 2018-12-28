﻿using System.Windows.Forms;
using mRemoteNG.Themes;

namespace mRemoteNG.UI.Forms.Input
{
    public partial class FrmInputBox : Form
    {
        private readonly DisplayProperties _display = new DisplayProperties();
        internal string returnValue;

        public FrmInputBox(string title, string promptText, ref string value)
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
