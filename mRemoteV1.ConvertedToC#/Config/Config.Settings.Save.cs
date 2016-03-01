using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using mRemoteNG.App.Runtime;
using System.Xml;
using System.IO;

namespace mRemoteNG.Config
{
	namespace Settings
	{
		public class Save
		{
			#region "Public Methods"
			public static void Save()
			{
				try {
					var _with1 = frmMain;
					Tools.WindowPlacement windowPlacement = new Tools.WindowPlacement(frmMain);
					if (_with1.WindowState == FormWindowState.Minimized & windowPlacement.RestoreToMaximized) {
						_with1.Opacity = 0;
						_with1.WindowState = FormWindowState.Maximized;
					}

					mRemoteNG.My.Settings.MainFormLocation = _with1.Location;
					mRemoteNG.My.Settings.MainFormSize = _with1.Size;

					if (!(_with1.WindowState == FormWindowState.Normal)) {
						mRemoteNG.My.Settings.MainFormRestoreLocation = _with1.RestoreBounds.Location;
						mRemoteNG.My.Settings.MainFormRestoreSize = _with1.RestoreBounds.Size;
					}

					mRemoteNG.My.Settings.MainFormState = _with1.WindowState;

					mRemoteNG.My.Settings.MainFormKiosk = _with1.Fullscreen.Value;

					mRemoteNG.My.Settings.FirstStart = false;
					mRemoteNG.My.Settings.ResetPanels = false;
					mRemoteNG.My.Settings.ResetToolbars = false;
					mRemoteNG.My.Settings.NoReconnect = false;

					mRemoteNG.My.Settings.ExtAppsTBLocation = _with1.tsExternalTools.Location;
					if (_with1.tsExternalTools.Parent != null) {
						mRemoteNG.My.Settings.ExtAppsTBParentDock = _with1.tsExternalTools.Parent.Dock.ToString;
					}
					mRemoteNG.My.Settings.ExtAppsTBVisible = _with1.tsExternalTools.Visible;
					mRemoteNG.My.Settings.ExtAppsTBShowText = _with1.cMenToolbarShowText.Checked;

					mRemoteNG.My.Settings.QuickyTBLocation = _with1.tsQuickConnect.Location;
					if (_with1.tsQuickConnect.Parent != null) {
						mRemoteNG.My.Settings.QuickyTBParentDock = _with1.tsQuickConnect.Parent.Dock.ToString;
					}
					mRemoteNG.My.Settings.QuickyTBVisible = _with1.tsQuickConnect.Visible;

					mRemoteNG.My.Settings.ConDefaultPassword = mRemoteNG.Security.Crypt.Encrypt(mRemoteNG.My.Settings.ConDefaultPassword, mRemoteNG.App.Info.General.EncryptionKey);

					mRemoteNG.My.Settings.Save();

					SavePanelsToXML();
					SaveExternalAppsToXML();
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "Saving settings failed" + Constants.vbNewLine + Constants.vbNewLine + ex.Message, false);
				}
			}

			public static void SavePanelsToXML()
			{
				try {
					if (Directory.Exists(mRemoteNG.App.Info.Settings.SettingsPath) == false) {
						Directory.CreateDirectory(mRemoteNG.App.Info.Settings.SettingsPath);
					}

					My.MyProject.Forms.frmMain.pnlDock.SaveAsXml(mRemoteNG.App.Info.Settings.SettingsPath + "\\" + mRemoteNG.App.Info.Settings.LayoutFileName);
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "SavePanelsToXML failed" + Constants.vbNewLine + Constants.vbNewLine + ex.Message, false);
				}
			}

			public static void SaveExternalAppsToXML()
			{
				try {
					if (Directory.Exists(mRemoteNG.App.Info.Settings.SettingsPath) == false) {
						Directory.CreateDirectory(mRemoteNG.App.Info.Settings.SettingsPath);
					}

					XmlTextWriter xmlTextWriter = new XmlTextWriter(mRemoteNG.App.Info.Settings.SettingsPath + "\\" + mRemoteNG.App.Info.Settings.ExtAppsFilesName, System.Text.Encoding.UTF8);
					xmlTextWriter.Formatting = Formatting.Indented;
					xmlTextWriter.Indentation = 4;

					xmlTextWriter.WriteStartDocument();
					xmlTextWriter.WriteStartElement("Apps");

					foreach (Tools.ExternalTool extA in mRemoteNG.App.Runtime.ExternalTools) {
						xmlTextWriter.WriteStartElement("App");
						xmlTextWriter.WriteAttributeString("DisplayName", "", extA.DisplayName);
						xmlTextWriter.WriteAttributeString("FileName", "", extA.FileName);
						xmlTextWriter.WriteAttributeString("Arguments", "", extA.Arguments);
						xmlTextWriter.WriteAttributeString("WaitForExit", "", extA.WaitForExit);
						xmlTextWriter.WriteAttributeString("TryToIntegrate", "", extA.TryIntegrate);
						xmlTextWriter.WriteEndElement();
					}

					xmlTextWriter.WriteEndElement();
					xmlTextWriter.WriteEndDocument();

					xmlTextWriter.Close();
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "SaveExternalAppsToXML failed" + Constants.vbNewLine + Constants.vbNewLine + ex.Message, false);
				}
			}
			#endregion
		}
	}
}
