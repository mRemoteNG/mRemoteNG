using System.Diagnostics;

namespace mRemoteNG.Messages.MessageWriters
{
    public class DebugConsoleMessageWriter : IMessageWriter
    {
        public void Write(IMessage message)
        {
            var textToPrint = $"{message.Class}: {message.Text}";
            Debug.Print(textToPrint);
        }
    }
}