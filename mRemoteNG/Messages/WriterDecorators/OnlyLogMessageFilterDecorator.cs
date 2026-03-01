using System;
using mRemoteNG.Messages.MessageWriters;

namespace mRemoteNG.Messages.WriterDecorators
{
    public class OnlyLogMessageFilter : IMessageWriter
    {
        private readonly IMessageWriter _decoratedWriter;

        public OnlyLogMessageFilter(IMessageWriter decoratedWriter)
        {
            ArgumentNullException.ThrowIfNull(decoratedWriter);
            _decoratedWriter = decoratedWriter;
        }

        public void Write(IMessage message)
        {
            if (message.OnlyLog) return;
            _decoratedWriter.Write(message);
        }
    }
}