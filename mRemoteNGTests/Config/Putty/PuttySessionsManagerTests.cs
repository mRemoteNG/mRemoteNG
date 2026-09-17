using mRemoteNG.Config.Putty;
using mRemoteNG.Properties;
using mRemoteNG.Tree.Root;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Putty
{
    [TestFixture]
    public class PuttySessionsManagerTests
    {
        [Test]
        public void AddSessions_ClearsRootNodes_WhenPuttySessionsTreeDisplayIsDisabled()
        {
            PuttySessionsManager manager = PuttySessionsManager.Instance;
            bool originalValue = OptionsAdvancedPage.Default.ShowPuttySessionsInTree;
            RootPuttySessionsNodeInfo[] originalRoots = manager.RootPuttySessionsNodes.ToArray();

            try
            {
                manager.RootPuttySessionsNodes.Clear();
                manager.RootPuttySessionsNodes.Add(new RootPuttySessionsNodeInfo());
                OptionsAdvancedPage.Default.ShowPuttySessionsInTree = false;

                manager.AddSessions();

                Assert.That(manager.RootPuttySessionsNodes, Is.Empty);
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
