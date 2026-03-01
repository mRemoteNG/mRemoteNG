using System.Collections.Generic;

namespace mRemoteNG.Credential.Repositories
{
    public static class CredentialRepoUnlockerBuilder
    {
        public static CompositeRepositoryUnlocker Build(IEnumerable<ICredentialRepository> repos)
        {
            return new CompositeRepositoryUnlocker(repos);
        }
    }
}