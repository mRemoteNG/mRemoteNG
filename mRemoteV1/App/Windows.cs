using mRemoteNG.Forms;
using mRemoteNG.Forms.OptionsPages;
using mRemoteNG.Messages;
using System;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.App
{
    public class Windows
    {
        public static UI.Window.ConnectionTreeWindow treeForm;
        public static DockContent treePanel = new DockContent();
        public static UI.Window.ConfigWindow configForm;
        public static DockContent configPanel = new DockContent();
        public static UI.Window.ErrorAndInfoWindow errorsForm;
        public static DockContent errorsPanel = new DockContent();
        public static UI.Window.SessionsWindow sessionsForm;
        public static DockContent sessionsPanel = new DockContent();
        public static UI.Window.ScreenshotManagerWindow screenshotForm;
        public static DockContent screenshotPanel = new DockContent();
        public static ExportForm exportForm;
        public static DockContent exportPanel = new DockContent();
        public static UI.Window.AboutWindow aboutForm;
        public static DockContent aboutPanel = new DockContent();
        public static UI.Window.UpdateWindow updateForm;
        public static DockContent updatePanel = new DockContent();
        public static UI.Window.SSHTransferWindow sshtransferForm;
        public static DockContent sshtransferPanel = new DockContent();
        public static UI.Window.ActiveDirectoryImportWindow adimportForm;
        public static DockContent adimportPanel = new DockContent();
        public static UI.Window.HelpWindow helpForm;
        public static DockContent helpPanel = new DockContent();
        public static UI.Window.ExternalToolsWindow externalappsForm;
        public static DockContent externalappsPanel = new DockContent();
        public static UI.Window.PortScanWindow portscanForm;
        public static DockContent portscanPanel = new DockContent();
        public static UI.Window.UltraVNCWindow ultravncscForm;
        public static DockContent ultravncscPanel = new DockContent();
        public static UI.Window.ComponentsCheckWindow componentscheckForm;
        public static DockContent componentscheckPanel = new DockContent();
        public static UI.Window.AnnouncementWindow AnnouncementForm;
        public static DockContent AnnouncementPanel = new DockContent();

        public static void Show(UI.Window.WindowType windowType, bool portScanImport = false)
        {
            try
            {
                if (windowType.Equals(UI.Window.WindowType.About))
                {
                    if (aboutForm == null || aboutForm.IsDisposed)
                    {
                        aboutForm = new UI.Window.AboutWindow(aboutPanel);
                        aboutPanel = aboutForm;
                    }
                    aboutForm.Show(frmMain.Default.pnlDock);
                }
                else if (windowType.Equals(UI.Window.WindowType.ActiveDirectoryImport))
                {
                    if (adimportForm == null || adimportForm.IsDisposed)
                    {
                        adimportForm = new UI.Window.ActiveDirectoryImportWindow(adimportPanel);
                        adimportPanel = adimportForm;
                    }
                    adimportPanel.Show(frmMain.Default.pnlDock);
                }
                else if (windowType.Equals(UI.Window.WindowType.Options))
                {
                    using (OptionsForm optionsForm = new OptionsForm())
                    {
                        optionsForm.ShowDialog(frmMain.Default);
                    }
                }
                else if (windowType.Equals(UI.Window.WindowType.SSHTransfer))
                {
                    sshtransferForm = new UI.Window.SSHTransferWindow(sshtransferPanel);
                    sshtransferPanel = sshtransferForm;
                    sshtransferForm.Show(frmMain.Default.pnlDock);
                }
                else if (windowType.Equals(UI.Window.WindowType.Update))
                {
                    if (updateForm == null || updateForm.IsDisposed)
                    {
                        updateForm = new UI.Window.UpdateWindow(updatePanel);
                        updatePanel = updateForm;
                    }
                    updateForm.Show(frmMain.Default.pnlDock);
                }
                else if (windowType.Equals(UI.Window.WindowType.Help))
                {
                    if (helpForm == null || helpForm.IsDisposed)
                    {
                        helpForm = new UI.Window.HelpWindow(helpPanel);
                        helpPanel = helpForm;
                    }
                    helpForm.Show(frmMain.Default.pnlDock);
                }
                else if (windowType.Equals(UI.Window.WindowType.ExternalApps))
                {
                    if (externalappsForm == null || externalappsForm.IsDisposed)
                    {
                        externalappsForm = new UI.Window.ExternalToolsWindow(externalappsPanel);
                        externalappsPanel = externalappsForm;
                    }
                    externalappsForm.Show(frmMain.Default.pnlDock);
                }
                else if (windowType.Equals(UI.Window.WindowType.PortScan))
                {
                    portscanForm = new UI.Window.PortScanWindow(portscanPanel, portScanImport);
                    portscanPanel = portscanForm;
                    portscanForm.Show(frmMain.Default.pnlDock);
                }
                else if (windowType.Equals(UI.Window.WindowType.UltraVNCSC))
                {
                    if (ultravncscForm == null || ultravncscForm.IsDisposed)
                    {
                        ultravncscForm = new UI.Window.UltraVNCWindow(ultravncscPanel);
                        ultravncscPanel = ultravncscForm;
                    }
                    ultravncscForm.Show(frmMain.Default.pnlDock);
                }
                else if (windowType.Equals(UI.Window.WindowType.ComponentsCheck))
                {
                    if (componentscheckForm == null || componentscheckForm.IsDisposed)
                    {
                        componentscheckForm = new UI.Window.ComponentsCheckWindow(componentscheckPanel);
                        componentscheckPanel = componentscheckForm;
                    }
                    componentscheckForm.Show(frmMain.Default.pnlDock);
                }
                else if (windowType.Equals(UI.Window.WindowType.Announcement))
                {
                    if (AnnouncementForm == null || AnnouncementForm.IsDisposed)
                    {
                        AnnouncementForm = new UI.Window.AnnouncementWindow(AnnouncementPanel);
                        AnnouncementPanel = AnnouncementForm;
                    }
                    AnnouncementForm.Show(frmMain.Default.pnlDock);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "App.Runtime.Windows.Show() failed." + Environment.NewLine + ex.Message, true);
            }
        }

        public static void ShowUpdatesTab()
        {
            using (OptionsForm optionsForm = new OptionsForm())
            {
                optionsForm.ShowDialog(frmMain.Default, typeof(UpdatesPage));
            }

        }
    }
}