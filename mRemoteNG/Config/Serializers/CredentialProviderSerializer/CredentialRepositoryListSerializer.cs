using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using mRemoteNG.Credential;


namespace mRemoteNG.Config.Serializers.CredentialProviderSerializer
{
    public class CredentialRepositoryListSerializer
    {
        public string Serialize(IEnumerable<ICredentialRepository> credentialProviderCatalog)
        {
            var xmlDocument = new XDocument(new XDeclaration("1.0", "utf-8", null));
            var rootElement = new XElement("CredentialRepositories",
                from provider in credentialProviderCatalog
                select new XElement("CredentialRepository",
                    new XAttribute("Id", provider.Config.Id),
                    new XAttribute("TypeName", provider.Config.TypeName),
                    new XAttribute("Title", provider.Config.Title),
                    new XAttribute("Source", provider.Config.Source)
                )
            );
            xmlDocument.Add(rootElement);
            var declaration = xmlDocument.Declaration.ToString();
            var documentBody = xmlDocument.ToString();
            return string.Concat(declaration, Environment.NewLine, documentBody);
        }
    }
}