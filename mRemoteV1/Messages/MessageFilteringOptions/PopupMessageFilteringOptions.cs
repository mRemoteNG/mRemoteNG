using mRemoteNG.Messages.MessageWriters;

namespace mRemoteNG.Messages
{
    public class PopupMessageFilteringOptions : IMessageTypeFilteringOptions
    {
        public bool AllowDebugMessages
        {
            get { return Settings.Default.PopupMessageWriterWriteDebugMsgs; }
            set { Settings.Default.PopupMessageWriterWriteDebugMsgs = value; }
        }

        public bool AllowInfoMessages
        {
            get { return Settings.Default.PopupMessageWriterWriteInfoMsgs; }
            set { Settings.Default.PopupMessageWriterWriteInfoMsgs = value; }
        }

        public bool AllowWarningMessages
        {
            get { return Settings.Default.PopupMessageWriterWriteWarningMsgs; }
            set { Settings.Default.PopupMessageWriterWriteWarningMsgs = value; }
        }

        public bool AllowErrorMessages
        {
            get { return Settings.Default.PopupMessageWriterWriteErrorMsgs; }
            set { Settings.Default.PopupMessageWriterWriteErrorMsgs = value; }
        }
    }
}