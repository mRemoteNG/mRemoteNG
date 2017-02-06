namespace mRemoteNG.Messages.MessageWriters
{
    public interface IMessageWriter
    {
        bool AllowDebugMessages { get; set; }
        bool AllowInfoMessages { get; set; }
        bool AllowWarningMessages { get; set; }
        bool AllowErrorMessages { get; set; }

        void Write(IMessage message);
    }
}