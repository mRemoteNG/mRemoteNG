using System;
using System.Security;


namespace mRemoteNG.Credential
{
    public interface ICredentialRecord
    {
        Guid Id { get; }

        string Title { get; set; }

        string Username { get; set; }

        SecureString Password { get; set; }

        string Domain { get; set; }
    }
}