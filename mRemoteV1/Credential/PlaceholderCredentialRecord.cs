using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;

namespace mRemoteNG.Credential
{
    public class PlaceholderCredentialRecord : ICredentialRecord
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Guid Id { get; }

        [ReadOnly(true)]
        public string Title { get; set; } = Language.CredentialUnavailable;

        [ReadOnly(true)]
        public string Username { get; set; } = Language.CredentialUnavailable;

        [ReadOnly(true)]
        public SecureString Password { get; set; } = new SecureString();

        [ReadOnly(true)]
        public string Domain { get; set; } = Language.CredentialUnavailable;

        public PlaceholderCredentialRecord(IEnumerable<Guid> id)
        {
            Id = id.FirstOrDefault();
        }

        public override string ToString() => Language.CredentialUnavailable;
    }
}
