namespace mRemoteNG.Connection.Protocol.Telnet
{
    public class ProtocolTelnet : PuttyBase
    {
        public enum Defaults
        {
            Port = 23
        }

        public ProtocolTelnet()
        {
            PuttyProtocol = Putty_Protocol.telnet;
        }
    }
}