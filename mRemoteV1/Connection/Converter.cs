using System;
using mRemoteNG.Connection.Protocol;

namespace mRemoteNG.Connection
{
    public static class Converter
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