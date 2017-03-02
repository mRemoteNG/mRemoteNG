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
        private Timer _ECTimer;

        private ErrorAndInfoWindow MCForm { get; set; }

        public MessageCollector(ErrorAndInfoWindow MessageCollectorForm)
        {
            MCForm = MessageCollectorForm;
            CreateTimer();
        }

        #region Public Methods
        public void AddMessage(MessageClass MsgClass, string MsgText, bool OnlyLog = false)
        {
            var nMsg = new Message(MsgClass, MsgText, DateTime.Now);

            if (nMsg.MsgClass == MessageClass.ReportMsg)
            {
                AddReportMessage(nMsg);
                return;
            }

            var EnableTimer = true; // used to control if we SWITCH to the notifiation panel. Message will still be added regardless.

            if (nMsg.MsgClass == MessageClass.InformationMsg)
            {
                AddInfoMessage(nMsg);

                if (!Settings.Default.SwitchToMCOnInformation)
                    EnableTimer = false;
            }

            if (nMsg.MsgClass == MessageClass.WarningMsg)
            {
                AddWarningMessage(nMsg);

                if (!Settings.Default.SwitchToMCOnWarning)
                    EnableTimer = false;
            }

            if (nMsg.MsgClass == MessageClass.ErrorMsg)
            {
                AddErrorMessage(nMsg);

                if (!Settings.Default.SwitchToMCOnError)
                    EnableTimer = false;
            }

            if (OnlyLog)
                return;

            if (Settings.Default.ShowNoMessageBoxes)
            {
                /* These if statements need to be split so we can:
                 * control that no messages boxes will be dispalyed
                 * add items to the notifications panel
                 * NOT switch to the notification panel if configured that way
                 */
                if(EnableTimer)
                    _ECTimer.Enabled = true;
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
            _ECTimer = new Timer
            {
                Enabled = false,
                Interval = 300
            };
            _ECTimer.Tick += SwitchTimerTick;
        }

        private void SwitchTimerTick(object sender, EventArgs e)
        {
            SwitchToMessage();
            _ECTimer.Enabled = false;
        }

        private void SwitchToMessage()
        {
            MCForm.PreviousActiveForm = (DockContent)frmMain.Default.pnlDock.ActiveContent;
            ShowMCForm();
            MCForm.lvErrorCollector.Focus();
            MCForm.lvErrorCollector.SelectedItems.Clear();
            MCForm.lvErrorCollector.Items[0].Selected = true;
            MCForm.lvErrorCollector.FocusedItem = MCForm.lvErrorCollector.Items[0];
        }

        private static void ShowMessageBox(Message Msg)
        {
            switch (Msg.MsgClass)
            {
                case MessageClass.InformationMsg:
                    MessageBox.Show(Msg.MsgText, string.Format(Language.strTitleInformation, Msg.MsgDate), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case MessageClass.WarningMsg:
                    MessageBox.Show(Msg.MsgText, string.Format(Language.strTitleWarning, Msg.MsgDate), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case MessageClass.ErrorMsg:
                    MessageBox.Show(Msg.MsgText, string.Format(Language.strTitleError, Msg.MsgDate), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }
        #endregion
		
        #region Delegates
		private delegate void ShowMCFormCB();
		private void ShowMCForm()
		{
			if (frmMain.Default.pnlDock.InvokeRequired)
			{
				var d = new ShowMCFormCB(ShowMCForm);
				frmMain.Default.pnlDock.Invoke(d);
			}
			else
			{
                MCForm.Show(frmMain.Default.pnlDock);
			}
		}

        private delegate void AddToListCB(ListViewItem lvItem);
        private void AddToList(ListViewItem lvItem)
        {
            if (MCForm.lvErrorCollector.InvokeRequired)
            {
                var d = new AddToListCB(AddToList);
                MCForm.lvErrorCollector.Invoke(d, new object[] { lvItem });
            }
            else
            {
                MCForm.lvErrorCollector.Items.Insert(0, lvItem);
            }
        }
        #endregion
	}
}