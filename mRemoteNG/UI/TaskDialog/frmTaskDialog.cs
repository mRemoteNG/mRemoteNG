using mRemoteNG.Themes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using mRemoteNG.UI.Controls;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.TaskDialog
{
    [SupportedOSPlatform("windows")]
    public partial class frmTaskDialog : Form
    {
        //--------------------------------------------------------------------------------

        #region PRIVATE members

        //--------------------------------------------------------------------------------

        private string _mainInstruction = "Main Instruction Text";

        private readonly Font _mainInstructionFont =
            new Font("Segoe UI", 11.75F, FontStyle.Regular, GraphicsUnit.Point, 0);

        private readonly List<MrngRadioButton> _radioButtonCtrls = new List<MrngRadioButton>();
        private readonly DisplayProperties _display = new DisplayProperties();
        private Control _focusControl;

        private bool _isVista = false;

        private int _mainInstructionLeftMargin;
        private int _mainInstructionRightMargin;

        #endregion

        //--------------------------------------------------------------------------------

        #region PROPERTIES

        //--------------------------------------------------------------------------------
        public ESysIcons MainIcon { get; set; } = ESysIcons.Question;
        public ESysIcons FooterIcon { get; set; } = ESysIcons.Warning;

        public string Title
        {
            get => Text;
            set => Text = value;
        }

        public string MainInstruction
        {
            get => _mainInstruction;
            set
            {
                _mainInstruction = value;
                Invalidate();
            }
        }

        public string Content
        {
            get => lbContent.Text;
            set => lbContent.Text = value;
        }

        public string ExpandedInfo
        {
            get => lbExpandedInfo.Text;
            set => lbExpandedInfo.Text = value;
        }

        public string Footer
        {
            get => lbFooter.Text;
            set => lbFooter.Text = value;
        }

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

        public string VerificationText
        {
            get => cbVerify.Text;
            set => cbVerify.Text = value;
        }

        public bool VerificationCheckBoxChecked
        {
            get => cbVerify.Checked;
            set => cbVerify.Checked = value;
        }

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
            var formHeight = 0;
            imgMain.Width = _display.ScaleWidth(imgMain.Width);
            imgMain.Height = _display.ScaleHeight(imgMain.Height);

            // Setup Main Instruction
            switch (MainIcon)
            {
                case ESysIcons.Information:
                    imgMain.Image = SystemIcons.Information.ToBitmap();
                    break;
                case ESysIcons.Question:
                    imgMain.Image = SystemIcons.Question.ToBitmap();
                    break;
                case ESysIcons.Warning:
                    imgMain.Image = SystemIcons.Warning.ToBitmap();
                    break;
                case ESysIcons.Error:
                    imgMain.Image = SystemIcons.Error.ToBitmap();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            lbMainInstruction.Text = _mainInstruction;
            lbMainInstruction.Font = _mainInstructionFont;
            AdjustLabelHeight(lbMainInstruction);
            pnlMainInstruction.Height = Math.Max(41, lbMainInstruction.Height + _display.ScaleHeight(16));

            _mainInstructionLeftMargin = imgMain.Left + imgMain.Width + _display.ScaleWidth(imgMain.Padding.Right);
            _mainInstructionRightMargin = _display.ScaleWidth(8);
            formHeight += pnlMainInstruction.Height;

            // Setup Content
            pnlContent.Visible = Content != "";
            if (Content != "")
            {
                AdjustLabelHeight(lbContent);
                pnlContent.Height = lbContent.Height + _display.ScaleHeight(4);
                formHeight += pnlContent.Height;
            }

            var showVerifyCheckbox = cbVerify.Text != "";
            cbVerify.Visible = showVerifyCheckbox;

            // Setup Expanded Info and Buttons panels
            if (ExpandedInfo == "")
            {
                pnlExpandedInfo.Visible = false;
                lbShowHideDetails.Visible = false;
                cbVerify.Top = _display.ScaleHeight(12);
                pnlButtons.Height = _display.ScaleHeight(40);
            }
            else
            {
                AdjustLabelHeight(lbExpandedInfo);
                pnlExpandedInfo.Height = lbExpandedInfo.Height + _display.ScaleHeight(4);
                pnlExpandedInfo.Visible = Expanded;
                lbShowHideDetails.Text = Expanded ? "        Hide details" : "        Show details";
                lbShowHideDetails.ImageIndex = Expanded ? 0 : 3;
                if (!showVerifyCheckbox)
                    pnlButtons.Height = _display.ScaleHeight(40);
                if (Expanded)
                    formHeight += pnlExpandedInfo.Height;
            }

            // Setup RadioButtons
            pnlRadioButtons.Visible = RadioButtons != "";
            if (RadioButtons != "")
            {
                var arr = RadioButtons.Split('|');
                var pnlHeight = _display.ScaleHeight(12);
                for (var i = 0; i < arr.Length; i++)
                {
                    var rb = new MrngRadioButton {Parent = pnlRadioButtons};
                    rb.Location = new Point(_display.ScaleWidth(60), _display.ScaleHeight(4) + i * rb.Height);
                    rb.Text = arr[i];
                    rb.Tag = i;
                    rb.Checked = DefaultButtonIndex == i;
                    rb.Width = Width - rb.Left - _display.ScaleWidth(15);
                    pnlHeight += rb.Height;
                    _radioButtonCtrls.Add(rb);
                }

                pnlRadioButtons.Height = pnlHeight;
                formHeight += pnlRadioButtons.Height;
            }

            // Setup CommandButtons
            pnlCommandButtons.Visible = CommandButtons != "";
            if (CommandButtons != "")
            {
                var arr = CommandButtons.Split('|');
                var t = _display.ScaleHeight(8);
                var pnlHeight = _display.ScaleHeight(16);
                for (var i = 0; i < arr.Length; i++)
                {
                    var btn = new CommandButton
                    {
                        Parent = pnlCommandButtons, Location = new Point(_display.ScaleWidth(50), t)
                    };
                    if (_isVista) // <- tweak font if vista
                        btn.Font = new Font(btn.Font, FontStyle.Regular);
                    btn.Text = arr[i];
                    btn.Size = new Size(Width - btn.Left - _display.ScaleWidth(15), btn.GetBestHeight());
                    t += btn.Height;
                    pnlHeight += btn.Height;
                    btn.Tag = i;
                    btn.Click += CommandButton_Click;
                    if (i == DefaultButtonIndex)
                        _focusControl = btn;
                }

                pnlCommandButtons.Height = pnlHeight;
                formHeight += pnlCommandButtons.Height;
            }

            // Setup Buttons
            switch (Buttons)
            {
                case ETaskDialogButtons.YesNo:
                    bt1.Visible = false;
                    bt2.Text = Language.Yes;
                    bt2.DialogResult = DialogResult.Yes;
                    bt3.Text = Language.No;
                    bt3.DialogResult = DialogResult.No;
                    AcceptButton = bt2;
                    CancelButton = bt3;
                    break;
                case ETaskDialogButtons.YesNoCancel:
                    bt1.Text = Language.Yes;
                    bt1.DialogResult = DialogResult.Yes;
                    bt2.Text = Language.No;
                    bt2.DialogResult = DialogResult.No;
                    bt3.Text = Language._Cancel;
                    bt3.DialogResult = DialogResult.Cancel;
                    AcceptButton = bt1;
                    CancelButton = bt3;
                    break;
                case ETaskDialogButtons.OkCancel:
                    bt1.Visible = false;
                    bt2.Text = Language._Ok;
                    bt2.DialogResult = DialogResult.OK;
                    bt3.Text = Language._Cancel;
                    bt3.DialogResult = DialogResult.Cancel;
                    AcceptButton = bt2;
                    CancelButton = bt3;
                    break;
                case ETaskDialogButtons.Ok:
                    bt1.Visible = false;
                    bt2.Visible = false;
                    bt3.Text = Language._Ok;
                    bt3.DialogResult = DialogResult.OK;
                    AcceptButton = bt3;
                    CancelButton = bt3;
                    break;
                case ETaskDialogButtons.Close:
                    bt1.Visible = false;
                    bt2.Visible = false;
                    bt3.Text = Language._Close;
                    bt3.DialogResult = DialogResult.Cancel;
                    CancelButton = bt3;
                    break;
                case ETaskDialogButtons.Cancel:
                    bt1.Visible = false;
                    bt2.Visible = false;
                    bt3.Text = Language._Cancel;
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

            ControlBox = Buttons == ETaskDialogButtons.Cancel ||
                         Buttons == ETaskDialogButtons.Close ||
                         Buttons == ETaskDialogButtons.OkCancel ||
                         Buttons == ETaskDialogButtons.YesNoCancel;

            if (!showVerifyCheckbox && ExpandedInfo == "" && Buttons == ETaskDialogButtons.None)
                pnlButtons.Visible = false;
            else
                formHeight += pnlButtons.Height;

            pnlFooter.Visible = Footer != "";
            if (Footer != "")
            {
                AdjustLabelHeight(lbFooter);
                pnlFooter.Height = Math.Max(_display.ScaleHeight(28), lbFooter.Height + _display.ScaleHeight(16));
                switch (FooterIcon)
                {
                    case ESysIcons.Information:
                        imgFooter.Image = ResizeBitmap(SystemIcons.Information.ToBitmap(), 16, 16);
                        break;
                    case ESysIcons.Question:
                        imgFooter.Image = ResizeBitmap(SystemIcons.Question.ToBitmap(), 16, 16);
                        break;
                    case ESysIcons.Warning:
                        imgFooter.Image = ResizeBitmap(SystemIcons.Warning.ToBitmap(), 16, 16);
                        break;
                    case ESysIcons.Error:
                        imgFooter.Image = ResizeBitmap(SystemIcons.Error.ToBitmap(), 16, 16);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                formHeight += pnlFooter.Height;
            }

            ClientSize = new Size(ClientSize.Width, formHeight);

            _formBuilt = true;
            ThemeManager.getInstance().ThemeChanged += ApplyTheme;
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            if (!ThemeManager.getInstance().ActiveAndExtended) return;

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
            pnlCommandButtons.BackColor =
                ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            pnlCommandButtons.ForeColor =
                ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
            pnlMainInstruction.BackColor =
                ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            pnlMainInstruction.ForeColor =
                ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
            pnlContent.BackColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            pnlContent.ForeColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
            pnlExpandedInfo.BackColor =
                ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            pnlExpandedInfo.ForeColor =
                ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
            pnlRadioButtons.BackColor =
                ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            pnlRadioButtons.ForeColor =
                ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
        }

        //--------------------------------------------------------------------------------
        private Image ResizeBitmap(Image srcImg, int newWidth, int newHeight)
        {
            var percentWidth = _display.ScaleWidth(newWidth) / (float)srcImg.Width;
            var percentHeight = _display.ScaleHeight(newHeight) / (float)srcImg.Height;

            var resizePercent = percentHeight < percentWidth ? percentHeight : percentWidth;

            var w = (int)(srcImg.Width * resizePercent);
            var h = (int)(srcImg.Height * resizePercent);
            var b = new Bitmap(w, h);

            using (var g = Graphics.FromImage(b))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(srcImg, 0, 0, w, h);
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

            using (var g = Graphics.FromHwnd(lb.Handle))
            {
                var stringSize = g.MeasureString(text, textFont, layoutSize);
                lb.Height = (int)stringSize.Height + 4;
            }
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
            lbShowHideDetails.ImageIndex = Expanded ? 1 : 4;
        }

        //--------------------------------------------------------------------------------
        private void lbDetails_MouseLeave(object sender, EventArgs e)
        {
            lbShowHideDetails.ImageIndex = Expanded ? 0 : 3;
        }

        //--------------------------------------------------------------------------------
        private void lbDetails_MouseUp(object sender, MouseEventArgs e)
        {
            lbShowHideDetails.ImageIndex = Expanded ? 1 : 4;
        }

        //--------------------------------------------------------------------------------
        private void lbDetails_MouseDown(object sender, MouseEventArgs e)
        {
            lbShowHideDetails.ImageIndex = Expanded ? 2 : 5;
        }

        //--------------------------------------------------------------------------------
        private void lbDetails_Click(object sender, EventArgs e)
        {
            Expanded = !Expanded;
            pnlExpandedInfo.Visible = Expanded;
            lbShowHideDetails.Text = Expanded ? "        Hide details" : "        Show details";
            if (Expanded)
                Height += pnlExpandedInfo.Height;
            else
                Height -= pnlExpandedInfo.Height;
        }

        //--------------------------------------------------------------------------------
        private void frmTaskDialog_Shown(object sender, EventArgs e)
        {
            if (CTaskDialog.PlaySystemSounds)
            {
                switch (MainIcon)
                {
                    case ESysIcons.Error:
                        System.Media.SystemSounds.Hand.Play();
                        break;
                    case ESysIcons.Information:
                        System.Media.SystemSounds.Asterisk.Play();
                        break;
                    case ESysIcons.Question:
                        System.Media.SystemSounds.Asterisk.Play();
                        break;
                    case ESysIcons.Warning:
                        System.Media.SystemSounds.Exclamation.Play();
                        break;
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