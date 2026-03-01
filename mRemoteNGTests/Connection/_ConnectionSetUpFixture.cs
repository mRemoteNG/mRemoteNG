using System.Runtime.Versioning;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.Connection
{
    [SetUpFixture]
    [SupportedOSPlatform("windows")]
    public class ConnectionSetUpFixture
    {
        private TestScope? _scope;

        [OneTimeSetUp]
        public void BeforeAllConnectionTests()
        {
            _scope = TestScope.Begin();
        }

        [OneTimeTearDown]
        public void AfterAllConnectionTests()
        {
            _scope?.Dispose();
        }
    }
}
