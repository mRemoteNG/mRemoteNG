namespace mRemoteNG.Connection.Protocol.RAW
{
    public class ProtocolRAW : PuttyBase
    {
        public enum Defaults
        {
            Port = 23
        }

        public ProtocolRAW()
        {
            PuttyProtocol = Putty_Protocol.raw;
        }
    }
}