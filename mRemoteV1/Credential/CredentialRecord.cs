using System.Security;


namespace mRemoteNG.Credential
{
    public class CredentialRecord : ICredential
    {
        public string Username { get; set; } = "";

        public SecureString Password { get; set; } = new SecureString();

        public string Domain { get; set; } = "";
    }
}