namespace mRemoteNG.Messages.MessageFilteringOptions
{
    public interface IMessageTypeFilteringOptions
    {
        bool AllowTraceMessages { get; set; }
        bool AllowDebugMessages { get; set; }
        bool AllowInfoMessages { get; set; }
        bool AllowWarningMessages { get; set; }
        bool AllowErrorMessages { get; set; }
    }
}