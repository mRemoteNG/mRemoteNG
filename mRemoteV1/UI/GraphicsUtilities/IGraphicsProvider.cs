using System.Drawing;

namespace mRemoteNG.UI.GraphicsUtilities
{
    public interface IGraphicsProvider
    {
        SizeF GetResolutionScalingFactor();
    }
}