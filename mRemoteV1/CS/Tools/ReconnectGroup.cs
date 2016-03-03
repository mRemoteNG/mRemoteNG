// VBConversions Note: VB project level imports
using System.Collections.Generic;
using System;
using AxWFICALib;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using AxMSTSCLib;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
// End of VB project level imports


namespace mRemoteNG
{
	public partial class ReconnectGroup
	{
		public ReconnectGroup()
		{
			InitializeComponent();
		}
		private bool _ServerReady;
public bool ServerReady
		{
			get
			{
				return _ServerReady;
			}
			set
			{
				if (value == true)
				{
					SetStatusImage(global::My.Resources.HostStatus_On);
				}
				else
				{
					SetStatusImage(global::My.Resources.HostStatus_Off);
				}
				
				_ServerReady = value;
			}
		}
		
		private delegate void SetStatusImageCB(Image Img);
		private void SetStatusImage(Image Img)
		{
			if (pbServerStatus.InvokeRequired)
			{
				SetStatusImageCB d = new SetStatusImageCB(SetStatusImage);
				ParentForm.Invoke(d, new object[] {Img});
			}
			else
			{
				pbServerStatus.Image = Img;
			}
		}
		
		public void chkReconnectWhenReady_CheckedChanged(System.Object sender, System.EventArgs e)
		{
			_ReconnectWhenReady = chkReconnectWhenReady.Checked;
		}
		
		private bool _ReconnectWhenReady;
public bool ReconnectWhenReady
		{
			get
			{
				return _ReconnectWhenReady;
			}
			set
			{
				_ReconnectWhenReady = value;
				SetCheckbox(value);
			}
		}
		
		private delegate void SetCheckboxCB(bool Val);
		private void SetCheckbox(bool Val)
		{
			if (chkReconnectWhenReady.InvokeRequired)
			{
				SetCheckboxCB d = new SetCheckboxCB(SetCheckbox);
				ParentForm.Invoke(d, new object[] {Val});
			}
			else
			{
				chkReconnectWhenReady.Checked = Val;
			}
		}
		
		public delegate void CloseClickedEventHandler();
		private CloseClickedEventHandler CloseClickedEvent;
		
		public event CloseClickedEventHandler CloseClicked
		{
			add
			{
				CloseClickedEvent = (CloseClickedEventHandler) System.Delegate.Combine(CloseClickedEvent, value);
			}
			remove
			{
				CloseClickedEvent = (CloseClickedEventHandler) System.Delegate.Remove(CloseClickedEvent, value);
			}
		}
		
		
		public void btnClose_Click(System.Object sender, System.EventArgs e)
		{
			if (CloseClickedEvent != null)
				CloseClickedEvent();
		}
		
		public void tmrAnimation_Tick(System.Object sender, System.EventArgs e)
		{
			switch (lblAnimation.Text)
			{
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
			if (this.InvokeRequired)
			{
				DisposeReconnectGroupCB d = new DisposeReconnectGroupCB(DisposeReconnectGroup);
				ParentForm.Invoke(d);
			}
			else
			{
				this.Dispose();
			}
		}
		
		public void ReconnectGroup_Load(object sender, System.EventArgs e)
		{
			ApplyLanguage();
		}
		
		private void ApplyLanguage()
		{
			grpAutomaticReconnect.Text = My.Language.strGroupboxAutomaticReconnect;
			btnClose.Text = My.Language.strButtonClose;
			lblServerStatus.Text = My.Language.strLabelServerStatus;
			chkReconnectWhenReady.Text = My.Language.strCheckboxReconnectWhenReady;
		}
	}
	
}
