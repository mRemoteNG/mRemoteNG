using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using mRemoteNG.App.Info;
using mRemoteNG.Tools;
using mRemoteNG.UI.Controls;
using mRemoteNG.UI.Forms;
using static mRemoteNG.App.Runtime;

namespace mRemoteNG.Config.Settings
{
    public static class SettingsSaver
    {
        public static void SaveSettings(Control quickConnectToolStrip, ExternalToolsToolStrip externalToolsToolStrip)
        {
            try
            {
                var frmMain = FrmMain.Default;
                var windowPlacement = new WindowPlacement(FrmMain.Default);
                if (frmMain.WindowState == FormWindowState.Minimized & windowPlacement.RestoreToMaximized)
                {
                    frmMain.Opacity = 0;
                    frmMain.WindowState = FormWindowState.Maximized;
                }

                mRemoteNG.Settings.Default.MainFormLocation = frmMain.Location;
                mRemoteNG.Settings.Default.MainFormSize = frmMain.Size;

                if (frmMain.WindowState != FormWindowState.Normal)
                {
                    mRemoteNG.Settings.Default.MainFormRestoreLocation = frmMain.RestoreBounds.Location;
                    mRemoteNG.Settings.Default.MainFormRestoreSize = frmMain.RestoreBounds.Size;
                }

                mRemoteNG.Settings.Default.MainFormState = frmMain.WindowState;

                if (frmMain._fullscreen != null)
                {
                    mRemoteNG.Settings.Default.MainFormKiosk = frmMain._fullscreen.Value;
                }

                mRemoteNG.Settings.Default.FirstStart = false;
                mRemoteNG.Settings.Default.ResetPanels = false;
                mRemoteNG.Settings.Default.ResetToolbars = false;
                mRemoteNG.Settings.Default.NoReconnect = false;

                mRemoteNG.Settings.Default.ExtAppsTBLocation = externalToolsToolStrip.Location;
                if (externalToolsToolStrip.Parent != null)
                {
                    mRemoteNG.Settings.Default.ExtAppsTBParentDock = externalToolsToolStrip.Parent.Dock.ToString();
                }
                mRemoteNG.Settings.Default.ExtAppsTBVisible = externalToolsToolStrip.Visible;
                mRemoteNG.Settings.Default.ExtAppsTBShowText = externalToolsToolStrip.CMenToolbarShowText.Checked;

                mRemoteNG.Settings.Default.QuickyTBLocation = quickConnectToolStrip.Location;
                if (quickConnectToolStrip.Parent != null)
                {
                    mRemoteNG.Settings.Default.QuickyTBParentDock = quickConnectToolStrip.Parent.Dock.ToString();
                }
                mRemoteNG.Settings.Default.QuickyTBVisible = quickConnectToolStrip.Visible;
                mRemoteNG.Settings.Default.Save();

                SavePanelsToXML();
                SaveExternalAppsToXML();
            }
            catch (Exception ex)
            {
                MessageCollector.AddExceptionStackTrace("Saving settings failed", ex);
            }
        }

        private static void SavePanelsToXML()
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
                MessageCollector.AddExceptionStackTrace("SavePanelsToXML failed", ex);
            }
        }

        public static void SaveExternalAppsToXML()
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

                foreach (ExternalTool extA in ExternalTools)
                {
                    xmlTextWriter.WriteStartElement("App");
                    xmlTextWriter.WriteAttributeString("DisplayName", "", extA.DisplayName);
                    xmlTextWriter.WriteAttributeString("FileName", "", extA.FileName);
                    xmlTextWriter.WriteAttributeString("Arguments", "", extA.Arguments);
                    xmlTextWriter.WriteAttributeString("WaitForExit", "", Convert.ToString(extA.WaitForExit));
                    xmlTextWriter.WriteAttributeString("TryToIntegrate", "", Convert.ToString(extA.TryIntegrate));
                    xmlTextWriter.WriteEndElement();
                }

                xmlTextWriter.WriteEndElement();
                xmlTextWriter.WriteEndDocument();

                xmlTextWriter.Close();
            }
            catch (Exception ex)
            {
                MessageCollector.AddExceptionStackTrace("SaveExternalAppsToXML failed", ex);
            }
        }
    }
}
