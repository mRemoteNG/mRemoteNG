using System.Runtime.Versioning;
using System.Windows.Forms;

namespace mRemoteNG.UI
{
    [SupportedOSPlatform("windows")]
    internal static class InputDialog
    {
        /// <summary>
        /// Shows a simple input dialog and returns the entered text, or null if cancelled.
        /// </summary>
        public static string Prompt(string title, string prompt, string defaultValue = "")
        {
            using var form = new Form
            {
                Text = title,
                Width = 350,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var lbl = new Label { Left = 10, Top = 15, Width = 310, Text = prompt };
            var txt = new TextBox { Left = 10, Top = 40, Width = 310, Text = defaultValue };
            var btnOk = new Button { Text = "OK", Left = 160, Top = 75, Width = 75, DialogResult = DialogResult.OK };
            var btnCancel = new Button { Text = "Cancel", Left = 245, Top = 75, Width = 75, DialogResult = DialogResult.Cancel };

            form.Controls.AddRange(new Control[] { lbl, txt, btnOk, btnCancel });
            form.AcceptButton = btnOk;
            form.CancelButton = btnCancel;

            return form.ShowDialog() == DialogResult.OK ? txt.Text : null;
        }
    }
}
