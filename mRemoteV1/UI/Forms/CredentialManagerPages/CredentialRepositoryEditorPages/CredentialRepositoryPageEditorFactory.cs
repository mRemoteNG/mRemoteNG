using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.UI.Controls.PageSequence;

namespace mRemoteNG.UI.Forms.CredentialManagerPages.CredentialRepositoryEditorPages
{
    public class CredentialRepositoryPageEditorFactory
    {
        public static SequencedControl BuildXmlCredentialRepositoryEditorPage<T>(T config, ICredentialRepositoryList repositoryList) where T : ICredentialRepositoryConfig
        {
            return new XmlCredentialRepositoryEditorPage(config, repositoryList);
        }
    }
}