using System.Threading;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.Tools;
using mRemoteNG.UI.Controls;
using mRemoteNG.UI.Window;
using NSubstitute;
using NUnit.Framework;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNGTests.UI.Window
{
	public class ConnectionTreeWindowTests
    {
        private ConnectionTreeWindow _connectionTreeWindow;

        [SetUp]
        public void Setup()
        {
	        var connectionInitiator = Substitute.For<IConnectionInitiator>();
	        var connectionTree = new ConnectionTree();
			var sshTransferWindow = new SSHTransferWindow();
            var externalToolsService = new ExternalToolsService();
	        var connectionContextMenu = new ConnectionContextMenu(connectionTree, connectionInitiator, sshTransferWindow, new Export(new CredentialRepositoryList()), externalToolsService);
            _connectionTreeWindow = new ConnectionTreeWindow(new DockContent(), connectionInitiator) {ConnectionTreeContextMenu = connectionContextMenu};
        }

        [TearDown]
        public void Teardown()
        {
            _connectionTreeWindow.Close();
        }

        [Test, Apartment(ApartmentState.STA)]
        public void CanShowWindow()
        {
            _connectionTreeWindow.Show();
            Assert.That(_connectionTreeWindow.Visible);
        }
    }
}