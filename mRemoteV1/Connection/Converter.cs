using System;

namespace mRemoteNG.Connection.Protocol
{
    public class Converter
    {
        public static string ProtocolToString(ProtocolType protocol)
        {
            return protocol.ToString();
        }

        public static ProtocolType StringToProtocol(string protocol)
        {
            try
            {
                return (ProtocolType)Enum.Parse(typeof(ProtocolType), protocol, true);
            }
            catch (Exception)
            {
                return ProtocolType.RDP;
            }
        }
    }
}