using System;
using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using mRemoteNG.My;
using mRemoteNG.UI.Window;
using mRemoteNG.App;
using mRemoteNG.UI.Forms;


namespace mRemoteNG.Messages
{
	public class MessageCollector
    {
        private Timer _ECTimer;
        private ErrorAndInfoWindow _MCForm;

        public ErrorAndInfoWindow MCForm
		{
			get { return this._MCForm; }
			set { this._MCForm = value; }
		}

        public MessageCollector(ErrorAndInfoWindow MessageCollectorForm)
        {
            this._MCForm = MessageCollectorForm;
            CreateTimer();
        }

        #region Public Methods
        public void AddMessage(MessageClass MsgClass, string MsgText, bool OnlyLog = false)
        {
            Message nMsg = new Message(MsgClass, MsgText, DateTime.Now);

            if (nMsg.MsgClass == MessageClass.ReportMsg)
            {
                AddReportMessage(nMsg);
                return;
            }

            if (Settings.Default.SwitchToMCOnInformation && nMsg.MsgClass == MessageClass.InformationMsg)
                AddInfoMessage(OnlyLog, nMsg);
            
            if (Settings.Default.SwitchToMCOnWarning && nMsg.MsgClass == MessageClass.WarningMsg)
                AddWarningMessage(OnlyLog, nMsg);

            if (Settings.Default.SwitchToMCOnError && nMsg.MsgClass == MessageClass.ErrorMsg)
                AddErrorMessage(OnlyLog, nMsg);

            if (!OnlyLog)
            {
                if (Settings.Default.ShowNoMessageBoxes)
                    _ECTimer.Enabled = true;
                else
                    ShowMessageBox(nMsg);

                ListViewItem lvItem = BuildListViewItem(nMsg);
                AddToList(lvItem);
            }
        }

        private void AddInfoMessage(bool OnlyLog, Message nMsg)
        {
            Debug.Print("Info: " + nMsg.MsgText);
            if (Settings.Default.WriteLogFile)
                Runtime.Log.Info(nMsg.MsgText);
        }

        private void AddWarningMessage(bool OnlyLog, Message nMsg)
        {
            Debug.Print("Warning: " + nMsg.MsgText);
            if (Settings.Default.WriteLogFile)
                Runtime.Log.Warn(nMsg.MsgText);
        }

        private void AddErrorMessage(bool OnlyLog, Message nMsg)
        {
            Debug.Print("Error: " + nMsg.MsgText);
            Runtime.Log.Error(nMsg.MsgText);
        }

        private static void AddReportMessage(Message nMsg)
        {
            Debug.Print("Report: " + nMsg.MsgText);
            if (Settings.Default.WriteLogFile)
                Runtime.Log.Info(nMsg.MsgText);
        }

        private static ListViewItem BuildListViewItem(Message nMsg)
        {
            ListViewItem lvItem = new ListViewItem();
            lvItem.ImageIndex = Convert.ToInt32(nMsg.MsgClass);
            lvItem.Text = nMsg.MsgText.Replace(Environment.NewLine, "  ");
            lvItem.Tag = nMsg;
            return lvItem;
        }

        public void AddExceptionMessage(string message, Exception ex, MessageClass msgClass = MessageClass.ErrorMsg, bool logOnly = false)
        {
            AddMessage(msgClass, message + Environment.NewLine + Tools.MiscTools.GetExceptionMessageRecursive(ex), logOnly);
        }
        #endregion

        #region Private Methods
        private void CreateTimer()
        {
            _ECTimer = new Timer();
            _ECTimer.Enabled = false;
            _ECTimer.Interval = 300;
            _ECTimer.Tick += SwitchTimerTick;
        }

        private void SwitchTimerTick(object sender, System.EventArgs e)
        {
            this.SwitchToMessage();
            this._ECTimer.Enabled = false;
        }

        private void SwitchToMessage()
        {
            this._MCForm.PreviousActiveForm = (WeifenLuo.WinFormsUI.Docking.DockContent)frmMain.Default.pnlDock.ActiveContent;
            this.ShowMCForm();
            this._MCForm.lvErrorCollector.Focus();
            this._MCForm.lvErrorCollector.SelectedItems.Clear();
            this._MCForm.lvErrorCollector.Items[0].Selected = true;
            this._MCForm.lvErrorCollector.FocusedItem = this._MCForm.lvErrorCollector.Items[0];
        }

        private static void ShowMessageBox(Messages.Message Msg)
        {
            switch (Msg.MsgClass)
            {
                case Messages.MessageClass.InformationMsg:
                    MessageBox.Show(Msg.MsgText, string.Format(My.Language.strTitleInformation, Msg.MsgDate), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case Messages.MessageClass.WarningMsg:
                    MessageBox.Show(Msg.MsgText, string.Format(My.Language.strTitleWarning, Msg.MsgDate), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case Messages.MessageClass.ErrorMsg:
                    MessageBox.Show(Msg.MsgText, string.Format(My.Language.strTitleError, Msg.MsgDate), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
				ShowMCFormCB d = new ShowMCFormCB(ShowMCForm);
				frmMain.Default.pnlDock.Invoke(d);
			}
			else
			{
				this._MCForm.Show(frmMain.Default.pnlDock);
			}
		}

        delegate void AddToListCB(ListViewItem lvItem);
        private void AddToList(ListViewItem lvItem)
        {
            if (this._MCForm.lvErrorCollector.InvokeRequired)
            {
                AddToListCB d = new AddToListCB(AddToList);
                this._MCForm.lvErrorCollector.Invoke(d, new object[] { lvItem });
            }
            else
            {
                this._MCForm.lvErrorCollector.Items.Insert(0, lvItem);
            }
        }
        #endregion
	}
}