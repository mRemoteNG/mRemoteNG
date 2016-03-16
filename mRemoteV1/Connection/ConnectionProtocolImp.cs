using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace mRemoteNG.Connection
{
    public class ConnectionProtocolImp : ConnectionProtocol
    {
        private ConnectionProtocol _strategy;
        private ConnectingEventHandler ConnectingEvent;
        private ConnectedEventHandler ConnectedEvent;
        private DisconnectedEventHandler DisconnectedEvent;
        private ErrorOccuredEventHandler ErrorOccuredEvent;
        private ClosingEventHandler ClosingEvent;
        private ClosedEventHandler ClosedEvent;

        Protocols Name { get; }
        PropertyInfo[] SupportedSettings { get; }
        Version ProtocolVersion { get; }

        public ConnectionProtocolImp(ConnectionProtocol Protocol)
        {
            this._strategy = Protocol;
        }

        public ConnectionProtocolImp(Protocols ProtocolEnum)
        {
            this._strategy = ProtocolManagerImp.GetConnectionProtocol(ProtocolEnum);
        }

        public void Initialize()
        {
            _strategy.Initialize();
        }

        public void Connect()
        {
            _strategy.Connect();
        }

        public void Disconnect()
        {
            _strategy.Disconnect();
        }

        delegate void ConnectingEventHandler(object sender);
        delegate void ConnectedEventHandler(object sender);
        delegate void DisconnectedEventHandler(object sender, string DisconnectedMessage);
        delegate void ErrorOccuredEventHandler(object sender, string ErrorMessage);
        delegate void ClosingEventHandler(object sender);
        delegate void ClosedEventHandler(object sender);

        event ConnectingEventHandler Connecting;
        event ConnectedEventHandler Connected;
        event DisconnectedEventHandler Disconnected;
        event ErrorOccuredEventHandler ErrorOccured;
        event ClosingEventHandler Closing;
        event ClosedEventHandler Closed;

        void Event_Connecting(object sender);
        void Event_Connected(object sender);
        void Event_Disconnected(object sender, string DisconnectedMessage);
        void Event_ErrorOccured(object sender, string ErrorMsg);
        void Event_Closing(object sender);
        void Event_Closed(object sender);
    }
}