using System;
using System.Xml.Linq;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Security;

namespace mRemoteNG.Credential.Repositories
{
    public class CredentialRepositoryFactory
    {
        public static ICredentialRepository Build(XElement repositoryXElement)
        {
            var typeName = repositoryXElement.Attribute("TypeName")?.Value;
            if (typeName == "Xml")
                return BuildXmlRepository(repositoryXElement);
            throw new Exception("Could not build repository for the specified type");
        }

        private static ICredentialRepository BuildXmlRepository(XElement repositoryXElement)
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
            var cryptoProvider = CryptographyProviderFactory.BuildCryptographyProviderFromSettings();
            return new XmlCredentialRepository(config, dataProvider, cryptoProvider);
        }
    }
}