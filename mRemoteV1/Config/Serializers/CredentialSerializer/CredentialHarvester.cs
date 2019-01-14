using mRemoteNG.Connection;
using mRemoteNG.Credential;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mRemoteNG.Config.Serializers.CredentialSerializer
{
    public class CredentialHarvester
    {
        private readonly IEqualityComparer<ICredentialRecord> _credentialComparer = new CredentialDomainUserPasswordComparer();

        /// <summary>
        /// Maps a <see cref="ConnectionInfo"/> (by its id) to the <see cref="ICredentialRecord"/>
        /// object that was harvested
        /// </summary>
        /// <param name="harvestConfig"></param>
        public ConnectionToCredentialMap Harvest<T>(HarvestConfig<T> harvestConfig)
        {
            if (harvestConfig == null)
                throw new ArgumentNullException(nameof(harvestConfig));
            
            var credentialMap = new ConnectionToCredentialMap();
            foreach (var element in harvestConfig.ItemEnumerator())
            {
                var newCredential = new CredentialRecord
                {
                    Title = harvestConfig.TitleSelector(element),
                    Username = harvestConfig.UsernameSelector(element),
                    Domain = harvestConfig.DomainSelector(element),
                    Password = harvestConfig.PasswordSelector(element)
                };

                if (!EntryHasSomeCredentialData(newCredential))
                    continue;

                var connectionId = harvestConfig.ConnectionGuidSelector(element);

                var existingCredential = credentialMap.Values.FirstOrDefault(record => _credentialComparer.Equals(newCredential, record));
                credentialMap.Add(connectionId, existingCredential ?? newCredential);
            }

            return credentialMap;
        }

        private bool EntryHasSomeCredentialData(ICredentialRecord e)
        {
            return !_credentialComparer.Equals(e, new NullCredentialRecord());
        }
    }
}