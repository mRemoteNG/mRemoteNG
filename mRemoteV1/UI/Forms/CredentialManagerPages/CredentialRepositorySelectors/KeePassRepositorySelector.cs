using System.Drawing;
using mRemoteNG.Credential;
using mRemoteNG.UI.Controls;

namespace mRemoteNG.UI.Forms.CredentialManagerPages.CredentialRepositorySelectors
{
    public class KeePassRepositorySelector : ISelectionTarget<ICredentialProvider>
    {
        public string Text { get; set; } = "KeePass";
        public Image Image { get; } = Resources.keepass_32x32;
        public IFactory<ICredentialProvider> Factory { get; }
    }
}