namespace mRemoteNG.Messages.MessageWriters
{
    public interface IMessageWriter
    {
        void Write(IMessage message);
    }
}