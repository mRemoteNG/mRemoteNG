using System.Security;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Config.Putty;
using mRemoteNG.Config.Serializers.Xml;
using mRemoteNG.Connection;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
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
            var import = new Import(Substitute.For<IWin32Window>());
            var connectionsService = new ConnectionsService(PuttySessionsManager.Instance, import, connectionTree);
            var export = new Export(new CredentialRepositoryList(), connectionsService, connectionTree);
            var connectionContextMenu = new ConnectionContextMenu(connectionTree, connectionInitiator, sshTransferWindow, export, externalToolsService, import, connectionsService);
            _connectionTreeWindow = new ConnectionTreeWindow(new DockContent(), connectionInitiator, connectionsService) {ConnectionTreeContextMenu = connectionContextMenu};
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