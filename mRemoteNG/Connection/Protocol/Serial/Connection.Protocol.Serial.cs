using System.Runtime.Versioning;

namespace mRemoteNG.Connection.Protocol.Serial
{
    [SupportedOSPlatform("windows")]
    public class ProtocolSerial : PuttyBase
    {
        public ProtocolSerial()
        {
            this.PuttyProtocol = Putty_Protocol.serial;
        }

        public enum Defaults
        {
            Port = 9600
        }

        public enum Parity
        {
            None,
            Odd,
            Even,
            Mark,
            Space
        }

        public enum StopBits
        {
            One,
            OnePointFive,
            Two
        }

        public enum FlowControl
        {
            None,
            XonXoff,
            RtsCts,
            DsrDtr
        }
    }
}
