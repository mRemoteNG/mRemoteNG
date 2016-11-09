using System;
using System.Security;


namespace mRemoteNG.Credential
{
    public class CredentialRecord : ICredentialRecord
    {
        public Guid Id { get; } = Guid.NewGuid();

        public string Username { get; set; } = "";

        public SecureString Password { get; set; } = new SecureString();

        public string Domain { get; set; } = "";


        public CredentialRecord()
        {
        }

        public CredentialRecord(ICredentialRecord otherCredential)
        {
            Username = otherCredential.Username;
            Password = otherCredential.Password;
            Domain = otherCredential.Domain;
        }

        public CredentialRecord(Guid customGuid)
        {
            Id = customGuid;
        }
    }
}