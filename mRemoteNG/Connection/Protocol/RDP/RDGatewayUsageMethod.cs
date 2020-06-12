using mRemoteNG.Resources.Language;
using mRemoteNG.Tools;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public enum RDGatewayUsageMethod
    {
        [LocalizedAttributes.LocalizedDescription(nameof(Language.Never))]
        Never = 0, // TSC_PROXY_MODE_NONE_DIRECT

        [LocalizedAttributes.LocalizedDescription(nameof(Language.Always))]
        Always = 1, // TSC_PROXY_MODE_DIRECT

        [LocalizedAttributes.LocalizedDescription(nameof(Language.Detect))]
        Detect = 2 // TSC_PROXY_MODE_DETECT
    }
}