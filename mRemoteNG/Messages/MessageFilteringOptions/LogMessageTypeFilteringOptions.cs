using mRemoteNG.Properties;

namespace mRemoteNG.Messages.MessageFilteringOptions
{
    public class LogMessageTypeFilteringOptions : IMessageTypeFilteringOptions
    {
        public bool AllowDebugMessages
        {
            get => Settings.Default.TextLogMessageWriterWriteDebugMsgs;
            set => Settings.Default.TextLogMessageWriterWriteDebugMsgs = value;
        }

        public bool AllowInfoMessages
        {
            get => Settings.Default.TextLogMessageWriterWriteInfoMsgs;
            set => Settings.Default.TextLogMessageWriterWriteInfoMsgs = value;
        }

        public bool AllowWarningMessages
        {
            get => Settings.Default.TextLogMessageWriterWriteWarningMsgs;
            set => Settings.Default.TextLogMessageWriterWriteWarningMsgs = value;
        }

        public bool AllowErrorMessages
        {
            get => Settings.Default.TextLogMessageWriterWriteErrorMsgs;
            set => Settings.Default.TextLogMessageWriterWriteErrorMsgs = value;
        }
    }
}