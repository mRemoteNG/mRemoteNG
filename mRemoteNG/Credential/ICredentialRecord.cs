using System;
using System.ComponentModel;
using System.Security;

namespace mRemoteNG.Credential
{
    /// <summary>
    /// Represents a named set of username/domain/password information.
    /// </summary>
    [TypeConverter(typeof(CredentialRecordTypeConverter))]
    public interface ICredentialRecord : INotifyPropertyChanged
    {
        /// <summary>
        /// An Id which uniquely identifies this credential record.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// A friendly name for this credential record.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// The username portion of the credential.
        /// </summary>
        string Username { get; set; }

        /// <summary>
        /// The domain portion of the credential.
        /// </summary>
        string Domain { get; set; }

        /// <summary>
        /// The password
        /// </summary>
        SecureString Password { get; set; }
    }
}