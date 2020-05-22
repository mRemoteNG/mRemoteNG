namespace mRemoteNG.Connection.Protocol.Http
{
    public class ProtocolHTTP : HTTPBase
    {
        public ProtocolHTTP() : base()
        {
            httpOrS = "http";
            defaultPort = (int)Defaults.Port;
        }

        public enum Defaults
        {
            Port = 80
        }
    }
}