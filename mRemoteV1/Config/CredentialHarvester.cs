using mRemoteNG.Connection;
using mRemoteNG.Credential;
using mRemoteNG.Security;
using mRemoteNG.Security.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Xml.Linq;

namespace mRemoteNG.Config
{
    public class CredentialHarvester
    {
        private readonly IEqualityComparer<ICredentialRecord> _credentialComparer = new CredentialDomainUserPasswordComparer();

        /// <summary>
        /// Maps a <see cref="ConnectionInfo"/> (by its id) to the <see cref="ICredentialRecord"/>
        /// object that was harvested
        /// </summary>
        /// <param name="xDocument"></param>
        /// <param name="decryptionKey"></param>
        /// <returns></returns>
        public ConnectionToCredentialMap Harvest(XDocument xDocument, SecureString decryptionKey)
        {
            if (xDocument == null)
                throw new ArgumentNullException(nameof(xDocument));
            
            var cryptoProvider = new CryptoProviderFactoryFromXml(xDocument.Root).Build();

            var credentialMap = new ConnectionToCredentialMap();
            foreach (var element in xDocument.Descendants("Node"))
            {
                if (!EntryHasSomeCredentialData(element))
                    continue;

                var newCredential = BuildCredential(element, cryptoProvider, decryptionKey);

                Guid.TryParse(element.Attribute("Id")?.Value, out var connectionId);
                if (connectionId == Guid.Empty)
                {
                    //error
                }

                var existingCredential = credentialMap.Values.FirstOrDefault(record => _credentialComparer.Equals(newCredential, record));
                credentialMap.Add(connectionId, existingCredential ?? newCredential);
            }

            return credentialMap;
        }

        private ICredentialRecord BuildCredential(XElement element, ICryptographyProvider cryptographyProvider, SecureString decryptionKey)
        {
            var user = element.Attribute("Username")?.Value;
            var domain = element.Attribute("Domain")?.Value;

            var credential = new CredentialRecord
            {
                Title = $"{domain}{(string.IsNullOrEmpty(domain) ? "" : "\\")}{user}",
                Username = user,
                Domain = domain,
                Password = cryptographyProvider.Decrypt(element.Attribute("Password")?.Value, decryptionKey).ConvertToSecureString()
            };
            return credential;
        }

        private static bool EntryHasSomeCredentialData(XElement e)
        {
            return e.Attribute("Username")?.Value != "" || 
                   e.Attribute("Domain")?.Value != "" ||
                   e.Attribute("Password")?.Value != "";
        }
    }
}