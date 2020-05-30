using mRemoteNG.Tools;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public enum RDPColors
    {
        [LocalizedAttributes.LocalizedDescription(nameof(Language.RDP256Colors))]
        Colors256 = 8,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.RDP32768Colors))]
        Colors15Bit = 15,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.RDP65536Colors))]
        Colors16Bit = 16,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.RDP16777216Colors))]
        Colors24Bit = 24,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.RDP4294967296Colors))]
        Colors32Bit = 32
    }
}