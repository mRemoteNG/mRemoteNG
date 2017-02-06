using System.Diagnostics;

namespace mRemoteNG.Messages.MessageWriters
{
    public class DebugConsoleMessageWriter : IMessageWriter
    {
        public bool AllowDebugMessages { get; set; } = true;
        public bool AllowInfoMessages { get; set; } = true;
        public bool AllowWarningMessages { get; set; } = true;
        public bool AllowErrorMessages { get; set; } = true;

        public void Write(IMessage message)
        {
            var textToPrint = $"{message.Class}: {message.Text}";
            Debug.Print(textToPrint);
        }
    }
}