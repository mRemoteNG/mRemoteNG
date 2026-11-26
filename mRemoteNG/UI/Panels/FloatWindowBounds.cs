using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace mRemoteNG.UI.Panels
{
    /// <summary>Keeps floating panel windows on a screen.</summary>
    [SupportedOSPlatform("windows")]
    internal static class FloatWindowBounds
    {
        /// <summary>
        /// Fits the bounds into the working area of the screen they overlap most, shrinking them when they
        /// are larger. Bounds that are on no screen, for instance on a monitor that has been disconnected,
        /// are centred on the primary screen.
        /// </summary>
        internal static Rectangle KeepOnScreen(Rectangle bounds)
        {
            Screen primary = Screen.PrimaryScreen ?? Screen.AllScreens.First();
            return KeepOnScreen(bounds, Screen.AllScreens.Select(screen => screen.WorkingArea).ToList(),
                                primary.WorkingArea);
        }

        internal static Rectangle KeepOnScreen(Rectangle bounds, IReadOnlyList<Rectangle> workingAreas,
                                               Rectangle primaryWorkingArea)
        {
            Rectangle target = primaryWorkingArea;
            long largestOverlap = 0;
            foreach (Rectangle workingArea in workingAreas)
            {
                Rectangle overlap = Rectangle.Intersect(workingArea, bounds);
                long area = (long)overlap.Width * overlap.Height;
                if (area > largestOverlap)
                {
                    largestOverlap = area;
                    target = workingArea;
                }
            }

            int width = Math.Min(bounds.Width, target.Width);
            int height = Math.Min(bounds.Height, target.Height);

            if (largestOverlap == 0)
            {
                return new Rectangle(target.Left + (target.Width - width) / 2,
                                     target.Top + (target.Height - height) / 2,
                                     width, height);
            }

            int x = Math.Clamp(bounds.X, target.Left, target.Right - width);
            int y = Math.Clamp(bounds.Y, target.Top, target.Bottom - height);
            return new Rectangle(x, y, width, height);
        }
    }
}
