using System;

namespace mRemoteNG.Credential.Repositories
{
    public class CredentialRepositoryTypeNotSupportedException : Exception
    {
        public CredentialRepositoryTypeNotSupportedException()
        {
        }

        public CredentialRepositoryTypeNotSupportedException(string message)
            : base(message)
        {
        }

        public CredentialRepositoryTypeNotSupportedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
