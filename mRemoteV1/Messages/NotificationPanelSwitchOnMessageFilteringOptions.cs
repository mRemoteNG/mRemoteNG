using mRemoteNG.Messages.MessageWriters;

namespace mRemoteNG.Messages
{
    public class NotificationPanelSwitchOnMessageFilteringOptions : IMessageTypeFilteringOptions
    {
        public bool AllowDebugMessages
        {
            get { return false; }
            set { }
        }

        public bool AllowInfoMessages
        {
            get { return Settings.Default.SwitchToMCOnInformation; }
            set { Settings.Default.SwitchToMCOnInformation = value; }
        }

        public bool AllowWarningMessages
        {
            get { return Settings.Default.SwitchToMCOnWarning; }
            set { Settings.Default.SwitchToMCOnWarning = value; }
        }

        public bool AllowErrorMessages
        {
            get { return Settings.Default.SwitchToMCOnError; }
            set { Settings.Default.SwitchToMCOnError = value; }
        }
    }
}