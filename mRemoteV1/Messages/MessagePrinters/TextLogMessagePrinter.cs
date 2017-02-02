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
            if (!WeShouldPrint(message.Class))
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

        public void Print(IEnumerable<IMessage> messages)
        {
            foreach (var message in messages)
                Print(message);
        }

        private bool WeShouldPrint(MessageClass msgClass)
        {
            switch (msgClass)
            {
                case MessageClass.InformationMsg:
                    if (PrintInfoMessages) return true;
                    break;
                case MessageClass.WarningMsg:
                    if (PrintWarningMessages) return true;
                    break;
                case MessageClass.ErrorMsg:
                    if (PrintErrorMessages) return true;
                    break;
                case MessageClass.DebugMsg:
                    if (PrintDebugMessages) return true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(msgClass), msgClass, null);
            }
            return false;
        }
    }
}