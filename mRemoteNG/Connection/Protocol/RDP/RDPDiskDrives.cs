using mRemoteNG.Tools;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public enum RDPDiskDrives
    {
        [LocalizedAttributes.LocalizedDescription(nameof(Language.RdpDrivesNone))]
        None,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.RdpDrivesLocal))]
        Local,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.RdpDrivesAll))]
        All,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.RdpDrivesCustom))]
        Custom
    }
}