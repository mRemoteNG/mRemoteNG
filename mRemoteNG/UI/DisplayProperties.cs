using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.Versioning;
using mRemoteNG.Tools;
using mRemoteNG.UI.GraphicsUtilities;

namespace mRemoteNG.UI
{
    /// <summary>
    /// Creates a new <see cref="DisplayProperties"/> instance with the given
    /// <see cref="IGraphicsProvider"/>.
    /// </summary>
    /// <param name="graphicsProvider"></param>
    [SupportedOSPlatform("windows")]
    public class DisplayProperties(IGraphicsProvider graphicsProvider)
    {
        private readonly IGraphicsProvider _graphicsProvider = graphicsProvider.ThrowIfNull(nameof(graphicsProvider));

        public SizeF ResolutionScalingFactor => _graphicsProvider.GetResolutionScalingFactor();

        /// <summary>
        /// Creates a new <see cref="DisplayProperties"/> instance with the default
        /// <see cref="IGraphicsProvider"/> of type <see cref="GdiPlusGraphicsProvider"/>
        /// </summary>
        public DisplayProperties()
            : this(new GdiPlusGraphicsProvider())
        {
        }

        /// <summary>
        /// Scale the given nominal width value by the <see cref="ResolutionScalingFactor"/>
        /// </summary>
        /// <param name="width"></param>
        public int ScaleWidth(float width)
        {
            return CalculateScaledValue(width, ResolutionScalingFactor.Width);
        }

        /// <summary>
        /// Scale the given nominal height value by the <see cref="ResolutionScalingFactor"/>
        /// </summary>
        /// <param name="height"></param>
        public int ScaleHeight(float height)
        {
            return CalculateScaledValue(height, ResolutionScalingFactor.Height);
        }

        /// <summary>
        /// Scales the height and width of the given <see cref="Size"/> struct
        /// by the <see cref="ResolutionScalingFactor"/>
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public Size ScaleSize(Size size)
        {
            return new Size(ScaleWidth(size.Width), ScaleHeight(size.Height));
        }

        /// <summary>
        /// Scales the given image by <see cref="ResolutionScalingFactor"/>
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <returns>The resized image.</returns>
        /// <remarks>
        /// Code from https://stackoverflow.com/questions/1922040/how-to-resize-an-image-c-sharp
        /// </remarks>
        public Bitmap ScaleImage(Image image)
        {
            ArgumentNullException.ThrowIfNull(image);

            int width = ScaleWidth(image.Width);
            int height = ScaleHeight(image.Height);
            Rectangle destRect = new(0, 0, width, height);
            Bitmap destImage = new(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (Graphics graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (ImageAttributes wrapMode = new())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public Bitmap ScaleImage(Icon icon)
        {
            ArgumentNullException.ThrowIfNull(icon);

            return ScaleImage(icon.ToBitmap());
        }

        /// <summary>
        /// Scale the given nominal height value by the <see cref="ResolutionScalingFactor"/>
        /// </summary>
        /// <param name="width"></param>
        private static int CalculateScaledValue(float value, float scalingValue)
        {
            return (int)Math.Round(value * scalingValue);
        }
    }
}