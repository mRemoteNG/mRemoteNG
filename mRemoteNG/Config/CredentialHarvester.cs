using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Security;
using System.Xml.Linq;
using mRemoteNG.Credential;
using mRemoteNG.Security;
using mRemoteNG.Security.Factories;

namespace mRemoteNG.Config
{
    [SupportedOSPlatform("windows")]
    public class CredentialHarvester
    {
        private readonly IEqualityComparer<ICredentialRecord> _credentialComparer = new CredentialDomainUserComparer();

        // maps a connectioninfo (by its id) to the credential object that was harvested
        public Dictionary<Guid, ICredentialRecord> ConnectionToCredentialMap { get; } =
            new Dictionary<Guid, ICredentialRecord>();

        public IEnumerable<ICredentialRecord> Harvest(XDocument xDocument, SecureString decryptionKey)
        {
            if (xDocument == null)
                throw new ArgumentNullException(nameof(xDocument));

            var cryptoProvider = new CryptoProviderFactoryFromXml(xDocument.Root).Build();

            foreach (var element in xDocument.Descendants("Node"))
            {
                if (!EntryHasSomeCredentialData(element)) continue;
                var newCredential = BuildCredential(element, cryptoProvider, decryptionKey);

                Guid connectionId;
                Guid.TryParse(element.Attribute("Id")?.Value, out connectionId);
                if (connectionId == Guid.Empty)
                {
                    //error
                }

                if (ConnectionToCredentialMap.Values.Contains(newCredential, _credentialComparer))
                {
                    var existingCredential = ConnectionToCredentialMap.Values.First(record => _credentialComparer.Equals(newCredential, record));
                    ConnectionToCredentialMap.Add(connectionId, existingCredential);
                }
                else
                    ConnectionToCredentialMap.Add(connectionId, newCredential);
            }

            return ConnectionToCredentialMap.Values.Distinct(_credentialComparer);
        }

        private ICredentialRecord BuildCredential(XElement element, ICryptographyProvider cryptographyProvider, SecureString decryptionKey)
        {
            var credential = new CredentialRecord
            {
                Title = $"{element.Attribute("Username")?.Value}\\{element.Attribute("Domain")?.Value}",
                Username = element.Attribute("Username")?.Value,
                Domain = element.Attribute("Domain")?.Value,
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