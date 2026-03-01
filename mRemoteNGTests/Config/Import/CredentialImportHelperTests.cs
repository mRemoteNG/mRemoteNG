using System;
using System.Collections.Generic;
using System.Security;
using mRemoteNG.Config.Import;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Credential;
using mRemoteNG.Security;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Import
{
    [TestFixture]
    public class CredentialImportHelperTests
    {
        [Test]
        public void ExtractCredentials_ExtractsAndClearsCredentials()
        {
            // Arrange
            var connection = new ConnectionInfo
            {
                Name = "TestConnection",
                Username = "user",
                Password = "password",
                Domain = "domain"
            };

            var repository = Substitute.For<ICredentialRepository>();
            var records = new List<ICredentialRecord>();
            repository.CredentialRecords.Returns(records);

            // Act
            CredentialImportHelper.ExtractCredentials(connection, repository);

            // Assert
            Assert.That(records.Count, Is.EqualTo(1));
            var record = records[0];
            Assert.That(record.Username, Is.EqualTo("user"));
            Assert.That(record.Domain, Is.EqualTo("domain"));
            
            Assert.That(connection.CredentialId, Is.EqualTo(record.Id.ToString()));
            Assert.That(connection.Username, Is.Empty);
            Assert.That(connection.Password, Is.Empty);
            Assert.That(connection.Domain, Is.Empty);
        }

        [Test]
        public void ExtractCredentials_RecursesIntoContainers()
        {
            // Arrange
            var container = new ContainerInfo();
            var child = new ConnectionInfo
            {
                Name = "Child",
                Username = "childUser",
                Password = "childPassword"
            };
            container.AddChild(child);

            var repository = Substitute.For<ICredentialRepository>();
            var records = new List<ICredentialRecord>();
            repository.CredentialRecords.Returns(records);

            // Act
            CredentialImportHelper.ExtractCredentials(container, repository);

            // Assert
            Assert.That(records.Count, Is.EqualTo(1));
            var record = records[0];
            Assert.That(record.Username, Is.EqualTo("childUser"));
            
            Assert.That(child.CredentialId, Is.EqualTo(record.Id.ToString()));
            Assert.That(child.Username, Is.Empty);
        }
    }
}
