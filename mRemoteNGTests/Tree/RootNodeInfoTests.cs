using mRemoteNG.Security;
using mRemoteNG.Tree.Root;
using NUnit.Framework;


namespace mRemoteNGTests.Tree
{
    public class RootNodeInfoTests
    {
        private RootNodeInfo _rootNodeInfo;

        [SetUp]
        public void Setup()
        {
            _rootNodeInfo = new RootNodeInfo(RootNodeType.Connection);
        }

        [Test]
        public void DefaultPasswordReturnsExpectedValue()
        {
            var defaultPassword = _rootNodeInfo.DefaultPassword.ConvertToUnsecureString();
            Assert.That(defaultPassword, Is.EqualTo("mR3m"));
        }

        [TestCase("a", true)]
        [TestCase("mR3m", true)]
        [TestCase("", false)]
        [TestCase(null, false)]
        public void PasswordPropertyReflectsWhetherACustomPasswordIsInUse(string password, bool expected)
        {
            _rootNodeInfo.PasswordString = password;
            Assert.That(_rootNodeInfo.Password, Is.EqualTo(expected));
        }
    }
}