using System.Collections.Generic;

namespace mRemoteNG.Credential
{
    public class CredentialRepoUnlockerBuilder
    {
        public CompositeRepositoryUnlocker Build(IEnumerable<ICredentialRepository> repos)
        {
            return new CompositeRepositoryUnlocker(repos);
        }
    }
}