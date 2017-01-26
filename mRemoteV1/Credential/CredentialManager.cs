using System;
using System.Collections.Generic;


namespace mRemoteNG.Credential
{
    public class CredentialManager
    {
        private readonly IList<ICredentialRecord> _credentialRecords;

        public CredentialManager()
        {
            _credentialRecords = new List<ICredentialRecord>();
        }

        public void Add(ICredentialRecord credentialRecord)
        {
            _credentialRecords.Add(credentialRecord);
            RaiseCredentialsChangedEvent(this);
        }

        public void AddRange(IEnumerable<ICredentialRecord> credentialRecords)
        {
            foreach (var credential in credentialRecords)
                _credentialRecords.Add(credential);
            RaiseCredentialsChangedEvent(this);
        }

        public void Remove(ICredentialRecord credentialRecord)
        {
            _credentialRecords.Remove(credentialRecord);
            RaiseCredentialsChangedEvent(this);
        }

        public IEnumerable<ICredentialRecord> GetCredentialRecords()
        {
            return _credentialRecords;
        }

        public event EventHandler CredentialsChanged;
        private void RaiseCredentialsChangedEvent(object sender)
        {
            CredentialsChanged?.Invoke(sender, EventArgs.Empty);
        }
    }
}