using System;
using mRemoteNG.Connection;

namespace mRemoteNG.Config.Putty
{
    public class PuttySessionChangedEventArgs : EventArgs
    {
        public PuttySessionChangedEventArgs(PuttySessionInfo sessionChanged = null)
        {
            Session = sessionChanged;
        }

        public PuttySessionInfo Session { get; set; }
    }
}