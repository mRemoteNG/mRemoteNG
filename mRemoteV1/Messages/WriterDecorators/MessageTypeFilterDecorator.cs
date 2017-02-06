using System;

namespace mRemoteNG.Messages.MessageWriters
{
    public class MessageTypeFilterDecorator : IMessageWriter
    {
        private readonly IMessageTypeFilteringOptions _filter;
        private readonly IMessageWriter _decoratedWriter;

        public MessageTypeFilterDecorator(IMessageTypeFilteringOptions filter, IMessageWriter decoratedWriter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));
            if (decoratedWriter == null)
                throw new ArgumentNullException(nameof(decoratedWriter));

            _filter = filter;
            _decoratedWriter = decoratedWriter;
        }

        public void Write(IMessage message)
        {
            if (WeShouldWrite(message))
                _decoratedWriter.Write(message);
        }

        private bool WeShouldWrite(IMessage message)
        {
            switch (message.Class)
            {
                case MessageClass.InformationMsg:
                    if (_filter.AllowInfoMessages) return true;
                    break;
                case MessageClass.WarningMsg:
                    if (_filter.AllowWarningMessages) return true;
                    break;
                case MessageClass.ErrorMsg:
                    if (_filter.AllowErrorMessages) return true;
                    break;
                case MessageClass.DebugMsg:
                    if (_filter.AllowDebugMessages) return true;
                    break;
            }
            return false;
        }
    }
}
