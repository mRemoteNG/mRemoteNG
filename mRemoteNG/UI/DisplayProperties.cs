using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.Versioning;
using mRemoteNG.Tools;
using mRemoteNG.UI.GraphicsUtilities;

namespace mRemoteNG.UI
{
    [SupportedOSPlatform("windows")]
    public class DisplayProperties
    {
        private readonly IGraphicsProvider _graphicsProvider;

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
        /// Creates a new <see cref="DisplayProperties"/> instance with the given
        /// <see cref="IGraphicsProvider"/>.
        /// </summary>
        /// <param name="graphicsProvider"></param>
        public DisplayProperties(IGraphicsProvider graphicsProvider)
        {
            _graphicsProvider = graphicsProvider.ThrowIfNull(nameof(graphicsProvider));
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
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            var width = ScaleWidth(image.Width);
            var height = ScaleHeight(image.Height);
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public Bitmap ScaleImage(Icon icon)
        {
            if (icon == null)
                throw new ArgumentNullException(nameof(icon));

            return ScaleImage(icon.ToBitmap());
        }

        /// <summary>
        /// Scale the given nominal height value by the <see cref="ResolutionScalingFactor"/>
        /// </summary>
        /// <param name="width"></param>
        private int CalculateScaledValue(float value, float scalingValue)
        {
            return (int)Math.Round(value * scalingValue);
        }
    }
}