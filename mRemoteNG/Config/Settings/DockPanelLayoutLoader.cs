using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Window;
using System;
using System.IO;
using mRemoteNG.Messages;
using WeifenLuo.WinFormsUI.Docking;
using System.Runtime.Versioning;

namespace mRemoteNG.Config.Settings
{
    [SupportedOSPlatform("windows")]
    public class DockPanelLayoutLoader
    {
        private readonly FrmMain _mainForm;
        private readonly MessageCollector _messageCollector;

        public DockPanelLayoutLoader(FrmMain mainForm, MessageCollector messageCollector)
        {
            if (mainForm == null)
                throw new ArgumentNullException(nameof(mainForm));
            if (messageCollector == null)
                throw new ArgumentNullException(nameof(messageCollector));

            _mainForm = mainForm;
            _messageCollector = messageCollector;
        }

        public void LoadPanelsFromXml()
        {
            try
            {
                while (_mainForm.pnlDock.Contents.Count > 0)
                {
                    DockContent dc = (DockContent)_mainForm.pnlDock.Contents[0];
                    dc.Close();
                }

#if !PORTABLE
                string oldPath =
 Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\" + GeneralAppInfo.ProductName + "\\" + SettingsFileInfo.LayoutFileName;
#endif
                string newPath = SettingsFileInfo.SettingsPath + "\\" + SettingsFileInfo.LayoutFileName;
                if (File.Exists(newPath))
                {
                    _mainForm.pnlDock.LoadFromXml(newPath, GetContentFromPersistString);
#if !PORTABLE
				}
				else if (File.Exists(oldPath))
				{
					_mainForm.pnlDock.LoadFromXml(oldPath, GetContentFromPersistString);
#endif
                }
                else
                {
                    _mainForm.SetDefaultLayout();
                }

                // Regardless of what the persisted layout contained, force the
                // Connections panel to sit above/before the Config panel. Older
                // DockPanelSuite (3.1.1) does not allow reordering auto-hide tabs by
                // dragging, so a layout saved with the wrong order would otherwise be
                // stuck that way.
                EnforcePanelOrder();
            }
            catch (Exception ex)
            {
                _messageCollector.AddExceptionMessage("LoadPanelsFromXML failed", ex);
            }
        }

        /// <summary>
        /// Ensures the Connections panel is ordered before the Config panel, handling
        /// both possible arrangements:
        ///   1. Both are tabs inside the same pane -> reorder the tab index.
        ///   2. They are separate panes on the same dock edge (e.g. two auto-hide
        ///      tabs on the left) -> re-order the panes so Connections comes first.
        /// This is defensive and never throws, so a failure here cannot break startup.
        /// NOTE: requires verification on Windows (no WinForms/DockPanelSuite runtime
        /// available in the Linux/WSL build environment).
        /// </summary>
        private void EnforcePanelOrder()
        {
            EnforcePanelOrder(_mainForm, _messageCollector);
        }

        /// <summary>
        /// Ensures the Connections panel is always ordered before (above) the Config
        /// panel. Safe to call at any time (startup, after auto-hide toggles, etc.);
        /// it never throws.
        /// </summary>
        public static void EnforcePanelOrder(FrmMain mainForm, MessageCollector messageCollector)
        {
            try
            {
                if (mainForm == null || mainForm.pnlDock == null)
                    return;

                ConnectionTreeWindow tree = AppWindows.TreeForm;
                ConfigWindow config = AppWindows.ConfigForm;

                if (tree == null || config == null || tree.IsDisposed || config.IsDisposed)
                    return;

                DockPane treePane = tree.Pane;
                DockPane configPane = config.Pane;

                if (treePane == null || configPane == null)
                    return;

                // Case 1: Connections and Config are tabs in the same pane.
                // Make Connections the first tab.
                if (ReferenceEquals(treePane, configPane))
                {
                    if (treePane.Contents.IndexOf(tree) > treePane.Contents.IndexOf(config))
                        treePane.SetContentIndex(tree, 0);
                    return;
                }

                // Case 2: Separate panes on the same dock edge (e.g. two auto-hide tabs
                // on the left). The auto-hide strip renders tabs in the order the panes
                // appear in DockPanel.Panes, so we must make the Connections pane precede
                // the Config pane there.
                if (tree.DockState != config.DockState)
                    return;

                DockPaneCollection panes = mainForm.pnlDock.Panes;
                int treeIndex = panes.IndexOf(treePane);
                int configIndex = panes.IndexOf(configPane);

                // If Config currently appears before Connections, re-append both panes in
                // the desired order (Connections first, Config second). Re-showing a panel
                // moves its pane to the end of the collection for that dock edge, so doing
                // Tree then Config guarantees Connections ends up on top. Re-showing only
                // one panel is unreliable because the surviving pane's position depends on
                // where DockPanelSuite happened to place it.
                if (treeIndex >= 0 && configIndex >= 0 && configIndex < treeIndex)
                {
                    double treePortion = tree.AutoHidePortion;
                    double configPortion = config.AutoHidePortion;
                    DockState treeState = tree.DockState;
                    DockState configState = config.DockState;

                    tree.Show(mainForm.pnlDock, treeState);
                    tree.AutoHidePortion = treePortion;

                    config.Show(mainForm.pnlDock, configState);
                    config.AutoHidePortion = configPortion;
                }
            }
            catch (Exception ex)
            {
                messageCollector.AddExceptionMessage("EnforcePanelOrder failed", ex);
            }
        }

        private IDockContent? GetContentFromPersistString(string persistString)
        {
            // pnlLayout.xml persistence XML fix for refactoring to mRemoteNG
            if (persistString.StartsWith("mRemote."))
                persistString = persistString.Replace("mRemote.", "mRemoteNG.");

            try
            {
                if (persistString == typeof(ConfigWindow).ToString())
                    return AppWindows.ConfigForm;

                if (persistString == typeof(ConnectionTreeWindow).ToString())
                    return AppWindows.TreeForm;

                if (persistString == typeof(ErrorAndInfoWindow).ToString())
                    return AppWindows.ErrorsForm;
            }
            catch (Exception ex)
            {
                _messageCollector.AddExceptionMessage("GetContentFromPersistString failed", ex);
            }

            return null;
        }
    }
}