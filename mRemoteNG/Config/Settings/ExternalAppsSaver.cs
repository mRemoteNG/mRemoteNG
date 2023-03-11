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
    public class ExternalAppsSaver
    {
        public void Save(IEnumerable<ExternalTool> externalTools)
        {
            try
            {
                if (Directory.Exists(SettingsFileInfo.SettingsPath) == false)
                {
                    Directory.CreateDirectory(SettingsFileInfo.SettingsPath);
                }

                var xmlTextWriter =
                    new XmlTextWriter(SettingsFileInfo.SettingsPath + "\\" + SettingsFileInfo.ExtAppsFilesName,
                                      Encoding.UTF8)
                    {
                        Formatting = Formatting.Indented,
                        Indentation = 4
                    };

                xmlTextWriter.WriteStartDocument();
                xmlTextWriter.WriteStartElement("Apps");

                foreach (var extA in externalTools)
                {
                    xmlTextWriter.WriteStartElement("App");
                    xmlTextWriter.WriteAttributeString("DisplayName", "", extA.DisplayName);
                    xmlTextWriter.WriteAttributeString("FileName", "", extA.FileName);
                    xmlTextWriter.WriteAttributeString("Arguments", "", extA.Arguments);
                    xmlTextWriter.WriteAttributeString("WorkingDir", "", extA.WorkingDir);
                    xmlTextWriter.WriteAttributeString("WaitForExit", "", Convert.ToString(extA.WaitForExit));
                    xmlTextWriter.WriteAttributeString("TryToIntegrate", "", Convert.ToString(extA.TryIntegrate));
                    xmlTextWriter.WriteAttributeString("RunElevated", "", Convert.ToString(extA.RunElevated));
                    xmlTextWriter.WriteAttributeString("ShowOnToolbar", "", Convert.ToString(extA.ShowOnToolbar));
                    xmlTextWriter.WriteEndElement();
                }

                xmlTextWriter.WriteEndElement();
                xmlTextWriter.WriteEndDocument();

                xmlTextWriter.Close();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("SaveExternalAppsToXML failed", ex);
            }
        }
    }
}