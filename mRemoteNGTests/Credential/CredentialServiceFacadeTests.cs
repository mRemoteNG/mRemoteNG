using System;
using System.Collections.Generic;
using mRemoteNG.Config;
using mRemoteNG.Credential;
using NSubstitute;
using NUnit.Framework;
// ReSharper disable ObjectCreationAsStatement

namespace mRemoteNGTests.Credential
{
    public class CredentialServiceFacadeTests
    {
        private CredentialServiceFacade _credentialService;
        private ICredentialRepositoryList _credentialRepositoryList;
        private ILoader<IEnumerable<ICredentialRepository>> _loader;
        private ISaver<IEnumerable<ICredentialRepository>> _saver;

        [SetUp]
        public void Setup()
        {
            _credentialRepositoryList = Substitute.For<ICredentialRepositoryList>();
            _loader = Substitute.For<ILoader<IEnumerable<ICredentialRepository>>>();
            _saver = Substitute.For<ISaver<IEnumerable<ICredentialRepository>>>();
            _credentialService = new CredentialServiceFacade(_credentialRepositoryList, _loader, _saver);
        }

        [Test]
        public void CantProvideNullRepoListToCtor()
        {
            Assert.Throws<ArgumentNullException>(() => new CredentialServiceFacade(null, _loader, _saver));
        }

        [Test]
        public void CantProvideNullRepoLoaderToCtor()
        {
            Assert.Throws<ArgumentNullException>(() => new CredentialServiceFacade(_credentialRepositoryList, null, _saver));
        }

        [Test]
        public void CantProvideNullRepoSaverToCtor()
        {
            Assert.Throws<ArgumentNullException>(() => new CredentialServiceFacade(_credentialRepositoryList, _loader, null));
        }

        [Test]
        public void AddRepoCallsUnderlyingList()
        {
            var newRepo = Substitute.For<ICredentialRepository>();
            _credentialService.AddRepository(newRepo);
            _credentialRepositoryList.Received().AddProvider(newRepo);
        }

        [Test]
        public void RemoveRepoCallsUnderlyingList()
        {
            var repo = Substitute.For<ICredentialRepository>();
            _credentialService.RemoveRepository(repo);
            _credentialRepositoryList.Received().RemoveProvider(repo);
        }

        [Test]
        public void GetCredentialRecordsCallsUnderlyingList()
        {
            _credentialService.GetCredentialRecords();
            _credentialRepositoryList.Received().GetCredentialRecords();
        }

        [Test]
        public void GetCredentialRecordCallsUnderlyingList()
        {
            var guid = Guid.NewGuid();
            _credentialService.GetCredentialRecord(guid);
            _credentialRepositoryList.Received().GetCredentialRecord(guid);
        }

        [Test]
        public void LoadRepoListsAddsRepoFromLoader()
        {
            var repo = Substitute.For<ICredentialRepository>();
            _loader.Load().Returns(new[] { repo });
            _credentialService.LoadRepositoryList();
            _credentialRepositoryList.Received().AddProvider(repo);
        }

        [Test]
        public void SaveRepoListsAddsRepoFromLoader()
        {
            _credentialService.SaveRepositoryList();
            _saver.Received().Save(_credentialRepositoryList);
        }
    }
}