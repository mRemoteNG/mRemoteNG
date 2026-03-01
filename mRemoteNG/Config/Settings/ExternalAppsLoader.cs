using System;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.UI.Forms;
using System.IO;
using System.Xml;
using mRemoteNG.Messages;
using mRemoteNG.Security;
using mRemoteNG.Tools;
using mRemoteNG.UI.Controls;
using System.Runtime.Versioning;

namespace mRemoteNG.Config.Settings
{
    [SupportedOSPlatform("windows")]
    public class ExternalAppsLoader
    {
        private readonly FrmMain _mainForm;
        private readonly MessageCollector _messageCollector;
        private readonly ExternalToolsToolStrip _externalToolsToolStrip;

        public ExternalAppsLoader(FrmMain mainForm, MessageCollector messageCollector, ExternalToolsToolStrip externalToolsToolStrip)
        {
            ArgumentNullException.ThrowIfNull(mainForm);
            ArgumentNullException.ThrowIfNull(messageCollector);
            ArgumentNullException.ThrowIfNull(externalToolsToolStrip);
            _mainForm = mainForm;
            _messageCollector = messageCollector;
            _externalToolsToolStrip = externalToolsToolStrip;
        }


        public void LoadExternalAppsFromXML()
        {
            string resolvedPath = SettingsFileInfo.ExtAppsFilePath;
            bool hasCustomPath = !string.IsNullOrWhiteSpace(Properties.Settings.Default.CustomExtAppsFilePath?.Trim());
#if !PORTABLE
            string oldPath =
 Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), GeneralAppInfo.ProductName, SettingsFileInfo.ExtAppsFilesName);
#endif
            XmlDocument? xDom = null;
            bool fallbackToBuiltInShellPresets = false;

            if (File.Exists(resolvedPath))
            {
                _messageCollector.AddMessage(MessageClass.InformationMsg, $"Loading External Apps from: {resolvedPath}",
                                             true);
                xDom = SecureXmlHelper.LoadXmlFromFile(resolvedPath);
            }
#if !PORTABLE
            else if (!hasCustomPath && File.Exists(oldPath))
            {
                _messageCollector.AddMessage(MessageClass.InformationMsg, $"Loading External Apps from: {oldPath}", true);
                xDom = SecureXmlHelper.LoadXmlFromFile(oldPath);
            }
#endif
            else
            {
                _messageCollector.AddMessage(MessageClass.WarningMsg, "Loading External Apps failed: Could not FIND file! Falling back to built-in shell presets.");
                fallbackToBuiltInShellPresets = true;
            }

            if (xDom?.DocumentElement != null)
            {
                foreach (XmlElement xEl in xDom.DocumentElement.ChildNodes)
                {
                    ExternalTool extA = new()
                    {
                        DisplayName = xEl.Attributes["DisplayName"]?.Value ?? string.Empty,
                        FileName = xEl.Attributes["FileName"]?.Value ?? string.Empty,
                        IconPath = xEl.Attributes["IconPath"]?.Value ?? string.Empty,
                        Arguments = xEl.Attributes["Arguments"]?.Value ?? string.Empty
                    };

                    // check before, since old save files won't have this set
                    if (xEl.HasAttribute("WorkingDir"))
                        extA.WorkingDir = xEl.Attributes["WorkingDir"]?.Value ?? string.Empty;
                    if (xEl.HasAttribute("RunElevated"))
                        extA.RunElevated = bool.Parse(xEl.Attributes["RunElevated"]!.Value);

                    if (xEl.HasAttribute("WaitForExit"))
                    {
                        extA.WaitForExit = bool.Parse(xEl.Attributes["WaitForExit"]!.Value);
                    }

                    if (xEl.HasAttribute("TryToIntegrate"))
                    {
                        extA.TryIntegrate = bool.Parse(xEl.Attributes["TryToIntegrate"]!.Value);
                    }

                    if (xEl.HasAttribute("ShowOnToolbar"))
                    {
                        extA.ShowOnToolbar = bool.Parse(xEl.Attributes["ShowOnToolbar"]!.Value);
                    }

                    if (xEl.HasAttribute("Category"))
                        extA.Category = xEl.Attributes["Category"]?.Value ?? string.Empty;
                    if (xEl.HasAttribute("Hidden"))
                        extA.Hidden = bool.Parse(xEl.Attributes["Hidden"]!.Value);
                    if (xEl.HasAttribute("AuthType"))
                        extA.AuthenticationType = xEl.Attributes["AuthType"]?.Value ?? string.Empty;
                    if (xEl.HasAttribute("AuthUsername"))
                        extA.AuthenticationUsername = xEl.Attributes["AuthUsername"]?.Value ?? string.Empty;
                    if (xEl.HasAttribute("AuthPassword"))
                        extA.AuthenticationPassword = ExternalAppsSaver.UnprotectValue(xEl.Attributes["AuthPassword"]?.Value ?? string.Empty);
                    if (xEl.HasAttribute("PrivateKeyFile"))
                        extA.PrivateKeyFile = xEl.Attributes["PrivateKeyFile"]?.Value ?? string.Empty;
                    if (xEl.HasAttribute("Passphrase"))
                        extA.Passphrase = ExternalAppsSaver.UnprotectValue(xEl.Attributes["Passphrase"]?.Value ?? string.Empty);

                    if (xEl.HasAttribute("Hotkey") && int.TryParse(xEl.Attributes["Hotkey"]!.Value, out int hotkeyValue))
                        extA.Hotkey = (System.Windows.Forms.Keys)hotkeyValue;

                    _messageCollector.AddMessage(MessageClass.InformationMsg,
                                                 $"Adding External App: {extA.DisplayName} {extA.FileName} {extA.Arguments}",
                                                 true);
                    Runtime.ExternalToolsService.ExternalTools.Add(extA);
                }
            }
            else
            {
                if (!fallbackToBuiltInShellPresets)
                {
                    _messageCollector.AddMessage(MessageClass.WarningMsg, "Loading External Apps failed: Could not LOAD file! Falling back to built-in shell presets.");
                }
                AddBuiltInShellPresetIfMissing("cmd.exe", "%ComSpec%");
                AddBuiltInShellPresetIfMissing("pwsh.exe", "pwsh.exe");
                AddBuiltInShellPresetIfMissing("wsl.exe", @"%windir%\system32\wsl.exe");
                AddBuiltInShellPresetIfMissing("Ping", "ping.exe", "-t %HOSTNAME%");
                AddBuiltInShellPresetIfMissing("Traceroute", "tracert.exe", "%HOSTNAME%");
            }

            _externalToolsToolStrip.SwitchToolBarText(Properties.Settings.Default.ExtAppsTBShowText);
            _externalToolsToolStrip.AddExternalToolsToToolBar();
        }

        private void AddBuiltInShellPresetIfMissing(string displayName, string fileName, string arguments = "", bool tryIntegrate = true)
        {
            foreach (ExternalTool existingTool in Runtime.ExternalToolsService.ExternalTools)
            {
                if (string.Equals(existingTool.DisplayName, displayName, StringComparison.OrdinalIgnoreCase))
                    return;
            }

            ExternalTool shellPreset = new()
            {
                DisplayName = displayName,
                FileName = fileName,
                Arguments = arguments,
                TryIntegrate = tryIntegrate,
                ShowOnToolbar = false
            };

            Runtime.ExternalToolsService.ExternalTools.Add(shellPreset);
            _messageCollector.AddMessage(MessageClass.InformationMsg,
                                         $"Adding built-in shell preset: {shellPreset.DisplayName} {shellPreset.FileName}",
                                         true);
        }
    }
}