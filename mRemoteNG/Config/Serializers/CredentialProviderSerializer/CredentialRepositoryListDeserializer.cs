using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;

namespace mRemoteNG.Config.Serializers.CredentialProviderSerializer
{
    public class CredentialRepositoryListDeserializer
    {
        public IEnumerable<ICredentialRepository> Deserialize(string xml, IEnumerable<ICredentialRepositoryFactory> factories)
        {
            if (string.IsNullOrEmpty(xml))
                return new ICredentialRepository[0];

            var xdoc = XDocument.Parse(xml);
            var repoEntries = xdoc.Descendants("CredentialRepository");

            return repoEntries
                .Select(ParseConfigEntries)
                .Select(config => 
                    factories
                        .FirstOrDefault(f => string.Equals(f.SupportsConfigType, config.TypeName))?
                        .Build(config));
        }

        public ICredentialRepositoryConfig ParseConfigEntries(XElement repositoryXElement)
        {
            var stringId = repositoryXElement.Attribute("Id")?.Value;
            Guid.TryParse(stringId, out var id);

            if (id.Equals(Guid.Empty))
                id = Guid.NewGuid();

            var config = new CredentialRepositoryConfig(id)
            {
                TypeName = repositoryXElement.Attribute("TypeName")?.Value,
                Title = repositoryXElement.Attribute("Title")?.Value,
                Source = repositoryXElement.Attribute("Source")?.Value
            };

            return config;
        }
    }
}