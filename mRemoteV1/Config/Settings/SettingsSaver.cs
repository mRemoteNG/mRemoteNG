using System;
using System.Windows.Forms;
using mRemoteNG.App;
using System.Xml;
using System.IO;


namespace mRemoteNG.Config.Settings
{
	public class SettingsSaver
	{
		public static void SaveSettings()
		{
			try
			{
                frmMain with_1 = frmMain.Default;
                Tools.WindowPlacement windowPlacement = new Tools.WindowPlacement(frmMain.Default);
				if (with_1.WindowState == FormWindowState.Minimized & windowPlacement.RestoreToMaximized)
				{
					with_1.Opacity = 0;
					with_1.WindowState = FormWindowState.Maximized;
				}
						
				My.Settings.Default.MainFormLocation = with_1.Location;
				My.Settings.Default.MainFormSize = with_1.Size;
						
				if (!(with_1.WindowState == FormWindowState.Normal))
				{
					My.Settings.Default.MainFormRestoreLocation = with_1.RestoreBounds.Location;
					My.Settings.Default.MainFormRestoreSize = with_1.RestoreBounds.Size;
				}
						
				My.Settings.Default.MainFormState = with_1.WindowState;
						
				My.Settings.Default.MainFormKiosk = with_1.Fullscreen.Value;
						
				My.Settings.Default.FirstStart = false;
				My.Settings.Default.ResetPanels = false;
				My.Settings.Default.ResetToolbars = false;
				My.Settings.Default.NoReconnect = false;
						
				My.Settings.Default.ExtAppsTBLocation = with_1.tsExternalTools.Location;
				if (with_1.tsExternalTools.Parent != null)
				{
					My.Settings.Default.ExtAppsTBParentDock = with_1.tsExternalTools.Parent.Dock.ToString();
				}
				My.Settings.Default.ExtAppsTBVisible = with_1.tsExternalTools.Visible;
				My.Settings.Default.ExtAppsTBShowText = with_1.cMenToolbarShowText.Checked;
						
				My.Settings.Default.QuickyTBLocation = with_1.tsQuickConnect.Location;
				if (with_1.tsQuickConnect.Parent != null)
				{
					My.Settings.Default.QuickyTBParentDock = with_1.tsQuickConnect.Parent.Dock.ToString();
				}
				My.Settings.Default.QuickyTBVisible = with_1.tsQuickConnect.Visible;
						
				My.Settings.Default.ConDefaultPassword = Security.Crypt.Encrypt(System.Convert.ToString(My.Settings.Default.ConDefaultPassword), App.Info.GeneralAppInfo.EncryptionKey);
						
				My.Settings.Default.Save();
						
				SavePanelsToXML();
				SaveExternalAppsToXML();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Saving settings failed" + Environment.NewLine + Environment.NewLine + ex.Message, false);
			}
		}
				
		public static void SavePanelsToXML()
		{
			try
			{
				if (Directory.Exists(App.Info.SettingsFileInfo.SettingsPath) == false)
				{
					Directory.CreateDirectory(App.Info.SettingsFileInfo.SettingsPath);
				}
						
				frmMain.Default.pnlDock.SaveAsXml(App.Info.SettingsFileInfo.SettingsPath + "\\" + App.Info.SettingsFileInfo.LayoutFileName);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "SavePanelsToXML failed" + Environment.NewLine + Environment.NewLine + ex.Message, false);
			}
		}
				
		public static void SaveExternalAppsToXML()
		{
			try
			{
				if (Directory.Exists(App.Info.SettingsFileInfo.SettingsPath) == false)
				{
					Directory.CreateDirectory(App.Info.SettingsFileInfo.SettingsPath);
				}
						
				XmlTextWriter xmlTextWriter = new XmlTextWriter(App.Info.SettingsFileInfo.SettingsPath + "\\" + App.Info.SettingsFileInfo.ExtAppsFilesName, System.Text.Encoding.UTF8);
				xmlTextWriter.Formatting = Formatting.Indented;
				xmlTextWriter.Indentation = 4;
						
				xmlTextWriter.WriteStartDocument();
				xmlTextWriter.WriteStartElement("Apps");
						
				foreach (Tools.ExternalTool extA in Runtime.ExternalTools)
				{
					xmlTextWriter.WriteStartElement("App");
					xmlTextWriter.WriteAttributeString("DisplayName", "", extA.DisplayName);
					xmlTextWriter.WriteAttributeString("FileName", "", extA.FileName);
					xmlTextWriter.WriteAttributeString("Arguments", "", extA.Arguments);
					xmlTextWriter.WriteAttributeString("WaitForExit", "", System.Convert.ToString(extA.WaitForExit));
					xmlTextWriter.WriteAttributeString("TryToIntegrate", "", System.Convert.ToString(extA.TryIntegrate));
					xmlTextWriter.WriteEndElement();
				}
						
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteEndDocument();
						
				xmlTextWriter.Close();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "SaveExternalAppsToXML failed" + Environment.NewLine + Environment.NewLine + ex.Message, false);
			}
		}
	}
}