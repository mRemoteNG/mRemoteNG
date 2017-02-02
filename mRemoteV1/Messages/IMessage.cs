using System;

namespace mRemoteNG.Messages
{
    public interface IMessage
    {
        MessageClass MsgClass { get; set; }

        string MsgText { get; set; }

        DateTime MsgDate { get; set; }
    }
}