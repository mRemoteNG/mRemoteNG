using System;
using System.Windows.Forms;
using System.Threading;
using mRemoteNG.App;


namespace mRemoteNG.Connection.Protocol
{
	public abstract class Base
    {
        #region Private Variables
        private string _Name;
        private UI.Window.Connection _connectionWindow;
        private InterfaceControl _interfaceControl;
        private Control _Control;
        private mRemoteNG.Connection.Info.Force _Force;
        private ConnectingEventHandler ConnectingEvent;
        private ConnectedEventHandler ConnectedEvent;
        private DisconnectedEventHandler DisconnectedEvent;
        private ErrorOccuredEventHandler ErrorOccuredEvent;
        private ClosingEventHandler ClosingEvent;
        private ClosedEventHandler ClosedEvent;
        #endregion

        #region Public Properties
        #region Control
        public string Name
		{
			get { return this._Name; }
			set { this._Name = value; }
		}
		
        public UI.Window.Connection ConnectionWindow
		{
			get { return _connectionWindow; }
			set
			{
				_connectionWindow = value;
				_connectionWindow.ResizeBegin += ResizeBegin;
				_connectionWindow.Resize += Resize;
				_connectionWindow.ResizeEnd += ResizeEnd;
			}
		}

        public InterfaceControl InterfaceControl
		{
			get { return _interfaceControl; }
			set
			{
				_interfaceControl = value;
				ConnectionWindow = _interfaceControl.GetContainerControl() as UI.Window.Connection;
			}
		}
		
        public Control Control
		{
			get { return this._Control; }
			set { this._Control = value; }
		}
        #endregion
		
        public mRemoteNG.Connection.Info.Force Force
		{
			get { return this._Force; }
			set { this._Force = value; }
		}
				
		public System.Timers.Timer tmrReconnect = new System.Timers.Timer(2000);
		public ReconnectGroup ReconnectGroup;
        #endregion
				
        #region Methods
        //public abstract int GetDefaultPort();

		public virtual void Focus()
		{
			try
			{
				this._Control.Focus();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn\'t focus Control (Connection.Protocol.Base)" + Environment.NewLine + ex.Message, true);
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
			try
			{
				this._interfaceControl.Parent.Tag = this._interfaceControl;
				this._interfaceControl.Show();
						
				if (this._Control != null)
				{
					this._Control.Name = this._Name;
					this._Control.Parent = this._interfaceControl;
					this._Control.Location = this._interfaceControl.Location;
					this._Control.Size = this.InterfaceControl.Size;
					this._Control.Anchor = this._interfaceControl.Anchor;
				}
						
				return true;
			}
			catch (Exception ex)
			{
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn\'t SetProps (Connection.Protocol.Base)" + Environment.NewLine + ex.Message, true);
				return false;
			}
		}
				
		public virtual bool Connect()
		{
			if (InterfaceControl.Info.Protocol != Protocols.RDP)
			{
                if (ConnectedEvent != null)
                {
                    ConnectedEvent(this);
                    return true;
                }
			}
            return false;
		}
				
		public virtual void Disconnect()
		{
			this.Close();
		}
				
		public virtual void Close()
		{
			Thread t = new Thread(new System.Threading.ThreadStart(CloseBG));
			t.SetApartmentState(System.Threading.ApartmentState.STA);
			t.IsBackground = true;
			t.Start();
		}
				
		private void CloseBG()
		{
            if (ClosedEvent != null)
            {
                ClosedEvent(this);
            }
			try
			{
				tmrReconnect.Enabled = false;
						
				if (this._Control != null)
				{
					try
					{
						this.DisposeControl();
					}
					catch (Exception ex)
					{
						Runtime.MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "Could not dispose control, probably form is already closed (Connection.Protocol.Base)" + Environment.NewLine + ex.Message, true);
					}
				}
						
				if (this._interfaceControl != null)
				{
					try
					{
						if (this._interfaceControl.Parent != null)
						{
							if (this._interfaceControl.Parent.Tag != null)
							{
								this.SetTagToNothing();
							}
									
							this.DisposeInterface();
						}
					}
					catch (Exception ex)
					{
                        Runtime.MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "Could not set InterfaceControl.Parent.Tag or Dispose Interface, probably form is already closed (Connection.Protocol.Base)" + Environment.NewLine + ex.Message, true);
					}
				}
			}
			catch (Exception ex)
			{
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn\'t Close InterfaceControl BG (Connection.Protocol.Base)" + Environment.NewLine + ex.Message, true);
			}
		}
				
		private delegate void DisposeInterfaceCB();
		private void DisposeInterface()
		{
			if (this._interfaceControl.InvokeRequired)
			{
				DisposeInterfaceCB s = new DisposeInterfaceCB(DisposeInterface);
				this._interfaceControl.Invoke(s);
			}
			else
			{
				this._interfaceControl.Dispose();
			}
		}
				
		private delegate void SetTagToNothingCB();
		private void SetTagToNothing()
		{
			if (this._interfaceControl.Parent.InvokeRequired)
			{
				SetTagToNothingCB s = new SetTagToNothingCB(SetTagToNothing);
				this._interfaceControl.Parent.Invoke(s);
			}
			else
			{
				this._interfaceControl.Parent.Tag = null;
			}
		}
				
		private delegate void DisposeControlCB();
		private void DisposeControl()
		{
			if (this._Control.InvokeRequired)
			{
				DisposeControlCB s = new DisposeControlCB(DisposeControl);
				this._Control.Invoke(s);
			}
			else
			{
				this._Control.Dispose();
			}
		}
        #endregion
				
        #region Events
		public delegate void ConnectingEventHandler(object sender);
		public event ConnectingEventHandler Connecting
		{
			add { ConnectingEvent = (ConnectingEventHandler) System.Delegate.Combine(ConnectingEvent, value); }
			remove { ConnectingEvent = (ConnectingEventHandler) System.Delegate.Remove(ConnectingEvent, value); }
		}
				
		public delegate void ConnectedEventHandler(object sender);
		public event ConnectedEventHandler Connected
		{
			add { ConnectedEvent = (ConnectedEventHandler) System.Delegate.Combine(ConnectedEvent, value); }
			remove { ConnectedEvent = (ConnectedEventHandler) System.Delegate.Remove(ConnectedEvent, value); }
		}
				
		public delegate void DisconnectedEventHandler(object sender, string DisconnectedMessage);
		public event DisconnectedEventHandler Disconnected
		{
			add { DisconnectedEvent = (DisconnectedEventHandler) System.Delegate.Combine(DisconnectedEvent, value); }
			remove { DisconnectedEvent = (DisconnectedEventHandler) System.Delegate.Remove(DisconnectedEvent, value); }
		}
				
		public delegate void ErrorOccuredEventHandler(object sender, string ErrorMessage);
		public event ErrorOccuredEventHandler ErrorOccured
		{
			add { ErrorOccuredEvent = (ErrorOccuredEventHandler) System.Delegate.Combine(ErrorOccuredEvent, value); }
			remove { ErrorOccuredEvent = (ErrorOccuredEventHandler) System.Delegate.Remove(ErrorOccuredEvent, value); }
		}
				
		public delegate void ClosingEventHandler(object sender);
		public event ClosingEventHandler Closing
		{
			add { ClosingEvent = (ClosingEventHandler) System.Delegate.Combine(ClosingEvent, value); }
			remove { ClosingEvent = (ClosingEventHandler) System.Delegate.Remove(ClosingEvent, value); }
		}
				
		public delegate void ClosedEventHandler(object sender);
		public event ClosedEventHandler Closed
		{
			add { ClosedEvent = (ClosedEventHandler) System.Delegate.Combine(ClosedEvent, value); }
			remove { ClosedEvent = (ClosedEventHandler) System.Delegate.Remove(ClosedEvent, value); }
		}
				
				
		public void Event_Closing(object sender)
		{
			if (ClosingEvent != null)
				ClosingEvent(sender);
		}
				
		public void Event_Closed(object sender)
		{
			if (ClosedEvent != null)
				ClosedEvent(sender);
		}
				
		public void Event_Connecting(object sender)
		{
			if (ConnectingEvent != null)
				ConnectingEvent(sender);
		}
				
		public void Event_Connected(object sender)
		{
			if (ConnectedEvent != null)
				ConnectedEvent(sender);
		}
				
		public void Event_Disconnected(object sender, string DisconnectedMessage)
		{
			if (DisconnectedEvent != null)
				DisconnectedEvent(sender, DisconnectedMessage);
		}
				
		public void Event_ErrorOccured(object sender, string ErrorMsg)
		{
			if (ErrorOccuredEvent != null)
				ErrorOccuredEvent(sender, ErrorMsg);
		}
				
		public void Event_ReconnectGroupCloseClicked()
		{
			Close();
		}
        #endregion
	}
}