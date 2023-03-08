using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;

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
            //This method could be optimized, as it is called for every control / subcontrol 
            //and causes overhead for 100s in the options page
            using (var f = new Form())
            {
                var g = f.CreateGraphics();
                return new SizeF(g.DpiX / BaselineDpi, g.DpiY / BaselineDpi);
            }
        }
    }
}