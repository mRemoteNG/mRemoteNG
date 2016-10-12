namespace mRemoteNG.Connection.Protocol.Serial
{
    public class ProtocolSerial : PuttyBase
    {
        public enum Defaults
        {
            Port = 9600
        }

        public ProtocolSerial()
        {
            PuttyProtocol = Putty_Protocol.serial;
        }
    }
}