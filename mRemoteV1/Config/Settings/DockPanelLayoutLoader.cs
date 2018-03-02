using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Window;
using System;
using System.IO;
using mRemoteNG.Messages;
using mRemoteNG.Tools;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.Config.Settings
{
    public class DockPanelLayoutLoader
    {
        private readonly FrmMain _mainForm;
        private readonly MessageCollector _messageCollector;
        private readonly Windows _windows;

        public DockPanelLayoutLoader(FrmMain mainForm, MessageCollector messageCollector, Windows windows)
        {
            _mainForm = mainForm.ThrowIfNull(nameof(mainForm));
            _messageCollector = messageCollector.ThrowIfNull(nameof(messageCollector));
            _windows = windows.ThrowIfNull(nameof(windows));
        }

        public void LoadPanelsFromXml()
        {
            try
            {
                while (_mainForm.pnlDock.Contents.Count > 0)
                {
                    var dc = (DockContent)_mainForm.pnlDock.Contents[0];
                    dc.Close();
                }

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
                    _mainForm.SetDefaultLayout();
                }
            }
            catch (Exception ex)
            {
                _messageCollector.AddExceptionMessage("LoadPanelsFromXML failed", ex);
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
                    return _windows.ConfigForm;

                if (persistString == typeof(ConnectionTreeWindow).ToString())
                    return _windows.TreeForm;

                if (persistString == typeof(ErrorAndInfoWindow).ToString())
                    return _windows.ErrorsForm;

                if (persistString == typeof(ScreenshotManagerWindow).ToString())
                    return _windows.ScreenshotForm;
            }
            catch (Exception ex)
            {
                _messageCollector.AddExceptionMessage("GetContentFromPersistString failed", ex);
            }

            return null;
        }
    }
}