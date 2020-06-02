using System;

namespace mRemoteNG.Credential
{
    public class CredentialChangedEventArgs : EventArgs
    {
        public ICredentialRecord CredentialRecord { get; }
        public ICredentialRepository Repository { get; }

        public CredentialChangedEventArgs(ICredentialRecord credentialRecord, ICredentialRepository repository)
        {
            if (credentialRecord == null)
                throw new ArgumentNullException(nameof(credentialRecord));
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            CredentialRecord = credentialRecord;
            Repository = repository;
        }
    }
}