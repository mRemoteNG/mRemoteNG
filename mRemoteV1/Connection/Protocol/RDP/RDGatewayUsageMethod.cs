using mRemoteNG.Tools;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public enum RDGatewayUsageMethod
    {
        [LocalizedAttributes.LocalizedDescription(nameof(Language.strNever))]
        Never = 0, // TSC_PROXY_MODE_NONE_DIRECT

        [LocalizedAttributes.LocalizedDescription(nameof(Language.strAlways))]
        Always = 1, // TSC_PROXY_MODE_DIRECT

        [LocalizedAttributes.LocalizedDescription(nameof(Language.strDetect))]
        Detect = 2 // TSC_PROXY_MODE_DETECT
    }
}