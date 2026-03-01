using System;

namespace mRemoteNG.Credential.Repositories
{
    public class CredentialRepositoryChangedArgs : EventArgs
    {
        public ICredentialRepository Repository { get; }

        public CredentialRepositoryChangedArgs(ICredentialRepository repository)
        {
            ArgumentNullException.ThrowIfNull(repository);
            Repository = repository;
        }
    }
}