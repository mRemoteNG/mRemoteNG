using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Messages;
using mRemoteNG.Security;
using mRemoteNG.Tools;
using mRemoteNG.UI.Forms;

namespace mRemoteNG.Config.Settings
{
    public class SettingsSaver
    {
        public static void SaveSettings()
        {
            try
            {
                var with1 = frmMain.Default;
                var windowPlacement = new WindowPlacement(frmMain.Default);
                if (with1.WindowState == FormWindowState.Minimized & windowPlacement.RestoreToMaximized)
                {
                    with1.Opacity = 0;
                    with1.WindowState = FormWindowState.Maximized;
                }

                My.Settings.Default.MainFormLocation = with1.Location;
                My.Settings.Default.MainFormSize = with1.Size;

                if (with1.WindowState != FormWindowState.Normal)
                {
                    My.Settings.Default.MainFormRestoreLocation = with1.RestoreBounds.Location;
                    My.Settings.Default.MainFormRestoreSize = with1.RestoreBounds.Size;
                }

                My.Settings.Default.MainFormState = with1.WindowState;

                My.Settings.Default.MainFormKiosk = with1.Fullscreen.Value;

                My.Settings.Default.FirstStart = false;
                My.Settings.Default.ResetPanels = false;
                My.Settings.Default.ResetToolbars = false;
                My.Settings.Default.NoReconnect = false;

                My.Settings.Default.ExtAppsTBLocation = with1.tsExternalTools.Location;
                if (with1.tsExternalTools.Parent != null)
                {
                    My.Settings.Default.ExtAppsTBParentDock = with1.tsExternalTools.Parent.Dock.ToString();
                }
                My.Settings.Default.ExtAppsTBVisible = with1.tsExternalTools.Visible;
                My.Settings.Default.ExtAppsTBShowText = with1.cMenToolbarShowText.Checked;

                My.Settings.Default.QuickyTBLocation = with1.tsQuickConnect.Location;
                if (with1.tsQuickConnect.Parent != null)
                {
                    My.Settings.Default.QuickyTBParentDock = with1.tsQuickConnect.Parent.Dock.ToString();
                }
                My.Settings.Default.QuickyTBVisible = with1.tsQuickConnect.Visible;

                My.Settings.Default.ConDefaultPassword =
                    Crypt.Encrypt(Convert.ToString(My.Settings.Default.ConDefaultPassword), GeneralAppInfo.EncryptionKey);

                My.Settings.Default.Save();

                SavePanelsToXML();
                SaveExternalAppsToXML();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                    "Saving settings failed" + Environment.NewLine + Environment.NewLine + ex.Message, false);
            }
        }

        public static void SavePanelsToXML()
        {
            try
            {
                if (Directory.Exists(SettingsFileInfo.SettingsPath) == false)
                {
                    Directory.CreateDirectory(SettingsFileInfo.SettingsPath);
                }

                frmMain.Default.pnlDock.SaveAsXml(SettingsFileInfo.SettingsPath + "\\" + SettingsFileInfo.LayoutFileName);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                    "SavePanelsToXML failed" + Environment.NewLine + Environment.NewLine + ex.Message, false);
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
                        Encoding.UTF8);
                xmlTextWriter.Formatting = Formatting.Indented;
                xmlTextWriter.Indentation = 4;

                xmlTextWriter.WriteStartDocument();
                xmlTextWriter.WriteStartElement("Apps");

                foreach (ExternalTool extA in Runtime.ExternalTools)
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
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                    "SaveExternalAppsToXML failed" + Environment.NewLine + Environment.NewLine + ex.Message, false);
            }
        }
    }
}