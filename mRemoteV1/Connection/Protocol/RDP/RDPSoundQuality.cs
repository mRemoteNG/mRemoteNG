using mRemoteNG.Tools;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public enum RDPSoundQuality
    {
        [LocalizedAttributes.LocalizedDescription(nameof(Language.strRDPSoundQualityDynamic))]
        Dynamic = 0,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.strRDPSoundQualityMedium))]
        Medium = 1,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.strRDPSoundQualityHigh))]
        High = 2
    }
}