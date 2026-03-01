using System;
using System.Drawing;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.GraphicsUtilities
{
    [SupportedOSPlatform("windows")]
    /// <summary>
    /// Gets environment graphics information using the Windows GDI+ API.
    /// </summary>
    public class GdiPlusGraphicsProvider : IGraphicsProvider
    {
        // Dpi of a 'normal' definition screen
        private const int BaselineDpi = 96;


        public SizeF GetResolutionScalingFactor()
        {
            // Use Graphics.FromHwnd(IntPtr.Zero) to get the screen DC directly.
            // This avoids creating a Form (expensive, and the old code leaked the
            // Graphics object it created via Form.CreateGraphics()). The screen DC
            // is always available, including inside Remote Desktop sessions (#1805).
            try
            {
                using Graphics g = Graphics.FromHwnd(IntPtr.Zero);
                return new SizeF(g.DpiX / BaselineDpi, g.DpiY / BaselineDpi);
            }
            catch
            {
                // Fall back to 1:1 scale if the display context is unavailable
                // (e.g., headless or heavily restricted RDP environments).
                return new SizeF(1.0f, 1.0f);
            }
        }
    }
}