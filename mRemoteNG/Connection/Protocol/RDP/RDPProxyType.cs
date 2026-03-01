using mRemoteNG.Resources.Language;
using mRemoteNG.Tools;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public enum RDPProxyType
    {
        [LocalizedAttributes.LocalizedDescription(nameof(Language.None))]
        None = 0,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.Http))]
        HTTP = 1,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.Socks5))]
        SOCKS5 = 2
    }
}
