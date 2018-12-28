using System.Drawing;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.UI.Controls;
using mRemoteNG.UI.Controls.PageSequence;

namespace mRemoteNG.UI.Forms.CredentialManagerPages.CredentialRepositorySelectors
{
    public class KeePassRepositorySelector : ISelectionTarget<ICredentialRepositoryConfig>
    {
        public string Text { get; set; } = "KeePass";
        public Image Image { get; } = Resources.keepass_32x32;
        public ICredentialRepositoryConfig Config { get; } = new CredentialRepositoryConfig {TypeName = "KeePass"};

        public SequencedControl BuildEditorPage(ICredentialRepositoryList repositoryList)
        {
            // TODO
            return new SequencedControl();
        }
    }
}