using System.Collections.Generic;

namespace mRemoteNG.Messages.MessagePrinters
{
    public interface IMessagePrinter
    {
        void Print(IMessage message);

        void Print(IEnumerable<IMessage> messages);
    }
}