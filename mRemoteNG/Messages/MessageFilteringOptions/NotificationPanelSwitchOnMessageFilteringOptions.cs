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
            get => Settings.Default.SwitchToMCOnInformation;
            set => Settings.Default.SwitchToMCOnInformation = value;
        }

        public bool AllowWarningMessages
        {
            get => Settings.Default.SwitchToMCOnWarning;
            set => Settings.Default.SwitchToMCOnWarning = value;
        }

        public bool AllowErrorMessages
        {
            get => Settings.Default.SwitchToMCOnError;
            set => Settings.Default.SwitchToMCOnError = value;
        }
    }
}