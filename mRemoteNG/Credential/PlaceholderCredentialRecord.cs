using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Versioning;
using System.Security;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.Credential
{
    [SupportedOSPlatform("windows")]
    public class PlaceholderCredentialRecord(IEnumerable<Guid> id) : ICredentialRecord
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Guid Id { get; } = id.FirstOrDefault();

        [ReadOnly(true)] public string Title { get; set; } = Language.CredentialUnavailable;

        [ReadOnly(true)] public string Username { get; set; } = Language.CredentialUnavailable;

        [ReadOnly(true)] public SecureString Password { get; set; } = new SecureString();

        [ReadOnly(true)] public string Domain { get; set; } = Language.CredentialUnavailable;

        public override string ToString() => Language.CredentialUnavailable;
    }
}