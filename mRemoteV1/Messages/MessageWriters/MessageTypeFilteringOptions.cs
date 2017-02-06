namespace mRemoteNG.Messages.MessageWriters
{
    public class MessageTypeFilteringOptions : IMessageTypeFilter
    {
        public bool AllowDebugMessages { get; set; }
        public bool AllowInfoMessages { get; set; }
        public bool AllowWarningMessages { get; set; }
        public bool AllowErrorMessages { get; set; }
    }
}
