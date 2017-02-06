using System.Collections.Generic;

namespace mRemoteNG.Messages.MessageWriters
{
    public interface IMessageWriter
    {
        bool PrintDebugMessages { get; set; }
        bool PrintInfoMessages { get; set; }
        bool PrintWarningMessages { get; set; }
        bool PrintErrorMessages { get; set; }

        void Print(IMessage message);
    }
}