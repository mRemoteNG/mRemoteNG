using System;
using System.Windows.Forms;

namespace mRemoteNG.UI.Controls.Base
{
    public class NGSearchBox : NGTextBox
    {
        private Button btClear = new Button();
        private ToolTip btClearToolTip = new ToolTip();

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
            btClearToolTip.SetToolTip(btClear, Language.ClearSearchString);
        }

        private void AddClearButton()
        {
            btClear.BackgroundImage = Resources.Delete;
            btClear.BackgroundImageLayout = ImageLayout.Stretch;
            btClear.Width = 16;
            btClear.Dock = DockStyle.Right;
            btClear.Click += BtClear_Click;
            btClear.LostFocus += FocusLost;
            Controls.Add(btClear);
        }

        private void FocusLost(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Text))
            {
                Text = Language.strSearchPrompt;
                btClear.Visible = false;
            }
        }

        private void FocusGot(object sender, EventArgs e) => Text = "";

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // NGSearchBox
            // 
            this.TextChanged += new System.EventHandler(this.NGSearchBox_TextChanged);
            this.ResumeLayout(false);

        }

        private void BtClear_Click(object sender, EventArgs e) => Text = string.Empty;

        private void NGSearchBox_TextChanged(object sender, EventArgs e)
        {
            if (Text == Language.strSearchPrompt)
                btClear.Visible = false;
            else
                btClear.Visible = TextLength > 0;
        }
    }
}