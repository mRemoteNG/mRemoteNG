﻿using System;
using mRemoteNG.Messages;
using mRemoteNG.UI;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Window;

namespace mRemoteNG.App
{
    public static class Windows
    {
        private static AboutWindow _aboutForm;
        private static ActiveDirectoryImportWindow _adimportForm;
        private static HelpWindow _helpForm;
        private static ExternalToolsWindow _externalappsForm;
        private static PortScanWindow _portscanForm;
        private static ScreenshotManagerWindow _screenshotmanagerForm;
        private static UltraVNCWindow _ultravncscForm;
        private static ComponentsCheckWindow _componentscheckForm;
        private static ConnectionTreeWindow _treeForm;

        internal static ConnectionTreeWindow TreeForm
        {
            get => _treeForm ?? (_treeForm = new ConnectionTreeWindow());
            set => _treeForm = value;
        }

        internal static ConfigWindow ConfigForm { get; set; } = new ConfigWindow();
        internal static ErrorAndInfoWindow ErrorsForm { get; set; } = new ErrorAndInfoWindow();
        internal static ScreenshotManagerWindow ScreenshotForm { get; set; } = new ScreenshotManagerWindow();
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
                    case WindowType.About:
                        if (_aboutForm == null || _aboutForm.IsDisposed)
                            _aboutForm = new AboutWindow();
                        _aboutForm.Show(dockPanel);
                        break;
                    case WindowType.ActiveDirectoryImport:
                        if (_adimportForm == null || _adimportForm.IsDisposed)
                            _adimportForm = new ActiveDirectoryImportWindow();
                        _adimportForm.Show(dockPanel);
                        break;
                    case WindowType.Options:
                        using (var optionsForm = new FrmOptions())
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
                        if (UpdateForm == null || UpdateForm.IsDisposed)
                            UpdateForm = new UpdateWindow();
                        UpdateForm.Show(dockPanel);
                        break;
                    case WindowType.Help:
                        if (_helpForm == null || _helpForm.IsDisposed)
                            _helpForm = new HelpWindow();
                        _helpForm.Show(dockPanel);
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
                    case WindowType.ScreenshotManager:
                        _screenshotmanagerForm = new ScreenshotManagerWindow();
                        _screenshotmanagerForm.Show(dockPanel);
                        break;
                    case WindowType.UltraVNCSC:
                        if (_ultravncscForm == null || _ultravncscForm.IsDisposed)
                            _ultravncscForm = new UltraVNCWindow();
                        _ultravncscForm.Show(dockPanel);
                        break;
                    case WindowType.ComponentsCheck:
                        Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                                                            "Showing ComponentsCheck window", true);
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