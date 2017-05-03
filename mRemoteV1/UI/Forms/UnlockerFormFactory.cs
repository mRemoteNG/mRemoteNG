using System.Collections.Generic;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;

namespace mRemoteNG.UI.Forms
{
    public class UnlockerFormFactory
    {
        public CompositeCredentialRepoUnlockerForm Build(IEnumerable<ICredentialRepository> repositories)
        {
            return new CompositeCredentialRepoUnlockerForm(
                new CompositeRepositoryUnlocker(repositories)
            );
        }
    }
}