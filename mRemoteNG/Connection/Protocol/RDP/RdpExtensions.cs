using System.Drawing;
using System.Text.RegularExpressions;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public static class RdpExtensions
    {
        // Fixed pixel resolutions are encoded in the enum member name as "Res{Width}x{Height}".
        // FitToWindow/Fullscreen/SmartSize are not fixed sizes and return an empty rectangle.
        public static Rectangle GetResolutionRectangle(this RDPResolutions resolution)
        {
            Match match = Regex.Match(resolution.ToString(), @"^Res(\d+)x(\d+)$");
            if (!match.Success)
                return new Rectangle(0, 0, 0, 0);

            return new Rectangle(0, 0, int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
        }
    }
}
