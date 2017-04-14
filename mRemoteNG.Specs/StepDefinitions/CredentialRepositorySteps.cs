using System.Security;
using mRemoteNG.Credential;
using mRemoteNG.Security;
using mRemoteNG.Specs.Utilities;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace mRemoteNG.Specs.StepDefinitions
{
    [Binding]
    public class CredentialRepositorySteps
    {
        private ICredentialRepository _credentialRepository;
        private readonly SecureString _key = "somePassword".ConvertToSecureString();

        [Given(@"I have a credential repository")]
        public void GivenIHaveACredentialRepository()
        {
            var utilityFactory = new XmlCredentialRepoBuilder {EncryptionKey = _key};
            _credentialRepository = utilityFactory.BuildXmlCredentialRepo();
        }

        [Given(@"The repository has (.*) credentials")]
        public void GivenTheRepositoryHasCredentials(int numberOfCreds)
        {
            for (var i = 0; i < numberOfCreds; i++)
                _credentialRepository.CredentialRecords.Add(new CredentialRecord());
            Assert.That(_credentialRepository.CredentialRecords.Count, Is.EqualTo(numberOfCreds));
        }

        [Given(@"The credential repository is loaded")]
        public void GivenTheCredentialRepositoryIsLoaded()
        {
            _credentialRepository.LoadCredentials(_key);
            Assert.That(_credentialRepository.IsLoaded);
        }

        [Given(@"the credential repository is unloaded")]
        public void GivenTheCredentialRepositoryIsUnloaded()
        {
            Assert.That(_credentialRepository.IsLoaded, Is.False);
        }

        [When(@"I click load")]
        public void WhenIClickLoad()
        {
            _credentialRepository.LoadCredentials(_key);
        }

        [Then(@"the credential repository is loaded")]
        public void ThenTheCredentialRepositoryIsLoaded()
        {
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
