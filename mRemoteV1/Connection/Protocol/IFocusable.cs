using System;

namespace mRemoteNG.Connection.Protocol
{
    public interface IFocusable
    {
        bool HasFocus { get; }
        void Focus();
        event EventHandler FocusChanged;
    }
}
