using System;
using System.Linq;
using System.Xml.Linq;
using mRemoteNG.Config.Serializers.CredentialProviderSerializer;
using mRemoteNG.Credential;
using NSubstitute;
using NUnit.Framework;


namespace mRemoteNGTests.Config.Serializers;

public class CredentialProviderSerializerTests
{
    private CredentialRepositoryListSerializer _credentialProviderSerializer;

    [SetUp]
    public void Setup()
    {
        _credentialProviderSerializer = new CredentialRepositoryListSerializer();
    }

    private ICredentialRepository InitializeMockProvider()
    {
        var provider = Substitute.For<ICredentialRepository>();
        provider.Config.TypeName.Returns("ProviderName");
        provider.Config.Id.Returns(Guid.NewGuid());
        return provider;
    }

    [Test]
    public void SerializeExists()
    {
        var mockProvider = InitializeMockProvider();
        var providers = new[] { mockProvider };
        var serializedContent = _credentialProviderSerializer.Serialize(providers);
        Assert.That(serializedContent, Is.Not.Null);
    }

    [Test]
    public void RepositoryIsWrittenWithItsConfiguration()
    {
        var provider = InitializeMockProvider();
        provider.Config.Title.Returns("Title");
        provider.Config.Source.Returns("Source");

        var repositories = XDocument.Parse(_credentialProviderSerializer.Serialize(new[] { provider }))
            .Root.Elements("CredentialRepository").ToList();

        Assert.That(repositories, Has.Count.EqualTo(1));
        Assert.That(repositories[0].Attribute("Id")?.Value, Is.EqualTo(provider.Config.Id.ToString()));
        Assert.That(repositories[0].Attribute("TypeName")?.Value, Is.EqualTo("ProviderName"));
        Assert.That(repositories[0].Attribute("Title")?.Value, Is.EqualTo("Title"));
        Assert.That(repositories[0].Attribute("Source")?.Value, Is.EqualTo("Source"));
    }
}