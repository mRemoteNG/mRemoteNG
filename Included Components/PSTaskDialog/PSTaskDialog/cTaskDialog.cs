using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace PSTaskDialog
{
  //--------------------------------------------------------------------------------
  #region PUBLIC enums
  //--------------------------------------------------------------------------------
  public enum eSysIcons { Information, Question, Warning, Error };
  public enum eTaskDialogButtons { YesNo, YesNoCancel, OKCancel, OK, Close, Cancel, None }
  #endregion

  //--------------------------------------------------------------------------------
  public static class cTaskDialog
  {
    // PUBLIC static values...
    static public bool VerificationChecked = false;
    static public int RadioButtonResult = -1;
    static public int CommandButtonResult = -1;
    static public int EmulatedFormWidth = 450;
    static public bool ForceEmulationMode = false;
    static public bool UseToolWindowOnXP = true;
    static public bool PlaySystemSounds = true;
    static public EventHandler OnTaskDialogShown = null;
    static public EventHandler OnTaskDialogClosed = null;

    //--------------------------------------------------------------------------------
    #region ShowTaskDialogBox
    //--------------------------------------------------------------------------------
    static public DialogResult ShowTaskDialogBox(IWin32Window Owner,
                                                 string Title,
                                                 string MainInstruction,
                                                 string Content,
                                                 string ExpandedInfo,
                                                 string Footer,
                                                 string VerificationText,
                                                 string RadioButtons,
                                                 string CommandButtons,
                                                 eTaskDialogButtons Buttons,
                                                 eSysIcons MainIcon,
                                                 eSysIcons FooterIcon,
                                                 int DefaultIndex)

    {
      DialogResult result;
      if (OnTaskDialogShown != null)
        OnTaskDialogShown(null, EventArgs.Empty);

      if (VistaTaskDialog.IsAvailableOnThisOS && !ForceEmulationMode)
      {
        // [OPTION 1] Show Vista TaskDialog
        VistaTaskDialog vtd = new VistaTaskDialog();

        vtd.WindowTitle = Title;
        vtd.MainInstruction = MainInstruction;
        vtd.Content = Content;
        vtd.ExpandedInformation = ExpandedInfo;
        vtd.Footer = Footer;

        // Radio Buttons
        if (RadioButtons != "")
        {
          List<VistaTaskDialogButton> lst = new List<VistaTaskDialogButton>();
          string[] arr = RadioButtons.Split(new char[] { '|' });
          for (int i = 0; i < arr.Length; i++)
          {
            try
            {
              VistaTaskDialogButton button = new VistaTaskDialogButton();
              button.ButtonId = 1000 + i;
              button.ButtonText = arr[i];
              lst.Add(button);
            }
            catch (FormatException)
            {
            }
          }
          vtd.RadioButtons = lst.ToArray();
          vtd.NoDefaultRadioButton = (DefaultIndex == -1);
          if (DefaultIndex >= 0)
            vtd.DefaultRadioButton = DefaultIndex;
        }

        // Custom Buttons
        if (CommandButtons != "")
        {
          List<VistaTaskDialogButton> lst = new List<VistaTaskDialogButton>();
          string[] arr = CommandButtons.Split(new char[] { '|' });
          for (int i = 0; i < arr.Length; i++)
          {
            try
            {
              VistaTaskDialogButton button = new VistaTaskDialogButton();
              button.ButtonId = 2000 + i;
              button.ButtonText = arr[i];
              lst.Add(button);
            }
            catch (FormatException)
            {
            }
          }
          vtd.Buttons = lst.ToArray();
          if (DefaultIndex >= 0)
            vtd.DefaultButton = DefaultIndex;
        }

        switch (Buttons)
        {
          case eTaskDialogButtons.YesNo:
            vtd.CommonButtons = VistaTaskDialogCommonButtons.Yes | VistaTaskDialogCommonButtons.No;
            break;
          case eTaskDialogButtons.YesNoCancel:
            vtd.CommonButtons = VistaTaskDialogCommonButtons.Yes | VistaTaskDialogCommonButtons.No | VistaTaskDialogCommonButtons.Cancel;
            break;
          case eTaskDialogButtons.OKCancel:
            vtd.CommonButtons = VistaTaskDialogCommonButtons.Ok | VistaTaskDialogCommonButtons.Cancel;
            break;
          case eTaskDialogButtons.OK:
            vtd.CommonButtons = VistaTaskDialogCommonButtons.Ok;
            break;
          case eTaskDialogButtons.Close:
            vtd.CommonButtons = VistaTaskDialogCommonButtons.Close;
            break;
          case eTaskDialogButtons.Cancel:
            vtd.CommonButtons = VistaTaskDialogCommonButtons.Cancel;
            break;
          default:
            vtd.CommonButtons = 0;
            break;
        }

        switch (MainIcon)
        {
          case eSysIcons.Information: vtd.MainIcon = VistaTaskDialogIcon.Information; break;
          case eSysIcons.Question: vtd.MainIcon = VistaTaskDialogIcon.Information; break;
          case eSysIcons.Warning: vtd.MainIcon = VistaTaskDialogIcon.Warning; break;
          case eSysIcons.Error: vtd.MainIcon = VistaTaskDialogIcon.Error; break;
        }

        switch (FooterIcon)
        {
          case eSysIcons.Information: vtd.FooterIcon = VistaTaskDialogIcon.Information; break;
          case eSysIcons.Question: vtd.FooterIcon = VistaTaskDialogIcon.Information; break;
          case eSysIcons.Warning: vtd.FooterIcon = VistaTaskDialogIcon.Warning; break;
          case eSysIcons.Error: vtd.FooterIcon = VistaTaskDialogIcon.Error; break;
        }

        vtd.EnableHyperlinks = false;
        vtd.ShowProgressBar = false;
        vtd.AllowDialogCancellation = (Buttons == eTaskDialogButtons.Cancel ||
                                       Buttons == eTaskDialogButtons.Close ||
                                       Buttons == eTaskDialogButtons.OKCancel ||
                                       Buttons == eTaskDialogButtons.YesNoCancel);
        vtd.CallbackTimer = false;
        vtd.ExpandedByDefault = false;
        vtd.ExpandFooterArea = false;
        vtd.PositionRelativeToWindow = true;
        vtd.RightToLeftLayout = false;
        vtd.NoDefaultRadioButton = false;
        vtd.CanBeMinimized = false;
        vtd.ShowMarqueeProgressBar = false;
        vtd.UseCommandLinks = (CommandButtons != "");
        vtd.UseCommandLinksNoIcon = false;
        vtd.VerificationText = VerificationText;
        vtd.VerificationFlagChecked = false;
        vtd.ExpandedControlText = "Hide details";
        vtd.CollapsedControlText = "Show details";
        vtd.Callback = null;

        // Show the Dialog
        result = (DialogResult)vtd.Show((vtd.CanBeMinimized ? null : Owner), out VerificationChecked, out RadioButtonResult);

        // if a command button was clicked, then change return result
        // to "DialogResult.OK" and set the CommandButtonResult
        if ((int)result >= 2000)
        {
          CommandButtonResult = ((int)result - 2000);
          result = DialogResult.OK;
        }
        if (RadioButtonResult >= 1000)
          RadioButtonResult -= 1000;  // deduct the ButtonID start value for radio buttons
      }
      else
      {
        // [OPTION 2] Show Emulated Form
        using (frmTaskDialog td = new frmTaskDialog())
        {
          td.Title = Title;
          td.MainInstruction = MainInstruction;
          td.Content = Content;
          td.ExpandedInfo = ExpandedInfo;
          td.Footer = Footer;
          td.RadioButtons = RadioButtons;
          td.CommandButtons = CommandButtons;
          td.Buttons = Buttons;
          td.MainIcon = MainIcon;
          td.FooterIcon = FooterIcon;
          td.VerificationText = VerificationText;
          td.Width = EmulatedFormWidth;
          td.DefaultButtonIndex = DefaultIndex;
          td.BuildForm();
          result = td.ShowDialog(Owner);

          RadioButtonResult = td.RadioButtonIndex;
          CommandButtonResult = td.CommandButtonClickedIndex;
          VerificationChecked = td.VerificationCheckBoxChecked;
        }
      }
      if (OnTaskDialogClosed != null)
        OnTaskDialogClosed(null, EventArgs.Empty);
      return result;
    }

    //--------------------------------------------------------------------------------
    // Overloaded versions...
    //--------------------------------------------------------------------------------
    static public DialogResult ShowTaskDialogBox(IWin32Window Owner,
                                                 string Title,
                                                 string MainInstruction,
                                                 string Content,
                                                 string ExpandedInfo,
                                                 string Footer,
                                                 string VerificationText,
                                                 string RadioButtons,
                                                 string CommandButtons,
                                                 eTaskDialogButtons Buttons,
                                                 eSysIcons MainIcon,
                                                 eSysIcons FooterIcon)
    {
      return ShowTaskDialogBox(Owner, Title, MainInstruction, Content, ExpandedInfo, Footer, VerificationText, RadioButtons, CommandButtons, Buttons, MainIcon, FooterIcon, 0);
    }

    static public DialogResult ShowTaskDialogBox(string Title,
                                                 string MainInstruction,
                                                 string Content,
                                                 string ExpandedInfo,
                                                 string Footer,
                                                 string VerificationText,
                                                 string RadioButtons,
                                                 string CommandButtons,
                                                 eTaskDialogButtons Buttons,
                                                 eSysIcons MainIcon,
                                                 eSysIcons FooterIcon)
    {
      return ShowTaskDialogBox(null, Title, MainInstruction, Content, ExpandedInfo, Footer, VerificationText, RadioButtons, CommandButtons, Buttons, MainIcon, FooterIcon, 0);
    }

    #endregion

    //--------------------------------------------------------------------------------
    #region MessageBox
    //--------------------------------------------------------------------------------
    static public DialogResult MessageBox(IWin32Window Owner,
                                          string Title,
                                          string MainInstruction,
                                          string Content,
                                          string ExpandedInfo,
                                          string Footer,
                                          string VerificationText,
                                          eTaskDialogButtons Buttons,
                                          eSysIcons MainIcon,
                                          eSysIcons FooterIcon)
    {
      return ShowTaskDialogBox(Owner, Title, MainInstruction, Content, ExpandedInfo, Footer, VerificationText, "", "", Buttons, MainIcon, FooterIcon);
    }

    //--------------------------------------------------------------------------------
    // Overloaded versions...
    //--------------------------------------------------------------------------------
    static public DialogResult MessageBox(string Title,
                                          string MainInstruction,
                                          string Content,
                                          string ExpandedInfo,
                                          string Footer,
                                          string VerificationText,
                                          eTaskDialogButtons Buttons,
                                          eSysIcons MainIcon,
                                          eSysIcons FooterIcon)
    {
      return ShowTaskDialogBox(null, Title, MainInstruction, Content, ExpandedInfo, Footer, VerificationText, "", "", Buttons, MainIcon, FooterIcon);
    }

    static public DialogResult MessageBox(IWin32Window Owner, 
                                          string Title,
                                          string MainInstruction,
                                          string Content,
                                          eTaskDialogButtons Buttons,
                                          eSysIcons MainIcon)
    {
      return MessageBox(Owner, Title, MainInstruction, Content, "", "", "", Buttons, MainIcon, eSysIcons.Information);
    }

    static public DialogResult MessageBox(string Title,
                                          string MainInstruction,
                                          string Content,
                                          eTaskDialogButtons Buttons,
                                          eSysIcons MainIcon)
    {
      return MessageBox(null, Title, MainInstruction, Content, "", "", "", Buttons, MainIcon, eSysIcons.Information);
    }

    //--------------------------------------------------------------------------------
    #endregion

    //--------------------------------------------------------------------------------
    #region ShowRadioBox
    //--------------------------------------------------------------------------------
    static public int ShowRadioBox(IWin32Window Owner,
                                   string Title,
                                   string MainInstruction,
                                   string Content,
                                   string ExpandedInfo,
                                   string Footer,
                                   string VerificationText,
                                   string RadioButtons,
                                   eSysIcons MainIcon,
                                   eSysIcons FooterIcon,
                                   int DefaultIndex)
    {
      DialogResult res = ShowTaskDialogBox(Owner, Title, MainInstruction, Content, ExpandedInfo, Footer, VerificationText,
                                           RadioButtons, "", eTaskDialogButtons.OKCancel, MainIcon, FooterIcon, DefaultIndex);
      if (res == DialogResult.OK)
        return RadioButtonResult;
      else
        return -1;
    }

    //--------------------------------------------------------------------------------
    // Overloaded versions...
    //--------------------------------------------------------------------------------
    static public int ShowRadioBox(string Title,
                                   string MainInstruction,
                                   string Content,
                                   string ExpandedInfo,
                                   string Footer,
                                   string VerificationText,
                                   string RadioButtons,
                                   eSysIcons MainIcon,
                                   eSysIcons FooterIcon,
                                   int DefaultIndex)
    {
      DialogResult res = ShowTaskDialogBox(null, Title, MainInstruction, Content, ExpandedInfo, Footer, VerificationText,
                                           RadioButtons, "", eTaskDialogButtons.OKCancel, MainIcon, FooterIcon, DefaultIndex);
      if (res == DialogResult.OK)
        return RadioButtonResult;
      else
        return -1;
    }

    static public int ShowRadioBox(IWin32Window Owner,
                                   string Title,
                                   string MainInstruction,
                                   string Content,
                                   string ExpandedInfo,
                                   string Footer,
                                   string VerificationText,
                                   string RadioButtons,
                                   eSysIcons MainIcon,
                                   eSysIcons FooterIcon)
    {
      return ShowRadioBox(Owner, Title, MainInstruction, Content, ExpandedInfo, Footer, VerificationText, RadioButtons, eSysIcons.Question, eSysIcons.Information, 0);
    }

    static public int ShowRadioBox(IWin32Window Owner,
                                   string Title,
                                   string MainInstruction,
                                   string Content,
                                   string RadioButtons,
                                   int DefaultIndex)
    {
      return ShowRadioBox(Owner, Title, MainInstruction, Content, "", "", "", RadioButtons, eSysIcons.Question, eSysIcons.Information, DefaultIndex);
    }

    static public int ShowRadioBox(IWin32Window Owner,
                                   string Title,
                                   string MainInstruction,
                                   string Content,
                                   string RadioButtons)
    {
      return ShowRadioBox(Owner, Title, MainInstruction, Content, "", "", "", RadioButtons, eSysIcons.Question, eSysIcons.Information, 0);
    }

    static public int ShowRadioBox(string Title,
                                   string MainInstruction,
                                   string Content,
                                   string RadioButtons)
    {
      return ShowRadioBox(null, Title, MainInstruction, Content, "", "", "", RadioButtons, eSysIcons.Question, eSysIcons.Information, 0);
    }
    #endregion

    //--------------------------------------------------------------------------------
    #region ShowCommandBox
    //--------------------------------------------------------------------------------
    static public int ShowCommandBox(IWin32Window Owner,
                                     string Title,
                                     string MainInstruction,
                                     string Content,
                                     string ExpandedInfo,
                                     string Footer,
                                     string VerificationText,
                                     string CommandButtons,
                                     bool ShowCancelButton,
                                     eSysIcons MainIcon,
                                     eSysIcons FooterIcon)
    {
      DialogResult res = ShowTaskDialogBox(Owner, Title, MainInstruction, Content, ExpandedInfo, Footer, VerificationText,
                                           "", CommandButtons, (ShowCancelButton ? eTaskDialogButtons.Cancel : eTaskDialogButtons.None),
                                           MainIcon, FooterIcon);
      if (res == DialogResult.OK)
        return CommandButtonResult;
      else
        return -1;
    }

    //--------------------------------------------------------------------------------
    // Overloaded versions...
    //--------------------------------------------------------------------------------
    static public int ShowCommandBox(string Title,
                                     string MainInstruction,
                                     string Content,
                                     string ExpandedInfo,
                                     string Footer,
                                     string VerificationText,
                                     string CommandButtons,
                                     bool ShowCancelButton,
                                     eSysIcons MainIcon,
                                     eSysIcons FooterIcon)
    {
      DialogResult res = ShowTaskDialogBox(null, Title, MainInstruction, Content, ExpandedInfo, Footer, VerificationText,
                                           "", CommandButtons, (ShowCancelButton ? eTaskDialogButtons.Cancel : eTaskDialogButtons.None),
                                           MainIcon, FooterIcon);
      if (res == DialogResult.OK)
        return CommandButtonResult;
      else
        return -1;
    }

    static public int ShowCommandBox(IWin32Window Owner, string Title, string MainInstruction, string Content, string CommandButtons, bool ShowCancelButton)
    {
      return ShowCommandBox(Owner, Title, MainInstruction, Content, "", "", "", CommandButtons, ShowCancelButton, eSysIcons.Question, eSysIcons.Information);
    }

    static public int ShowCommandBox(string Title, string MainInstruction, string Content, string CommandButtons, bool ShowCancelButton)
    {
      return ShowCommandBox(null, Title, MainInstruction, Content, "", "", "", CommandButtons, ShowCancelButton, eSysIcons.Question, eSysIcons.Information);
    }

    #endregion

    //--------------------------------------------------------------------------------
  }
}
