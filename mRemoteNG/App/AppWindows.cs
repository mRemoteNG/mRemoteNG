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
    public static class AppWindows
    {
        private static ActiveDirectoryImportWindow? _adimportForm;
        private static ExternalToolsWindow? _externalappsForm;
        private static PortScanWindow? _portscanForm;
        private static ConnectionTesterWindow? _connectionTesterForm;
        private static UltraVNCWindow? _ultravncscForm;
        private static ConnectionTreeWindow? _treeForm;
        private static KeyboardShortcutsWindow? _keyboardShortcutsForm;
        private static ActiveConnectionsWindow? _activeConnectionsForm;

        internal static ConnectionTreeWindow? TreeForm
        {
            get => _treeForm ?? (_treeForm = new ConnectionTreeWindow());
            set => _treeForm = value;
        }

        internal static ConfigWindow ConfigForm { get; set; } = new ConfigWindow();
        internal static ErrorAndInfoWindow ErrorsForm { get; set; } = new ErrorAndInfoWindow();
        internal static UpdateWindow UpdateForm { get; set; } = new UpdateWindow();
        internal static SSHTransferWindow SshtransferForm { get; private set; } = new SSHTransferWindow();
        internal static OptionsWindow? OptionsFormWindow { get; private set; }


        public static void Show(WindowType windowType)
        {
            try
            {
                WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel = FrmMain.Default.pnlDock;
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (windowType)
                {
                    case WindowType.ActiveDirectoryImport:
                        if (_adimportForm == null || _adimportForm.IsDisposed)
                            _adimportForm = new ActiveDirectoryImportWindow();
                        _adimportForm.Show(dockPanel);
                        break;
                    case WindowType.Options:
                        if (OptionsFormWindow == null || OptionsFormWindow.IsDisposed)
                            OptionsFormWindow = new OptionsWindow();
                        OptionsFormWindow.SetActivatedPage(Language.StartupExit);
                        // Reload controls from stored settings before every show so that any
                        // edits left over from a previous hide (Tab-X without Apply/OK) are
                        // discarded.  Safe on first call — no-op until FrmOptions is embedded.
                        OptionsFormWindow.RefreshSettings();
                        OptionsFormWindow.Show(dockPanel);
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
                    case WindowType.ConnectionTester:
                        if (_connectionTesterForm == null || _connectionTesterForm.IsDisposed)
                            _connectionTesterForm = new ConnectionTesterWindow();
                        _connectionTesterForm.Show(dockPanel);
                        break;
                    case WindowType.UltraVNCSC:
                        if (_ultravncscForm == null || _ultravncscForm.IsDisposed)
                            _ultravncscForm = new UltraVNCWindow();
                        _ultravncscForm.Show(dockPanel);
                        break;
                    case WindowType.KeyboardShortcuts:
                        if (_keyboardShortcutsForm == null || _keyboardShortcutsForm.IsDisposed)
                            _keyboardShortcutsForm = new KeyboardShortcutsWindow();
                        _keyboardShortcutsForm.Show(dockPanel);
                        break;
                    case WindowType.ActiveConnections:
                        if (_activeConnectionsForm == null || _activeConnectionsForm.IsDisposed)
                            _activeConnectionsForm = new ActiveConnectionsWindow();
                        _activeConnectionsForm.RefreshList();
                        _activeConnectionsForm.Show(dockPanel);
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