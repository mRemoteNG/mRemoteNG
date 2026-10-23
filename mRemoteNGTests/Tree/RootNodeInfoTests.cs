using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using mRemoteNG.Resources.Language;
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

        [Test]
        public void PasswordStringReturnsDefaultWhenPasswordPropertySetWithoutPasswordString()
        {
            // Edge case: Password property set to true directly without setting PasswordString
            _rootNodeInfo.Password = true;
            Assert.That(_rootNodeInfo.PasswordString, Is.EqualTo(_rootNodeInfo.DefaultPassword));
        }

        [TestCase(RootNodeType.Connection, TreeNodeType.Root)]
        [TestCase(RootNodeType.PuttySessions, TreeNodeType.PuttyRoot)]
        public void RootNodeHasCorrectTreeNodeType(RootNodeType rootNodeType, TreeNodeType expectedTreeNodeType)
        {
            var rootNode = new RootNodeInfo(rootNodeType);
            Assert.That(rootNode.GetTreeNodeType(), Is.EqualTo(expectedTreeNodeType));
        }

        [TestCase(RootNodeType.Connection)]
        [TestCase(RootNodeType.PuttySessions)]
        public void NewRootNodeIsNamedConnections(RootNodeType rootNodeType)
        {
            var rootNode = new RootNodeInfo(rootNodeType, "a0000000-0000-4000-8000-000000000001");
            Assert.That(rootNode.Name, Is.EqualTo(Language.Connections));
            Assert.That(rootNode.Type, Is.EqualTo(rootNodeType));
        }
    }
}