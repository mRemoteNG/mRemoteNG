using mRemoteNG.Credential;

namespace mRemoteNG.UI.Forms
{
    public class CredAndRepoModel
    {
        public ICredentialRecord CredentialRecord { get; set; }

        public ICredentialRepository AssignedRepository { get; set; }

        public CredAndRepoModel(ICredentialRecord credentialRecord)
        {
            CredentialRecord = credentialRecord;
        }
    }
}
