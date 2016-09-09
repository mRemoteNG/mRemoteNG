/* 
 * http://www.csharp-examples.net/inputbox/ 
 * 
 */
using System;
using System.Windows.Forms;
using System.Drawing;

namespace mRemoteNG.UI.Forms.Input
{
    internal static class input
    {
        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            var form = new Form();
            var label = new Label();
            var textBox = new TextBox();
            var buttonOk = new Button();
            var buttonCancel = new Button();

            label.Text = promptText;
            label.AutoSize = true;
            label.SetBounds(9, 20, 372, 13);

            textBox.Text = value;
            textBox.BorderStyle = BorderStyle.Fixed3D;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            textBox.SetBounds(12, 36, 372, 20);

            buttonOk.Text = "OK";
            buttonOk.DialogResult = DialogResult.OK;
            buttonOk.FlatStyle = FlatStyle.Flat;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonOk.SetBounds(228, 72, 75, 23);

            buttonCancel.Text = "Cancel";
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

            var dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }
    }
}

