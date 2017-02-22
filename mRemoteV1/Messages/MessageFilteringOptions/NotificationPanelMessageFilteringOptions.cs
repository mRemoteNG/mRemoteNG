using mRemoteNG.Messages.MessageWriters;

namespace mRemoteNG.Messages
{
    public class NotificationPanelMessageFilteringOptions : IMessageTypeFilteringOptions
    {
        public bool AllowDebugMessages
        {
            get { return Settings.Default.NotificationPanelWriterWriteDebugMsgs; }
            set { Settings.Default.NotificationPanelWriterWriteDebugMsgs = value; }
        }

        public bool AllowInfoMessages
        {
            get { return Settings.Default.NotificationPanelWriterWriteInfoMsgs; }
            set { Settings.Default.NotificationPanelWriterWriteInfoMsgs = value; }
        }

        public bool AllowWarningMessages
        {
            get { return Settings.Default.NotificationPanelWriterWriteWarningMsgs; }
            set { Settings.Default.NotificationPanelWriterWriteWarningMsgs = value; }
        }

        public bool AllowErrorMessages
        {
            get { return Settings.Default.NotificationPanelWriterWriteErrorMsgs; }
            set { Settings.Default.NotificationPanelWriterWriteErrorMsgs = value; }
        }
    }
}