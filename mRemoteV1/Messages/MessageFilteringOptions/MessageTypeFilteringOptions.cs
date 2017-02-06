using mRemoteNG.Messages.MessageWriters;

namespace mRemoteNG.Messages
{
    public class MessageTypeFilteringOptions : IMessageTypeFilteringOptions
    {
        public bool AllowDebugMessages { get; set; }
        public bool AllowInfoMessages { get; set; }
        public bool AllowWarningMessages { get; set; }
        public bool AllowErrorMessages { get; set; }
    }
}