using System;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Tools;
using mRemoteNG.UI.Window;


namespace mRemoteNG.Connection.Protocol
{
	public abstract class ProtocolBase
    {
	    private ConnectionWindow _connectionWindow;
        private InterfaceControl _interfaceControl;

        #region Public Properties
        #region Control
        private string Name { get; }

	    protected ConnectionWindow ConnectionWindow
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
				ConnectionWindow = _interfaceControl.GetContainerControl() as ConnectionWindow;
			}
		}

        protected Control Control { get; set; }

	    #endregion

        public ConnectionInfo.Force Force { get; set; }

	    public readonly System.Timers.Timer tmrReconnect = new System.Timers.Timer(2000);
        protected ReconnectGroup ReconnectGroup;
        #endregion

        protected ProtocolBase(string name)
        {
            Name = name;
        }

        protected ProtocolBase()
        {
        }

        #region Methods
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
		    RaiseConnectionConnectedEvent(this);
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
		    RaiseConnectionClosedEvent(this);
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
						Runtime.MessageCollector?.AddExceptionStackTrace("Couldn't dispose control, probably form is already closed (Connection.Protocol.Base)", ex);
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
                    Runtime.MessageCollector?.AddExceptionStackTrace("Couldn't set InterfaceControl.Parent.Tag or Dispose Interface, probably form is already closed (Connection.Protocol.Base)", ex);
			    }
			}
			catch (Exception ex)
			{
                Runtime.MessageCollector?.AddExceptionStackTrace("Couldn't Close InterfaceControl BG (Connection.Protocol.Base)", ex);
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
        public event EventHandler Connecting;
        public event EventHandler Connected;
        public event EventHandler Closing;
        public event EventHandler Closed;
		
		public delegate void DisconnectedEventHandler(object sender, string disconnectedMessage);
        public event DisconnectedEventHandler Disconnected;
		
		public delegate void ErrorOccuredEventHandler(object sender, string errorMessage);
        public event ErrorOccuredEventHandler ErrorOccured;
				
        protected void RaiseConnectionClosingEvent(object sender)
		{
		    Closing?.Invoke(sender, EventArgs.Empty);
		}

	    protected void RaiseConnectionClosedEvent(object sender)
	    {
	        Closed?.Invoke(sender, EventArgs.Empty);
	    }

	    protected void RaiseConnectionConnectingEvent(object sender)
	    {
	        Connecting?.Invoke(sender, EventArgs.Empty);
	    }

	    protected void RaiseConnectionConnectedEvent(object sender)
	    {
	        Connected?.Invoke(sender, EventArgs.Empty);
	    }

	    protected void RaiseConnectionDisconnectedEvent(object sender, string disconnectedMessage)
	    {
	        Disconnected?.Invoke(sender, disconnectedMessage);
	    }

	    protected void RaiseErrorOccuredEvent(object sender, string errorMsg)
	    {
	        ErrorOccured?.Invoke(sender, errorMsg);
	    }
        #endregion
	}
}