using System;
using mRemoteNG.Connection;


namespace mRemoteNG.Config.Putty
{
    public class PuttySessionChangedEventArgs : EventArgs
    {
        public PuttySessionInfo Session { get; set; }

        public PuttySessionChangedEventArgs(PuttySessionInfo sessionChanged = null)
        {
            Session = sessionChanged;
        }
    }
}