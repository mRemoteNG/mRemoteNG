using System.Security;


namespace mRemoteNG.Credential
{
    public interface ICredential
    {
        string Username { get; set; }
        SecureString Password { get; set; }
        string Domain { get; set; }
    }
}