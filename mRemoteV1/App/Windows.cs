using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Window;
using System;
using mRemoteNG.Messages;
using mRemoteNG.UI;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.App
{
    public static class Windows
    {
        private static AboutWindow _aboutForm;
        private static DockContent _aboutPanel = new DockContent();
        private static DockContent _sshtransferPanel = new DockContent();
        private static ActiveDirectoryImportWindow _adimportForm;
        private static DockContent _adimportPanel = new DockContent();
        private static HelpWindow _helpForm;
        private static DockContent _helpPanel = new DockContent();
        private static ExternalToolsWindow _externalappsForm;
        private static DockContent _externalappsPanel = new DockContent();
        private static PortScanWindow _portscanForm;
        private static DockContent _portscanPanel = new DockContent();
        private static UltraVNCWindow _ultravncscForm;
        private static DockContent _ultravncscPanel = new DockContent();
        private static ComponentsCheckWindow _componentscheckForm;
        private static DockContent _componentscheckPanel = new DockContent();

        public static ConnectionTreeWindow TreeForm { get; set; }
        public static DockContent TreePanel { get; set; } = new DockContent();
        public static ConfigWindow ConfigForm { get; set; }
        public static DockContent ConfigPanel { get; set; } = new DockContent();
        public static ErrorAndInfoWindow ErrorsForm { get; set; }
        public static DockContent ErrorsPanel { get; set; } = new DockContent();
        public static ScreenshotManagerWindow ScreenshotForm { get; set; }
        public static DockContent ScreenshotPanel { get; set; } = new DockContent();
        public static UpdateWindow UpdateForm { get; set; }
        public static DockContent UpdatePanel { get; set; } = new DockContent();
        public static SSHTransferWindow SshtransferForm { get; set; }
        
        
        public static void Show(WindowType windowType)
        {
            try
            {
                if (windowType.Equals(WindowType.About))
                {
                    if (_aboutForm == null || _aboutForm.IsDisposed)
                    {
                        _aboutForm = new AboutWindow(_aboutPanel);
                        _aboutPanel = _aboutForm;
                    }
                    _aboutForm.Show(frmMain.Default.pnlDock);
                }
                else if (windowType.Equals(WindowType.ActiveDirectoryImport))
                {
                    if (_adimportForm == null || _adimportForm.IsDisposed)
                    {
                        _adimportForm = new ActiveDirectoryImportWindow(_adimportPanel);
                        _adimportPanel = _adimportForm;
                    }
                    _adimportPanel.Show(frmMain.Default.pnlDock);
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
                    SshtransferForm = new SSHTransferWindow(_sshtransferPanel);
                    _sshtransferPanel = SshtransferForm;
                    SshtransferForm.Show(frmMain.Default.pnlDock);
                }
                else if (windowType.Equals(WindowType.Update))
                {
                    if (UpdateForm == null || UpdateForm.IsDisposed)
                    {
                        UpdateForm = new UpdateWindow(UpdatePanel);
                        UpdatePanel = UpdateForm;
                    }
                    UpdateForm.Show(frmMain.Default.pnlDock);
                }
                else if (windowType.Equals(WindowType.Help))
                {
                    if (_helpForm == null || _helpForm.IsDisposed)
                    {
                        _helpForm = new HelpWindow(_helpPanel);
                        _helpPanel = _helpForm;
                    }
                    _helpForm.Show(frmMain.Default.pnlDock);
                }
                else if (windowType.Equals(WindowType.ExternalApps))
                {
                    if (_externalappsForm == null || _externalappsForm.IsDisposed)
                    {
                        _externalappsForm = new ExternalToolsWindow(_externalappsPanel);
                        _externalappsPanel = _externalappsForm;
                    }
                    _externalappsForm.Show(frmMain.Default.pnlDock);
                }
                else if (windowType.Equals(WindowType.PortScan))
                {
                    _portscanForm = new PortScanWindow(_portscanPanel);
                    _portscanPanel = _portscanForm;
                    _portscanForm.Show(frmMain.Default.pnlDock);
                }
                else if (windowType.Equals(WindowType.UltraVNCSC))
                {
                    if (_ultravncscForm == null || _ultravncscForm.IsDisposed)
                    {
                        _ultravncscForm = new UltraVNCWindow(_ultravncscPanel);
                        _ultravncscPanel = _ultravncscForm;
                    }
                    _ultravncscForm.Show(frmMain.Default.pnlDock);
                }
                else if (windowType.Equals(WindowType.ComponentsCheck))
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Showing ComponentsCheck window", true);
                    if (_componentscheckForm == null || _componentscheckForm.IsDisposed)
                    {
                        _componentscheckForm = new ComponentsCheckWindow(_componentscheckPanel);
                        _componentscheckPanel = _componentscheckForm;
                    }
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