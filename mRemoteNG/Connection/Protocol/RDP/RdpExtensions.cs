using System;
using System.Drawing;
using System.Globalization;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public static class RdpExtensions
    {
        public static Rectangle GetResolutionRectangle(this RDPResolutions resolution)
        {
            string[]? resolutionParts = null;
            if (resolution != RDPResolutions.FitToWindow & resolution != RDPResolutions.Fullscreen &
                resolution != RDPResolutions.SmartSize & resolution != RDPResolutions.SmartSizeAspect)
            {
                resolutionParts = resolution.ToString().Replace("Res", "", StringComparison.Ordinal).Split('x');
            }

            if (resolutionParts == null || resolutionParts.Length != 2)
            {
                return new Rectangle(0, 0, 0, 0);
            }
            else
            {
                return new Rectangle(0, 0, Convert.ToInt32(resolutionParts[0], CultureInfo.InvariantCulture), Convert.ToInt32(resolutionParts[1], CultureInfo.InvariantCulture));
            }
        }
    }
}
