using System.Runtime.Versioning;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.IntegrationTests
{
    [SetUpFixture]
    [SupportedOSPlatform("windows")]
    public class IntegrationSetUpFixture
    {
        private TestScope? _scope;

        [OneTimeSetUp]
        public void BeforeAllIntegrationTests()
        {
            _scope = TestScope.Begin();
        }

        [OneTimeTearDown]
        public void AfterAllIntegrationTests()
        {
            _scope?.Dispose();
        }
    }
}
