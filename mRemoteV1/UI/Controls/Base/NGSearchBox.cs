using System;
using System.Drawing;
using System.Windows.Forms;

namespace mRemoteNG.UI.Controls.Base
{
    public class NGSearchBox : NGTextBox
    {
        private bool _showDefaultText = true;
        private bool _settingDefaultText = true;
        private PictureBox pbClear = new PictureBox();
        private ToolTip btClearToolTip = new ToolTip();

        //public override string Text { get; set; }

        public NGSearchBox()
        {
            InitializeComponent();
            LostFocus += FocusLost;
            GotFocus += FocusGot;
            AddClearButton();
            ApplyLanguage();
        }

        private void ApplyLanguage()
        {
            btClearToolTip.SetToolTip(pbClear, Language.ClearSearchString);
        }

        private void AddClearButton()
        {
            pbClear.Image = Resources.Delete;
            pbClear.Width = 20;
            pbClear.Dock = DockStyle.Right;
            pbClear.Cursor = Cursors.Default;
            pbClear.Click += PbClear_Click;
            pbClear.LostFocus += FocusLost;
            Controls.Add(pbClear);
        }

        private void FocusLost(object sender, EventArgs e)
        {
            if (_showDefaultText)
            {
                _settingDefaultText = true;
                Text = Language.strSearchPrompt;
                pbClear.Visible = false;
            }
        }

        private void FocusGot(object sender, EventArgs e)
        {
            if (_showDefaultText)
                Text = "";
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // NGSearchBox
            // 
            this.TextChanged += new System.EventHandler(this.NGSearchBox_TextChanged);
            this.ResumeLayout(false);
        }

        private void PbClear_Click(object sender, EventArgs e) => Text = string.Empty;

        private void NGSearchBox_TextChanged(object sender, EventArgs e)
        {
            if (!_settingDefaultText)
            {
                _showDefaultText = string.IsNullOrEmpty(Text);
            }

            pbClear.Visible = !_showDefaultText && TextLength > 0;
            _settingDefaultText = false;
        }
    }
}