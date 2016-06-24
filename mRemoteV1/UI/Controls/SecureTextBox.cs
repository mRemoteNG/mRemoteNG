using System.Security;
using System.Windows.Forms;

namespace mRemoteNG.UI.Controls
{
    public partial class SecureTextBox : TextBox
    {
        public SecureString SecureString { get; } = new SecureString();

        public SecureTextBox()
        {
            InitializeComponent();
            KeyPress += SecureTextBox_KeyPress;
            KeyDown += SecureTextBox_KeyDown;
        }

        private void SecureTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b')
                ProcessBackspace();
            else
                ProcessNewCharacter(e.KeyChar);

            e.Handled = true;
        }

        private void ProcessBackspace()
        {
            if (SelectionLength > 0)
            {
                RemoveSelectedCharacters();
                ResetDisplayCharacters(SelectionStart);
            }
            else if (SelectionStart > 0)
            {
                SecureString.RemoveAt(SelectionStart - 1);
                ResetDisplayCharacters(SelectionStart - 1);
            }
        }

        private void ProcessNewCharacter(char character)
        {
            if (SelectionLength > 0)
            {
                RemoveSelectedCharacters();
            }

            SecureString.InsertAt(SelectionStart, character);
            ResetDisplayCharacters(SelectionStart + 1);
        }

        private void RemoveSelectedCharacters()
        {
            for (var i = 0; i < SelectionLength; i++)
            {
                SecureString.RemoveAt(SelectionStart);
            }
        }

        private void ResetDisplayCharacters(int caretPosition)
        {
            Text = new string(PasswordChar, SecureString.Length);
            SelectionStart = caretPosition;
        }

        private void SecureTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                ProcessDelete();
                e.Handled = true;
            }
            else if (IsIgnorableKey(e.KeyCode))
            {
                e.Handled = true;
            }
        }

        private void ProcessDelete()
        {
            if (SelectionLength > 0)
                RemoveSelectedCharacters();
            else if (SelectionStart < Text.Length)
                SecureString.RemoveAt(SelectionStart);

            ResetDisplayCharacters(SelectionStart);
        }

        private bool IsIgnorableKey(Keys key)
        {
            return key == Keys.Escape || key == Keys.Enter;
        }
    }
}