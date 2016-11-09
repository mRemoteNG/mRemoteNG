using System;


namespace mRemoteNG.Credential
{
    public interface ICredentialProvider
    {
        Guid Id { get; }

        string Name { get; }

        ICredentialList LoadCredentials();
    }
}