using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Window;
using System;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.App
{
    public static class Windows
    {
        public static ConnectionTreeWindow treeForm;
        public static DockContent treePanel = new DockContent();
        public static ConfigWindow configForm;
        public static DockContent configPanel = new DockContent();
        public static ErrorAndInfoWindow errorsForm;
        public static DockContent errorsPanel = new DockContent();
        public static ScreenshotManagerWindow screenshotForm;
        public static DockContent screenshotPanel = new DockContent();
        public static ExportForm exportForm;
        public static DockContent exportPanel = new DockContent();
        private static AboutWindow aboutForm;
        private static DockContent aboutPanel = new DockContent();
        public static UpdateWindow updateForm;
        public static DockContent updatePanel = new DockContent();
        public static SSHTransferWindow sshtransferForm;
        private static DockContent sshtransferPanel = new DockContent();
        private static ActiveDirectoryImportWindow adimportForm;
        private static DockContent adimportPanel = new DockContent();
        private static HelpWindow helpForm;
        private static DockContent helpPanel = new DockContent();
        private static ExternalToolsWindow externalappsForm;
        private static DockContent externalappsPanel = new DockContent();
        private static PortScanWindow portscanForm;
        private static DockContent portscanPanel = new DockContent();
        private static UltraVNCWindow ultravncscForm;
        private static DockContent ultravncscPanel = new DockContent();
        private static ComponentsCheckWindow componentscheckForm;
        private static DockContent componentscheckPanel = new DockContent();
        public static AnnouncementWindow AnnouncementForm;
        public static DockContent AnnouncementPanel = new DockContent();

        public static void Show(WindowType windowType)
        {
            try
            {
                if (windowType.Equals(WindowType.About))
                {
                    if (aboutForm == null || aboutForm.IsDisposed)
                    {
                        aboutForm = new AboutWindow(aboutPanel);
                        aboutPanel = aboutForm;
                    }
                    aboutForm.Show(frmMain.Default.pnlDock);
                }
                else if (windowType.Equals(WindowType.ActiveDirectoryImport))
                {
                    if (adimportForm == null || adimportForm.IsDisposed)
                    {
                        adimportForm = new ActiveDirectoryImportWindow(adimportPanel);
                        adimportPanel = adimportForm;
                    }
                    adimportPanel.Show(frmMain.Default.pnlDock);
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
                    sshtransferForm = new SSHTransferWindow(sshtransferPanel);
                    sshtransferPanel = sshtransferForm;
                    sshtransferForm.Show(frmMain.Default.pnlDock);
                }
                else if (windowType.Equals(WindowType.Update))
                {
                    if (updateForm == null || updateForm.IsDisposed)
                    {
                        updateForm = new UpdateWindow(updatePanel);
                        updatePanel = updateForm;
                    }
                    updateForm.Show(frmMain.Default.pnlDock);
                }
                else if (windowType.Equals(WindowType.Help))
                {
                    if (helpForm == null || helpForm.IsDisposed)
                    {
                        helpForm = new HelpWindow(helpPanel);
                        helpPanel = helpForm;
                    }
                    helpForm.Show(frmMain.Default.pnlDock);
                }
                else if (windowType.Equals(WindowType.ExternalApps))
                {
                    if (externalappsForm == null || externalappsForm.IsDisposed)
                    {
                        externalappsForm = new ExternalToolsWindow(externalappsPanel);
                        externalappsPanel = externalappsForm;
                    }
                    externalappsForm.Show(frmMain.Default.pnlDock);
                }
                else if (windowType.Equals(WindowType.PortScan))
                {
                    portscanForm = new PortScanWindow(portscanPanel);
                    portscanPanel = portscanForm;
                    portscanForm.Show(frmMain.Default.pnlDock);
                }
                else if (windowType.Equals(WindowType.UltraVNCSC))
                {
                    if (ultravncscForm == null || ultravncscForm.IsDisposed)
                    {
                        ultravncscForm = new UltraVNCWindow(ultravncscPanel);
                        ultravncscPanel = ultravncscForm;
                    }
                    ultravncscForm.Show(frmMain.Default.pnlDock);
                }
                else if (windowType.Equals(WindowType.ComponentsCheck))
                {
                    if (componentscheckForm == null || componentscheckForm.IsDisposed)
                    {
                        componentscheckForm = new ComponentsCheckWindow(componentscheckPanel);
                        componentscheckPanel = componentscheckForm;
                    }
                    componentscheckForm.Show(frmMain.Default.pnlDock);
                }
                else if (windowType.Equals(WindowType.Announcement))
                {
                    if (AnnouncementForm == null || AnnouncementForm.IsDisposed)
                    {
                        AnnouncementForm = new AnnouncementWindow(AnnouncementPanel);
                        AnnouncementPanel = AnnouncementForm;
                    }
                    AnnouncementForm.Show(frmMain.Default.pnlDock);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("App.Runtime.Windows.Show() failed.", ex);
            }
        }
    }
}