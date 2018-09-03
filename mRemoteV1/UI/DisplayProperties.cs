using System;
using System.Drawing;
using System.Windows.Forms;

namespace mRemoteNG.UI
{
    public class DisplayProperties
    {
        // Dpi of a 'normal' definition screen
        private const int BaselineDpi = 96;

        public SizeF ResolutionScalingFactor { get; } = GetResolutionScalingFactor();

        /// <summary>
        /// Scale the given nominal width value by the <see cref="ResolutionScalingFactor"/>
        /// </summary>
        /// <param name="width"></param>
        public int CalculateScaledWidth(int width)
        {
            return (int) Math.Round(width * ResolutionScalingFactor.Width);
        }

        private static SizeF GetResolutionScalingFactor()
        {
            using (var g = new Form().CreateGraphics())
            {
                return new SizeF(g.DpiX/BaselineDpi, g.DpiY / BaselineDpi);
            }
        }
    }
}
