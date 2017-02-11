using System.Drawing;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.UI.Controls;

namespace mRemoteNG.UI.Forms.CredentialManagerPages.CredentialRepositorySelectors
{
    public class XmlCredentialRepositorySelector : ISelectionTarget<ICredentialRepositoryConfig>
    {
        public string Text { get; set; } = "XML";
        public Image Image { get; } = Resources.xml;
        public ICredentialRepositoryConfig Config { get; } = new CredentialRepositoryConfig {TypeName = "Xml"};
    }
}