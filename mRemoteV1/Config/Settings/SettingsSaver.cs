using System;
using System.IO;
using System.Security;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Messages;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
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

                mRemoteNG.Settings.Default.MainFormLocation = with1.Location;
                mRemoteNG.Settings.Default.MainFormSize = with1.Size;

                if (with1.WindowState != FormWindowState.Normal)
                {
                    mRemoteNG.Settings.Default.MainFormRestoreLocation = with1.RestoreBounds.Location;
                    mRemoteNG.Settings.Default.MainFormRestoreSize = with1.RestoreBounds.Size;
                }

                mRemoteNG.Settings.Default.MainFormState = with1.WindowState;

                if (with1.Fullscreen != null)
                {
                    mRemoteNG.Settings.Default.MainFormKiosk = with1.Fullscreen.Value;
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

                SecureString _password = null;
                // Determine which crypto provider we should use based on the settings
                ICryptographyProvider cryptoProvider = null;

                switch ((CryptoProviders)mRemoteNG.Settings.Default.CryptoProvider)
                {
                    case CryptoProviders.Rijndael:
                        _password = GeneralAppInfo.EncryptionKey;
                        cryptoProvider = new CryptographyProviderFactory().CreateLegacyRijndaelCryptographyProvider();
                        break;
                    case CryptoProviders.AEAD:
                        _password = GeneralAppInfo.StrongEncryptionKey;
                        cryptoProvider = new CryptographyProviderFactory()
                            .CreateAeadCryptographyProvider((BlockCipherEngines)mRemoteNG.Settings.Default.CryptoBlockCipherEngine,
                            (BlockCipherModes)mRemoteNG.Settings.Default.CryptoBlockCipherMode);
                        break;
                    default:
                        _password = GeneralAppInfo.StrongEncryptionKey;
                        cryptoProvider = new CryptographyProviderFactory()
                            .CreateAeadCryptographyProvider((BlockCipherEngines)mRemoteNG.Settings.Default.CryptoBlockCipherEngine,
                            (BlockCipherModes)mRemoteNG.Settings.Default.CryptoBlockCipherMode);
                        break;
                }

                mRemoteNG.Settings.Default.ConDefaultPassword =
                    cryptoProvider.Encrypt(Convert.ToString(mRemoteNG.Settings.Default.ConDefaultPassword), _password);

                mRemoteNG.Settings.Default.Save();

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