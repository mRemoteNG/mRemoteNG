using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace mRemoteNG.Credential
{
    public class CredentialManager
    {
        private readonly IList<INotifyingCredentialRecord> _credentialRecords;

        public CredentialManager()
        {
            _credentialRecords = new List<INotifyingCredentialRecord>();
        }

        public void Add(INotifyingCredentialRecord credentialRecord)
        {
            _credentialRecords.Add(credentialRecord);
            credentialRecord.PropertyChanged += CredentialOnPropertyChanged;
            RaiseCredentialsChangedEvent(this);
        }

        public void AddRange(IEnumerable<INotifyingCredentialRecord> credentialRecords)
        {
            foreach (var credential in credentialRecords)
            {
                _credentialRecords.Add(credential);
                credential.PropertyChanged += CredentialOnPropertyChanged;
            }
            RaiseCredentialsChangedEvent(this);
        }

        public void Remove(INotifyingCredentialRecord credentialRecord)
        {
            _credentialRecords.Remove(credentialRecord);
            credentialRecord.PropertyChanged -= CredentialOnPropertyChanged;
            RaiseCredentialsChangedEvent(this);
        }

        public IEnumerable<INotifyingCredentialRecord> GetCredentialRecords()
        {
            return _credentialRecords;
        }

        private void CredentialOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            RaiseCredentialsChangedEvent(this);
        }

        public event EventHandler CredentialsChanged;
        private void RaiseCredentialsChangedEvent(object sender)
        {
            CredentialsChanged?.Invoke(sender, EventArgs.Empty);
        }
    }
}