using System;
using System.ComponentModel;
using System.Security;

namespace mRemoteNG.Credential.Repositories
{
    public interface ICredentialRepositoryConfig : INotifyPropertyChanged
    {
        /// <summary>
        /// An Id which uniquely identifies this credential repository.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// A friendly name for this credential repository
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// A name which uniquely identifies the type of credential repository this
        /// config represents. This is used for determining which <see cref="ICredentialRepositoryFactory"/>
        /// to use to create the <see cref="ICredentialRepository"/>.
        /// </summary>
        string TypeName { get; }

        /// <summary>
        /// A string representing how to access the persisted credential records.
        /// This may be a file path, URL, database connection string, etc. depending
        /// on the implementation of the <see cref="ICredentialRepository"/>.
        /// </summary>
        string Source { get; set; }

        /// <summary>
        /// The password necessary to unlock and access the underlying repository.
        /// </summary>
        SecureString Key { get; set; }
    }
}