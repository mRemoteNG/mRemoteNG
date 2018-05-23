using System;
using System.Security;
using System.Windows.Forms;
using System.Xml.Linq;
using mRemoteNG.App;
using mRemoteNG.App.Update;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Config.Putty;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Config.Serializers.Xml;
using mRemoteNG.Config.Settings;
using mRemoteNG.Connection;
using mRemoteNG.Security;
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
            var frmMain = new FrmMain();
	        var connectionInitiator = Substitute.For<IConnectionInitiator>();
            var import = new Import(Substitute.For<IWin32Window>());
            var shutdown = new Shutdown(new SettingsSaver(new ExternalToolsService()), new ConnectionsService(PuttySessionsManager.Instance, import, frmMain), frmMain);
            var connectionsService = new ConnectionsService(PuttySessionsManager.Instance, import, frmMain);
            Func<NotificationAreaIcon> notificationIconBuilder = () => new NotificationAreaIcon(frmMain, connectionInitiator, shutdown, connectionsService);
            Func<SecureString> encryptionKeySelectionFunc = () => connectionsService.EncryptionKey;
            var databaseConnectorFactory = new DatabaseConnectorFactory(encryptionKeySelectionFunc);
            var appUpdater = new AppUpdater(encryptionKeySelectionFunc);
            _optionsForm = new frmOptions(connectionInitiator, type => {}, notificationIconBuilder, connectionsService, appUpdater, databaseConnectorFactory, frmMain);
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