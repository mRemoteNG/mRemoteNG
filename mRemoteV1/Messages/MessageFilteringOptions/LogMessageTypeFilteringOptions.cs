using mRemoteNG.Messages.MessageWriters;

namespace mRemoteNG.Messages
{
    public class LogMessageTypeFilteringOptions : IMessageTypeFilteringOptions
    {
        public bool AllowDebugMessages
        {
            get { return Settings.Default.TextLogMessageWriterWriteDebugMsgs; }
            set { Settings.Default.TextLogMessageWriterWriteDebugMsgs = value; }
        }

        public bool AllowInfoMessages
        {
            get { return Settings.Default.TextLogMessageWriterWriteInfoMsgs; }
            set { Settings.Default.TextLogMessageWriterWriteInfoMsgs = value; }
        }

        public bool AllowWarningMessages
        {
            get { return Settings.Default.TextLogMessageWriterWriteWarningMsgs; }
            set { Settings.Default.TextLogMessageWriterWriteWarningMsgs = value; }
        }

        public bool AllowErrorMessages
        {
            get { return Settings.Default.TextLogMessageWriterWriteErrorMsgs; }
            set { Settings.Default.TextLogMessageWriterWriteErrorMsgs = value; }
        }
    }
}