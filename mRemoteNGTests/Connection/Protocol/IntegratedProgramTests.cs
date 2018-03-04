using System;
using System.Security;
using System.Threading;
using mRemoteNG.App;
using mRemoteNG.App.Update;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Config.Putty;
using mRemoteNG.Config.Settings;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.Tools;
using mRemoteNG.UI.Controls;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Window;
using NSubstitute;
using NUnit.Framework;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNGTests.Connection.Protocol
{
	public class IntegratedProgramTests
    {
        private ExternalToolsService _externalToolsService;
        private IConnectionInitiator _connectionInitiator;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _connectionInitiator = Substitute.For<IConnectionInitiator>();
            var extTool = new ExternalTool(_connectionInitiator)
            {
                DisplayName = "notepad",
                FileName = @"%windir%\system32\notepad.exe",
                Arguments = "",
                TryIntegrate = true
            };
            _externalToolsService = new ExternalToolsService();
            _externalToolsService.ExternalTools.Add(extTool);
        }

        [Test]
		[Apartment(ApartmentState.STA)]
		public void CanStartExternalApp()
		{
			var sut = new IntegratedProgram(_externalToolsService);
			sut.InterfaceControl = BuildInterfaceControl("notepad", sut);
			sut.Initialize();
			var appStarted = sut.Connect();
			sut.Disconnect();
			Assert.That(appStarted);
		}

		[Test]
		[Apartment(ApartmentState.STA)]
		public void ConnectingToExternalAppThatDoesntExistDoesNothing()
		{
			var sut = new IntegratedProgram(_externalToolsService);
			sut.InterfaceControl = BuildInterfaceControl("doesntExist", sut);
			var appInitialized = sut.Initialize();
			Assert.That(appInitialized, Is.False);
		}

		private InterfaceControl BuildInterfaceControl(string extAppName, ProtocolBase sut)
		{
            var import = new Import();
            var connectionsService = new ConnectionsService(PuttySessionsManager.Instance, import);
			var configWindow = new ConfigWindow(new DockContent());
			var sshTransferWindow = new SSHTransferWindow();
			var connectionTreeWindow = new ConnectionTreeWindow(new DockContent(), _connectionInitiator, connectionsService);
		    Func<SecureString> encryptionKeySelectionFunc = () => connectionsService.EncryptionKey;
            var connectionTree = connectionTreeWindow.ConnectionTree;
            var export = new Export(new CredentialRepositoryList(), connectionsService);
			var connectionTreeContextMenu = new ConnectionContextMenu(connectionTree, _connectionInitiator, sshTransferWindow, export, _externalToolsService, import);
			connectionTreeWindow.ConnectionTreeContextMenu = connectionTreeContextMenu;
			var errorAndInfoWindow = new ErrorAndInfoWindow(new DockContent(), connectionTreeWindow);
			var screenshotManagerWindow = new ScreenshotManagerWindow(new DockContent());
		    var shutdown = new Shutdown(new SettingsSaver(new ExternalToolsService()), new ConnectionsService(PuttySessionsManager.Instance, import));
		    var appUpdater = new AppUpdater(encryptionKeySelectionFunc);
		    Func<UpdateWindow> updateWindowBuilder = () => new UpdateWindow(new DockContent(), shutdown, appUpdater);
            Func<NotificationAreaIcon> notificationAreaIconBuilder = () => new NotificationAreaIcon(FrmMain.Default, _connectionInitiator, shutdown);
		    Func<ExternalToolsWindow> externalToolsWindowBuilder = () => new ExternalToolsWindow(_connectionInitiator, _externalToolsService);
		    Func<PortScanWindow> portScanWindowBuilder = () => new PortScanWindow(() => connectionTreeWindow.SelectedNode, import);
		    Func<ActiveDirectoryImportWindow> activeDirectoryImportWindowBuilder = () => new ActiveDirectoryImportWindow(() => connectionTreeWindow.SelectedNode, import);
		    var databaseConnectorFactory = new DatabaseConnectorFactory(encryptionKeySelectionFunc);
            var windows = new Windows(_connectionInitiator, connectionTreeWindow, configWindow, errorAndInfoWindow, screenshotManagerWindow, 
                sshTransferWindow, updateWindowBuilder, notificationAreaIconBuilder, externalToolsWindowBuilder, 
                connectionsService, portScanWindowBuilder, activeDirectoryImportWindowBuilder, appUpdater, databaseConnectorFactory);
			var connectionWindow = new ConnectionWindow(new DockContent(), _connectionInitiator, windows, _externalToolsService);
			var connectionInfo = new ConnectionInfo {ExtApp = extAppName};
			return new InterfaceControl(connectionWindow, sut, connectionInfo);
		}
	}
}