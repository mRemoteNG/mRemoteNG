using System.Runtime.Versioning;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.Tree
{
    [SetUpFixture]
    [SupportedOSPlatform("windows")]
    public class TreeSetUpFixture
    {
        private TestScope? _scope;

        [OneTimeSetUp]
        public void BeforeAllTreeTests()
        {
            _scope = TestScope.Begin();
        }

        [OneTimeTearDown]
        public void AfterAllTreeTests()
        {
            _scope?.Dispose();
        }
    }
}
