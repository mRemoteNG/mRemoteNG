using System;

namespace mRemoteNG.Credential.Repositories
{
    public class CredentialRepositoryChangedArgs : EventArgs
    {
        public ICredentialRepository Repository { get; }

        public CredentialRepositoryChangedArgs(ICredentialRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            Repository = repository;
        }
    }
}