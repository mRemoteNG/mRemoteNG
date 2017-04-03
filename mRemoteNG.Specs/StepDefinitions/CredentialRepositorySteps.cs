using System;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace mRemoteNG.Specs.StepDefinitions
{
    [Binding]
    public class CredentialRepositorySteps
    {
        private XmlCredentialRepository _credentialRepository;

        [Given(@"I have a credential repository")]
        public void GivenIHaveACredentialRepository()
        {
            _credentialRepository = new XmlCredentialRepository(new CredentialRepositoryConfig(), new FileDataProvider(string.Empty));
        }

        [Given(@"The repository has (.*) credentials")]
        public void GivenTheRepositoryHasCredentials(int numberOfCreds)
        {
            for (var i = 0; i < numberOfCreds; i++)
                _credentialRepository.CredentialRecords.Add(new CredentialRecord());
        }

        [Given(@"The credential repository is loaded")]
        public void GivenTheCredentialRepositoryIsLoaded()
        {
            _credentialRepository.LoadCredentials();
            Assert.That(_credentialRepository.IsLoaded);
        }

        [When(@"I click add credential")]
        public void WhenIClickAddCredential()
        {
            _credentialRepository.CredentialRecords.Add(new CredentialRecord());
        }

        [Then(@"the repository has (.*) credentials")]
        public void ThenTheRepositoryHasCredentials(int numberOfCreds)
        {
            Assert.That(_credentialRepository.CredentialRecords.Count, Is.EqualTo(numberOfCreds));
        }

        [When(@"I click unload")]
        public void WhenIClickUnload()
        {
            _credentialRepository.UnloadCredentials();
        }
        
        [Then(@"the credentials in the repository will no longer be available")]
        public void ThenTheCredentialsInTheRepositoryWillNoLongerBeAvailable()
        {
            Assert.That(_credentialRepository.CredentialRecords, Is.Empty);
        }
    }
}
