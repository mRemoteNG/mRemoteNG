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

        // A user-typed custom resolution "WidthxHeight" (e.g. "1920x1080"). Whitespace and a
        // "*" separator ("1920*1080") are tolerated. Values are clamped to the RDP protocol
        // limits (200-8192 px per axis); anything outside that or unparseable returns false.
        public static bool TryParseCustomResolution(string value, out int width, out int height)
        {
            width = 0;
            height = 0;
            if (string.IsNullOrWhiteSpace(value))
                return false;

            Match match = Regex.Match(value.Trim(), @"^(\d{1,5})\s*[x*]\s*(\d{1,5})$", RegexOptions.IgnoreCase);
            if (!match.Success)
                return false;

            int w = int.Parse(match.Groups[1].Value);
            int h = int.Parse(match.Groups[2].Value);
            if (w < 200 || w > 8192 || h < 200 || h > 8192)
                return false;

            width = w;
            height = h;
            return true;
        }
    }
}
