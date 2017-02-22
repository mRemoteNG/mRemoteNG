using System;

namespace mRemoteNG.Messages.MessageWriters
{
    public class OnlyLogMessageFilter : IMessageWriter
    {
        private readonly IMessageWriter _decoratedWriter;

        public OnlyLogMessageFilter(IMessageWriter decoratedWriter)
        {
            if (decoratedWriter == null)
                throw new ArgumentNullException(nameof(decoratedWriter));

            _decoratedWriter = decoratedWriter;
        }

        public void Write(IMessage message)
        {
            if (message.OnlyLog) return;
            _decoratedWriter.Write(message);
        }
    }
}