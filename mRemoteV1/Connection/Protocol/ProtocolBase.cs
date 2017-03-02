using System;
using System.Windows.Forms;
using System.Threading;
using mRemoteNG.App;
using mRemoteNG.Tools;


namespace mRemoteNG.Connection.Protocol
{
	public abstract class ProtocolBase
    {
        #region Private Variables

	    private UI.Window.ConnectionWindow _connectionWindow;
        private InterfaceControl _interfaceControl;
	    private ConnectingEventHandler ConnectingEvent;
        private ConnectedEventHandler ConnectedEvent;
        private DisconnectedEventHandler DisconnectedEvent;
        private ErrorOccuredEventHandler ErrorOccuredEvent;
        private ClosingEventHandler ClosingEvent;
        private ClosedEventHandler ClosedEvent;
        #endregion

        #region Public Properties
        #region Control
        private string Name { get; }

	    protected UI.Window.ConnectionWindow ConnectionWindow
		{
			get { return _connectionWindow; }
	        private set
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
				ConnectionWindow = _interfaceControl.GetContainerControl() as UI.Window.ConnectionWindow;
			}
		}

        protected Control Control { get; set; }

	    #endregion

        public ConnectionInfo.Force Force { get; set; }

	    public readonly System.Timers.Timer tmrReconnect = new System.Timers.Timer(2000);
        protected ReconnectGroup ReconnectGroup;

        protected ProtocolBase(string name)
        {
            Name = name;
        }

        protected ProtocolBase()
        {
        }

        #endregion

        #region Methods
        //public abstract int GetDefaultPort();

        public virtual void Focus()
		{
			try
			{
				Control.Focus();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionStackTrace("Couldn't focus Control (Connection.Protocol.Base)", ex);
			}
		}

        public virtual void ResizeBegin(object sender, EventArgs e)
		{		
		}

        public virtual void Resize(object sender, EventArgs e)
		{
		}

        public virtual void ResizeEnd(object sender, EventArgs e)
		{
		}
				
		public virtual bool Initialize()
		{
			try
			{
				_interfaceControl.Parent.Tag = _interfaceControl;
				_interfaceControl.Show();

			    if (Control == null) return true;
			    Control.Name = Name;
			    Control.Parent = _interfaceControl;
			    Control.Location = _interfaceControl.Location;
			    Control.Size = InterfaceControl.Size;
			    Control.Anchor = _interfaceControl.Anchor;

			    return true;
			}
			catch (Exception ex)
			{
                Runtime.MessageCollector.AddExceptionStackTrace("Couldn't SetProps (Connection.Protocol.Base)", ex);
				return false;
			}
		}
				
		public virtual bool Connect()
		{
		    if (InterfaceControl.Info.Protocol == ProtocolType.RDP) return false;
		    if (ConnectedEvent == null) return false;
		    ConnectedEvent(this);
		    return true;
		}
				
		public virtual void Disconnect()
		{
			Close();
		}
				
		public virtual void Close()
		{
			var t = new Thread(CloseBG);
			t.SetApartmentState(ApartmentState.STA);
			t.IsBackground = true;
			t.Start();
		}
				
		private void CloseBG()
		{
		    ClosedEvent?.Invoke(this);
		    try
			{
				tmrReconnect.Enabled = false;
						
				if (Control != null)
				{
					try
					{
						DisposeControl();
					}
					catch (Exception ex)
					{
						Runtime.MessageCollector.AddExceptionStackTrace("Couldn't dispose control, probably form is already closed (Connection.Protocol.Base)", ex);
					}
				}

			    if (_interfaceControl == null) return;

			    try
			    {
			        if (_interfaceControl.Parent == null) return;

			        if (_interfaceControl.Parent.Tag != null)
			        {
			            SetTagToNothing();
			        }
									
			        DisposeInterface();
			    }
			    catch (Exception ex)
			    {
                    Runtime.MessageCollector.AddExceptionStackTrace("Couldn't set InterfaceControl.Parent.Tag or Dispose Interface, probably form is already closed (Connection.Protocol.Base)", ex);
			    }
			}
			catch (Exception ex)
			{
                Runtime.MessageCollector.AddExceptionStackTrace("Couldn't Close InterfaceControl BG (Connection.Protocol.Base)", ex);
			}
		}
				
		private delegate void DisposeInterfaceCB();
		private void DisposeInterface()
		{
			if (_interfaceControl.InvokeRequired)
			{
				var s = new DisposeInterfaceCB(DisposeInterface);
				_interfaceControl.Invoke(s);
			}
			else
			{
				_interfaceControl.Dispose();
			}
		}
				
		private delegate void SetTagToNothingCB();
		private void SetTagToNothing()
		{
			if (_interfaceControl.Parent.InvokeRequired)
			{
				var s = new SetTagToNothingCB(SetTagToNothing);
				_interfaceControl.Parent.Invoke(s);
			}
			else
			{
				_interfaceControl.Parent.Tag = null;
			}
		}
				
		private delegate void DisposeControlCB();
		private void DisposeControl()
		{
			if (Control.InvokeRequired)
			{
				var s = new DisposeControlCB(DisposeControl);
				Control.Invoke(s);
			}
			else
			{
				Control.Dispose();
			}
		}
        #endregion
		
        #region Events
		public delegate void ConnectingEventHandler(object sender);
		public event ConnectingEventHandler Connecting
		{
			add { ConnectingEvent = (ConnectingEventHandler) Delegate.Combine(ConnectingEvent, value); }
			remove { ConnectingEvent = (ConnectingEventHandler) Delegate.Remove(ConnectingEvent, value); }
		}
				
		public delegate void ConnectedEventHandler(object sender);
		public event ConnectedEventHandler Connected
		{
			add { ConnectedEvent = (ConnectedEventHandler) Delegate.Combine(ConnectedEvent, value); }
			remove { ConnectedEvent = (ConnectedEventHandler) Delegate.Remove(ConnectedEvent, value); }
		}
				
		public delegate void DisconnectedEventHandler(object sender, string DisconnectedMessage);
		public event DisconnectedEventHandler Disconnected
		{
			add { DisconnectedEvent = (DisconnectedEventHandler) Delegate.Combine(DisconnectedEvent, value); }
			remove { DisconnectedEvent = (DisconnectedEventHandler) Delegate.Remove(DisconnectedEvent, value); }
		}
				
		public delegate void ErrorOccuredEventHandler(object sender, string ErrorMessage);
		public event ErrorOccuredEventHandler ErrorOccured
		{
			add { ErrorOccuredEvent = (ErrorOccuredEventHandler) Delegate.Combine(ErrorOccuredEvent, value); }
			remove { ErrorOccuredEvent = (ErrorOccuredEventHandler) Delegate.Remove(ErrorOccuredEvent, value); }
		}
				
		public delegate void ClosingEventHandler(object sender);
		public event ClosingEventHandler Closing
		{
			add { ClosingEvent = (ClosingEventHandler) Delegate.Combine(ClosingEvent, value); }
			remove { ClosingEvent = (ClosingEventHandler) Delegate.Remove(ClosingEvent, value); }
		}
				
		public delegate void ClosedEventHandler(object sender);
		public event ClosedEventHandler Closed
		{
			add { ClosedEvent = (ClosedEventHandler) Delegate.Combine(ClosedEvent, value); }
			remove { ClosedEvent = (ClosedEventHandler) Delegate.Remove(ClosedEvent, value); }
		}
				
				
		public void Event_Closing(object sender)
		{
		    ClosingEvent?.Invoke(sender);
		}

	    protected void Event_Closed(object sender)
	    {
	        ClosedEvent?.Invoke(sender);
	    }

	    protected void Event_Connecting(object sender)
	    {
	        ConnectingEvent?.Invoke(sender);
	    }

	    protected void Event_Connected(object sender)
	    {
	        ConnectedEvent?.Invoke(sender);
	    }

	    protected void Event_Disconnected(object sender, string DisconnectedMessage)
	    {
	        DisconnectedEvent?.Invoke(sender, DisconnectedMessage);
	    }

	    protected void Event_ErrorOccured(object sender, string ErrorMsg)
	    {
	        ErrorOccuredEvent?.Invoke(sender, ErrorMsg);
	    }

	    protected void Event_ReconnectGroupCloseClicked()
		{
			Close();
		}
        #endregion
	}
}