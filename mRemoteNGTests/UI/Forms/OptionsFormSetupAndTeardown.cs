using System;
using System.Security;
using mRemoteNG.App;
using mRemoteNG.App.Update;
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
            var shutdown = new Shutdown(new SettingsSaver(new ExternalToolsService()), new ConnectionsService(PuttySessionsManager.Instance, import));
            Func<NotificationAreaIcon> notificationIconBuilder = () => new NotificationAreaIcon(FrmMain.Default, connectionInitiator, shutdown);
            var connectionsService = new ConnectionsService(PuttySessionsManager.Instance, import);
            var appUpdater = new AppUpdater(() => connectionsService.EncryptionKey);
            _optionsForm = new frmOptions(connectionInitiator, type => {}, notificationIconBuilder, connectionsService, appUpdater);
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