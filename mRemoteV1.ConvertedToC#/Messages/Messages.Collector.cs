using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace mRemoteNG.Messages
{
	public class Collector
	{
		#region "Public Properties"
		private UI.Window.ErrorsAndInfos _MCForm;
		public UI.Window.ErrorsAndInfos MCForm {
			get { return this._MCForm; }
			set { this._MCForm = value; }
		}
		#endregion


		private Timer ECTimer;
		public Collector(UI.Window.ErrorsAndInfos MessageCollectorForm)
		{
			this._MCForm = MessageCollectorForm;
			CreateTimer();
		}

		private void CreateTimer()
		{
			ECTimer = new Timer();
			ECTimer.Enabled = false;
			ECTimer.Interval = 300;
			ECTimer.Tick += SwitchTimerTick;
		}

		public delegate void AddToListCB(ListViewItem lvItem);
		private void AddToList(ListViewItem lvItem)
		{
			if (this._MCForm.lvErrorCollector.InvokeRequired) {
				AddToListCB d = new AddToListCB(AddToList);
				this._MCForm.lvErrorCollector.Invoke(d, new object[] { lvItem });
			} else {
				this._MCForm.lvErrorCollector.Items.Insert(0, lvItem);
			}
		}

		public void AddMessage(Messages.MessageClass MsgClass, string MsgText, bool OnlyLog = false)
		{
			Messages.Message nMsg = new Messages.Message();
			nMsg.MsgClass = MsgClass;
			nMsg.MsgText = MsgText;
			nMsg.MsgDate = DateAndTime.Now;

			if (mRemoteNG.My.Settings.SwitchToMCOnInformation & nMsg.MsgClass == mRemoteNG.Messages.MessageClass.InformationMsg) {
				Debug.Print("Info: " + nMsg.MsgText);
				if (mRemoteNG.My.Settings.WriteLogFile) {
					mRemoteNG.App.Runtime.Log.Info(nMsg.MsgText);
				}

				if (OnlyLog) {
					return;
				}

				if (mRemoteNG.My.Settings.ShowNoMessageBoxes) {
					ECTimer.Enabled = true;
				} else {
					ShowMessageBox(nMsg);
				}
			}

			if (mRemoteNG.My.Settings.SwitchToMCOnWarning & nMsg.MsgClass == mRemoteNG.Messages.MessageClass.WarningMsg) {
				Debug.Print("Warning: " + nMsg.MsgText);
				if (mRemoteNG.My.Settings.WriteLogFile) {
					mRemoteNG.App.Runtime.Log.Warn(nMsg.MsgText);
				}

				if (OnlyLog) {
					return;
				}

				if (mRemoteNG.My.Settings.ShowNoMessageBoxes) {
					ECTimer.Enabled = true;
				} else {
					ShowMessageBox(nMsg);
				}
			}

			if (mRemoteNG.My.Settings.SwitchToMCOnError & nMsg.MsgClass == mRemoteNG.Messages.MessageClass.ErrorMsg) {
				Debug.Print("Error: " + nMsg.MsgText);

				// Always log error messages
				mRemoteNG.App.Runtime.Log.Error(nMsg.MsgText);

				if (OnlyLog) {
					return;
				}

				if (mRemoteNG.My.Settings.ShowNoMessageBoxes) {
					ECTimer.Enabled = true;
				} else {
					ShowMessageBox(nMsg);
				}
			}

			if (nMsg.MsgClass == MessageClass.ReportMsg) {
				Debug.Print("Report: " + nMsg.MsgText);

				if (mRemoteNG.My.Settings.WriteLogFile) {
					mRemoteNG.App.Runtime.Log.Info(nMsg.MsgText);
				}

				return;
			}

			ListViewItem lvItem = new ListViewItem();
			lvItem.ImageIndex = Convert.ToInt32(nMsg.MsgClass);
			lvItem.Text = nMsg.MsgText.Replace(Constants.vbNewLine, "  ");
			lvItem.Tag = nMsg;

			AddToList(lvItem);
		}

		public void AddExceptionMessage(string message, Exception ex, MessageClass msgClass = MessageClass.ErrorMsg, bool logOnly = false)
		{
			AddMessage(msgClass, message + Constants.vbNewLine + mRemoteNG.Tools.Misc.GetExceptionMessageRecursive(ex), logOnly);
		}

		private void SwitchTimerTick(object sender, System.EventArgs e)
		{
			this.SwitchToMessage();
			this.ECTimer.Enabled = false;
		}

		private void SwitchToMessage()
		{
			this._MCForm.PreviousActiveForm = My.MyProject.Forms.frmMain.pnlDock.ActiveContent;
			this.ShowMCForm();
			this._MCForm.lvErrorCollector.Focus();
			this._MCForm.lvErrorCollector.SelectedItems.Clear();
			this._MCForm.lvErrorCollector.Items[0].Selected = true;
			this._MCForm.lvErrorCollector.FocusedItem = this._MCForm.lvErrorCollector.Items[0];
		}

		private static void ShowMessageBox(Messages.Message Msg)
		{
			switch (Msg.MsgClass) {
				case mRemoteNG.Messages.MessageClass.InformationMsg:
					MessageBox.Show(Msg.MsgText, string.Format(mRemoteNG.My.Language.strTitleInformation, Msg.MsgDate), MessageBoxButtons.OK, MessageBoxIcon.Information);
					break;
				case mRemoteNG.Messages.MessageClass.WarningMsg:
					MessageBox.Show(Msg.MsgText, string.Format(mRemoteNG.My.Language.strTitleWarning, Msg.MsgDate), MessageBoxButtons.OK, MessageBoxIcon.Warning);
					break;
				case mRemoteNG.Messages.MessageClass.ErrorMsg:
					MessageBox.Show(Msg.MsgText, string.Format(mRemoteNG.My.Language.strTitleError, Msg.MsgDate), MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;
			}
		}

		#region "Delegates"
		private delegate void ShowMCFormCB();
		private void ShowMCForm()
		{
			if (My.MyProject.Forms.frmMain.pnlDock.InvokeRequired) {
				ShowMCFormCB d = new ShowMCFormCB(ShowMCForm);
				My.MyProject.Forms.frmMain.pnlDock.Invoke(d);
			} else {
				this._MCForm.Show(My.MyProject.Forms.frmMain.pnlDock);
			}
		}
		#endregion
	}
}
