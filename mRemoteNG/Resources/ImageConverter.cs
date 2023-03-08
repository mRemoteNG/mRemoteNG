using System.Drawing;
using System.Runtime.Versioning;

namespace mRemoteNG.Resources
{
    [SupportedOSPlatform("windows")]
    class ImageConverter
    {
        /// <summary>
        /// Draws an Icon from a Bitmap
        /// </summary>
        /// <param name="SVGString"></param>
        /// <returns></returns>
        internal static Icon GetImageAsIcon(Bitmap bitmap)
        {
            var icon = Icon.FromHandle(bitmap.GetHicon());

            return icon;
        }
    }
}
