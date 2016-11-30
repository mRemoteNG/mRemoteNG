using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.UI.Forms;
using System;
using System.IO;
using System.Xml;
using mRemoteNG.Tools;

namespace mRemoteNG.Config.Settings
{
    public class ExternalAppsLoader
    {
        private readonly frmMain _MainForm;

        public ExternalAppsLoader(frmMain MainForm)
        {
            _MainForm = MainForm;
        }


        public void LoadExternalAppsFromXML()
        {
            var oldPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\" + GeneralAppInfo.ProductName + "\\" + SettingsFileInfo.ExtAppsFilesName;
            var newPath = SettingsFileInfo.SettingsPath + "\\" + SettingsFileInfo.ExtAppsFilesName;
            var xDom = new XmlDocument();
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

            if (xDom.DocumentElement != null)
                foreach (XmlElement xEl in xDom.DocumentElement.ChildNodes)
                {
                    var extA = new ExternalTool
                    {
                        DisplayName = xEl.Attributes["DisplayName"].Value,
                        FileName = xEl.Attributes["FileName"].Value,
                        Arguments = xEl.Attributes["Arguments"].Value
                    };

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

            frmMain.Default.AddExternalToolsToToolBar();
        }
    }
}