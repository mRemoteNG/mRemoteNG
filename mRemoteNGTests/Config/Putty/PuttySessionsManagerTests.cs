using mRemoteNG.Config.Putty;
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
        public void AddSessions_DoesNotClearExistingRootNodes_WhenPuttySessionsTreeDisplayIsDisabled()
        {
            PuttySessionsManager manager = PuttySessionsManager.Instance;
            bool originalValue = OptionsAdvancedPage.Default.ShowPuttySessionsInTree;
            RootPuttySessionsNodeInfo[] originalRoots = manager.RootPuttySessionsNodes.ToArray();
            RootPuttySessionsNodeInfo sentinelRoot = new RootPuttySessionsNodeInfo();

            try
            {
                manager.RootPuttySessionsNodes.Clear();
                manager.RootPuttySessionsNodes.Add(sentinelRoot);
                OptionsAdvancedPage.Default.ShowPuttySessionsInTree = false;

                manager.AddSessions();

                Assert.That(manager.RootPuttySessionsNodes, Contains.Item(sentinelRoot));
            }
            finally
            {
                OptionsAdvancedPage.Default.ShowPuttySessionsInTree = originalValue;
                manager.RootPuttySessionsNodes.Clear();
                manager.RootPuttySessionsNodes.AddRange(originalRoots);
            }
        }

        [Test]
        public void AddSessions_DoesNotClearExistingRootNodes_WhenPuttySessionsTreeDisplayIsEnabled()
        {
            PuttySessionsManager manager = PuttySessionsManager.Instance;
            bool originalValue = OptionsAdvancedPage.Default.ShowPuttySessionsInTree;
            RootPuttySessionsNodeInfo[] originalRoots = manager.RootPuttySessionsNodes.ToArray();
            RootPuttySessionsNodeInfo sentinelRoot = new RootPuttySessionsNodeInfo();

            try
            {
                manager.RootPuttySessionsNodes.Clear();
                manager.RootPuttySessionsNodes.Add(sentinelRoot);
                OptionsAdvancedPage.Default.ShowPuttySessionsInTree = true;

                manager.AddSessions();

                Assert.That(manager.RootPuttySessionsNodes, Contains.Item(sentinelRoot));
            }
            finally
            {
                OptionsAdvancedPage.Default.ShowPuttySessionsInTree = originalValue;
                manager.RootPuttySessionsNodes.Clear();
                manager.RootPuttySessionsNodes.AddRange(originalRoots);
            }
        }

        [Test]
        public void AddSessions_RetainsCachedRootsThroughDisableEnableToggle()
        {
            PuttySessionsManager manager = PuttySessionsManager.Instance;
            bool originalValue = OptionsAdvancedPage.Default.ShowPuttySessionsInTree;
            RootPuttySessionsNodeInfo[] originalRoots = manager.RootPuttySessionsNodes.ToArray();
            RootPuttySessionsNodeInfo sentinelRoot = new RootPuttySessionsNodeInfo();

            try
            {
                manager.RootPuttySessionsNodes.Clear();
                manager.RootPuttySessionsNodes.Add(sentinelRoot);

                OptionsAdvancedPage.Default.ShowPuttySessionsInTree = false;
                manager.AddSessions();
                OptionsAdvancedPage.Default.ShowPuttySessionsInTree = true;
                manager.AddSessions();

                Assert.That(manager.RootPuttySessionsNodes, Contains.Item(sentinelRoot));
            }
            finally
            {
                OptionsAdvancedPage.Default.ShowPuttySessionsInTree = originalValue;
                manager.RootPuttySessionsNodes.Clear();
                manager.RootPuttySessionsNodes.AddRange(originalRoots);
            }
        }
    }
}
