using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.UI.Forms;
using System;
using System.IO;
using System.Xml;

namespace mRemoteNG.Config.Settings
{
    public class ExternalAppsLoader
    {
        private frmMain _MainForm;

        public ExternalAppsLoader(frmMain MainForm)
        {
            _MainForm = MainForm;
        }


        public void LoadExternalAppsFromXML()
        {
            string oldPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\" + (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.ProductName + "\\" + SettingsFileInfo.ExtAppsFilesName;
            string newPath = SettingsFileInfo.SettingsPath + "\\" + SettingsFileInfo.ExtAppsFilesName;
            XmlDocument xDom = new XmlDocument();
            if (File.Exists(newPath))
            {
                xDom.Load(newPath);
#if !PORTABLE
			}
			else if (File.Exists(oldPath))
			{
				xDom.Load(oldPath);
#endif
            }
            else
            {
                return;
            }

            foreach (XmlElement xEl in xDom.DocumentElement.ChildNodes)
            {
                Tools.ExternalTool extA = new Tools.ExternalTool();
                extA.DisplayName = xEl.Attributes["DisplayName"].Value;
                extA.FileName = xEl.Attributes["FileName"].Value;
                extA.Arguments = xEl.Attributes["Arguments"].Value;

                if (xEl.HasAttribute("WaitForExit"))
                {
                    extA.WaitForExit = bool.Parse(xEl.Attributes["WaitForExit"].Value);
                }

                if (xEl.HasAttribute("TryToIntegrate"))
                {
                    extA.TryIntegrate = bool.Parse(xEl.Attributes["TryToIntegrate"].Value);
                }

                Runtime.ExternalTools.Add(extA);
            }

            _MainForm.SwitchToolBarText(Convert.ToBoolean(mRemoteNG.Settings.Default.ExtAppsTBShowText));

            xDom = null;

            frmMain.Default.AddExternalToolsToToolBar();
        }
    }
}