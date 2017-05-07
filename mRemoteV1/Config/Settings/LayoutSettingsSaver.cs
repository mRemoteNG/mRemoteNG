using System;
using System.IO;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.UI.Forms;

namespace mRemoteNG.Config.Settings
{
    public class LayoutSettingsSaver
    {
        public void Save()
        {
            try
            {
                if (Directory.Exists(SettingsFileInfo.SettingsPath) == false)
                {
                    Directory.CreateDirectory(SettingsFileInfo.SettingsPath);
                }

                FrmMain.Default.pnlDock.SaveAsXml(SettingsFileInfo.SettingsPath + "\\" + SettingsFileInfo.LayoutFileName);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("SavePanelsToXML failed", ex);
            }
        }
    }
}