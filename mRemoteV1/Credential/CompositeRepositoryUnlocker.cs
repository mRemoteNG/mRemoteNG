using System;
using System.Collections.Generic;
using System.Security;

namespace mRemoteNG.Credential
{
    public class CompositeRepositoryUnlocker
    {
        private readonly List<ICredentialRepository> _repositories = new List<ICredentialRepository>();

        public IEnumerable<ICredentialRepository> Repositories => _repositories;
        public ICredentialRepository SelectedRepository { get; set; }

        public CompositeRepositoryUnlocker(IEnumerable<ICredentialRepository> repositories)
        {
            if (repositories == null)
                throw new ArgumentNullException(nameof(repositories));

            _repositories.AddRange(repositories);
            SelectedRepository = _repositories[0];
        }

        public void Unlock(SecureString key)
        {
            SelectedRepository.LoadCredentials(key);
        }

        public void SelectNextLockedRepository()
        {
            SelectedRepository = GetNextLockedRepo();
        }

        private ICredentialRepository GetNextLockedRepo()
        {
            var initialIndex = _repositories.IndexOf(SelectedRepository);
            var currentIndex = initialIndex + 1;
            if (currentIndex >= _repositories.Count) return SelectedRepository;
            while (_repositories[currentIndex].IsLoaded)
            {
                if (currentIndex == initialIndex)
                    return null;
                currentIndex += 1;
                if (currentIndex >= _repositories.Count)
                    currentIndex = 0;
            }
            return _repositories[currentIndex];
        }
    }
}