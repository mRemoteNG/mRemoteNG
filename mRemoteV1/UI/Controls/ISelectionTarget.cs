using System.Drawing;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.Tools;
using mRemoteNG.UI.Controls.PageSequence;
using mRemoteNG.UI.Forms.CredentialManager;

namespace mRemoteNG.UI.Controls
{
    public interface ISelectionTarget<out T>
    {
        string Text { get; set; }
        Image Image { get; }
        T DefaultConfig { get; }
        SequencedControl BuildEditorPage(
            Optional<ICredentialRepositoryConfig> config,
            ICredentialRepositoryList repositoryList, 
            PageWorkflowController pageWorkflowController);
    }
}