using System;
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
            Func<NotificationAreaIcon> notificationAreaIconBuilder)
        {
            _connectionInitiator = connectionInitiator.ThrowIfNull(nameof(connectionInitiator));
            TreeForm = treeForm.ThrowIfNull(nameof(treeForm));
            ConfigForm = configForm.ThrowIfNull(nameof(configForm));
            ErrorsForm = errorAndInfoWindow.ThrowIfNull(nameof(errorAndInfoWindow));
            ScreenshotForm = screenshotForm.ThrowIfNull(nameof(screenshotForm));
            SshtransferForm = sshtransferForm.ThrowIfNull(nameof(sshtransferForm));
            _updateWindowBuilder = updateWindowBuilder;
            _notificationAreaIconBuilder = notificationAreaIconBuilder;
        }

        public void Show(WindowType windowType)
        {
            try
            {
                var dockPanel = FrmMain.Default.pnlDock;
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (windowType)
                {
                    case WindowType.About:
                        if (_aboutForm == null || _aboutForm.IsDisposed)
                            _aboutForm = new AboutWindow();
                        _aboutForm.Show(dockPanel);
                        break;
                    case WindowType.ActiveDirectoryImport:
                        if (_adimportForm == null || _adimportForm.IsDisposed)
                            _adimportForm = new ActiveDirectoryImportWindow(() => TreeForm.SelectedNode);
                        _adimportForm.Show(dockPanel);
                        break;
                    case WindowType.Options:
                        using (var optionsForm = new frmOptions(_connectionInitiator, Show, _notificationAreaIconBuilder))
                        {
                            optionsForm.ShowDialog(dockPanel);
                        }
                        break;
                    case WindowType.SSHTransfer:
                        if (SshtransferForm == null || SshtransferForm.IsDisposed)
                            SshtransferForm = new SSHTransferWindow();
                        SshtransferForm.Show(dockPanel);
                        break;
                    case WindowType.Update:
                        if (_updateForm == null || _updateForm.IsDisposed)
                            _updateForm = _updateWindowBuilder();
                        _updateForm.Show(dockPanel);
                        break;
                    case WindowType.Help:
                        if (_helpForm == null || _helpForm.IsDisposed)
                            _helpForm = new HelpWindow();
                        _helpForm.Show(dockPanel);
                        break;
                    case WindowType.ExternalApps:
                        if (_externalappsForm == null || _externalappsForm.IsDisposed)
                            _externalappsForm = new ExternalToolsWindow(_connectionInitiator);
                        _externalappsForm.Show(dockPanel);
                        break;
                    case WindowType.PortScan:
                        _portscanForm = new PortScanWindow(() => TreeForm.SelectedNode);
                        _portscanForm.Show(dockPanel);
                        break;
                    case WindowType.UltraVNCSC:
                        if (_ultravncscForm == null || _ultravncscForm.IsDisposed)
                            _ultravncscForm = new UltraVNCWindow(Show);
                        _ultravncscForm.Show(dockPanel);
                        break;
                    case WindowType.ComponentsCheck:
                        Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Showing ComponentsCheck window", true);
                        if (_componentscheckForm == null || _componentscheckForm.IsDisposed)
                            _componentscheckForm = new ComponentsCheckWindow();
                        _componentscheckForm.Show(dockPanel);
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