using System;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.UI.Forms;
using System.IO;
using System.Xml;
using mRemoteNG.Messages;
using mRemoteNG.Tools;

namespace mRemoteNG.Config.Settings
{
    public class ExternalAppsLoader
    {
        private readonly frmMain _mainForm;
        private readonly MessageCollector _messageCollector;

        public ExternalAppsLoader(frmMain mainForm, MessageCollector messageCollector)
        {
            if (mainForm == null)
                throw new ArgumentNullException(nameof(mainForm));
            if (messageCollector == null)
                throw new ArgumentNullException(nameof(messageCollector));

            _mainForm = mainForm;
            _messageCollector = messageCollector;
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
                _messageCollector.AddMessage(MessageClass.InformationMsg, $"Loading External Apps from: {newPath}", true);
                xDom.Load(newPath);
            }
#if !PORTABLE
			else if (File.Exists(oldPath))
			{
                _messageCollector.AddMessage(MessageClass.InformationMsg, $"Loading External Apps from: {oldPath}", true);
                xDom.Load(oldPath);

			}
#endif
            else
            {
                _messageCollector.AddMessage(MessageClass.WarningMsg, "Loading External Apps failed: Could not FIND file!");
                return;
            }

            if (xDom.DocumentElement == null)
            {
                _messageCollector.AddMessage(MessageClass.WarningMsg, "Loading External Apps failed: Could not LOAD file!");
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

                _messageCollector.AddMessage(MessageClass.InformationMsg, $"Adding External App: {extA.DisplayName} {extA.FileName} {extA.Arguments}", true);
                Runtime.ExternalTools.Add(extA);
            }

            _mainForm.SwitchToolBarText(mRemoteNG.Settings.Default.ExtAppsTBShowText);

            frmMain.Default.AddExternalToolsToToolBar();
        }
    }
}