using System.Collections.Generic;
using System.Windows.Forms;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;

namespace mRemoteNG.UI.Forms.CredentialManagerPages.CredentialRepositoryEditorPages
{
    public class CredentialRepositoryPageEditorFactory
    {
        public static Control BuildXmlCredentialRepositoryEditorPage<T>(T config, ICredentialRepositoryList repositoryList, Control previousPage) where T : ICredentialRepositoryConfig
        {
            return new XmlCredentialRepositoryEditorPage(config, repositoryList, previousPage);
        }
    }
}