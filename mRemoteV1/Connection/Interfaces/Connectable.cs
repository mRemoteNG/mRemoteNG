using System;
using System.Collections.Generic;
using System.Text;

namespace mRemoteNG.Connection
{
    public interface Connectable
    {
        void Connect();
        void Disconnect();
        //void Close();

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