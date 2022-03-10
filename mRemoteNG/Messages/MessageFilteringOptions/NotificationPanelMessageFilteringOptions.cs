using mRemoteNG.Properties;

namespace mRemoteNG.Messages.MessageFilteringOptions
{
    public class NotificationPanelMessageFilteringOptions : IMessageTypeFilteringOptions
    {
        public bool AllowDebugMessages
        {
            get => Properties.OptionsNotificationsPage.Default.NotificationPanelWriterWriteDebugMsgs;
            set => Properties.OptionsNotificationsPage.Default.NotificationPanelWriterWriteDebugMsgs = value;
        }

        public bool AllowInfoMessages
        {
            get => Properties.OptionsNotificationsPage.Default.NotificationPanelWriterWriteInfoMsgs;
            set => Properties.OptionsNotificationsPage.Default.NotificationPanelWriterWriteInfoMsgs = value;
        }

        public bool AllowWarningMessages
        {
            get => Properties.OptionsNotificationsPage.Default.NotificationPanelWriterWriteWarningMsgs;
            set => Properties.OptionsNotificationsPage.Default.NotificationPanelWriterWriteWarningMsgs = value;
        }

        public bool AllowErrorMessages
        {
            get => Properties.OptionsNotificationsPage.Default.NotificationPanelWriterWriteErrorMsgs;
            set => Properties.OptionsNotificationsPage.Default.NotificationPanelWriterWriteErrorMsgs = value;
        }
    }
}