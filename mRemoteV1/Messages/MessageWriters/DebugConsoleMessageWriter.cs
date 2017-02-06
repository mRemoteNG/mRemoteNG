using System.Diagnostics;

namespace mRemoteNG.Messages.MessageWriters
{
    public class DebugConsoleMessageWriter : IMessageWriter
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
    }
}