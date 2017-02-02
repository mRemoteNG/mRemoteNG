using System;
using System.Collections.Generic;
using log4net;

namespace mRemoteNG.Messages.MessagePrinters
{
    public class TextLogMessagePrinter : IMessagePrinter
    {
        private readonly ILog _logger;

        public bool PrintDebugMessages { get; set; } = true;
        public bool PrintInfoMessages { get; set; } = true;
        public bool PrintWarningMessages { get; set; } = true;
        public bool PrintErrorMessages { get; set; } = true;

        public TextLogMessagePrinter(ILog logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _logger = logger;
        }

        public void Print(IMessage message)
        {
            switch (message.Class)
            {
                case MessageClass.InformationMsg:
                    if (PrintInfoMessages)
                        _logger.Info(message.Text);
                    break;
                case MessageClass.DebugMsg:
                    if (PrintDebugMessages)
                        _logger.Debug(message.Text);
                    break;
                case MessageClass.WarningMsg:
                    if (PrintWarningMessages)
                        _logger.Warn(message.Text);
                    break;
                case MessageClass.ErrorMsg:
                    if (PrintErrorMessages)
                        _logger.Error(message.Text);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Print(IEnumerable<IMessage> messages)
        {
            foreach (var message in messages)
                Print(message);
        }
    }
}