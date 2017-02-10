using System.Drawing;
using mRemoteNG.Credential;

namespace mRemoteNG.UI.Controls
{
    public interface ISelectionTarget<out T>
    {
        string Text { get; set; }
        Image Image { get; }
        IFactory<T> Factory { get; }
    }
}