using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using mRemoteNG.App.Info;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tools;
using mRemoteNG.UI.Forms;
using static mRemoteNG.App.Runtime;

namespace mRemoteNG.Config.Settings
{
    public static class SettingsSaver
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

                mRemoteNG.Settings.Default.MainFormLocation = with1.Location;
                mRemoteNG.Settings.Default.MainFormSize = with1.Size;

                if (with1.WindowState != FormWindowState.Normal)
                {
                    mRemoteNG.Settings.Default.MainFormRestoreLocation = with1.RestoreBounds.Location;
                    mRemoteNG.Settings.Default.MainFormRestoreSize = with1.RestoreBounds.Size;
                }

                mRemoteNG.Settings.Default.MainFormState = with1.WindowState;

                if (with1._fullscreen != null)
                {
                    mRemoteNG.Settings.Default.MainFormKiosk = with1._fullscreen.Value;
                }

                mRemoteNG.Settings.Default.FirstStart = false;
                mRemoteNG.Settings.Default.ResetPanels = false;
                mRemoteNG.Settings.Default.ResetToolbars = false;
                mRemoteNG.Settings.Default.NoReconnect = false;

                mRemoteNG.Settings.Default.ExtAppsTBLocation = with1.tsExternalTools.Location;
                if (with1.tsExternalTools.Parent != null)
                {
                    mRemoteNG.Settings.Default.ExtAppsTBParentDock = with1.tsExternalTools.Parent.Dock.ToString();
                }
                mRemoteNG.Settings.Default.ExtAppsTBVisible = with1.tsExternalTools.Visible;
                mRemoteNG.Settings.Default.ExtAppsTBShowText = with1.cMenToolbarShowText.Checked;

                mRemoteNG.Settings.Default.QuickyTBLocation = with1.tsQuickConnect.Location;
                if (with1.tsQuickConnect.Parent != null)
                {
                    mRemoteNG.Settings.Default.QuickyTBParentDock = with1.tsQuickConnect.Parent.Dock.ToString();
                }
                mRemoteNG.Settings.Default.QuickyTBVisible = with1.tsQuickConnect.Visible;

                var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
                mRemoteNG.Settings.Default.ConDefaultPassword =
                    cryptographyProvider.Encrypt(Convert.ToString(mRemoteNG.Settings.Default.ConDefaultPassword), EncryptionKey);

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

                frmMain.Default.pnlDock.SaveAsXml(SettingsFileInfo.SettingsPath + "\\" + SettingsFileInfo.LayoutFileName);
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