using System;
using log4net;

namespace mRemoteNG.Messages.MessageWriters
{
    public class TextLogMessageWriter : IMessageWriter
    {
        private readonly ILog _logger;

        public TextLogMessageWriter(ILog logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _logger = logger;
        }

        public void Write(IMessage message)
        {
            switch (message.Class)
            {
                case MessageClass.InformationMsg:
                    _logger.Info(message.Text);
                    break;
                case MessageClass.DebugMsg:
                    _logger.Debug(message.Text);
                    break;
                case MessageClass.WarningMsg:
                    _logger.Warn(message.Text);
                    break;
                case MessageClass.ErrorMsg:
                    _logger.Error(message.Text);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}