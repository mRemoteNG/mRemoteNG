using mRemoteNG.Properties;

namespace mRemoteNG.Messages.MessageFilteringOptions
{
    public class LogMessageTypeFilteringOptions : IMessageTypeFilteringOptions
    {
        public bool AllowDebugMessages
        {
            get => Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteDebugMsgs;
            set => Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteDebugMsgs = value;
        }

        public bool AllowInfoMessages
        {
            get => Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteInfoMsgs;
            set => Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteInfoMsgs = value;
        }

        public bool AllowWarningMessages
        {
            get => Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteWarningMsgs;
            set => Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteWarningMsgs = value;
        }

        public bool AllowErrorMessages
        {
            get => Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteErrorMsgs;
            set => Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteErrorMsgs = value;
        }
    }
}