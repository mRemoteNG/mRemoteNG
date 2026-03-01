using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;
using mRemoteNG.Tools;

namespace mRemoteNG.Config.Settings
{
    [SupportedOSPlatform("windows")]
    public static class ExternalAppsSaver
    {
        public static void Save(IEnumerable<ExternalTool> externalTools)
        {
            if (Properties.OptionsDBsPage.Default.UseSQLServer)
            {
                SaveToSql(externalTools);
            }
            else
            {
                SaveToXml(externalTools);
            }
        }

        private static void SaveToXml(IEnumerable<ExternalTool> externalTools)
        {
            try
            {
                string filePath = SettingsFileInfo.ExtAppsFilePath;
                string? directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                XmlTextWriter xmlTextWriter = new(filePath, Encoding.UTF8)
                    {
                        Formatting = Formatting.Indented,
                        Indentation = 4
                    };

                xmlTextWriter.WriteStartDocument();
                xmlTextWriter.WriteStartElement("Apps");

                foreach (ExternalTool extA in externalTools)
                {
                    xmlTextWriter.WriteStartElement("App");
                    xmlTextWriter.WriteAttributeString("DisplayName", "", extA.DisplayName);
                    xmlTextWriter.WriteAttributeString("FileName", "", extA.FileName);
                    xmlTextWriter.WriteAttributeString("IconPath", "", extA.IconPath);
                    xmlTextWriter.WriteAttributeString("Arguments", "", extA.Arguments);
                    xmlTextWriter.WriteAttributeString("WorkingDir", "", extA.WorkingDir);
                    xmlTextWriter.WriteAttributeString("WaitForExit", "", Convert.ToString(extA.WaitForExit));
                    xmlTextWriter.WriteAttributeString("TryToIntegrate", "", Convert.ToString(extA.TryIntegrate));
                    xmlTextWriter.WriteAttributeString("RunElevated", "", Convert.ToString(extA.RunElevated));
                    xmlTextWriter.WriteAttributeString("ShowOnToolbar", "", Convert.ToString(extA.ShowOnToolbar));
                    xmlTextWriter.WriteAttributeString("Category", "", extA.Category);
                    xmlTextWriter.WriteAttributeString("Hidden", "", Convert.ToString(extA.Hidden));
                    xmlTextWriter.WriteAttributeString("AuthType", "", extA.AuthenticationType);
                    xmlTextWriter.WriteAttributeString("AuthUsername", "", extA.AuthenticationUsername);
                    xmlTextWriter.WriteAttributeString("AuthPassword", "", ProtectValue(extA.AuthenticationPassword));
                    xmlTextWriter.WriteAttributeString("PrivateKeyFile", "", extA.PrivateKeyFile);
                    xmlTextWriter.WriteAttributeString("Passphrase", "", ProtectValue(extA.Passphrase));
                    if (extA.Hotkey != System.Windows.Forms.Keys.None)
                        xmlTextWriter.WriteAttributeString("Hotkey", "", Convert.ToString((int)extA.Hotkey, CultureInfo.InvariantCulture));
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

        private static void SaveToSql(IEnumerable<ExternalTool> externalTools)
        {
            try
            {
                if (Properties.OptionsDBsPage.Default.SQLReadOnly)
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                        "Skipping external tools save: SQL is read-only.");
                    return;
                }

                using IDatabaseConnector dbConnector = DatabaseConnectorFactory.DatabaseConnectorFromSettings();
                dbConnector.Connect();

                using DbTransaction transaction = dbConnector.DbConnection().BeginTransaction();
                try
                {
                    DbCommand cmd = dbConnector.DbCommand("DELETE FROM tblExternalTools");
                    cmd.Transaction = transaction;
                    cmd.ExecuteNonQuery();

                    foreach (ExternalTool extA in externalTools)
                    {
                        cmd = dbConnector.DbCommand(
                            "INSERT INTO tblExternalTools (DisplayName, FileName, IconPath, Arguments, WorkingDir, WaitForExit, TryIntegrate, RunElevated, ShowOnToolbar, Category, RunOnStartup, StopOnShutdown, Hotkey) " +
                            "VALUES (@DisplayName, @FileName, @IconPath, @Arguments, @WorkingDir, @WaitForExit, @TryIntegrate, @RunElevated, @ShowOnToolbar, @Category, @RunOnStartup, @StopOnShutdown, @Hotkey)");
                        cmd.Transaction = transaction;

                        AddParameter(cmd, "@DisplayName", extA.DisplayName);
                        AddParameter(cmd, "@FileName", extA.FileName);
                        AddParameter(cmd, "@IconPath", extA.IconPath);
                        AddParameter(cmd, "@Arguments", extA.Arguments);
                        AddParameter(cmd, "@WorkingDir", extA.WorkingDir);
                        AddParameter(cmd, "@WaitForExit", extA.WaitForExit);
                        AddParameter(cmd, "@TryIntegrate", extA.TryIntegrate);
                        AddParameter(cmd, "@RunElevated", extA.RunElevated);
                        AddParameter(cmd, "@ShowOnToolbar", extA.ShowOnToolbar);
                        AddParameter(cmd, "@Category", extA.Category);
                        AddParameter(cmd, "@RunOnStartup", extA.RunOnStartup);
                        AddParameter(cmd, "@StopOnShutdown", extA.StopOnShutdown);
                        AddParameter(cmd, "@Hotkey", (int)extA.Hotkey);

                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("SaveExternalAppsToSQL failed", ex);
            }
        }

        private static void AddParameter(DbCommand cmd, string name, object value)
        {
            DbParameter param = cmd.CreateParameter();
            param.ParameterName = name;
            param.Value = value;
            cmd.Parameters.Add(param);
        }

        internal static string ProtectValue(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return string.Empty;
            byte[] data = Encoding.UTF8.GetBytes(plainText);
            byte[] encrypted = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encrypted);
        }

        internal static string UnprotectValue(string protectedText)
        {
            if (string.IsNullOrEmpty(protectedText)) return string.Empty;
            try
            {
                byte[] data = Convert.FromBase64String(protectedText);
                byte[] decrypted = ProtectedData.Unprotect(data, null, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(decrypted);
            }
            catch
            {
                // Fallback: treat as plaintext (migration from unencrypted format)
                return protectedText;
            }
        }
    }
}
