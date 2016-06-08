using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mRemoteNG.Credential
{
    public class CredentialManager
    {
        private List<CredentialList> _credentailDataSources;


        public CredentialManager()
        {
            _credentailDataSources = new List<CredentialList>();
        }

        public void AddDataSource(CredentialList Source)
        {
            _credentailDataSources.Add(Source);
        }

        public void RemoveDataSource(CredentialList Source)
        {
            // find source in list
            // remove source
        }
    }
}