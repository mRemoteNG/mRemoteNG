using System.Drawing;
using mRemoteNG.Credential;
using mRemoteNG.UI.Controls;

namespace mRemoteNG.UI.Forms.CredentialManagerPages.CredentialRepositorySelectors
{
    public class XmlCredentialRepositorySelector : ISelectionTarget<ICredentialRepository>
    {
        public string Text { get; set; } = "XML";
        public Image Image { get; } = Resources.xml;
        public IFactory<ICredentialRepository> Factory { get; }
    }
}