namespace mRemoteNG.Connection.Protocol.Rlogin
{
    public class ProtocolRlogin : PuttyBase
    {
        public enum Defaults
        {
            Port = 513
        }

        public ProtocolRlogin()
        {
            PuttyProtocol = Putty_Protocol.rlogin;
        }
    }
}