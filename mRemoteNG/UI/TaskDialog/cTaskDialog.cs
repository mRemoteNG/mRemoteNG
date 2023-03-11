using System;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace mRemoteNG.UI.TaskDialog
{
    [SupportedOSPlatform("windows")]
    #region PUBLIC enums
    public enum ESysIcons
    {
        Information,
        Question,
        Warning,
        Error
    }

    public enum ETaskDialogButtons
    {
        YesNo,
        YesNoCancel,
        OkCancel,
        Ok,
        Close,
        Cancel,
        None
    }
    #endregion

    public static class CTaskDialog
    {
        // PUBLIC static values...
        public static bool VerificationChecked;
        public static int RadioButtonResult = -1;
        public static int CommandButtonResult = -1;
        public static int EmulatedFormWidth = 450;
        public static bool ForceEmulationMode = false;
        public static bool UseToolWindowOnXp = true;
        public static bool PlaySystemSounds = true;
        public static EventHandler OnTaskDialogShown = null;
        public static EventHandler OnTaskDialogClosed = null;

        #region [ShowTaskDialogBox]

        public static DialogResult ShowTaskDialogBox(IWin32Window owner, string title, string mainInstruction, string content, string expandedInfo, string footer, string verificationText, string radioButtons, string commandButtons, ETaskDialogButtons buttons, ESysIcons mainIcon, ESysIcons footerIcon, int defaultIndex)
        {
            DialogResult result;
            OnTaskDialogShown?.Invoke(null, EventArgs.Empty);

            using (var td = new frmTaskDialog())
            {
                var display = new DisplayProperties();
                td.Title = title;
                td.MainInstruction = mainInstruction;
                td.Content = content;
                td.ExpandedInfo = expandedInfo;
                td.Footer = footer;
                td.RadioButtons = radioButtons;
                td.CommandButtons = commandButtons;
                td.Buttons = buttons;
                td.MainIcon = mainIcon;
                td.FooterIcon = footerIcon;
                td.VerificationText = verificationText;
                td.Width = display.ScaleWidth(EmulatedFormWidth);
                td.DefaultButtonIndex = defaultIndex;
                td.BuildForm();
                result = td.ShowDialog(owner);

                RadioButtonResult = td.RadioButtonIndex;
                CommandButtonResult = td.CommandButtonClickedIndex;
                VerificationChecked = td.VerificationCheckBoxChecked;
            }

            OnTaskDialogClosed?.Invoke(null, EventArgs.Empty);
            return result;
        }

        //--------------------------------------------------------------------------------
        // Overloaded versions...
        //--------------------------------------------------------------------------------
        public static DialogResult ShowTaskDialogBox(IWin32Window owner,
                                                     string title,
                                                     string mainInstruction,
                                                     string content,
                                                     string expandedInfo,
                                                     string footer,
                                                     string verificationText,
                                                     string radioButtons,
                                                     string commandButtons,
                                                     ETaskDialogButtons buttons,
                                                     ESysIcons mainIcon,
                                                     ESysIcons footerIcon)
        {
            return ShowTaskDialogBox(owner, title, mainInstruction, content, expandedInfo, footer, verificationText, radioButtons, commandButtons, buttons, mainIcon, footerIcon, 0);
        }

        public static DialogResult ShowTaskDialogBox(string title,
                                                     string mainInstruction,
                                                     string content,
                                                     string expandedInfo,
                                                     string footer,
                                                     string verificationText,
                                                     string radioButtons,
                                                     string commandButtons,
                                                     ETaskDialogButtons buttons,
                                                     ESysIcons mainIcon,
                                                     ESysIcons footerIcon)
        {
            return ShowTaskDialogBox(null, title, mainInstruction, content, expandedInfo, footer, verificationText, radioButtons, commandButtons, buttons, mainIcon, footerIcon, 0);
        }

        #endregion

        #region [MessageBox]
        public static DialogResult MessageBox(IWin32Window owner, string title, string mainInstruction, string content, string expandedInfo, string footer, string verificationText, ETaskDialogButtons buttons, ESysIcons mainIcon, ESysIcons footerIcon)
        {
            return ShowTaskDialogBox(owner, title, mainInstruction, content, expandedInfo, footer, verificationText, "", "", buttons, mainIcon, footerIcon);
        }

        //--------------------------------------------------------------------------------
        // Overloaded versions...
        //--------------------------------------------------------------------------------
        public static DialogResult MessageBox(string title, string mainInstruction, string content, string expandedInfo, string footer, string verificationText, ETaskDialogButtons buttons, ESysIcons mainIcon, ESysIcons footerIcon)
        {
            return ShowTaskDialogBox(null, title, mainInstruction, content, expandedInfo, footer, verificationText, "", "", buttons, mainIcon, footerIcon);
        }

        public static DialogResult MessageBox(IWin32Window owner, string title, string mainInstruction, string content, ETaskDialogButtons buttons, ESysIcons mainIcon)
        {
            return MessageBox(owner, title, mainInstruction, content, "", "", "", buttons, mainIcon, ESysIcons.Information);
        }

        public static DialogResult MessageBox(string title, string mainInstruction, string content, ETaskDialogButtons buttons, ESysIcons mainIcon)
        {
            return MessageBox(null, title, mainInstruction, content, "", "", "", buttons, mainIcon, ESysIcons.Information);
        }

        //--------------------------------------------------------------------------------

        #endregion

        //--------------------------------------------------------------------------------

        #region [ShowRadioBox]

        //--------------------------------------------------------------------------------
        public static int ShowRadioBox(IWin32Window owner, string title, string mainInstruction, string content, string expandedInfo, string footer, string verificationText, string radioButtons, ESysIcons mainIcon, ESysIcons footerIcon, int defaultIndex)
        {
            var res = ShowTaskDialogBox(owner, title, mainInstruction, content, expandedInfo, footer, verificationText, radioButtons, "", ETaskDialogButtons.OkCancel, mainIcon, footerIcon, defaultIndex);
            if (res == DialogResult.OK)
                return RadioButtonResult;
            return -1;
        }

        //--------------------------------------------------------------------------------
        // Overloaded versions...
        //--------------------------------------------------------------------------------
        public static int ShowRadioBox(string title, string mainInstruction, string content, string expandedInfo, string footer, string verificationText, string radioButtons, ESysIcons mainIcon, ESysIcons footerIcon, int defaultIndex)
        {
            var res = ShowTaskDialogBox(null, title, mainInstruction, content, expandedInfo, footer, verificationText, radioButtons, "", ETaskDialogButtons.OkCancel, mainIcon, footerIcon, defaultIndex);
            if (res == DialogResult.OK)
                return RadioButtonResult;
            return -1;
        }

        public static int ShowRadioBox(IWin32Window owner, string title, string mainInstruction, string content, string expandedInfo, string footer, string verificationText, string radioButtons, ESysIcons mainIcon, ESysIcons footerIcon)
        {
            return ShowRadioBox(owner, title, mainInstruction, content, expandedInfo, footer, verificationText, radioButtons, ESysIcons.Question, ESysIcons.Information, 0);
        }

        public static int ShowRadioBox(IWin32Window owner, string title, string mainInstruction, string content, string radioButtons, int defaultIndex)
        {
            return ShowRadioBox(owner, title, mainInstruction, content, "", "", "", radioButtons, ESysIcons.Question, ESysIcons.Information, defaultIndex);
        }

        public static int ShowRadioBox(IWin32Window owner, string title, string mainInstruction, string content, string radioButtons)
        {
            return ShowRadioBox(owner, title, mainInstruction, content, "", "", "", radioButtons, ESysIcons.Question, ESysIcons.Information, 0);
        }

        public static int ShowRadioBox(string title, string mainInstruction, string content, string radioButtons)
        {
            return ShowRadioBox(null, title, mainInstruction, content, "", "", "", radioButtons, ESysIcons.Question, ESysIcons.Information, 0);
        }

        #endregion

        //--------------------------------------------------------------------------------

        #region ShowCommandBox

        //--------------------------------------------------------------------------------
        public static int ShowCommandBox(IWin32Window owner, string title, string mainInstruction, string content, string expandedInfo, string footer, string verificationText,string commandButtons, bool showCancelButton, ESysIcons mainIcon, ESysIcons footerIcon)
        {
            var res = ShowTaskDialogBox(owner, title, mainInstruction, content, expandedInfo, footer, verificationText, "", commandButtons, showCancelButton ? ETaskDialogButtons.Cancel : ETaskDialogButtons.None, mainIcon, footerIcon);
            if (res == DialogResult.OK)
                return CommandButtonResult;
            return -1;
        }

        //--------------------------------------------------------------------------------
        // Overloaded versions...
        //--------------------------------------------------------------------------------
        public static int ShowCommandBox(string title, string mainInstruction, string content, string expandedInfo, string footer, string verificationText, string commandButtons, bool showCancelButton, ESysIcons mainIcon, ESysIcons footerIcon)
        {
            var res = ShowTaskDialogBox(null, title, mainInstruction, content, expandedInfo, footer, verificationText,"", commandButtons, showCancelButton ? ETaskDialogButtons.Cancel : ETaskDialogButtons.None, mainIcon, footerIcon);
            if (res == DialogResult.OK)
                return CommandButtonResult;
            return -1;
        }

        public static int ShowCommandBox(IWin32Window owner, string title, string mainInstruction, string content, string commandButtons, bool showCancelButton)
        {
            return ShowCommandBox(owner, title, mainInstruction, content, "", "", "", commandButtons, showCancelButton, ESysIcons.Question, ESysIcons.Information);
        }

        public static int ShowCommandBox(string title, string mainInstruction, string content, string commandButtons, bool showCancelButton)
        {
            return ShowCommandBox(null, title, mainInstruction, content, "", "", "", commandButtons, showCancelButton, ESysIcons.Question, ESysIcons.Information);
        }

        #endregion
    }
}