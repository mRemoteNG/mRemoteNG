using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using mRemoteNG.App.Runtime;

namespace mRemoteNG.Connection
{
	namespace Protocol
	{
		public class Base
		{
			#region "Properties"
			#region "Control"
			private string _Name;
			public string Name {
				get { return this._Name; }
				set { this._Name = value; }
			}

			private UI.Window.Connection withEventsField__connectionWindow;
			private UI.Window.Connection _connectionWindow {
				get { return withEventsField__connectionWindow; }
				set {
					if (withEventsField__connectionWindow != null) {
						withEventsField__connectionWindow.ResizeBegin -= ResizeBegin;
						withEventsField__connectionWindow.Resize -= Resize;
						withEventsField__connectionWindow.ResizeEnd -= ResizeEnd;
					}
					withEventsField__connectionWindow = value;
					if (withEventsField__connectionWindow != null) {
						withEventsField__connectionWindow.ResizeBegin += ResizeBegin;
						withEventsField__connectionWindow.Resize += Resize;
						withEventsField__connectionWindow.ResizeEnd += ResizeEnd;
					}
				}
			}
			public UI.Window.Connection ConnectionWindow {
				get { return _connectionWindow; }
				set { _connectionWindow = value; }
			}

			private InterfaceControl _interfaceControl;
			public InterfaceControl InterfaceControl {
				get { return _interfaceControl; }
				set {
					_interfaceControl = value;
					ConnectionWindow = _interfaceControl.GetContainerControl() as UI.Window.Connection;
				}
			}

			private Control _Control;
			public Control Control {
				get { return this._Control; }
				set { this._Control = value; }
			}
			#endregion

			private mRemoteNG.Connection.Info.Force _Force;
			public mRemoteNG.Connection.Info.Force Force {
				get { return this._Force; }
				set { this._Force = value; }
			}

			public System.Timers.Timer tmrReconnect = new System.Timers.Timer(2000);
			private ReconnectGroup withEventsField_ReconnectGroup;
			public ReconnectGroup ReconnectGroup {
				get { return withEventsField_ReconnectGroup; }
				set {
					if (withEventsField_ReconnectGroup != null) {
						withEventsField_ReconnectGroup.CloseClicked -= Event_ReconnectGroupCloseClicked;
					}
					withEventsField_ReconnectGroup = value;
					if (withEventsField_ReconnectGroup != null) {
						withEventsField_ReconnectGroup.CloseClicked += Event_ReconnectGroupCloseClicked;
					}
				}
				#endregion
			}

			#region "Methods"
			public virtual void Focus()
			{
				try {
					this._Control.Focus();
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "Couldn't focus Control (Connection.Protocol.Base)" + Constants.vbNewLine + ex.Message, true);
				}
			}


			public virtual void ResizeBegin(System.Object sender, EventArgs e)
			{
			}


			public virtual void Resize(System.Object sender, EventArgs e)
			{
			}


			public virtual void ResizeEnd(System.Object sender, EventArgs e)
			{
			}

			public virtual bool SetProps()
			{
				try {
					this._interfaceControl.Parent.Tag = this._interfaceControl;
					this._interfaceControl.Show();

					if (this._Control != null) {
						this._Control.Name = this._Name;
						this._Control.Parent = this._interfaceControl;
						this._Control.Location = this._interfaceControl.Location;
						this._Control.Size = this.InterfaceControl.Size;
						this._Control.Anchor = this._interfaceControl.Anchor;
					}

					return true;
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "Couldn't SetProps (Connection.Protocol.Base)" + Constants.vbNewLine + ex.Message, true);
					return false;
				}
			}

			public virtual bool Connect()
			{
				if (InterfaceControl.Info.Protocol != Protocols.RDP) {
					if (Connected != null) {
						Connected(this);
					}
				}
			}

			public virtual void Disconnect()
			{
				this.Close();
			}

			public virtual void Close()
			{
				Thread t = new Thread(CloseBG);
				t.SetApartmentState(System.Threading.ApartmentState.STA);
				t.IsBackground = true;
				t.Start();
			}

			private void CloseBG()
			{
				if (Closed != null) {
					Closed(this);
				}

				try {
					tmrReconnect.Enabled = false;

					if (this._Control != null) {
						try {
							this.DisposeControl();
						} catch (Exception ex) {
							mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.WarningMsg, "Could not dispose control, probably form is already closed (Connection.Protocol.Base)" + Constants.vbNewLine + ex.Message, true);
						}
					}

					if (this._interfaceControl != null) {
						try {
							if (this._interfaceControl.Parent != null) {
								if (this._interfaceControl.Parent.Tag != null) {
									this.SetTagToNothing();
								}

								this.DisposeInterface();
							}
						} catch (Exception ex) {
							mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.WarningMsg, "Could not set InterfaceControl.Parent.Tag or Dispose Interface, probably form is already closed (Connection.Protocol.Base)" + Constants.vbNewLine + ex.Message, true);
						}
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "Couldn't Close InterfaceControl BG (Connection.Protocol.Base)" + Constants.vbNewLine + ex.Message, true);
				}
			}

			private delegate void DisposeInterfaceCB();
			private void DisposeInterface()
			{
				if (this._interfaceControl.InvokeRequired) {
					DisposeInterfaceCB s = new DisposeInterfaceCB(DisposeInterface);
					this._interfaceControl.Invoke(s);
				} else {
					this._interfaceControl.Dispose();
				}
			}

			private delegate void SetTagToNothingCB();
			private void SetTagToNothing()
			{
				if (this._interfaceControl.Parent.InvokeRequired) {
					SetTagToNothingCB s = new SetTagToNothingCB(SetTagToNothing);
					this._interfaceControl.Parent.Invoke(s);
				} else {
					this._interfaceControl.Parent.Tag = null;
				}
			}

			private delegate void DisposeControlCB();
			private void DisposeControl()
			{
				if (this._Control.InvokeRequired) {
					DisposeControlCB s = new DisposeControlCB(DisposeControl);
					this._Control.Invoke(s);
				} else {
					this._Control.Dispose();
				}
			}
			#endregion

			#region "Events"
			public event ConnectingEventHandler Connecting;
			public delegate void ConnectingEventHandler(object sender);
			public event ConnectedEventHandler Connected;
			public delegate void ConnectedEventHandler(object sender);
			public event DisconnectedEventHandler Disconnected;
			public delegate void DisconnectedEventHandler(object sender, string DisconnectedMessage);
			public event ErrorOccuredEventHandler ErrorOccured;
			public delegate void ErrorOccuredEventHandler(object sender, string ErrorMessage);
			public event ClosingEventHandler Closing;
			public delegate void ClosingEventHandler(object sender);
			public event ClosedEventHandler Closed;
			public delegate void ClosedEventHandler(object sender);

			public void Event_Closing(object sender)
			{
				if (Closing != null) {
					Closing(sender);
				}
			}

			public void Event_Closed(object sender)
			{
				if (Closed != null) {
					Closed(sender);
				}
			}

			public void Event_Connecting(object sender)
			{
				if (Connecting != null) {
					Connecting(sender);
				}
			}

			public void Event_Connected(object sender)
			{
				if (Connected != null) {
					Connected(sender);
				}
			}

			public void Event_Disconnected(object sender, string DisconnectedMessage)
			{
				if (Disconnected != null) {
					Disconnected(sender, DisconnectedMessage);
				}
			}

			public void Event_ErrorOccured(object sender, string ErrorMsg)
			{
				if (ErrorOccured != null) {
					ErrorOccured(sender, ErrorMsg);
				}
			}

			public void Event_ReconnectGroupCloseClicked()
			{
				Close();
			}
			#endregion
		}
	}
}
