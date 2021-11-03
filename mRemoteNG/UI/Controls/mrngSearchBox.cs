using System;
using System.Windows.Forms;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Controls
{
    public class MrngSearchBox : MrngTextBox
    {
        private bool _showDefaultText = true;
        private bool _settingDefaultText = true;
        private readonly PictureBox _pbClear = new PictureBox();
        private readonly ToolTip _btClearToolTip = new ToolTip();

        public MrngSearchBox()
        {
            TextChanged += NGSearchBox_TextChanged;
            LostFocus += FocusLost;
            GotFocus += FocusGot;
            AddClearButton();
            ApplyLanguage();
        }

        private void ApplyLanguage()
        {
            _btClearToolTip.SetToolTip(_pbClear, Language.ClearSearchString);
        }

        private void AddClearButton()
        {
            _pbClear.Image = Properties.Resources.Close_16x;
            _pbClear.Width = 20;
            _pbClear.Dock = DockStyle.Right;
            _pbClear.Cursor = Cursors.Default;
            _pbClear.Click += PbClear_Click;
            _pbClear.LostFocus += FocusLost;
            Controls.Add(_pbClear);
        }

        private void FocusLost(object sender, EventArgs e)
        {
            if (!_showDefaultText)
                return;

            _settingDefaultText = true;
            Text = Language.SearchPrompt;
            _pbClear.Visible = false;
        }

        private void FocusGot(object sender, EventArgs e)
        {
            if (_showDefaultText)
                Text = "";
        }

        private void PbClear_Click(object sender, EventArgs e) => Text = string.Empty;

        private void NGSearchBox_TextChanged(object sender, EventArgs e)
        {
            if (!_settingDefaultText)
            {
                _showDefaultText = string.IsNullOrEmpty(Text);
            }

            _pbClear.Visible = !_showDefaultText && TextLength > 0;
            _settingDefaultText = false;
        }
    }
}