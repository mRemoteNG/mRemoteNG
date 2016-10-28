using System;
using System.Security;


namespace mRemoteNG.Credential
{
    public class CredentialRecord : ICredential
    {
        public Guid UniqueId { get; }

        public string Username { get; set; } = "";

        public SecureString Password { get; set; } = new SecureString();

        public string Domain { get; set; } = "";


        public CredentialRecord()
        {
            UniqueId = Guid.NewGuid();
        }

        public CredentialRecord(Guid customGuid)
        {
            UniqueId = customGuid;
        }
    }
}