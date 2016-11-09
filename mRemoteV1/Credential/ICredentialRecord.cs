using System;
using System.Security;


namespace mRemoteNG.Credential
{
    public interface ICredentialRecord
    {
        Guid Id { get; }

        string Username { get; set; }

        SecureString Password { get; set; }

        string Domain { get; set; }
    }
}