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
        public IDictionary<Guid, ICredentialRecord> ConnectionToCredentialMap { get; } =
            new Dictionary<Guid, ICredentialRecord>();

        public IEnumerable<ICredentialRecord> Harvest(XDocument xDocument, SecureString decryptionKey)
        {
            ArgumentNullException.ThrowIfNull(xDocument);

            XElement root = xDocument.Root ?? throw new InvalidOperationException("XML document has no root element.");
            ICryptographyProvider cryptoProvider = new CryptoProviderFactoryFromXml(root).Build();

            foreach (XElement element in xDocument.Descendants("Node"))
            {
                if (!EntryHasSomeCredentialData(element)) continue;
                ICredentialRecord newCredential = BuildCredential(element, cryptoProvider, decryptionKey);

                Guid connectionId;
                _ = Guid.TryParse(element.Attribute("Id")?.Value, out connectionId);
                if (connectionId == Guid.Empty)
                {
                    //error
                }

                if (ConnectionToCredentialMap.Values.Contains(newCredential, _credentialComparer))
                {
                    ICredentialRecord existingCredential = ConnectionToCredentialMap.Values.First(record => _credentialComparer.Equals(newCredential, record));
                    ConnectionToCredentialMap.Add(connectionId, existingCredential);
                }
                else
                    ConnectionToCredentialMap.Add(connectionId, newCredential);
            }

            return ConnectionToCredentialMap.Values.Distinct(_credentialComparer);
        }

        private static ICredentialRecord BuildCredential(XElement element, ICryptographyProvider cryptographyProvider, SecureString decryptionKey)
        {
            string username = element.Attribute("Username")?.Value ?? string.Empty;
            string domain = element.Attribute("Domain")?.Value ?? string.Empty;
            string password = element.Attribute("Password")?.Value ?? string.Empty;
            CredentialRecord credential = new()
            {
                Title = $"{username}\\{domain}",
                Username = username,
                Domain = domain,
                Password = cryptographyProvider.Decrypt(password, decryptionKey).ConvertToSecureString()
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