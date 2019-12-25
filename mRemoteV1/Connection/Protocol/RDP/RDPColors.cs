using mRemoteNG.Tools;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public enum RDPColors
    {
        [LocalizedAttributes.LocalizedDescription(nameof(Language.strRDP256Colors))]
        Colors256 = 8,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.strRDP32768Colors))]
        Colors15Bit = 15,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.strRDP65536Colors))]
        Colors16Bit = 16,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.strRDP16777216Colors))]
        Colors24Bit = 24,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.strRDP4294967296Colors))]
        Colors32Bit = 32
    }
}