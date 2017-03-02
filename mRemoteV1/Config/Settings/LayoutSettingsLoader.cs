using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Window;
using System;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.Config.Settings
{
    public class LayoutSettingsLoader
    {
        private readonly frmMain _mainForm;

        public LayoutSettingsLoader(frmMain mainForm)
        {
            _mainForm = mainForm;
        }

        public void LoadPanelsFromXml()
        {
            try
            {
                Windows.TreePanel = null;
                Windows.ConfigPanel = null;
                Windows.ErrorsPanel = null;

                while (_mainForm.pnlDock.Contents.Count > 0)
                {
                    var dc = (DockContent)_mainForm.pnlDock.Contents[0];
                    dc.Close();
                }

                CreatePanels();
#if !PORTABLE
                var oldPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\" + GeneralAppInfo.ProductName + "\\" + SettingsFileInfo.LayoutFileName;
#endif
                var newPath = SettingsFileInfo.SettingsPath + "\\" + SettingsFileInfo.LayoutFileName;
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
                    frmMain.Default.SetDefaultLayout();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("LoadPanelsFromXML failed" + Environment.NewLine + ex.Message);
            }
        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            // pnlLayout.xml persistence XML fix for refactoring to mRemoteNG
            if (persistString.StartsWith("mRemote."))
                persistString = persistString.Replace("mRemote.", "mRemoteNG.");

            try
            {
                if (persistString == typeof(ConfigWindow).ToString())
                    return Windows.ConfigPanel;

                if (persistString == typeof(ConnectionTreeWindow).ToString())
                    return Windows.TreePanel;

                if (persistString == typeof(ErrorAndInfoWindow).ToString())
                    return Windows.ErrorsPanel;

                if (persistString == typeof(ScreenshotManagerWindow).ToString())
                    return Windows.ScreenshotPanel;
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("GetContentFromPersistString failed" + Environment.NewLine + ex.Message);
            }

            return null;
        }

        private void CreatePanels()
        {
            Windows.ConfigForm = new ConfigWindow(Windows.ConfigPanel);
            Windows.ConfigPanel = Windows.ConfigForm;

            Windows.TreeForm = new ConnectionTreeWindow(Windows.TreePanel);
            Windows.TreePanel = Windows.TreeForm;

            Windows.ErrorsForm = new ErrorAndInfoWindow(Windows.ErrorsPanel);
            Windows.ErrorsPanel = Windows.ErrorsForm;

            Windows.ScreenshotForm = new ScreenshotManagerWindow(Windows.ScreenshotPanel);
            Windows.ScreenshotPanel = Windows.ScreenshotForm;

            Windows.UpdateForm = new UpdateWindow(Windows.UpdatePanel);
            Windows.UpdatePanel = Windows.UpdateForm;
        }
    }
}