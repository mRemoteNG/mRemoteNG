using System.Collections.Generic;
using System.Diagnostics;

namespace mRemoteNG.Messages.MessagePrinters
{
    public class DebugMessagePrinter : IMessagePrinter
    {
        public bool PrintDebugMessages { get; set; } = true;
        public bool PrintInfoMessages { get; set; } = true;
        public bool PrintWarningMessages { get; set; } = true;
        public bool PrintErrorMessages { get; set; } = true;

        public void Print(IMessage message)
        {
            var textToPrint = $"{message.Class}: {message.Text}";
            Debug.Print(textToPrint);
        }

        public void Print(IEnumerable<IMessage> messages)
        {
            foreach (var message in messages)
                Print(message);
        }
    }
}