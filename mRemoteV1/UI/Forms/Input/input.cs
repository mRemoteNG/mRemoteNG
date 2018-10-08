/* 
 * http://www.csharp-examples.net/inputbox/ 
 * 
 */
using System;
using System.Windows.Forms;
using System.Drawing;
using mRemoteNG.UI.Controls.Base;

namespace mRemoteNG.UI.Forms.Input
{
    internal static class input
    {
        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            var form = new Form();
            var label = new NGLabel();
            var textBox = new NGTextBox();
            var buttonOk = new NGButton();
            var buttonCancel = new NGButton();

            label.Text = promptText;
            label.AutoSize = true;
            label.SetBounds(9, 20, 372, 13);

            textBox.Text = value;
            textBox.BorderStyle = BorderStyle.Fixed3D;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            textBox.SetBounds(12, 36, 372, 20);

            buttonOk.Text = Language.strButtonOK;
            buttonOk.DialogResult = DialogResult.OK;
            buttonOk.FlatStyle = FlatStyle.Flat;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonOk.SetBounds(228, 72, 75, 23);

            buttonCancel.Text = Language.strButtonCancel;
            buttonCancel.DialogResult = DialogResult.Cancel;
            buttonCancel.FlatStyle = FlatStyle.Flat;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.SetBounds(309, 72, 75, 23);

            form.Text = title;
            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] {label, textBox, buttonOk, buttonCancel});
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;
            form.BackColor = Themes.ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            form.ForeColor = Themes.ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");

            var dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }
    }
}

