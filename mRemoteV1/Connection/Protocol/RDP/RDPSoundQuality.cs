using mRemoteNG.Tools;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public enum RDPSoundQuality
    {
        [LocalizedAttributes.LocalizedDescription(nameof(Language.RDPSoundQualityDynamic))]
        Dynamic = 0,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.RDPSoundQualityMedium))]
        Medium = 1,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.RDPSoundQualityHigh))]
        High = 2
    }
}