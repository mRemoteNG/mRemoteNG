using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mRemoteNG.Credential;

namespace mRemoteNG.Config.Credentials
{
    public class CredentialSaver
    {
        private CredentialList _credentialList;

        public CredentialSaver(CredentialList credentialList)
        {
            _credentialList = credentialList;
        }

        public void Save()
        {
            
        }
    }
}