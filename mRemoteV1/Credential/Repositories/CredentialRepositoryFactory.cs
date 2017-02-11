using System;
using System.Xml.Linq;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;

namespace mRemoteNG.Credential.Repositories
{
    public class CredentialRepositoryFactory
    {
        public static ICredentialRepository Build(XElement repositoryXElement)
        {
            if (repositoryXElement.Name.LocalName == "")
                return BuildXmlRepository(repositoryXElement);
            throw new Exception("Could not build repository for the specified type");
        }

        private static ICredentialRepository BuildXmlRepository(XElement repositoryXElement)
        {
            var config = new CredentialRepositoryConfig(repositoryXElement.Name.LocalName)
            {
                Source = repositoryXElement.Attribute("Source")?.Value
            };
            var dataProvider = new FileDataProvider("");
            return new XmlCredentialRepository(config, dataProvider, new XmlCredentialDeserializer());
        }
    }
}