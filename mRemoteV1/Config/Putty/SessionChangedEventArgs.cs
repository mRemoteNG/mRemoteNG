using System;
using mRemoteNG.Connection;


namespace mRemoteNG.Config.Putty
{
    public class SessionChangedEventArgs : EventArgs
    {
        public PuttySessionInfo Session { get; set; }

        public SessionChangedEventArgs(PuttySessionInfo sessionChanged = null)
        {
            Session = sessionChanged;
        }
    }
}