using System.Drawing;
using System.Windows.Forms;

namespace mRemoteNG.UI
{
    public class DisplayProperties
    {
        // Dpi of a 'normal' definition screen
        private const int BaselineDpi = 96;

        public SizeF ResolutionScalingFactor { get; } = GetResolutionScalingFactor();

        private static SizeF GetResolutionScalingFactor()
        {
            using (var g = new Form().CreateGraphics())
            {
                return new SizeF(g.DpiX/BaselineDpi, g.DpiY / BaselineDpi);
            }
        }
    }
}
