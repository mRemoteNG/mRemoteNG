using System;
using log4net;

namespace mRemoteNG.Messages.MessageWriters
{
    public class TextLogMessageWriter : IMessageWriter
    {
        private readonly ILog _logger;

        public bool AllowDebugMessages { get; set; } = true;
        public bool AllowInfoMessages { get; set; } = true;
        public bool AllowWarningMessages { get; set; } = true;
        public bool AllowErrorMessages { get; set; } = true;

        public TextLogMessageWriter(ILog logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _logger = logger;
        }

        public void Write(IMessage message)
        {
            if (!WeShouldPrint(message))
                return;

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

        private bool WeShouldPrint(IMessage message)
        {
            switch (message.Class)
            {
                case MessageClass.InformationMsg:
                    if (AllowInfoMessages) return true;
                    break;
                case MessageClass.WarningMsg:
                    if (AllowWarningMessages) return true;
                    break;
                case MessageClass.ErrorMsg:
                    if (AllowErrorMessages) return true;
                    break;
                case MessageClass.DebugMsg:
                    if (AllowDebugMessages) return true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(message.Class), message.Class, null);
            }
            return false;
        }
    }
}