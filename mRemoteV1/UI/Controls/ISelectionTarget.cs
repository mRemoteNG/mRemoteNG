using System.Drawing;
using mRemoteNG.Credential;
using mRemoteNG.UI.Controls.PageSequence;

namespace mRemoteNG.UI.Controls
{
    public interface ISelectionTarget<out T>
    {
        string Text { get; set; }
        Image Image { get; }
        T Config { get; }
        SequencedControl BuildEditorPage(ICredentialRepositoryList repositoryList);
    }
}