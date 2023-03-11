using System;
using System.Runtime.Versioning;
using mRemoteNG.App;

namespace mRemoteNG.Messages.MessageWriters
{
    [SupportedOSPlatform("windows")]
    public class TextLogMessageWriter : IMessageWriter
    {
        private readonly Logger _logger;

        public TextLogMessageWriter(Logger logger)
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
                    _logger.Log.Info(message.Text);
                    break;
                case MessageClass.DebugMsg:
                    _logger.Log.Debug(message.Text);
                    break;
                case MessageClass.WarningMsg:
                    _logger.Log.Warn(message.Text);
                    break;
                case MessageClass.ErrorMsg:
                    _logger.Log.Error(message.Text);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}