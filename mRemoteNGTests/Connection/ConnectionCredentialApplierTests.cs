using System;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Credential;
using mRemoteNG.Security;
using NUnit.Framework;


namespace mRemoteNGTests.Connection
{
    public class ConnectionCredentialApplierTests
    {
        private ConnectionInfo _connectionInfo;
        private CredentialRecord _credentialRecord;

        [SetUp]
        public void Setup()
        {
            _connectionInfo = new ConnectionInfo
            {
                Name = "server",
                Hostname = "server.example.com",
                Username = "originalUser",
                Domain = "originalDomain",
                Password = "originalPassword"
            };

            _credentialRecord = new CredentialRecord
            {
                Title = "Admin",
                Username = "adminUser",
                Domain = "adminDomain",
                Password = "adminPassword".ConvertToSecureString()
            };
        }

        [Test]
        public void ApplyCredentialSetsUsernameFromCredential()
        {
            var result = ConnectionCredentialApplier.ApplyCredential(_connectionInfo, _credentialRecord);
            Assert.That(result.Username, Is.EqualTo("adminUser"));
        }

        [Test]
        public void ApplyCredentialSetsDomainFromCredential()
        {
            var result = ConnectionCredentialApplier.ApplyCredential(_connectionInfo, _credentialRecord);
            Assert.That(result.Domain, Is.EqualTo("adminDomain"));
        }

        [Test]
        public void ApplyCredentialSetsPasswordFromCredential()
        {
            var result = ConnectionCredentialApplier.ApplyCredential(_connectionInfo, _credentialRecord);
            Assert.That(result.Password, Is.EqualTo("adminPassword"));
        }

        [Test]
        public void ApplyCredentialDoesNotModifyOriginalConnection()
        {
            ConnectionCredentialApplier.ApplyCredential(_connectionInfo, _credentialRecord);
            Assert.Multiple(() =>
            {
                Assert.That(_connectionInfo.Username, Is.EqualTo("originalUser"));
                Assert.That(_connectionInfo.Domain, Is.EqualTo("originalDomain"));
                Assert.That(_connectionInfo.Password, Is.EqualTo("originalPassword"));
            });
        }

        [Test]
        public void ApplyCredentialPreservesOtherConnectionProperties()
        {
            var result = ConnectionCredentialApplier.ApplyCredential(_connectionInfo, _credentialRecord);
            Assert.Multiple(() =>
            {
                Assert.That(result.Name, Is.EqualTo("server"));
                Assert.That(result.Hostname, Is.EqualTo("server.example.com"));
            });
        }

        [Test]
        public void ApplyCredentialKeepsParentReference()
        {
            var parent = new ContainerInfo();
            _connectionInfo.SetParent(parent);
            var result = ConnectionCredentialApplier.ApplyCredential(_connectionInfo, _credentialRecord);
            Assert.That(result.Parent, Is.EqualTo(parent));
        }

        [Test]
        public void ApplyCredentialOverridesInheritedCredentialsAndExternalProvider()
        {
            var parent = new ContainerInfo
            {
                Username = "parentUser",
                Domain = "parentDomain",
                Password = "parentPassword",
                ExternalCredentialProvider = ExternalCredentialProvider.DelineaSecretServer,
                UserViaAPI = "parent-secret"
            };
            _connectionInfo.SetParent(parent);
            _connectionInfo.Inheritance.Username = true;
            _connectionInfo.Inheritance.Domain = true;
            _connectionInfo.Inheritance.Password = true;
            _connectionInfo.Inheritance.ExternalCredentialProvider = true;
            _connectionInfo.Inheritance.UserViaAPI = true;

            var result = ConnectionCredentialApplier.ApplyCredential(_connectionInfo, _credentialRecord);

            Assert.Multiple(() =>
            {
                Assert.That(result.Username, Is.EqualTo("adminUser"));
                Assert.That(result.Domain, Is.EqualTo("adminDomain"));
                Assert.That(result.Password, Is.EqualTo("adminPassword"));
                Assert.That(result.ExternalCredentialProvider, Is.EqualTo(ExternalCredentialProvider.None));
                Assert.That(result.UserViaAPI, Is.Empty);
                Assert.That(result.Inheritance.Username, Is.False);
                Assert.That(result.Inheritance.Domain, Is.False);
                Assert.That(result.Inheritance.Password, Is.False);
                Assert.That(result.Inheritance.ExternalCredentialProvider, Is.False);
                Assert.That(result.Inheritance.UserViaAPI, Is.False);
            });
        }

        [Test]
        public void ApplyCredentialThrowsWhenConnectionIsNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => ConnectionCredentialApplier.ApplyCredential(null, _credentialRecord));
        }

        [Test]
        public void ApplyCredentialThrowsWhenCredentialIsNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => ConnectionCredentialApplier.ApplyCredential(_connectionInfo, null));
        }
    }
}
