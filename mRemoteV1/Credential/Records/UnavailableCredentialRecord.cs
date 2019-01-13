using System;
using System.ComponentModel;
using System.Security;

namespace mRemoteNG.Credential
{
    public class UnavailableCredentialRecord : ICredentialRecord
    {
        public Guid Id { get; }

        [ReadOnly(true)]
        public string Title { get; set; } = Language.CredentialUnavailable;

        [ReadOnly(true)]
        public string Username { get; set; } = string.Empty;

        [ReadOnly(true)]
        public SecureString Password { get; set; } = new SecureString();

        [ReadOnly(true)]
        public string Domain { get; set; } = string.Empty;

        public UnavailableCredentialRecord(Guid id)
        {
            Id = id;
        }

        public override string ToString() => Language.CredentialUnavailable;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
