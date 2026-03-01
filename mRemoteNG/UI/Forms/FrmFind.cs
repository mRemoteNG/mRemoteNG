using System;
using System.Drawing;
using System.Windows.Forms;

namespace mRemoteNG.UI.Forms
{
    public class FrmFind : Form
    {
        private RichTextBox _rtbOutput;
        private Panel _pnlSearch;
        private TextBox _txtSearch;
        private Button _btnFindNext;
        private CheckBox _chkMatchCase;
        private Label _lblStatus;

        public FrmFind()
        {
            InitializeComponent();
        }

        public void SetContent(string text)
        {
            _rtbOutput.Text = text;
        }

        private void InitializeComponent()
        {
            this._pnlSearch = new Panel();
            this._txtSearch = new TextBox();
            this._btnFindNext = new Button();
            this._chkMatchCase = new CheckBox();
            this._lblStatus = new Label();
            this._rtbOutput = new RichTextBox();

            this.SuspendLayout();

            // 
            // _pnlSearch
            // 
            this._pnlSearch.Controls.Add(this._lblStatus);
            this._pnlSearch.Controls.Add(this._chkMatchCase);
            this._pnlSearch.Controls.Add(this._btnFindNext);
            this._pnlSearch.Controls.Add(this._txtSearch);
            this._pnlSearch.Dock = DockStyle.Top;
            this._pnlSearch.Height = 40;
            this._pnlSearch.Padding = new Padding(5);

            // 
            // _txtSearch
            // 
            this._txtSearch.Location = new Point(10, 10);
            this._txtSearch.Size = new Size(200, 23);
            this._txtSearch.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) FindNext(); };

            // 
            // _btnFindNext
            // 
            this._btnFindNext.Location = new Point(220, 8);
            this._btnFindNext.Text = "Find Next";
            this._btnFindNext.AutoSize = true;
            this._btnFindNext.Click += (s, e) => FindNext();

            // 
            // _chkMatchCase
            // 
            this._chkMatchCase.Location = new Point(310, 10);
            this._chkMatchCase.Text = "Match Case";
            this._chkMatchCase.AutoSize = true;

            // 
            // _lblStatus
            // 
            this._lblStatus.Location = new Point(420, 12);
            this._lblStatus.AutoSize = true;

            // 
            // _rtbOutput
            // 
            this._rtbOutput.Dock = DockStyle.Fill;
            this._rtbOutput.ReadOnly = true;
            this._rtbOutput.Font = new Font("Consolas", 10F);
            this._rtbOutput.HideSelection = false; // Keep highlight when focus is lost

            // 
            // FrmFind
            // 
            this.ClientSize = new Size(800, 600);
            this.Controls.Add(this._rtbOutput);
            this.Controls.Add(this._pnlSearch);
            this.Text = "Find in Session Output";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
        }

        private void FindNext()
        {
            string searchText = _txtSearch.Text;
            if (string.IsNullOrEmpty(searchText)) return;

            RichTextBoxFinds options = RichTextBoxFinds.None;
            if (_chkMatchCase.Checked) options |= RichTextBoxFinds.MatchCase;

            int startPos = _rtbOutput.SelectionStart + _rtbOutput.SelectionLength;
            // If already at end or selection is empty but start is not 0 (meaning we might want to search from current caret)
            
            // RichTextBox.Find searches from start to end.
            
            int foundPos = _rtbOutput.Find(searchText, startPos, options);

            if (foundPos == -1)
            {
                // Wrap around
                foundPos = _rtbOutput.Find(searchText, 0, startPos, options);
            }

            if (foundPos != -1)
            {
                _rtbOutput.Select(foundPos, searchText.Length);
                _rtbOutput.ScrollToCaret();
                _rtbOutput.Focus();
                _lblStatus.Text = "";
            }
            else
            {
                _lblStatus.Text = "Not found";
            }
        }
    }
}
