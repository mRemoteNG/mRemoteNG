using mRemoteNG.Properties;

namespace mRemoteNG.Messages.MessageFilteringOptions
{
    public class NotificationPanelMessageFilteringOptions : IMessageTypeFilteringOptions
    {
        public bool AllowDebugMessages
        {
            get => Settings.Default.NotificationPanelWriterWriteDebugMsgs;
            set => Settings.Default.NotificationPanelWriterWriteDebugMsgs = value;
        }

        public bool AllowInfoMessages
        {
            get => Settings.Default.NotificationPanelWriterWriteInfoMsgs;
            set => Settings.Default.NotificationPanelWriterWriteInfoMsgs = value;
        }

        public bool AllowWarningMessages
        {
            get => Settings.Default.NotificationPanelWriterWriteWarningMsgs;
            set => Settings.Default.NotificationPanelWriterWriteWarningMsgs = value;
        }

        public bool AllowErrorMessages
        {
            get => Settings.Default.NotificationPanelWriterWriteErrorMsgs;
            set => Settings.Default.NotificationPanelWriterWriteErrorMsgs = value;
        }
    }
}