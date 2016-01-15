using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PSTaskDialog
{
  public partial class frmTaskDialog : Form
  {
    //--------------------------------------------------------------------------------
    #region PRIVATE members
    //--------------------------------------------------------------------------------
    eSysIcons m_mainIcon = eSysIcons.Question;
    eSysIcons m_footerIcon = eSysIcons.Warning;

    string m_mainInstruction = "Main Instruction Text";
    int m_mainInstructionHeight = 0;
    Font m_mainInstructionFont = new Font("Arial", 11.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);

    List<RadioButton> m_radioButtonCtrls = new List<RadioButton>();
    string m_radioButtons = "";
    int m_initialRadioButtonIndex = 0;

    List<Button> m_cmdButtons = new List<Button>();
    string m_commandButtons = "";
    int m_commandButtonClicked = -1;

    int m_defaultButtonIndex = 0;
    Control m_focusControl = null;

    eTaskDialogButtons m_Buttons = eTaskDialogButtons.YesNoCancel;

    bool m_Expanded = false;
    bool m_isVista = false;
    #endregion

    //--------------------------------------------------------------------------------
    #region PROPERTIES
    //--------------------------------------------------------------------------------
    public eSysIcons MainIcon { get { return m_mainIcon; } set { m_mainIcon = value; } }
    public eSysIcons FooterIcon { get { return m_footerIcon; } set { m_footerIcon = value; } }

    public string Title { get { return this.Text; } set { this.Text = value; } }
    public string MainInstruction { get { return m_mainInstruction; } set { m_mainInstruction = value; this.Invalidate(); } }
    public string Content { get { return lbContent.Text; } set { lbContent.Text = value; } }
    public string ExpandedInfo { get { return lbExpandedInfo.Text; } set { lbExpandedInfo.Text = value; } }
    public string Footer { get { return lbFooter.Text; } set { lbFooter.Text = value; } }
    public int DefaultButtonIndex { get { return m_defaultButtonIndex; } set { m_defaultButtonIndex = value; } }

    public string RadioButtons { get { return m_radioButtons; } set { m_radioButtons = value; } }
    public int InitialRadioButtonIndex { get { return m_initialRadioButtonIndex; } set { m_initialRadioButtonIndex = value; } }
    public int RadioButtonIndex
    {
      get
      {
        foreach (RadioButton rb in m_radioButtonCtrls)
          if (rb.Checked)
            return (int)rb.Tag;
        return -1;
      }
    }

    public string CommandButtons { get { return m_commandButtons; } set { m_commandButtons = value; } }
    public int CommandButtonClickedIndex { get { return m_commandButtonClicked; } }

    public eTaskDialogButtons Buttons { get { return m_Buttons; } set { m_Buttons = value; } }

    public string VerificationText { get { return cbVerify.Text; } set { cbVerify.Text = value; } }
    public bool VerificationCheckBoxChecked { get { return cbVerify.Checked; } set { cbVerify.Checked = value; } }

    public bool Expanded { get { return m_Expanded; } set { m_Expanded = value; } }
    #endregion

    //--------------------------------------------------------------------------------
    #region CONSTRUCTOR
    //--------------------------------------------------------------------------------
    public frmTaskDialog()
    {
      InitializeComponent();

      m_isVista = VistaTaskDialog.IsAvailableOnThisOS;
      if (!m_isVista && cTaskDialog.UseToolWindowOnXP) // <- shall we use the smaller toolbar?
          this.FormBorderStyle = FormBorderStyle.FixedToolWindow;

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
    bool m_formBuilt = false;
    public void BuildForm()
    {
      int form_height = 0;

      // Setup Main Instruction
      switch (m_mainIcon)
      {
        case eSysIcons.Information: imgMain.Image = SystemIcons.Information.ToBitmap(); break;
        case eSysIcons.Question: imgMain.Image = SystemIcons.Question.ToBitmap(); break;
        case eSysIcons.Warning: imgMain.Image = SystemIcons.Warning.ToBitmap(); break;
        case eSysIcons.Error: imgMain.Image = SystemIcons.Error.ToBitmap(); break;
      }

      //AdjustLabelHeight(lbMainInstruction);
      //pnlMainInstruction.Height = Math.Max(41, lbMainInstruction.Height + 16);
      if (m_mainInstructionHeight == 0)
        GetMainInstructionTextSizeF();
      pnlMainInstruction.Height = Math.Max(41, m_mainInstructionHeight + 16);

      form_height += pnlMainInstruction.Height;

      // Setup Content
      pnlContent.Visible = (Content != "");
      if (Content != "")
      {
        AdjustLabelHeight(lbContent);
        pnlContent.Height = lbContent.Height + 4;
        form_height += pnlContent.Height;
      }

      bool show_verify_checkbox = (cbVerify.Text != "");
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
        pnlExpandedInfo.Visible = m_Expanded;
        lbShowHideDetails.Text = (m_Expanded ? "        Hide details" : "        Show details");
        lbShowHideDetails.ImageIndex = (m_Expanded ? 0 : 3);
        if (!show_verify_checkbox)
          pnlButtons.Height = 40;
        if (m_Expanded)
          form_height += pnlExpandedInfo.Height;
      }

      // Setup RadioButtons
      pnlRadioButtons.Visible = (m_radioButtons != "");
      if (m_radioButtons != "")
      {
        string[] arr = m_radioButtons.Split(new char[] { '|' });
        int pnl_height = 12;
        for (int i = 0; i < arr.Length; i++)
        {
          RadioButton rb = new RadioButton();
          rb.Parent = pnlRadioButtons;
          rb.Location = new Point(60, 4 + (i * rb.Height));
          rb.Text = arr[i];
          rb.Tag = i;
          rb.Checked = (m_defaultButtonIndex == i);
          rb.Width = this.Width - rb.Left - 15;
          pnl_height += rb.Height;
          m_radioButtonCtrls.Add(rb);
        }
        pnlRadioButtons.Height = pnl_height;
        form_height += pnlRadioButtons.Height;
      }

      // Setup CommandButtons
      pnlCommandButtons.Visible = (m_commandButtons != "");
      if (m_commandButtons != "")
      {
        string[] arr = m_commandButtons.Split(new char[] { '|' });
        int t = 8;
        int pnl_height = 16;
        for (int i = 0; i < arr.Length; i++)
        {
          CommandButton btn = new CommandButton();
          btn.Parent = pnlCommandButtons;
          btn.Location = new Point(50, t);
          if (m_isVista)  // <- tweak font if vista
            btn.Font = new Font(btn.Font, FontStyle.Regular);
          btn.Text = arr[i];
          btn.Size = new Size(this.Width - btn.Left - 15, btn.GetBestHeight());
          t += btn.Height;
          pnl_height += btn.Height;
          btn.Tag = i;
          btn.Click += new EventHandler(CommandButton_Click);
          if (i == m_defaultButtonIndex)
            m_focusControl = btn;
        }
        pnlCommandButtons.Height = pnl_height;
        form_height += pnlCommandButtons.Height;
      }

      // Setup Buttons
      switch (m_Buttons)
      {
        case eTaskDialogButtons.YesNo:
          bt1.Visible = false;
          bt2.Text = "&Yes";
          bt2.DialogResult = DialogResult.Yes;
          bt3.Text = "&No";
          bt3.DialogResult = DialogResult.No;
          this.AcceptButton = bt2;
          this.CancelButton = bt3;
          break;
        case eTaskDialogButtons.YesNoCancel:
          bt1.Text = "&Yes";
          bt1.DialogResult = DialogResult.Yes;
          bt2.Text = "&No";
          bt2.DialogResult = DialogResult.No;
          bt3.Text = "&Cancel";
          bt3.DialogResult = DialogResult.Cancel;
          this.AcceptButton = bt1;
          this.CancelButton = bt3;
          break;
        case eTaskDialogButtons.OKCancel:
          bt1.Visible = false;
          bt2.Text = "&OK";
          bt2.DialogResult = DialogResult.OK;
          bt3.Text = "&Cancel";
          bt3.DialogResult = DialogResult.Cancel;
          this.AcceptButton = bt2;
          this.CancelButton = bt3;
          break;
        case eTaskDialogButtons.OK:
          bt1.Visible = false;
          bt2.Visible = false;
          bt3.Text = "&OK";
          bt3.DialogResult = DialogResult.OK;
          this.AcceptButton = bt3;
          this.CancelButton = bt3;
          break;
        case eTaskDialogButtons.Close:
          bt1.Visible = false;
          bt2.Visible = false;
          bt3.Text = "&Close";
          bt3.DialogResult = DialogResult.Cancel;
          this.CancelButton = bt3;
          break;
        case eTaskDialogButtons.Cancel:
          bt1.Visible = false;
          bt2.Visible = false;
          bt3.Text = "&Cancel";
          bt3.DialogResult = DialogResult.Cancel;
          this.CancelButton = bt3;
          break;
        case eTaskDialogButtons.None:
          bt1.Visible = false;
          bt2.Visible = false;
          bt3.Visible = false;
          break;
      }

      this.ControlBox = (Buttons == eTaskDialogButtons.Cancel ||
                         Buttons == eTaskDialogButtons.Close ||
                         Buttons == eTaskDialogButtons.OKCancel ||
                         Buttons == eTaskDialogButtons.YesNoCancel);

      if (!show_verify_checkbox && ExpandedInfo == "" && m_Buttons == eTaskDialogButtons.None)
        pnlButtons.Visible = false;
      else
        form_height += pnlButtons.Height;

      pnlFooter.Visible = (Footer != "");
      if (Footer != "")
      {
        AdjustLabelHeight(lbFooter);
        pnlFooter.Height = Math.Max(28, lbFooter.Height + 16);
        switch (m_footerIcon)
        {
          case eSysIcons.Information:
            // SystemIcons.Information.ToBitmap().GetThumbnailImage(16, 16, null, IntPtr.Zero);
            imgFooter.Image = ResizeBitmap(SystemIcons.Information.ToBitmap(), 16, 16);
            break;
          case eSysIcons.Question:
            // SystemIcons.Question.ToBitmap().GetThumbnailImage(16, 16, null, IntPtr.Zero);
            imgFooter.Image = ResizeBitmap(SystemIcons.Question.ToBitmap(), 16, 16);
            break;
          case eSysIcons.Warning:
            // SystemIcons.Warning.ToBitmap().GetThumbnailImage(16, 16, null, IntPtr.Zero);
            imgFooter.Image = ResizeBitmap(SystemIcons.Warning.ToBitmap(), 16, 16);
            break;
          case eSysIcons.Error:
            // SystemIcons.Error.ToBitmap().GetThumbnailImage(16, 16, null, IntPtr.Zero);
            imgFooter.Image = ResizeBitmap(SystemIcons.Error.ToBitmap(), 16, 16);
            break;
        }
        form_height += pnlFooter.Height;
      }

      this.ClientSize = new Size(ClientSize.Width, form_height);

      m_formBuilt = true;
    }

    //--------------------------------------------------------------------------------
    Image ResizeBitmap(Image SrcImg, int NewWidth, int NewHeight)
    {
      float percent_width = (NewWidth / (float)SrcImg.Width);
      float percent_height = (NewHeight / (float)SrcImg.Height);

      float resize_percent = (percent_height < percent_width ? percent_height : percent_width);

      int w = (int)(SrcImg.Width * resize_percent);
      int h = (int)(SrcImg.Height * resize_percent);
      Bitmap b = new Bitmap(w, h);

      using (Graphics g = Graphics.FromImage(b))
      {
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        g.DrawImage(SrcImg, 0, 0, w, h);
      }
      return b;
    }
    
    //--------------------------------------------------------------------------------
    // utility function for setting a Label's height
    void AdjustLabelHeight(Label lb)
    {
      string text = lb.Text;
      Font textFont = lb.Font;
      SizeF layoutSize = new SizeF(lb.ClientSize.Width, 5000.0F);
      Graphics g = Graphics.FromHwnd(lb.Handle);
      SizeF stringSize = g.MeasureString(text, textFont, layoutSize);
      lb.Height = (int)stringSize.Height + 4;
      g.Dispose();
    }
    #endregion

    //--------------------------------------------------------------------------------
    #region EVENTS
    //--------------------------------------------------------------------------------
    void CommandButton_Click(object sender, EventArgs e)
    {
     	m_commandButtonClicked = (int)((CommandButton)sender).Tag;
      this.DialogResult = DialogResult.OK;
    }

    //--------------------------------------------------------------------------------
    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
    }

    //--------------------------------------------------------------------------------
    protected override void OnShown(EventArgs e)
    {
      if (!m_formBuilt)
        throw new Exception("frmTaskDialog : Please call .BuildForm() before showing the TaskDialog");
      base.OnShown(e);
    }

    //--------------------------------------------------------------------------------
    private void lbDetails_MouseEnter(object sender, EventArgs e)
    {
      lbShowHideDetails.ImageIndex = (m_Expanded ? 1 : 4);
    }

    //--------------------------------------------------------------------------------
    private void lbDetails_MouseLeave(object sender, EventArgs e)
    {
      lbShowHideDetails.ImageIndex = (m_Expanded ? 0 : 3);
    }

    //--------------------------------------------------------------------------------
    private void lbDetails_MouseUp(object sender, MouseEventArgs e)
    {
      lbShowHideDetails.ImageIndex = (m_Expanded ? 1 : 4);
    }

    //--------------------------------------------------------------------------------
    private void lbDetails_MouseDown(object sender, MouseEventArgs e)
    {
      lbShowHideDetails.ImageIndex =(m_Expanded ? 2 : 5);
    }

    //--------------------------------------------------------------------------------
    private void lbDetails_Click(object sender, EventArgs e)
    {
      m_Expanded = !m_Expanded;
      pnlExpandedInfo.Visible = m_Expanded;
      lbShowHideDetails.Text = (m_Expanded ? "        Hide details" : "        Show details");
      if (m_Expanded)
        this.Height += pnlExpandedInfo.Height;
      else
        this.Height -= pnlExpandedInfo.Height;
    }

    //--------------------------------------------------------------------------------
    const int MAIN_INSTRUCTION_LEFT_MARGIN = 46;
    const int MAIN_INSTRUCTION_RIGHT_MARGIN = 8;

    SizeF GetMainInstructionTextSizeF()
    {
      SizeF mzSize = new SizeF(pnlMainInstruction.Width - MAIN_INSTRUCTION_LEFT_MARGIN - MAIN_INSTRUCTION_RIGHT_MARGIN, 5000.0F);
      Graphics g = Graphics.FromHwnd(this.Handle);
      SizeF textSize = g.MeasureString(m_mainInstruction, m_mainInstructionFont, mzSize);
      m_mainInstructionHeight = (int)textSize.Height;
      return textSize;
    }

    private void pnlMainInstruction_Paint(object sender, PaintEventArgs e)
    {
      SizeF szL = GetMainInstructionTextSizeF();
      e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
      e.Graphics.DrawString(m_mainInstruction, m_mainInstructionFont, new SolidBrush(Color.DarkBlue), new RectangleF(new PointF(MAIN_INSTRUCTION_LEFT_MARGIN, 10), szL));
    }

    //--------------------------------------------------------------------------------
    private void frmTaskDialog_Shown(object sender, EventArgs e)
    {
      if (cTaskDialog.PlaySystemSounds)
      {
        switch (m_mainIcon)
        {
          case eSysIcons.Error: System.Media.SystemSounds.Hand.Play(); break;
          case eSysIcons.Information: System.Media.SystemSounds.Asterisk.Play(); break;
          case eSysIcons.Question: System.Media.SystemSounds.Asterisk.Play(); break;
          case eSysIcons.Warning: System.Media.SystemSounds.Exclamation.Play(); break;
        }
      }
      if (m_focusControl != null)
        m_focusControl.Focus();
    }

    #endregion

    //--------------------------------------------------------------------------------
  }
}
