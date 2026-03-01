using System.Runtime.Versioning;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.UI
{
    [SetUpFixture]
    [SupportedOSPlatform("windows")]
    public class UISetUpFixture
    {
        private TestScope? _scope;

        [OneTimeSetUp]
        public void BeforeAllUITests()
        {
            _scope = TestScope.Begin();
        }

        [OneTimeTearDown]
        public void AfterAllUITests()
        {
            _scope?.Dispose();
        }
    }
}
