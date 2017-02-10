using System.Windows.Forms;
using mRemoteNG.Credential.Repositories;

namespace mRemoteNG.UI.Forms.CredentialManagerPages.CredentialRepositoryEditorPages
{
    public class CredentialRepositoryPageEditorFactory
    {
        public static Control BuildXmlCredentialRepositoryEditorPage<T>(T config, Control previousPage) where T : ICredentialRepositoryConfig
        {
            return new XmlCredentialRepositoryEditorPage(config, previousPage);
        }
    }
}