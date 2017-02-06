using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Window;
using System;
using mRemoteNG.Messages;
using mRemoteNG.UI;

namespace mRemoteNG.App
{
    public static class Windows
    {
        private static AboutWindow _aboutForm;
        private static ActiveDirectoryImportWindow _adimportForm;
        private static HelpWindow _helpForm;
        private static ExternalToolsWindow _externalappsForm;
        private static PortScanWindow _portscanForm;
        private static UltraVNCWindow _ultravncscForm;
        private static ComponentsCheckWindow _componentscheckForm;

        public static ConnectionTreeWindow TreeForm { get; set; } = new ConnectionTreeWindow();
        public static ConfigWindow ConfigForm { get; set; } = new ConfigWindow();
        public static ErrorAndInfoWindow ErrorsForm { get; set; } = new ErrorAndInfoWindow();
        public static ScreenshotManagerWindow ScreenshotForm { get; set; } = new ScreenshotManagerWindow();
        public static UpdateWindow UpdateForm { get; set; } = new UpdateWindow();
        public static SSHTransferWindow SshtransferForm { get; set; }
        
        
        public static void Show(WindowType windowType)
        {
            try
            {
                if (windowType.Equals(WindowType.About))
                {
                    if (_aboutForm == null || _aboutForm.IsDisposed)
                        _aboutForm = new AboutWindow();
                    _aboutForm.Show(frmMain.Default.pnlDock);
                }
                else if (windowType.Equals(WindowType.ActiveDirectoryImport))
                {
                    if (_adimportForm == null || _adimportForm.IsDisposed)
                        _adimportForm = new ActiveDirectoryImportWindow();
                    _adimportForm.Show(frmMain.Default.pnlDock);
                }
                else if (windowType.Equals(WindowType.Options))
                {
                    using (var optionsForm = new frmOptions())
                    {
                        optionsForm.ShowDialog(frmMain.Default.pnlDock);
                    }
                }
                else if (windowType.Equals(WindowType.SSHTransfer))
                {
                    SshtransferForm = new SSHTransferWindow();
                    SshtransferForm.Show(frmMain.Default.pnlDock);
                }
                else if (windowType.Equals(WindowType.Update))
                {
                    if (UpdateForm == null || UpdateForm.IsDisposed)
                        UpdateForm = new UpdateWindow();
                    UpdateForm.Show(frmMain.Default.pnlDock);
                }
                else if (windowType.Equals(WindowType.Help))
                {
                    if (_helpForm == null || _helpForm.IsDisposed)
                        _helpForm = new HelpWindow();
                    _helpForm.Show(frmMain.Default.pnlDock);
                }
                else if (windowType.Equals(WindowType.ExternalApps))
                {
                    if (_externalappsForm == null || _externalappsForm.IsDisposed)
                        _externalappsForm = new ExternalToolsWindow();
                    _externalappsForm.Show(frmMain.Default.pnlDock);
                }
                else if (windowType.Equals(WindowType.PortScan))
                {
                    _portscanForm = new PortScanWindow();
                    _portscanForm.Show(frmMain.Default.pnlDock);
                }
                else if (windowType.Equals(WindowType.UltraVNCSC))
                {
                    if (_ultravncscForm == null || _ultravncscForm.IsDisposed)
                        _ultravncscForm = new UltraVNCWindow();
                    _ultravncscForm.Show(frmMain.Default.pnlDock);
                }
                else if (windowType.Equals(WindowType.ComponentsCheck))
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Showing ComponentsCheck window", true);
                    if (_componentscheckForm == null || _componentscheckForm.IsDisposed)
                        _componentscheckForm = new ComponentsCheckWindow();
                    _componentscheckForm.Show(frmMain.Default.pnlDock);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("App.Runtime.Windows.Show() failed.", ex);
            }
        }
    }
}