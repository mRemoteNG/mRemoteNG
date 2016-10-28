using System;
using System.Security;


namespace mRemoteNG.Credential
{
    public class CredentialRecord : ICredential
    {
        public Guid UniqueId { get; } = Guid.NewGuid();

        public string Username { get; set; } = "";

        public SecureString Password { get; set; } = new SecureString();

        public string Domain { get; set; } = "";


        public CredentialRecord()
        {
        }

        public CredentialRecord(ICredential otherCredential)
        {
            Username = otherCredential.Username;
            Password = otherCredential.Password;
            Domain = otherCredential.Domain;
        }

        public CredentialRecord(Guid customGuid)
        {
            UniqueId = customGuid;
        }
    }
}