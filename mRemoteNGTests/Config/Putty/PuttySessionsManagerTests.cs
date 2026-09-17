using mRemoteNG.Config.Putty;
using mRemoteNG.Connection;
using mRemoteNG.Properties;
using mRemoteNG.Tree.Root;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Putty
{
    [TestFixture]
    [NonParallelizable]
    public class PuttySessionsManagerTests
    {
        [Test]
        public void AddSessions_SkipsProviderRoots_WhenPuttySessionsTreeDisplayIsDisabled()
        {
            PuttySessionsManager manager = PuttySessionsManager.Instance;
            bool originalValue = OptionsAdvancedPage.Default.ShowPuttySessionsInTree;
            RootPuttySessionsNodeInfo[] originalRoots = manager.RootPuttySessionsNodes.ToArray();
            TestPuttySessionsProvider testProvider = new();

            try
            {
                manager.RootPuttySessionsNodes.Clear();
                manager.AddProvider(testProvider);
                OptionsAdvancedPage.Default.ShowPuttySessionsInTree = false;

                manager.AddSessions();

                Assert.That(manager.RootPuttySessionsNodes, Does.Not.Contain(testProvider.RootInfo));
            }
            finally
            {
                OptionsAdvancedPage.Default.ShowPuttySessionsInTree = originalValue;
                manager.RemoveProvider(testProvider);
                manager.RootPuttySessionsNodes.Clear();
                manager.RootPuttySessionsNodes.AddRange(originalRoots);
            }
        }

        [Test]
        public void AddSessions_AddsProviderRoots_WhenPuttySessionsTreeDisplayIsEnabled()
        {
            PuttySessionsManager manager = PuttySessionsManager.Instance;
            bool originalValue = OptionsAdvancedPage.Default.ShowPuttySessionsInTree;
            RootPuttySessionsNodeInfo[] originalRoots = manager.RootPuttySessionsNodes.ToArray();
            TestPuttySessionsProvider testProvider = new();

            try
            {
                manager.RootPuttySessionsNodes.Clear();
                manager.AddProvider(testProvider);
                OptionsAdvancedPage.Default.ShowPuttySessionsInTree = true;

                manager.AddSessions();

                Assert.That(manager.RootPuttySessionsNodes, Contains.Item(testProvider.RootInfo));
            }
            finally
            {
                OptionsAdvancedPage.Default.ShowPuttySessionsInTree = originalValue;
                manager.RemoveProvider(testProvider);
                manager.RootPuttySessionsNodes.Clear();
                manager.RootPuttySessionsNodes.AddRange(originalRoots);
            }
        }

        [Test]
        public void AddSessions_RestoresProviderRoots_WhenPuttySessionsTreeDisplayIsToggledOffAndOn()
        {
            PuttySessionsManager manager = PuttySessionsManager.Instance;
            bool originalValue = OptionsAdvancedPage.Default.ShowPuttySessionsInTree;
            RootPuttySessionsNodeInfo[] originalRoots = manager.RootPuttySessionsNodes.ToArray();
            TestPuttySessionsProvider testProvider = new();

            try
            {
                manager.RootPuttySessionsNodes.Clear();
                manager.AddProvider(testProvider);

                OptionsAdvancedPage.Default.ShowPuttySessionsInTree = false;
                manager.AddSessions();
                Assert.That(manager.RootPuttySessionsNodes, Does.Not.Contain(testProvider.RootInfo));

                OptionsAdvancedPage.Default.ShowPuttySessionsInTree = true;
                manager.AddSessions();

                Assert.That(manager.RootPuttySessionsNodes, Contains.Item(testProvider.RootInfo));
            }
            finally
            {
                OptionsAdvancedPage.Default.ShowPuttySessionsInTree = originalValue;
                manager.RemoveProvider(testProvider);
                manager.RootPuttySessionsNodes.Clear();
                manager.RootPuttySessionsNodes.AddRange(originalRoots);
            }
        }

        private sealed class TestPuttySessionsProvider : AbstractPuttySessionsProvider
        {
            public override string[] GetSessionNames(bool raw = false)
            {
                return ["TestSession"];
            }

            public override PuttySessionInfo GetSession(string sessionName)
            {
                return new PuttySessionInfo
                {
                    Name = sessionName,
                    PuttySession = sessionName,
                    RootRootPuttySessionsInfo = RootInfo
                };
            }
        }
    }
}
