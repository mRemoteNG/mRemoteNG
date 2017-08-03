using mRemoteNG.Themes;
using mRemoteNG.UI.Controls.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace mRemoteNG.UI.TaskDialog
{
    public partial class frmTaskDialog : Form
    {
        //--------------------------------------------------------------------------------
        #region PRIVATE members
        //--------------------------------------------------------------------------------

        private string _mainInstruction = "Main Instruction Text";
        private int _mainInstructionHeight;
        private readonly Font _mainInstructionFont = new Font("Segoe UI", 11.75F, FontStyle.Regular, GraphicsUnit.Point, 0);

        private readonly List<NGRadioButton> _radioButtonCtrls = new List<NGRadioButton>();

        private Control _focusControl;

        private bool _isVista = false;
        #endregion

        //--------------------------------------------------------------------------------
        #region PROPERTIES
        //--------------------------------------------------------------------------------
        public ESysIcons MainIcon { get; set; } = ESysIcons.Question;
        public ESysIcons FooterIcon { get; set; } = ESysIcons.Warning;

        public string Title { get { return Text; } set { Text = value; } }
        public string MainInstruction { get { return _mainInstruction; } set { _mainInstruction = value; Invalidate(); } }
        public string Content { get { return lbContent.Text; } set { lbContent.Text = value; } }
        public string ExpandedInfo { get { return lbExpandedInfo.Text; } set { lbExpandedInfo.Text = value; } }
        public string Footer { get { return lbFooter.Text; } set { lbFooter.Text = value; } }
        public int DefaultButtonIndex { get; set; }

        public string RadioButtons { get; set; } = "";

        public int RadioButtonIndex
        {
            get
            {
                foreach (var rb in _radioButtonCtrls)
                    if (rb.Checked)
                        return (int)rb.Tag;
                return -1;
            }
        }

        public string CommandButtons { get; set; } = "";
        public int CommandButtonClickedIndex { get; private set; } = -1;

        public ETaskDialogButtons Buttons { get; set; } = ETaskDialogButtons.YesNoCancel;

        public string VerificationText { get { return cbVerify.Text; } set { cbVerify.Text = value; } }
        public bool VerificationCheckBoxChecked { get { return cbVerify.Checked; } set { cbVerify.Checked = value; } }

        private bool Expanded { get; set; }

        #endregion

        //--------------------------------------------------------------------------------
        #region CONSTRUCTOR
        //--------------------------------------------------------------------------------
        public frmTaskDialog()
        {
            InitializeComponent();

            // _isVista = VistaTaskDialog.IsAvailableOnThisOS;
            if (!_isVista && CTaskDialog.UseToolWindowOnXp) // <- shall we use the smaller toolbar?
                FormBorderStyle = FormBorderStyle.FixedToolWindow;

            MainInstruction = "Main Instruction";
            Content = "";
            ExpandedInfo = "";
            Footer = "";
            VerificationText = "";
        }
        #endregion

        //--------------------------------------------------------------------------------
        #region BuildForm
        // This is the main routine that should be called before .ShowDialog()
        //--------------------------------------------------------------------------------
        private bool _formBuilt;
        public void BuildForm()
        {
            var form_height = 0;

            // Setup Main Instruction
            switch (MainIcon)
            {
                case ESysIcons.Information: imgMain.Image = SystemIcons.Information.ToBitmap(); break;
                case ESysIcons.Question: imgMain.Image = SystemIcons.Question.ToBitmap(); break;
                case ESysIcons.Warning: imgMain.Image = SystemIcons.Warning.ToBitmap(); break;
                case ESysIcons.Error: imgMain.Image = SystemIcons.Error.ToBitmap(); break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //AdjustLabelHeight(lbMainInstruction);
            //pnlMainInstruction.Height = Math.Max(41, lbMainInstruction.Height + 16);
            if (_mainInstructionHeight == 0)
                GetMainInstructionTextSizeF();
            pnlMainInstruction.Height = Math.Max(41, _mainInstructionHeight + 16);

            form_height += pnlMainInstruction.Height;

            // Setup Content
            pnlContent.Visible = (Content != "");
            if (Content != "")
            {
                AdjustLabelHeight(lbContent);
                pnlContent.Height = lbContent.Height + 4;
                form_height += pnlContent.Height;
            }

            var show_verify_checkbox = (cbVerify.Text != "");
            cbVerify.Visible = show_verify_checkbox;

            // Setup Expanded Info and Buttons panels
            if (ExpandedInfo == "")
            {
                pnlExpandedInfo.Visible = false;
                lbShowHideDetails.Visible = false;
                cbVerify.Top = 12;
                pnlButtons.Height = 40;
            }
            else
            {
                AdjustLabelHeight(lbExpandedInfo);
                pnlExpandedInfo.Height = lbExpandedInfo.Height + 4;
                pnlExpandedInfo.Visible = Expanded;
                lbShowHideDetails.Text = (Expanded ? "        Hide details" : "        Show details");
                lbShowHideDetails.ImageIndex = (Expanded ? 0 : 3);
                if (!show_verify_checkbox)
                    pnlButtons.Height = 40;
                if (Expanded)
                    form_height += pnlExpandedInfo.Height;
            }

            // Setup RadioButtons
            pnlRadioButtons.Visible = (RadioButtons != "");
            if (RadioButtons != "")
            {
                var arr = RadioButtons.Split(new char[] { '|' });
                var pnl_height = 12;
                for (var i = 0; i < arr.Length; i++)
                {
                    var rb = new NGRadioButton();
                    rb.Parent = pnlRadioButtons;
                    rb.Location = new Point(60, 4 + (i * rb.Height));
                    rb.Text = arr[i];
                    rb.Tag = i;
                    rb.Checked = (DefaultButtonIndex == i);
                    rb.Width = Width - rb.Left - 15;
                    pnl_height += rb.Height;
                    _radioButtonCtrls.Add(rb);
                }
                pnlRadioButtons.Height = pnl_height;
                form_height += pnlRadioButtons.Height;
            }

            // Setup CommandButtons
            pnlCommandButtons.Visible = (CommandButtons != "");
            if (CommandButtons != "")
            {
                var arr = CommandButtons.Split(new char[] { '|' });
                var t = 8;
                var pnl_height = 16;
                for (var i = 0; i < arr.Length; i++)
                {
                    var btn = new CommandButton();
                    btn.Parent = pnlCommandButtons;
                    btn.Location = new Point(50, t);
                    if (_isVista)  // <- tweak font if vista
                        btn.Font = new Font(btn.Font, FontStyle.Regular);
                    btn.Text = arr[i];
                    btn.Size = new Size(Width - btn.Left - 15, btn.GetBestHeight());
                    t += btn.Height;
                    pnl_height += btn.Height;
                    btn.Tag = i;
                    btn.Click += new EventHandler(CommandButton_Click);
                    if (i == DefaultButtonIndex)
                        _focusControl = btn;
                }
                pnlCommandButtons.Height = pnl_height;
                form_height += pnlCommandButtons.Height;
            }

            // Setup Buttons
            switch (Buttons)
            {
                case ETaskDialogButtons.YesNo:
                    bt1.Visible = false;
                    bt2.Text = "&Yes";
                    bt2.DialogResult = DialogResult.Yes;
                    bt3.Text = "&No";
                    bt3.DialogResult = DialogResult.No;
                    AcceptButton = bt2;
                    CancelButton = bt3;
                    break;
                case ETaskDialogButtons.YesNoCancel:
                    bt1.Text = "&Yes";
                    bt1.DialogResult = DialogResult.Yes;
                    bt2.Text = "&No";
                    bt2.DialogResult = DialogResult.No;
                    bt3.Text = "&Cancel";
                    bt3.DialogResult = DialogResult.Cancel;
                    AcceptButton = bt1;
                    CancelButton = bt3;
                    break;
                case ETaskDialogButtons.OkCancel:
                    bt1.Visible = false;
                    bt2.Text = "&OK";
                    bt2.DialogResult = DialogResult.OK;
                    bt3.Text = "&Cancel";
                    bt3.DialogResult = DialogResult.Cancel;
                    AcceptButton = bt2;
                    CancelButton = bt3;
                    break;
                case ETaskDialogButtons.Ok:
                    bt1.Visible = false;
                    bt2.Visible = false;
                    bt3.Text = "&OK";
                    bt3.DialogResult = DialogResult.OK;
                    AcceptButton = bt3;
                    CancelButton = bt3;
                    break;
                case ETaskDialogButtons.Close:
                    bt1.Visible = false;
                    bt2.Visible = false;
                    bt3.Text = "&Close";
                    bt3.DialogResult = DialogResult.Cancel;
                    CancelButton = bt3;
                    break;
                case ETaskDialogButtons.Cancel:
                    bt1.Visible = false;
                    bt2.Visible = false;
                    bt3.Text = "&Cancel";
                    bt3.DialogResult = DialogResult.Cancel;
                    CancelButton = bt3;
                    break;
                case ETaskDialogButtons.None:
                    bt1.Visible = false;
                    bt2.Visible = false;
                    bt3.Visible = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            ControlBox = (Buttons == ETaskDialogButtons.Cancel ||
                               Buttons == ETaskDialogButtons.Close ||
                               Buttons == ETaskDialogButtons.OkCancel ||
                               Buttons == ETaskDialogButtons.YesNoCancel);

            if (!show_verify_checkbox && ExpandedInfo == "" && Buttons == ETaskDialogButtons.None)
                pnlButtons.Visible = false;
            else
                form_height += pnlButtons.Height;

            pnlFooter.Visible = (Footer != "");
            if (Footer != "")
            {
                AdjustLabelHeight(lbFooter);
                pnlFooter.Height = Math.Max(28, lbFooter.Height + 16);
                switch (FooterIcon)
                {
                    case ESysIcons.Information:
                        // SystemIcons.Information.ToBitmap().GetThumbnailImage(16, 16, null, IntPtr.Zero);
                        imgFooter.Image = ResizeBitmap(SystemIcons.Information.ToBitmap(), 16, 16);
                        break;
                    case ESysIcons.Question:
                        // SystemIcons.Question.ToBitmap().GetThumbnailImage(16, 16, null, IntPtr.Zero);
                        imgFooter.Image = ResizeBitmap(SystemIcons.Question.ToBitmap(), 16, 16);
                        break;
                    case ESysIcons.Warning:
                        // SystemIcons.Warning.ToBitmap().GetThumbnailImage(16, 16, null, IntPtr.Zero);
                        imgFooter.Image = ResizeBitmap(SystemIcons.Warning.ToBitmap(), 16, 16);
                        break;
                    case ESysIcons.Error:
                        // SystemIcons.Error.ToBitmap().GetThumbnailImage(16, 16, null, IntPtr.Zero);
                        imgFooter.Image = ResizeBitmap(SystemIcons.Error.ToBitmap(), 16, 16);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                form_height += pnlFooter.Height;
            }

            ClientSize = new Size(ClientSize.Width, form_height);

            _formBuilt = true;
            Themes.ThemeManager.getInstance().ThemeChanged += ApplyTheme;
            ApplyTheme();
        }

        private void ApplyTheme()
        { 
            if(ThemeManager.getInstance().ThemingActive)
            {
                pnlButtons.BackColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
                pnlButtons.ForeColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
                panel2.BackColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
                panel2.ForeColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
                pnlFooter.BackColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
                pnlFooter.ForeColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
                panel5.BackColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
                panel5.ForeColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
                panel3.BackColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
                panel3.ForeColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
                pnlCommandButtons.BackColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
                pnlCommandButtons.ForeColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
                pnlMainInstruction.BackColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
                pnlMainInstruction.ForeColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
                pnlContent.BackColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
                pnlContent.ForeColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
                pnlExpandedInfo.BackColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
                pnlExpandedInfo.ForeColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
                pnlRadioButtons.BackColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
                pnlRadioButtons.ForeColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
            }  
        }

        //--------------------------------------------------------------------------------
        private static Image ResizeBitmap(Image SrcImg, int NewWidth, int NewHeight)
        {
            var percent_width = (NewWidth / (float)SrcImg.Width);
            var percent_height = (NewHeight / (float)SrcImg.Height);

            var resize_percent = (percent_height < percent_width ? percent_height : percent_width);

            var w = (int)(SrcImg.Width * resize_percent);
            var h = (int)(SrcImg.Height * resize_percent);
            var b = new Bitmap(w, h);

            using (var g = Graphics.FromImage(b))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(SrcImg, 0, 0, w, h);
            }
            return b;
        }

        //--------------------------------------------------------------------------------
        // utility function for setting a Label's height
        private static void AdjustLabelHeight(Control lb)
        {
            var text = lb.Text;
            var textFont = lb.Font;
            var layoutSize = new SizeF(lb.ClientSize.Width, 5000.0F);
            var g = Graphics.FromHwnd(lb.Handle);
            var stringSize = g.MeasureString(text, textFont, layoutSize);
            lb.Height = (int)stringSize.Height + 4;
            g.Dispose();
        }
        #endregion

        //--------------------------------------------------------------------------------
        #region EVENTS
        //--------------------------------------------------------------------------------
        private void CommandButton_Click(object sender, EventArgs e)
        {
            CommandButtonClickedIndex = (int)((CommandButton)sender).Tag;
            DialogResult = DialogResult.OK;
        }


        //--------------------------------------------------------------------------------
        protected override void OnShown(EventArgs e)
        {
            if (!_formBuilt)
                throw new Exception("frmTaskDialog : Please call .BuildForm() before showing the TaskDialog");
            base.OnShown(e);
        }

        //--------------------------------------------------------------------------------
        private void lbDetails_MouseEnter(object sender, EventArgs e)
        {
            lbShowHideDetails.ImageIndex = (Expanded ? 1 : 4);
        }

        //--------------------------------------------------------------------------------
        private void lbDetails_MouseLeave(object sender, EventArgs e)
        {
            lbShowHideDetails.ImageIndex = (Expanded ? 0 : 3);
        }

        //--------------------------------------------------------------------------------
        private void lbDetails_MouseUp(object sender, MouseEventArgs e)
        {
            lbShowHideDetails.ImageIndex = (Expanded ? 1 : 4);
        }

        //--------------------------------------------------------------------------------
        private void lbDetails_MouseDown(object sender, MouseEventArgs e)
        {
            lbShowHideDetails.ImageIndex = (Expanded ? 2 : 5);
        }

        //--------------------------------------------------------------------------------
        private void lbDetails_Click(object sender, EventArgs e)
        {
            Expanded = !Expanded;
            pnlExpandedInfo.Visible = Expanded;
            lbShowHideDetails.Text = (Expanded ? "        Hide details" : "        Show details");
            if (Expanded)
                Height += pnlExpandedInfo.Height;
            else
                Height -= pnlExpandedInfo.Height;
        }

        //--------------------------------------------------------------------------------
        private const int MAIN_INSTRUCTION_LEFT_MARGIN = 46;
        private const int MAIN_INSTRUCTION_RIGHT_MARGIN = 8;

        private SizeF GetMainInstructionTextSizeF()
        {
            var mzSize = new SizeF(pnlMainInstruction.Width - MAIN_INSTRUCTION_LEFT_MARGIN - MAIN_INSTRUCTION_RIGHT_MARGIN, 5000.0F);
            var g = Graphics.FromHwnd(Handle);
            var textSize = g.MeasureString(_mainInstruction, _mainInstructionFont, mzSize);
            _mainInstructionHeight = (int)textSize.Height;
            return textSize;
        }

        private void pnlMainInstruction_Paint(object sender, PaintEventArgs e)
        {
            var szL = GetMainInstructionTextSizeF();
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            e.Graphics.DrawString(_mainInstruction, _mainInstructionFont, new SolidBrush( ((Panel)sender).ForeColor), new RectangleF(new PointF(MAIN_INSTRUCTION_LEFT_MARGIN, 10), szL));
        }

        //--------------------------------------------------------------------------------
        private void frmTaskDialog_Shown(object sender, EventArgs e)
        {
            if (CTaskDialog.PlaySystemSounds)
            {
                switch (MainIcon)
                {
                    case ESysIcons.Error: System.Media.SystemSounds.Hand.Play(); break;
                    case ESysIcons.Information: System.Media.SystemSounds.Asterisk.Play(); break;
                    case ESysIcons.Question: System.Media.SystemSounds.Asterisk.Play(); break;
                    case ESysIcons.Warning: System.Media.SystemSounds.Exclamation.Play(); break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            _focusControl?.Focus();
        }

        #endregion

        //--------------------------------------------------------------------------------
    }
}
