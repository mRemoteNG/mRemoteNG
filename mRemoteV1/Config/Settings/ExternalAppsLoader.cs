using System;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.UI.Forms;
using System.IO;
using System.Xml;
using mRemoteNG.Connection;
using mRemoteNG.Messages;
using mRemoteNG.Tools;
using mRemoteNG.UI.Controls;

namespace mRemoteNG.Config.Settings
{
    public class ExternalAppsLoader
    {
        private readonly MessageCollector _messageCollector;
        private readonly ExternalToolsToolStrip _externalToolsToolStrip;
        private readonly IConnectionInitiator _connectionInitiator;

        public ExternalAppsLoader(MessageCollector messageCollector, ExternalToolsToolStrip externalToolsToolStrip, IConnectionInitiator connectionInitiator)
        {
            _messageCollector = messageCollector.ThrowIfNull(nameof(messageCollector));
            _externalToolsToolStrip = externalToolsToolStrip.ThrowIfNull(nameof(externalToolsToolStrip));
            _connectionInitiator = connectionInitiator.ThrowIfNull(nameof(connectionInitiator));
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
                var extA = new ExternalTool(_connectionInitiator)
                {
                    DisplayName = xEl.Attributes["DisplayName"].Value,
                    FileName = xEl.Attributes["FileName"].Value,
                    Arguments = xEl.Attributes["Arguments"].Value
                };

                // check before, since old save files won't have this set
                if (xEl.HasAttribute("WorkingDir"))
                    extA.WorkingDir = xEl.Attributes["WorkingDir"].Value;
                if (xEl.HasAttribute("RunElevated"))
                    extA.RunElevated = bool.Parse(xEl.Attributes["RunElevated"].Value);

                if (xEl.HasAttribute("WaitForExit"))
                {
                    extA.WaitForExit = bool.Parse(xEl.Attributes["WaitForExit"].Value);
                }

                if (xEl.HasAttribute("TryToIntegrate"))
                {
                    extA.TryIntegrate = bool.Parse(xEl.Attributes["TryToIntegrate"].Value);
                }

                if (xEl.HasAttribute("ShowOnToolbar"))
                {
                    extA.ShowOnToolbar = bool.Parse(xEl.Attributes["ShowOnToolbar"].Value);
                }

                _messageCollector.AddMessage(MessageClass.InformationMsg, $"Adding External App: {extA.DisplayName} {extA.FileName} {extA.Arguments}", true);
                Runtime.ExternalToolsService.ExternalTools.Add(extA);
            }

            _externalToolsToolStrip.SwitchToolBarText(mRemoteNG.Settings.Default.ExtAppsTBShowText);
            _externalToolsToolStrip.AddExternalToolsToToolBar();
        }
    }
}