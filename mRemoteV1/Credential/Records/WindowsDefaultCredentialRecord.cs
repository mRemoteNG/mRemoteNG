using System;
using System.ComponentModel;
using System.Security;

namespace mRemoteNG.Credential
{
    public class WindowsDefaultCredentialRecord : ICredentialRecord
    {
        private readonly string _title;
        public Guid Id { get; } = Guid.NewGuid();

        public string Title
        {
            get => _title;
            set {}
        }

        public string Username
        {
            get => Environment.UserName;
            set {}
        }

        public SecureString Password
        {
            get => new SecureString();
            set {}
        }

        public string Domain
        {
            get => Environment.UserDomainName;
            set {}
        }


        public WindowsDefaultCredentialRecord()
        {
            // TODO: localizations
            _title = "Windows Default Credentials";
        }

        public override string ToString()
        {
            return Title;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChangedEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
