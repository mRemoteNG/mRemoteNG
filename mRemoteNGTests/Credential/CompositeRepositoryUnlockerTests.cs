using System.Collections.Generic;
using System.Security;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.Credential
{
    public class CompositeRepositoryUnlockerTests
    {
        private CompositeRepositoryUnlocker _repositoryUnlocker;
        private IList<ICredentialRepository> _repos;

        [SetUp]
        public void Setup()
        {
            _repos = BuildRepos(3);
            _repositoryUnlocker = new CompositeRepositoryUnlocker(_repos);
        }

        [Test]
        public void WeCanCreateAnUnlockerThatWillHandleSomeRepos()
        {
            Assert.That(_repositoryUnlocker.Repositories, Is.EquivalentTo(_repos));
        }

        [Test]
        public void TheFirstRepoIsInitiallySelected()
        {
            Assert.That(_repositoryUnlocker.SelectedRepository, Is.EqualTo(_repos[0]));
        }

        [Test]
        public void WeCanUnlockARepository()
        {
            var key = new SecureString();
            _repositoryUnlocker.Unlock(key);
            _repositoryUnlocker.SelectedRepository.Received(1).LoadCredentials(key);
        }

        [Test]
        public void WeCanSelectTheNextLockedRepository()
        {
            _repos[1].IsLoaded.Returns(true);
            _repositoryUnlocker.SelectNextLockedRepository();
            Assert.That(_repositoryUnlocker.SelectedRepository, Is.EqualTo(_repos[2]));
        }

        [Test]
        public void SelectingTheNextRepoWhenOnlyOneRepoExistsDoesNothing()
        {
            var repos = BuildRepos(1);
            var repositoryUnlocker = new CompositeRepositoryUnlocker(repos);
            repositoryUnlocker.SelectNextLockedRepository();
            Assert.That(repositoryUnlocker.SelectedRepository, Is.EqualTo(repos[0]));
        }

        [Test]
        public void SelectionIsClearedIfThereAreNoMoreLockedRepositories()
        {
            foreach(var repo in _repos)
                repo.IsLoaded.Returns(true);
            _repositoryUnlocker.SelectNextLockedRepository();
            Assert.That(_repositoryUnlocker.SelectedRepository, Is.Null);
        }

        [Test]
        public void SelectionRemainsTheSameIfTheCurrentRepoIsTheOnlyOneLocked()
        {
            foreach (var repo in _repos)
                repo.IsLoaded.Returns(true);
            _repos[0].IsLoaded.Returns(false);
            _repositoryUnlocker.SelectNextLockedRepository();
            Assert.That(_repositoryUnlocker.SelectedRepository, Is.EqualTo(_repos[0]));
        }

        [Test]
        public void NothingIsSelectedIfNoReposExist()
        {
            var repositoryUnlocker = new CompositeRepositoryUnlocker(new ICredentialRepository[0]);
            repositoryUnlocker.SelectNextLockedRepository();
            Assert.That(repositoryUnlocker.SelectedRepository, Is.Null);
        }

        [Test]
        public void FirstLockedRepoSelectedIfNoRepoCurrentlySelected()
        {
            var repo = BuildRepos(1);
            repo[0].IsLoaded.Returns(false);
            var repositoryUnlocker = new CompositeRepositoryUnlocker(repo);
            repositoryUnlocker.SelectNextLockedRepository();
            Assert.That(repositoryUnlocker.SelectedRepository, Is.EqualTo(repo[0]));
        }

        private IList<ICredentialRepository> BuildRepos(int count)
        {
            var list = new List<ICredentialRepository>();
            for (var i=0; i < count; i++)
                list.Add(Substitute.For<ICredentialRepository>());
            return list;
        }
    }
}