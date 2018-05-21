using System;
using System.Security;
using mRemoteNG.App;
using mRemoteNG.App.Update;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Config.Putty;
using mRemoteNG.Config.Settings;
using mRemoteNG.Connection;
using mRemoteNG.Tools;
using mRemoteNG.UI.Forms;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Forms
{
	public class OptionsFormSetupAndTeardown
    {
        protected frmOptions _optionsForm;

        [OneTimeSetUp]
        public void OnetimeSetup()
        {
        }

        [SetUp]
        public void Setup()
        {
	        var connectionInitiator = Substitute.For<IConnectionInitiator>();
            var import = new Import();
            var shutdown = new Shutdown(new SettingsSaver(new ExternalToolsService()), new ConnectionsService(PuttySessionsManager.Instance, import), FrmMain.Default);
            Func<NotificationAreaIcon> notificationIconBuilder = () => new NotificationAreaIcon(FrmMain.Default, connectionInitiator, shutdown);
            var connectionsService = new ConnectionsService(PuttySessionsManager.Instance, import);
            Func<SecureString> encryptionKeySelectionFunc = () => connectionsService.EncryptionKey;
            var databaseConnectorFactory = new DatabaseConnectorFactory(encryptionKeySelectionFunc);
            var appUpdater = new AppUpdater(encryptionKeySelectionFunc);
            _optionsForm = new frmOptions(connectionInitiator, type => {}, notificationIconBuilder, connectionsService, appUpdater, databaseConnectorFactory, FrmMain.Default);
            _optionsForm.Show();
        }

        [TearDown]
        public void Teardown()
        {
            _optionsForm.Dispose();
            while (_optionsForm.Disposing) ;
            _optionsForm = null;
        }
    }
}