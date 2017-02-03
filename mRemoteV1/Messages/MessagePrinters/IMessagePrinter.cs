using System.Collections.Generic;

namespace mRemoteNG.Messages.MessagePrinters
{
    public interface IMessagePrinter
    {
        bool PrintDebugMessages { get; set; }
        bool PrintInfoMessages { get; set; }
        bool PrintWarningMessages { get; set; }
        bool PrintErrorMessages { get; set; }

        void Print(IMessage message);

        void Print(IEnumerable<IMessage> messages);
    }
}