using System.Drawing;
using System.Windows.Forms;

namespace mRemoteNG.UI.GraphicsUtilities
{
    /// <summary>
    /// Gets environment graphics information using the Windows GDI+ API.
    /// </summary>
    public class GdiPlusGraphicsProvider : IGraphicsProvider
    {
        // Dpi of a 'normal' definition screen
        private const int BaselineDpi = 96;

        public SizeF GetResolutionScalingFactor()
        {
            using (var g = new Form().CreateGraphics())
            {
                return new SizeF(g.DpiX / BaselineDpi, g.DpiY / BaselineDpi);
            }
        }
    }
}
