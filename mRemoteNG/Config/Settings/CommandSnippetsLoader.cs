using System;
using System.IO;
using System.Runtime.Versioning;
using System.Xml;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Messages;
using mRemoteNG.Security;
using mRemoteNG.Tools;

namespace mRemoteNG.Config.Settings
{
    [SupportedOSPlatform("windows")]
    public class CommandSnippetsLoader
    {
        private readonly MessageCollector _messageCollector;

        public CommandSnippetsLoader(MessageCollector messageCollector)
        {
            _messageCollector = messageCollector ?? throw new ArgumentNullException(nameof(messageCollector));
        }

        public void LoadCommandSnippetsFromXml()
        {
            string path = Path.Combine(SettingsFileInfo.SettingsPath, SettingsFileInfo.CmdSnippetsFileName);

            if (!File.Exists(path))
            {
                _messageCollector.AddMessage(MessageClass.InformationMsg,
                    "No command snippets file found, starting with empty list.", true);
                return;
            }

            _messageCollector.AddMessage(MessageClass.InformationMsg,
                $"Loading Command Snippets from: {path}", true);

            XmlDocument? xDom = SecureXmlHelper.LoadXmlFromFile(path);
            if (xDom?.DocumentElement == null) return;

            foreach (XmlElement xEl in xDom.DocumentElement.ChildNodes)
            {
                CommandSnippet snippet = new()
                {
                    Name = xEl.Attributes["Name"]?.Value ?? string.Empty,
                    Command = xEl.Attributes["Command"]?.Value ?? string.Empty,
                };

                if (xEl.HasAttribute("AutoExecute") &&
                    bool.TryParse(xEl.Attributes["AutoExecute"]!.Value, out bool autoExecute))
                {
                    snippet.AutoExecute = autoExecute;
                }

                _messageCollector.AddMessage(MessageClass.InformationMsg,
                    $"Adding Command Snippet: {snippet.Name}", true);
                Runtime.CommandSnippetsService.Snippets.Add(snippet);
            }
        }
    }
}
