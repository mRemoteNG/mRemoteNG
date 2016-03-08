using System;
using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using mRemoteNG.My;



namespace mRemoteNG.Messages
{
	public class Collector
    {
        #region Constructors
        public Collector(UI.Window.ErrorsAndInfos MessageCollectorForm)
        {
            this._MCForm = MessageCollectorForm;
            CreateTimer();
        }
        #endregion

        #region Private Properties
        private Timer ECTimer;
        private UI.Window.ErrorsAndInfos _MCForm;
        #endregion

        #region Public Properties
        public UI.Window.ErrorsAndInfos MCForm
		{
			get { return this._MCForm; }
			set { this._MCForm = value; }
		}
        #endregion

        #region Public Methods
        public void AddMessage(Messages.MessageClass MsgClass, string MsgText, bool OnlyLog = false)
        {
            Messages.Message nMsg = new Messages.Message();
            nMsg.MsgClass = MsgClass;
            nMsg.MsgText = MsgText;
            nMsg.MsgDate = DateTime.Now;

            if (Settings.Default.SwitchToMCOnInformation && nMsg.MsgClass == Messages.MessageClass.InformationMsg)
            {
                Debug.Print("Info: " + nMsg.MsgText);
                if (Settings.Default.WriteLogFile)
                {
                    App.Runtime.Log.Info(nMsg.MsgText);
                }

                if (OnlyLog)
                {
                    return;
                }

                if (Settings.Default.ShowNoMessageBoxes)
                {
                    ECTimer.Enabled = true;
                }
                else
                {
                    ShowMessageBox(nMsg);
                }
            }

            if (Settings.Default.SwitchToMCOnWarning && nMsg.MsgClass == Messages.MessageClass.WarningMsg)
            {
                Debug.Print("Warning: " + nMsg.MsgText);
                if (Settings.Default.WriteLogFile)
                {
                    App.Runtime.Log.Warn(nMsg.MsgText);
                }

                if (OnlyLog)
                {
                    return;
                }

                if (Settings.Default.ShowNoMessageBoxes)
                {
                    ECTimer.Enabled = true;
                }
                else
                {
                    ShowMessageBox(nMsg);
                }
            }

            if (Settings.Default.SwitchToMCOnError && nMsg.MsgClass == Messages.MessageClass.ErrorMsg)
            {
                Debug.Print("Error: " + nMsg.MsgText);

                // Always log error messages
                App.Runtime.Log.Error(nMsg.MsgText);

                if (OnlyLog)
                {
                    return;
                }

                if (Settings.Default.ShowNoMessageBoxes)
                {
                    ECTimer.Enabled = true;
                }
                else
                {
                    ShowMessageBox(nMsg);
                }
            }

            if (nMsg.MsgClass == MessageClass.ReportMsg)
            {
                Debug.Print("Report: " + nMsg.MsgText);

                if (Settings.Default.WriteLogFile)
                {
                    App.Runtime.Log.Info(nMsg.MsgText);
                }

                return;
            }

            ListViewItem lvItem = new ListViewItem();
            lvItem.ImageIndex = System.Convert.ToInt32(nMsg.MsgClass);
            lvItem.Text = nMsg.MsgText.Replace(Constants.vbNewLine, "  ");
            lvItem.Tag = nMsg;

            AddToList(lvItem);
        }

        public void AddExceptionMessage(string message, Exception ex, MessageClass msgClass = MessageClass.ErrorMsg, bool logOnly = false)
        {
            AddMessage(msgClass, message + Constants.vbNewLine + Tools.Misc.GetExceptionMessageRecursive(ex), logOnly);
        }
#endregion

        #region Private Methods
        private void CreateTimer()
        {
            ECTimer = new Timer();
            ECTimer.Enabled = false;
            ECTimer.Interval = 300;
            ECTimer.Tick += SwitchTimerTick;
        }

        private void SwitchTimerTick(object sender, System.EventArgs e)
        {
            this.SwitchToMessage();
            this.ECTimer.Enabled = false;
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
