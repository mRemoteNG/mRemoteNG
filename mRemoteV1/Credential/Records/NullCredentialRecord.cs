using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security;

namespace mRemoteNG.Credential
{
    public class NullCredentialRecord : ICredentialRecord
    {
        public Guid Id { get; } = Guid.Empty;

        [ReadOnly(true)]
        public string Title { get; set; } = $"--{Language.strNone}--";

        [ReadOnly(true)]
        public string Username { get; set; } = string.Empty;

        [ReadOnly(true)]
        public SecureString Password { get; set; } = new SecureString();

        [ReadOnly(true)]
        public string Domain { get; set; } = string.Empty;

        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            return Title;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
