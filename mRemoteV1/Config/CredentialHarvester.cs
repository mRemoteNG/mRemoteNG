using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Xml.Linq;
using mRemoteNG.Credential;
using mRemoteNG.Security;

namespace mRemoteNG.Config
{
    public class CredentialHarvester
    {
        public IEnumerable<ICredentialRecord> Harvest(XDocument xDocument, SecureString decryptionKey)
        {
            if (xDocument == null)
                throw new ArgumentNullException(nameof(xDocument));

            var cryptoProvider = CryptographyProviderFactory.BuildFromXml(xDocument.Root);

            var credentialList = from e in xDocument.Descendants("Node")
                                 where EntryHasSomeCredentialData(e)
                                 select new CredentialRecord
                                {
                                    Username = e.Attribute("Username")?.Value,
                                    Domain =  e.Attribute("Domain")?.Value,
                                    Password = cryptoProvider.Decrypt(e.Attribute("Password")?.Value, decryptionKey).ConvertToSecureString()
                                };

            return credentialList.Distinct(new CredentialDomainUserComparer());
        }

        private static bool EntryHasSomeCredentialData(XElement e)
        {
            return e.Attribute("Username")?.Value != "" || 
                   e.Attribute("Domain")?.Value != "" ||
                   e.Attribute("Password")?.Value != "";
        }
    }
}