using System.Collections.Generic;
using System.Security;
using mRemoteNG.Config;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Credential;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.Config;

public class CredentialRecordLoaderTests
{
    private CredentialRecordLoader _credentialRecordLoader;
    private IDataProvider<string> _dataProvider;
    private ISecureDeserializer<string, IEnumerable<ICredentialRecord>> _deserializer;

    [SetUp]
    public void Setup()
    {
        _dataProvider = Substitute.For<IDataProvider<string>>();
        _deserializer = Substitute.For<ISecureDeserializer<string, IEnumerable<ICredentialRecord>>>();
        _credentialRecordLoader = new CredentialRecordLoader(_dataProvider, _deserializer);
    }

    [Test]
    public void LoadsFromDataProvider()
    {
        _credentialRecordLoader.Load(new SecureString());
        _dataProvider.Received(1).Load();
    }

    [Test]
    public void DeserializesDataFromDataProvider()
    {
        var key = new SecureString();
        _dataProvider.Load().Returns("mydata");
        _credentialRecordLoader.Load(key);
        _deserializer.Received(1).Deserialize("mydata", key);
    }
}