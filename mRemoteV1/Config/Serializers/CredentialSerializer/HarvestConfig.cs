using System;
using System.Collections.Generic;
using System.Security;

namespace mRemoteNG.Config.Serializers.CredentialSerializer
{
    /// <summary>
    /// Configuration for the <see cref="CredentialHarvester"/> to allow it to
    /// iterate over and select values from any arbitrary data type. Each element
    /// of type <see cref="T"/> represents an object that contains intermixed
    /// connection and credential data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HarvestConfig<T>
    {
        /// <summary>
        /// This will be called to produce a list of all objects
        /// that should be iterated over.
        /// </summary>
        public Func<IEnumerable<T>> ItemEnumerator { get; set; }

        /// <summary>
        /// Given an item of type <see cref="T"/>, return
        /// the <see cref="Guid"/> that represents the connection's unique ID
        /// within mRemoteNG.
        /// </summary>
        public Func<T, Guid> ConnectionGuidSelector { get; set; }

        /// <summary>
        /// Given an item of type <see cref="T"/>, return a <see cref="string"/>
        /// that represents what the associated credential's title should be.
        /// </summary>
        public Func<T, string> TitleSelector { get; set; }

        /// <summary>
        /// Given an item of type <see cref="T"/>, return a <see cref="string"/>
        /// that represents what the associated credential's username should be.
        /// </summary>
        public Func<T, string> UsernameSelector { get; set; }

        /// <summary>
        /// Given an item of type <see cref="T"/>, return a <see cref="string"/>
        /// that represents what the associated credential's domain should be.
        /// </summary>
        public Func<T, string> DomainSelector { get; set; }

        /// <summary>
        /// Given an item of type <see cref="T"/>, return a <see cref="SecureString"/>
        /// that represents what the associated credential's password should be.
        /// </summary>
        public Func<T, SecureString> PasswordSelector { get; set; }
    }
}
