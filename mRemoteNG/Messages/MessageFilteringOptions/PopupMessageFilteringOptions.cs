using mRemoteNG.Properties;

namespace mRemoteNG.Messages.MessageFilteringOptions
{
    public class PopupMessageFilteringOptions : IMessageTypeFilteringOptions
    {
        public bool AllowDebugMessages
        {
            get => Settings.Default.PopupMessageWriterWriteDebugMsgs;
            set => Settings.Default.PopupMessageWriterWriteDebugMsgs = value;
        }

        public bool AllowInfoMessages
        {
            get => Settings.Default.PopupMessageWriterWriteInfoMsgs;
            set => Settings.Default.PopupMessageWriterWriteInfoMsgs = value;
        }

        public bool AllowWarningMessages
        {
            get => Settings.Default.PopupMessageWriterWriteWarningMsgs;
            set => Settings.Default.PopupMessageWriterWriteWarningMsgs = value;
        }

        public bool AllowErrorMessages
        {
            get => Settings.Default.PopupMessageWriterWriteErrorMsgs;
            set => Settings.Default.PopupMessageWriterWriteErrorMsgs = value;
        }
    }
}