using System;
using System.ComponentModel;
using System.Security;

namespace mRemoteNG.Credential.Repositories
{
    public class CredentialRepositoryConfig : ICredentialRepositoryConfig
    {
        private string _title = "New Credential Repository";
        private string _source = "";
        private SecureString _key = new SecureString();
        private string _typeName = "";
        private bool _loaded;

        public Guid Id { get; }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChangedEvent(nameof(Title));
            }
        }

        public string TypeName
        {
            get { return _typeName; }
            set
            {
                _typeName = value;
                RaisePropertyChangedEvent(nameof(TypeName));
            }
        }

        public string Source
        {
            get { return _source; }
            set
            {
                _source = value; 
                RaisePropertyChangedEvent(nameof(Source));
            }
        }

        public SecureString Key
        {
            get { return _key; }
            set
            {
                _key = value; 
                RaisePropertyChangedEvent(nameof(Key));
            }
        }

        public bool Loaded
        {
            get { return _loaded; }
            set
            {
                _loaded = value;
                RaisePropertyChangedEvent(nameof(Loaded));
            }
        }

        public CredentialRepositoryConfig() : this(Guid.NewGuid())
        {
        }

        public CredentialRepositoryConfig(Guid id)
        {
            Id = id;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChangedEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}