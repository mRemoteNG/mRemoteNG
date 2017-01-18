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
#if !PORTABLE
            var oldPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), GeneralAppInfo.ProductName, SettingsFileInfo.ExtAppsFilesName);
#endif
            var newPath = Path.Combine(SettingsFileInfo.SettingsPath, SettingsFileInfo.ExtAppsFilesName);
            var xDom = new XmlDocument();
            if (File.Exists(newPath))
            {
                Logger.Instance.Info($"Loading External Apps from: {newPath}");
                xDom.Load(newPath);
            }
#if !PORTABLE
			else if (File.Exists(oldPath))
			{
                Logger.Instance.Info($"Loading External Apps from: {oldPath}");
                xDom.Load(oldPath);

			}
#endif
            else
            {
                Logger.Instance.Warn("Loading External Apps failed: Could not FIND file!");
                return;
            }

            if (xDom.DocumentElement == null)
            {
                Logger.Instance.Warn("Loading External Apps failed: Could not LOAD file!");
                return;
            }

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

                Logger.Instance.Info($"Adding External App: {extA.DisplayName} {extA.FileName} {extA.Arguments}");
                Runtime.ExternalTools.Add(extA);
            }

            _MainForm.SwitchToolBarText(Convert.ToBoolean(mRemoteNG.Settings.Default.ExtAppsTBShowText));

            frmMain.Default.AddExternalToolsToToolBar();
        }
    }
}