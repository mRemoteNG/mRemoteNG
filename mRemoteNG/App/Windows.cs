#region Usings
using System;
using System.Runtime.Versioning;
using mRemoteNG.Resources.Language;
using mRemoteNG.UI;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Window;
#endregion

namespace mRemoteNG.App
{
    [SupportedOSPlatform("windows")]
    public static class Windows
    {
        private static ActiveDirectoryImportWindow _adimportForm;
        private static ExternalToolsWindow _externalappsForm;
        private static PortScanWindow _portscanForm;
        private static UltraVNCWindow _ultravncscForm;
        private static ConnectionTreeWindow _treeForm;

        internal static ConnectionTreeWindow TreeForm
        {
            get => _treeForm ?? (_treeForm = new ConnectionTreeWindow());
            set => _treeForm = value;
        }

        internal static ConfigWindow ConfigForm { get; set; } = new ConfigWindow();
        internal static ErrorAndInfoWindow ErrorsForm { get; set; } = new ErrorAndInfoWindow();
        private static UpdateWindow UpdateForm { get; set; } = new UpdateWindow();
        internal static SSHTransferWindow SshtransferForm { get; private set; } = new SSHTransferWindow();


        public static void Show(WindowType windowType)
        {
            try
            {
                var dockPanel = FrmMain.Default.pnlDock;
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (windowType)
                {
                    case WindowType.ActiveDirectoryImport:
                        if (_adimportForm == null || _adimportForm.IsDisposed)
                            _adimportForm = new ActiveDirectoryImportWindow();
                        _adimportForm.Show(dockPanel);
                        break;
                    case WindowType.Options:
                        FrmMain.OptionsForm.SetActivatedPage(Language.StartupExit);
                        FrmMain.OptionsForm.Visible = true;
                        break;
                    case WindowType.SSHTransfer:
                        if (SshtransferForm == null || SshtransferForm.IsDisposed)
                            SshtransferForm = new SSHTransferWindow();
                        SshtransferForm.Show(dockPanel);
                        break;
                    case WindowType.Update:
                        if (UpdateForm == null || UpdateForm.IsDisposed)
                            UpdateForm = new UpdateWindow();
                        UpdateForm.Show(dockPanel);
                        break;
                    case WindowType.ExternalApps:
                        if (_externalappsForm == null || _externalappsForm.IsDisposed)
                            _externalappsForm = new ExternalToolsWindow();
                        _externalappsForm.Show(dockPanel);
                        break;
                    case WindowType.PortScan:
                        _portscanForm = new PortScanWindow();
                        _portscanForm.Show(dockPanel);
                        break;
                    case WindowType.UltraVNCSC:
                        if (_ultravncscForm == null || _ultravncscForm.IsDisposed)
                            _ultravncscForm = new UltraVNCWindow();
                        _ultravncscForm.Show(dockPanel);
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