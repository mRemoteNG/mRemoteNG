using System;
using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.Connection;
using mRemoteNG.Tools;

namespace mRemoteNG.UI.Controls
{
    /// <summary>
    /// Provides an IntelliSense-like completion dropdown for text boxes that
    /// support external tool variables. When the user types the variable
    /// character ('%'), a dropdown of available variables is shown. Choosing an
    /// entry inserts the corresponding token at the caret. When a connection is
    /// available, the dropdown also shows a preview of the current value of each
    /// variable.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public sealed class VariableAutoCompleteMenu : IDisposable
    {
        private readonly TextBoxBase _textBox;
        private readonly ToolStripDropDown _dropDown;
        private readonly ListBox _listBox;
        private int _tokenStart = -1;
        private bool _disposed;

        /// <summary>
        /// Optional connection used to generate preview values for the variables.
        /// May be changed at runtime, for example when the selected connection
        /// changes.
        /// </summary>
        public ConnectionInfo ConnectionInfo { get; set; }

        public VariableAutoCompleteMenu(TextBoxBase textBox, ConnectionInfo connectionInfo = null)
        {
            _textBox = textBox ?? throw new ArgumentNullException(nameof(textBox));
            ConnectionInfo = connectionInfo;

            _listBox = new ListBox
            {
                BorderStyle = BorderStyle.None,
                IntegralHeight = false,
                Width = 320,
                Height = 160
            };
            _listBox.Click += ListBoxOnClick;
            _listBox.KeyDown += ListBoxOnKeyDown;
            _listBox.MouseDoubleClick += ListBoxOnClick;

            _dropDown = new ToolStripDropDown
            {
                AutoClose = true,
                AutoSize = true,
                DropShadowEnabled = true,
                Padding = Padding.Empty
            };
            _dropDown.Items.Add(new ToolStripControlHost(_listBox)
            {
                Padding = Padding.Empty,
                Margin = Padding.Empty,
                AutoSize = false,
                Size = _listBox.Size
            });

            _textBox.KeyPress += TextBoxOnKeyPress;
            _textBox.KeyDown += TextBoxOnKeyDown;
            _textBox.LostFocus += TextBoxOnLostFocus;
        }

        private void TextBoxOnKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '%')
            {
                // Defer so the '%' character is inserted into the text box before
                // we record the token start position and show the dropdown.
                _textBox.BeginInvoke(new Action(ShowDropDown));
            }
            else if (IsDropDownVisible && !char.IsLetterOrDigit(e.KeyChar))
            {
                HideDropDown();
            }
        }

        private void TextBoxOnKeyDown(object sender, KeyEventArgs e)
        {
            if (!IsDropDownVisible)
                return;

            switch (e.KeyCode)
            {
                case Keys.Down:
                    MoveSelection(1);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
                case Keys.Up:
                    MoveSelection(-1);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
                case Keys.Enter:
                case Keys.Tab:
                    CommitSelection();
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
                case Keys.Escape:
                    HideDropDown();
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
            }
        }

        private void TextBoxOnLostFocus(object sender, EventArgs e)
        {
            if (!_dropDown.Focused && !_listBox.Focused)
                HideDropDown();
        }

        private void ShowDropDown()
        {
            if (_disposed)
                return;

            _tokenStart = _textBox.SelectionStart;
            PopulateItems();
            if (_listBox.Items.Count == 0)
                return;

            _listBox.SelectedIndex = 0;

            Point caret = _textBox.GetPositionFromCharIndex(Math.Max(0, _textBox.SelectionStart - 1));
            caret.Y += _textBox.Font.Height;
            _dropDown.Show(_textBox, caret);
            _textBox.Focus();
        }

        private void PopulateItems()
        {
            _listBox.Items.Clear();
            ExternalToolArgumentParser parser = ConnectionInfo == null
                ? null
                : new ExternalToolArgumentParser(ConnectionInfo);

            foreach (ExternalToolVariable variable in ExternalToolVariable.SupportedVariables)
                _listBox.Items.Add(new VariableItem(variable, parser));
        }

        private bool IsDropDownVisible => _dropDown.Visible;

        private void MoveSelection(int delta)
        {
            int count = _listBox.Items.Count;
            if (count == 0)
                return;

            int next = _listBox.SelectedIndex + delta;
            next = Math.Max(0, Math.Min(count - 1, next));
            _listBox.SelectedIndex = next;
        }

        private void ListBoxOnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                CommitSelection();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                HideDropDown();
                e.Handled = true;
            }
        }

        private void ListBoxOnClick(object sender, EventArgs e)
        {
            CommitSelection();
        }

        private void CommitSelection()
        {
            if (_listBox.SelectedItem is not VariableItem item || _tokenStart < 0)
            {
                HideDropDown();
                return;
            }

            // The '%' that triggered the dropdown has already been inserted at
            // (_tokenStart - 1). Replace from that '%' up to the current caret
            // with the full variable token.
            int replaceStart = Math.Max(0, _tokenStart - 1);
            int caret = _textBox.SelectionStart;
            int replaceLength = Math.Max(0, caret - replaceStart);

            string text = _textBox.Text;
            if (replaceStart > text.Length)
                replaceStart = text.Length;
            if (replaceStart + replaceLength > text.Length)
                replaceLength = text.Length - replaceStart;

            string token = item.Variable.Token;
            string newText = text.Remove(replaceStart, replaceLength).Insert(replaceStart, token);
            _textBox.Text = newText;
            _textBox.SelectionStart = replaceStart + token.Length;
            _textBox.SelectionLength = 0;

            HideDropDown();
            _textBox.Focus();
        }

        private void HideDropDown()
        {
            _tokenStart = -1;
            if (_dropDown.Visible)
                _dropDown.Close();
        }

        public void Dispose()
        {
            if (_disposed)
                return;
            _disposed = true;

            _textBox.KeyPress -= TextBoxOnKeyPress;
            _textBox.KeyDown -= TextBoxOnKeyDown;
            _textBox.LostFocus -= TextBoxOnLostFocus;
            _listBox.Click -= ListBoxOnClick;
            _listBox.KeyDown -= ListBoxOnKeyDown;
            _listBox.MouseDoubleClick -= ListBoxOnClick;
            _dropDown.Dispose();
            _listBox.Dispose();
        }

        private sealed class VariableItem(ExternalToolVariable variable, ExternalToolArgumentParser parser)
        {
            public ExternalToolVariable Variable { get; } = variable;

            public override string ToString()
            {
                string preview = parser?.GetVariablePreview(Variable.Name) ?? string.Empty;
                string descriptionPart = string.IsNullOrEmpty(Variable.Description)
                    ? string.Empty
                    : $" - {Variable.Description}";
                string previewPart = string.IsNullOrEmpty(preview)
                    ? string.Empty
                    : $" = {preview}";
                return $"{Variable.Token}{descriptionPart}{previewPart}";
            }
        }
    }
}
