using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Controls.ConnectionInfoPropertyGrid
{
    public class PasswordRevealEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext? context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object? EditValue(ITypeDescriptorContext? context, IServiceProvider provider, object? value)
        {
            if (provider?.GetService(typeof(IWindowsFormsEditorService)) is not IWindowsFormsEditorService editorService)
                return value;

            using var passwordDialog = new PasswordRevealDialog(value as string ?? string.Empty);
            return editorService.ShowDialog(passwordDialog) == DialogResult.OK
                ? passwordDialog.Password
                : value;
        }

        private sealed class PasswordRevealDialog : Form
        {
            private readonly TextBox _passwordTextBox;

            public string Password => _passwordTextBox.Text;

            public PasswordRevealDialog(string password)
            {
                Text = Language.TitlePassword;
                FormBorderStyle = FormBorderStyle.FixedDialog;
                StartPosition = FormStartPosition.CenterParent;
                MaximizeBox = false;
                MinimizeBox = false;
                ShowInTaskbar = false;
                ClientSize = new Size(360, 130);

                var passwordLabel = new Label
                {
                    AutoSize = true,
                    Location = new Point(12, 12),
                    Text = Language.Password
                };

                _passwordTextBox = new TextBox
                {
                    Location = new Point(12, 32),
                    Width = 336,
                    Text = password,
                    UseSystemPasswordChar = true
                };

                var showPasswordCheckBox = new CheckBox
                {
                    AutoSize = true,
                    Location = new Point(12, 60),
                    Text = Language.ShowText
                };
                showPasswordCheckBox.CheckedChanged += (_, _) =>
                    _passwordTextBox.UseSystemPasswordChar = !showPasswordCheckBox.Checked;

                var okButton = new Button
                {
                    DialogResult = DialogResult.OK,
                    Location = new Point(192, 92),
                    Size = new Size(75, 23),
                    Text = "OK"
                };

                var cancelButton = new Button
                {
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(273, 92),
                    Size = new Size(75, 23),
                    Text = "Cancel"
                };

                AcceptButton = okButton;
                CancelButton = cancelButton;

                Controls.Add(passwordLabel);
                Controls.Add(_passwordTextBox);
                Controls.Add(showPasswordCheckBox);
                Controls.Add(okButton);
                Controls.Add(cancelButton);
            }
        }
    }
}
