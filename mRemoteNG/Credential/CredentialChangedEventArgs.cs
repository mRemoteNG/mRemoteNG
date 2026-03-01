using System;

namespace mRemoteNG.Credential
{
    public class CredentialChangedEventArgs : EventArgs
    {
        public ICredentialRecord CredentialRecord { get; }
        public ICredentialRepository Repository { get; }

        public CredentialChangedEventArgs(ICredentialRecord credentialRecord, ICredentialRepository repository)
        {
            ArgumentNullException.ThrowIfNull(credentialRecord);
            ArgumentNullException.ThrowIfNull(repository);
            CredentialRecord = credentialRecord;
            Repository = repository;
        }
    }
}