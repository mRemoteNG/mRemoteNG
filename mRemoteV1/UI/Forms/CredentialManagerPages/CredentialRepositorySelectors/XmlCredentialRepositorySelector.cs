using System.Drawing;
using mRemoteNG.Credential;
using mRemoteNG.UI.Controls;

namespace mRemoteNG.UI.Forms.CredentialManagerPages.CredentialRepositorySelectors
{
    public class XmlCredentialRepositorySelector : ISelectionTarget<ICredentialProvider>
    {
        public string Text { get; set; } = "XML";
        public Image Image { get; } = Resources.xml;
        public IFactory<ICredentialProvider> Factory { get; }
    }
}