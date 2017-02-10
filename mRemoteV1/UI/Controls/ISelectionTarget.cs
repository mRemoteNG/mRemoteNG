using System.Drawing;

namespace mRemoteNG.UI.Controls
{
    public interface ISelectionTarget<out T>
    {
        string Text { get; set; }
        Image Image { get; }
        T Config { get; }
    }
}