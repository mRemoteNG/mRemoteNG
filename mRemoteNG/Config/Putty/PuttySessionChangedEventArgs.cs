using System;
using mRemoteNG.Connection;


namespace mRemoteNG.Config.Putty
{
    public class PuttySessionChangedEventArgs(PuttySessionInfo? sessionChanged = null) : EventArgs
    {
        public PuttySessionInfo? Session { get; set; } = sessionChanged;
    }
}