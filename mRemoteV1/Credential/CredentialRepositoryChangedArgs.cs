using System;

namespace mRemoteNG.Credential
{
    public class CredentialRepositoryChangedArgs
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