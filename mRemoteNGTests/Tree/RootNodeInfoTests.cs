using mRemoteNG.Tree;
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
            var defaultPassword = _rootNodeInfo.DefaultPassword;
            Assert.That(defaultPassword, Is.EqualTo("mR3m"));
        }

        [TestCase("a", true)]
        [TestCase("mR3m", false)]
        [TestCase("", false)]
        [TestCase(null, false)]
        public void PasswordPropertyReflectsWhetherACustomPasswordIsInUse(string password, bool expected)
        {
            _rootNodeInfo.PasswordString = password;
            Assert.That(_rootNodeInfo.Password, Is.EqualTo(expected));
        }

        [TestCase("")]
        [TestCase(null)]
        public void PasswordStringReturnsDefaultPasswordWhenNoCustomOneIsSet(string password)
        {
            _rootNodeInfo.PasswordString = password;
            Assert.That(_rootNodeInfo.PasswordString, Is.EqualTo(_rootNodeInfo.DefaultPassword));
        }

        [TestCase("a")]
        [TestCase("1234")]
        public void PasswordStringReturnsCustomPassword(string password)
        {
            _rootNodeInfo.PasswordString = password;
            Assert.That(_rootNodeInfo.PasswordString, Is.EqualTo(password));
        }

        [TestCase(RootNodeType.Connection, TreeNodeType.Root)]
        [TestCase(RootNodeType.PuttySessions, TreeNodeType.PuttyRoot)]
        public void RootNodeHasCorrectTreeNodeType(RootNodeType rootNodeType, TreeNodeType expectedTreeNodeType)
        {
            var rootNode = new RootNodeInfo(rootNodeType);
            Assert.That(rootNode.GetTreeNodeType(), Is.EqualTo(expectedTreeNodeType));
        }
    }
}