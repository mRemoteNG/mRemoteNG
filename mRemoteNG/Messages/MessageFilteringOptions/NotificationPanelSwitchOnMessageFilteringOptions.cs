using mRemoteNG.Properties;

namespace mRemoteNG.Messages.MessageFilteringOptions
{
    public class NotificationPanelSwitchOnMessageFilteringOptions : IMessageTypeFilteringOptions
    {
        public bool AllowDebugMessages
        {
            get => false;
            set { }
        }

        public bool AllowInfoMessages
        {
            get => Properties.OptionsNotificationsPage.Default.SwitchToMCOnInformation;
            set => Properties.OptionsNotificationsPage.Default.SwitchToMCOnInformation = value;
        }

        public bool AllowWarningMessages
        {
            get => Properties.OptionsNotificationsPage.Default.SwitchToMCOnWarning;
            set => Properties.OptionsNotificationsPage.Default.SwitchToMCOnWarning = value;
        }

        public bool AllowErrorMessages
        {
            get => Properties.OptionsNotificationsPage.Default.SwitchToMCOnError;
            set => Properties.OptionsNotificationsPage.Default.SwitchToMCOnError = value;
        }
    }
}