using System.Collections.Generic;
using System.Windows.Forms;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.UI.Controls;

namespace mRemoteNG.UI.Forms.CredentialManagerPages.CredentialRepositoryEditorPages
{
    public class CredentialRepositoryPageEditorFactory
    {
        public static Control BuildXmlCredentialRepositoryEditorPage<T>(T config, ICredentialRepositoryList repositoryList, PageSequence pageSequence) where T : ICredentialRepositoryConfig
        {
            return new XmlCredentialRepositoryEditorPage(config, repositoryList, pageSequence);
        }
    }
}