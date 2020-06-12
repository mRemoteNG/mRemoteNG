using mRemoteNG.Resources.Language;
using mRemoteNG.Tools;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public enum RDPColors
    {
        [LocalizedAttributes.LocalizedDescription(nameof(Language.Rdp256Colors))]
        Colors256 = 8,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.Rdp32768Colors))]
        Colors15Bit = 15,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.Rdp65536Colors))]
        Colors16Bit = 16,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.Rdp16777216Colors))]
        Colors24Bit = 24,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.Rdp4294967296Colors))]
        Colors32Bit = 32
    }
}