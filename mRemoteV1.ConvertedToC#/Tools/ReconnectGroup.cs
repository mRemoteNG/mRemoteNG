using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
namespace mRemoteNG
{
	public partial class ReconnectGroup
	{
		private bool _ServerReady;
		public bool ServerReady {
			get { return _ServerReady; }
			set {
				if (value == true) {
					SetStatusImage(mRemoteNG.My.Resources.HostStatus_On);
				} else {
					SetStatusImage(mRemoteNG.My.Resources.HostStatus_Off);
				}

				_ServerReady = value;
			}
		}

		private delegate void SetStatusImageCB(Image Img);
		private void SetStatusImage(Image Img)
		{
			if (pbServerStatus.InvokeRequired) {
				SetStatusImageCB d = new SetStatusImageCB(SetStatusImage);
				ParentForm.Invoke(d, new object[] { Img });
			} else {
				pbServerStatus.Image = Img;
			}
		}

		private void chkReconnectWhenReady_CheckedChanged(System.Object sender, System.EventArgs e)
		{
			_ReconnectWhenReady = chkReconnectWhenReady.Checked;
		}

		private bool _ReconnectWhenReady;
		public bool ReconnectWhenReady {
			get { return _ReconnectWhenReady; }
			set {
				_ReconnectWhenReady = value;
				SetCheckbox(value);
			}
		}

		private delegate void SetCheckboxCB(bool Val);
		private void SetCheckbox(bool Val)
		{
			if (chkReconnectWhenReady.InvokeRequired) {
				SetCheckboxCB d = new SetCheckboxCB(SetCheckbox);
				ParentForm.Invoke(d, new object[] { Val });
			} else {
				chkReconnectWhenReady.Checked = Val;
			}
		}

		public event CloseClickedEventHandler CloseClicked;
		public delegate void CloseClickedEventHandler();

		private void btnClose_Click(System.Object sender, System.EventArgs e)
		{
			if (CloseClicked != null) {
				CloseClicked();
			}
		}

		private void tmrAnimation_Tick(System.Object sender, System.EventArgs e)
		{
			switch (lblAnimation.Text) {
				case "":
					lblAnimation.Text = "»";
					break;
				case "»":
					lblAnimation.Text = "»»";
					break;
				case "»»":
					lblAnimation.Text = "»»»";
					break;
				case "»»»":
					lblAnimation.Text = "";
					break;
			}
		}

		private delegate void DisposeReconnectGroupCB();
		public void DisposeReconnectGroup()
		{
			if (this.InvokeRequired) {
				DisposeReconnectGroupCB d = new DisposeReconnectGroupCB(DisposeReconnectGroup);
				ParentForm.Invoke(d);
			} else {
				this.Dispose();
			}
		}

		private void ReconnectGroup_Load(object sender, System.EventArgs e)
		{
			ApplyLanguage();
		}

		private void ApplyLanguage()
		{
			grpAutomaticReconnect.Text = mRemoteNG.My.Language.strGroupboxAutomaticReconnect;
			btnClose.Text = mRemoteNG.My.Language.strButtonClose;
			lblServerStatus.Text = mRemoteNG.My.Language.strLabelServerStatus;
			chkReconnectWhenReady.Text = mRemoteNG.My.Language.strCheckboxReconnectWhenReady;
		}
		public ReconnectGroup()
		{
			Load += ReconnectGroup_Load;
			InitializeComponent();
		}
	}
}
