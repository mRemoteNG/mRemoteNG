using System.Linq;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.Specs.Utilities;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace mRemoteNG.Specs.StepDefinitions
{
    [Binding]
    public class CredentialRepositoryListSteps
    {
        private CredentialRepositoryList _credentialRepositoryList;
        private readonly XmlCredentialRepoBuilder _credentialRepoUtilities = new XmlCredentialRepoBuilder();

        [Given(@"I have a credential repository list")]
        public void GivenIHaveACredentialRepositoryList()
        {
            _credentialRepositoryList = new CredentialRepositoryList();
        }
        
        [Given(@"It has (.*) repositories set up")]
        public void GivenItHasRepositoriesSetUp(int numberOfCredentialRepos)
        {
            for (var i = 0; i < numberOfCredentialRepos; i++)
                _credentialRepositoryList.AddProvider(_credentialRepoUtilities.BuildXmlCredentialRepo());
        }

        [When(@"I press add and complete the creation wizard")]
        public void WhenIPressAddAndCompleteTheCreationWizard()
        {
            var credentialRepo = _credentialRepoUtilities.BuildXmlCredentialRepo();
            _credentialRepositoryList.AddProvider(credentialRepo);
        }

        [When(@"I remove the first repository")]
        public void WhenIRemoveTheFirstRepository()
        {
            var firstRepo = _credentialRepositoryList.CredentialProviders.First();
            _credentialRepositoryList.RemoveProvider(firstRepo);
        }

        [Then(@"I will have (.*) credential repository")]
        public void ThenIWillHaveCredentialRepository(int expectedCredRepoCount)
        {
            Assert.That(_credentialRepositoryList.CredentialProviders.Count(), Is.EqualTo(expectedCredRepoCount));
        }
    }
}
