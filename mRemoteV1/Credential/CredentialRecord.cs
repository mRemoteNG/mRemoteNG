using System;
using System.Collections.Generic;
using System.Text;
using System.Security;

namespace mRemoteNG.Credential
{
    public interface CredentialRecord
    {
        string Username { get; set; }
        //SecureString Password { get; set; }
        string Password { get; set; }
        string Domain { get; set; }
    }
}