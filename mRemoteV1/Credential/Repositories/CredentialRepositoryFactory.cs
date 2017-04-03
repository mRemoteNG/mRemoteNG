using System;
using System.Xml.Linq;
using mRemoteNG.Config.DataProviders;

namespace mRemoteNG.Credential.Repositories
{
    public class CredentialRepositoryFactory
    {
        public ICredentialRepository Build(XElement repositoryXElement)
        {
            var typeName = repositoryXElement.Attribute("TypeName")?.Value;
            if (typeName == "Xml")
                return BuildXmlRepository(repositoryXElement);
            throw new Exception("Could not build repository for the specified type");
        }

        private ICredentialRepository BuildXmlRepository(XElement repositoryXElement)
        {
            var stringId = repositoryXElement.Attribute("Id")?.Value;
            Guid id;
            Guid.TryParse(stringId, out id);
            if (id.Equals(Guid.Empty)) id = Guid.NewGuid();
            var config = new CredentialRepositoryConfig(id)
            {
                TypeName = repositoryXElement.Attribute("TypeName")?.Value,
                Title = repositoryXElement.Attribute("Title")?.Value,
                Source = repositoryXElement.Attribute("Source")?.Value
            };
            var dataProvider = new FileDataProvider(config.Source);
            return new XmlCredentialRepository(config, dataProvider);
        }
    }
}