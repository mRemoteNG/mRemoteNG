using System;
using System.Diagnostics;
using System.Windows.Forms;
using mRemoteNG.UI.Window;
using mRemoteNG.App;
using mRemoteNG.UI.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Timer = System.Windows.Forms.Timer;

namespace mRemoteNG.Messages
{
    public class MessageCollector
    {
        private Timer _ecTimer;

        private ErrorAndInfoWindow McForm { get; }

        public MessageCollector(ErrorAndInfoWindow messageCollectorForm)
        {
            McForm = messageCollectorForm;
            CreateTimer();
        }

        #region Public Methods
        public void AddMessage(MessageClass msgClass, string msgText, bool onlyLog = false)
        {
            var nMsg = new Message(msgClass, msgText, DateTime.Now);

            if (nMsg.MsgClass == MessageClass.ReportMsg)
            {
                AddReportMessage(nMsg);
                return;
            }

            var enableTimer = true; // used to control if we SWITCH to the notifiation panel. Message will still be added regardless.

            if (nMsg.MsgClass == MessageClass.InformationMsg)
            {
                AddInfoMessage(nMsg);

                if (!Settings.Default.SwitchToMCOnInformation)
                    enableTimer = false;
            }

            if (nMsg.MsgClass == MessageClass.WarningMsg)
            {
                AddWarningMessage(nMsg);

                if (!Settings.Default.SwitchToMCOnWarning)
                    enableTimer = false;
            }

            if (nMsg.MsgClass == MessageClass.ErrorMsg)
            {
                AddErrorMessage(nMsg);

                if (!Settings.Default.SwitchToMCOnError)
                    enableTimer = false;
            }

            if (onlyLog)
                return;

            if (Settings.Default.ShowNoMessageBoxes)
            {
                /* These if statements need to be split so we can:
                 * control that no messages boxes will be dispalyed
                 * add items to the notifications panel
                 * NOT switch to the notification panel if configured that way
                 */
                if(enableTimer)
                    _ecTimer.Enabled = true;
            }
            else
                ShowMessageBox(nMsg);

            var lvItem = BuildListViewItem(nMsg);
            AddToList(lvItem);
        }

        private static void AddInfoMessage(Message nMsg)
        {
            Debug.Print("Info: " + nMsg.MsgText);
            if (Settings.Default.WriteLogFile)
                Logger.Instance.Info(nMsg.MsgText);
        }

        private static void AddWarningMessage(Message nMsg)
        {
            Debug.Print("Warning: " + nMsg.MsgText);
            if (Settings.Default.WriteLogFile)
                Logger.Instance.Warn(nMsg.MsgText);
        }

        private static void AddErrorMessage(Message nMsg)
        {
            Debug.Print("Error: " + nMsg.MsgText);
            Logger.Instance.Error(nMsg.MsgText);
        }

        private static void AddReportMessage(Message nMsg)
        {
            Debug.Print("Report: " + nMsg.MsgText);
            if (Settings.Default.WriteLogFile)
                Logger.Instance.Info(nMsg.MsgText);
        }

        private static ListViewItem BuildListViewItem(Message nMsg)
        {
            var lvItem = new ListViewItem
            {
                ImageIndex = Convert.ToInt32(nMsg.MsgClass),
                Text = nMsg.MsgText.Replace(Environment.NewLine, "  "),
                Tag = nMsg
            };
            return lvItem;
        }

        public void AddExceptionMessage(string message, Exception ex, MessageClass msgClass = MessageClass.ErrorMsg, bool logOnly = true)
        {
            AddMessage(msgClass, message + Environment.NewLine + Tools.MiscTools.GetExceptionMessageRecursive(ex), logOnly);
        }

        public void AddExceptionStackTrace(string message, Exception ex, MessageClass msgClass = MessageClass.ErrorMsg)
        {
            AddMessage(msgClass, message + Environment.NewLine + ex.StackTrace, true);
        }
        #endregion

        #region Private Methods
        private void CreateTimer()
        {
            _ecTimer = new Timer
            {
                Enabled = false,
                Interval = 300
            };
            _ecTimer.Tick += SwitchTimerTick;
        }

        private void SwitchTimerTick(object sender, EventArgs e)
        {
            SwitchToMessage();
            _ecTimer.Enabled = false;
        }

        private void SwitchToMessage()
        {
            McForm.PreviousActiveForm = (DockContent)frmMain.Default.pnlDock.ActiveContent;
            ShowMcForm();
            McForm.lvErrorCollector.Focus();
            McForm.lvErrorCollector.SelectedItems.Clear();
            McForm.lvErrorCollector.Items[0].Selected = true;
            McForm.lvErrorCollector.FocusedItem = McForm.lvErrorCollector.Items[0];
        }

        private static void ShowMessageBox(Message msg)
        {
            switch (msg.MsgClass)
            {
                case MessageClass.InformationMsg:
                    MessageBox.Show(msg.MsgText, string.Format(Language.strTitleInformation, msg.MsgDate), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case MessageClass.WarningMsg:
                    MessageBox.Show(msg.MsgText, string.Format(Language.strTitleWarning, msg.MsgDate), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case MessageClass.ErrorMsg:
                    MessageBox.Show(msg.MsgText, string.Format(Language.strTitleError, msg.MsgDate), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }
        #endregion
		
        #region Delegates
		private delegate void ShowMcFormCb();
		private void ShowMcForm()
		{
			if (frmMain.Default.pnlDock.InvokeRequired)
			{
				var d = new ShowMcFormCb(ShowMcForm);
				frmMain.Default.pnlDock.Invoke(d);
			}
			else
			{
                McForm.Show(frmMain.Default.pnlDock);
			}
		}

        private delegate void AddToListCb(ListViewItem lvItem);
        private void AddToList(ListViewItem lvItem)
        {
            if (McForm.lvErrorCollector.InvokeRequired)
            {
                var d = new AddToListCb(AddToList);
                McForm.lvErrorCollector.Invoke(d, lvItem);
            }
            else
            {
                McForm.lvErrorCollector.Items.Insert(0, lvItem);
            }
        }
        #endregion
	}
}