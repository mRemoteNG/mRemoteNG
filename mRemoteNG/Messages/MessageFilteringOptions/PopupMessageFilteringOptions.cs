using mRemoteNG.Properties;

namespace mRemoteNG.Messages.MessageFilteringOptions
{
    public class PopupMessageFilteringOptions : IMessageTypeFilteringOptions
    {
        public bool AllowDebugMessages
        {
            get => Properties.OptionsNotificationsPage.Default.PopupMessageWriterWriteDebugMsgs;
            set => Properties.OptionsNotificationsPage.Default.PopupMessageWriterWriteDebugMsgs = value;
        }

        public bool AllowInfoMessages
        {
            get => Properties.OptionsNotificationsPage.Default.PopupMessageWriterWriteInfoMsgs;
            set => Properties.OptionsNotificationsPage.Default.PopupMessageWriterWriteInfoMsgs = value;
        }

        public bool AllowWarningMessages
        {
            get => Properties.OptionsNotificationsPage.Default.PopupMessageWriterWriteWarningMsgs;
            set => Properties.OptionsNotificationsPage.Default.PopupMessageWriterWriteWarningMsgs = value;
        }

        public bool AllowErrorMessages
        {
            get => Properties.OptionsNotificationsPage.Default.PopupMessageWriterWriteErrorMsgs;
            set => Properties.OptionsNotificationsPage.Default.PopupMessageWriterWriteErrorMsgs = value;
        }
    }
}