using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Versioning;
using System.Text;
using System.Xml;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Tools;

namespace mRemoteNG.Config.Settings
{
    [SupportedOSPlatform("windows")]
    public static class CommandSnippetsSaver
    {
        public static void Save(IEnumerable<CommandSnippet> snippets)
        {
            try
            {
                if (!Directory.Exists(SettingsFileInfo.SettingsPath))
                    Directory.CreateDirectory(SettingsFileInfo.SettingsPath);

                XmlTextWriter writer = new(
                    Path.Combine(SettingsFileInfo.SettingsPath, SettingsFileInfo.CmdSnippetsFileName),
                    Encoding.UTF8)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4
                };

                writer.WriteStartDocument();
                writer.WriteStartElement("Snippets");

                foreach (CommandSnippet snippet in snippets)
                {
                    writer.WriteStartElement("Snippet");
                    writer.WriteAttributeString("Name", snippet.Name);
                    writer.WriteAttributeString("Command", snippet.Command);
                    writer.WriteAttributeString("AutoExecute", Convert.ToString(snippet.AutoExecute));
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("SaveCommandSnippets failed", ex);
            }
        }
    }
}
