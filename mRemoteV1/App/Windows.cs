using System;
using mRemoteNG.App.Update;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Connection;
using mRemoteNG.Messages;
using mRemoteNG.Tools;
using mRemoteNG.UI;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Window;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.App
{
	public class Windows
    {
        private readonly IConnectionInitiator _connectionInitiator;
        private AboutWindow _aboutForm;
        private ActiveDirectoryImportWindow _adimportForm;
        private HelpWindow _helpForm;
        private ExternalToolsWindow _externalappsForm;
        private PortScanWindow _portscanForm;
        private UltraVNCWindow _ultravncscForm;
        private ComponentsCheckWindow _componentscheckForm;
        private UpdateWindow _updateForm;
        private readonly Func<UpdateWindow> _updateWindowBuilder;
        private readonly Func<NotificationAreaIcon> _notificationAreaIconBuilder;
        private readonly Func<ExternalToolsWindow> _externalToolsWindowBuilder;
        private readonly Func<PortScanWindow> _portScanWindowBuilder;
        private readonly Func<ActiveDirectoryImportWindow> _activeDirectoryImportWindowBuilder;
        private readonly ConnectionsService _connectionsService;
        private readonly AppUpdater _appUpdater;
        private readonly DatabaseConnectorFactory _databaseConnectorFactory;
        private readonly FrmMain _frmMain;

        internal ConnectionTreeWindow TreeForm { get; }
        internal ConfigWindow ConfigForm { get; }
        internal ErrorAndInfoWindow ErrorsForm { get; }
        internal ScreenshotManagerWindow ScreenshotForm { get; }
        internal SSHTransferWindow SshtransferForm { get; private set; }

        public Windows(
            IConnectionInitiator connectionInitiator,
            ConnectionTreeWindow treeForm,
            ConfigWindow configForm,
            ErrorAndInfoWindow errorAndInfoWindow,
            ScreenshotManagerWindow screenshotForm,
            SSHTransferWindow sshtransferForm,
            Func<UpdateWindow> updateWindowBuilder,
            Func<NotificationAreaIcon> notificationAreaIconBuilder,
            Func<ExternalToolsWindow> externalToolsWindowBuilder, 
            ConnectionsService connectionsService, 
            Func<PortScanWindow> portScanWindowBuilder, 
            Func<ActiveDirectoryImportWindow> activeDirectoryImportWindowBuilder, 
            AppUpdater appUpdater, 
            DatabaseConnectorFactory databaseConnectorFactory,
            FrmMain frmMain)
        {
            _connectionInitiator = connectionInitiator.ThrowIfNull(nameof(connectionInitiator));
            TreeForm = treeForm.ThrowIfNull(nameof(treeForm));
            ConfigForm = configForm.ThrowIfNull(nameof(configForm));
            ErrorsForm = errorAndInfoWindow.ThrowIfNull(nameof(errorAndInfoWindow));
            ScreenshotForm = screenshotForm.ThrowIfNull(nameof(screenshotForm));
            SshtransferForm = sshtransferForm.ThrowIfNull(nameof(sshtransferForm));
            _updateWindowBuilder = updateWindowBuilder;
            _notificationAreaIconBuilder = notificationAreaIconBuilder;
            _externalToolsWindowBuilder = externalToolsWindowBuilder;
            _portScanWindowBuilder = portScanWindowBuilder;
            _activeDirectoryImportWindowBuilder = activeDirectoryImportWindowBuilder;
            _frmMain = frmMain;
            _databaseConnectorFactory = databaseConnectorFactory.ThrowIfNull(nameof(databaseConnectorFactory));
            _appUpdater = appUpdater.ThrowIfNull(nameof(appUpdater));
            _connectionsService = connectionsService.ThrowIfNull(nameof(connectionsService));
        }

        public void Show(WindowType windowType)
        {
            try
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (windowType)
                {
                    case WindowType.About:
                        if (_aboutForm == null || _aboutForm.IsDisposed)
                            _aboutForm = new AboutWindow();
                        _aboutForm.Show(_frmMain);
                        break;
                    case WindowType.ActiveDirectoryImport:
                        if (_adimportForm == null || _adimportForm.IsDisposed)
                            _adimportForm = _activeDirectoryImportWindowBuilder();
                        _adimportForm.Show(_frmMain);
                        break;
                    case WindowType.Options:
                        using (var optionsForm = new frmOptions(_connectionInitiator, Show, _notificationAreaIconBuilder, _connectionsService, _appUpdater, _databaseConnectorFactory, _frmMain))
                        {
                            optionsForm.ShowDialog(_frmMain);
                        }
                        break;
                    case WindowType.SSHTransfer:
                        if (SshtransferForm == null || SshtransferForm.IsDisposed)
                            SshtransferForm = new SSHTransferWindow(_frmMain);
                        SshtransferForm.Show(_frmMain);
                        break;
                    case WindowType.Update:
                        if (_updateForm == null || _updateForm.IsDisposed)
                            _updateForm = _updateWindowBuilder();
                        _updateForm.Show(_frmMain);
                        break;
                    case WindowType.Help:
                        if (_helpForm == null || _helpForm.IsDisposed)
                            _helpForm = new HelpWindow();
                        _helpForm.Show(_frmMain);
                        break;
                    case WindowType.ExternalApps:
                        if (_externalappsForm == null || _externalappsForm.IsDisposed)
                            _externalappsForm = _externalToolsWindowBuilder();
                        _externalappsForm.Show(_frmMain);
                        break;
                    case WindowType.PortScan:
                        _portscanForm = _portScanWindowBuilder();
                        _portscanForm.Show(_frmMain);
                        break;
                    case WindowType.UltraVNCSC:
                        if (_ultravncscForm == null || _ultravncscForm.IsDisposed)
                            _ultravncscForm = new UltraVNCWindow(Show);
                        _ultravncscForm.Show(_frmMain);
                        break;
                    case WindowType.ComponentsCheck:
                        Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Showing ComponentsCheck window", true);
                        if (_componentscheckForm == null || _componentscheckForm.IsDisposed)
                            _componentscheckForm = new ComponentsCheckWindow();
                        _componentscheckForm.Show(_frmMain);
                        break;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("App.Runtime.Windows.Show() failed.", ex);
            }
        }
    }
}