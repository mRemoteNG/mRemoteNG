using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Messages;
using mRemoteNG.UI.TaskDialog;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.UI
{
    [SupportedOSPlatform("windows")]
    public class DialogFactory
    {
        public static OpenFileDialog BuildLoadConnectionsDialog()
        {
            return new OpenFileDialog
            {
                Title = "",
                CheckFileExists = true,
                InitialDirectory = ConnectionsFileInfo.DefaultConnectionsPath,
                Filter = Language.FiltermRemoteXML + @"|*.xml|" + Language.FilterAll + @"|*.*"
            };
        }

        /// <summary>
        /// Creates and shows a dialog to either create a new connections file, load a different one,
        /// exit, or optionally cancel the operation.
        /// </summary>
        /// <param name="connectionFileName"></param>
        /// <param name="messageText"></param>
        /// <param name="showCancelButton"></param>
        public static void ShowLoadConnectionsFailedDialog(string connectionFileName, string messageText, bool showCancelButton)
        {
            var commandButtons = new List<string>
            {
                Language.ConfigurationCreateNew,
                Language.OpenADifferentFile,
                Language.Exit
            };

            if (showCancelButton)
                commandButtons.Add(Language._Cancel);

            var answered = false;
            while (!answered)
            {
                try
                {
                    CTaskDialog.ShowTaskDialogBox(GeneralAppInfo.ProductName, messageText, "", "", "", "", "", string.Join(" | ", commandButtons), ETaskDialogButtons.None, ESysIcons.Question, ESysIcons.Question);

                    switch (CTaskDialog.CommandButtonResult)
                    {
                        case 0: // New
                            var saveAsDialog = ConnectionsSaveAsDialog();
                            saveAsDialog.ShowDialog();
                            Runtime.ConnectionsService.NewConnectionsFile(saveAsDialog.FileName);
                            answered = true;
                            break;
                        case 1: // Load
                            Runtime.LoadConnections(true);
                            answered = true;
                            break;
                        case 2: // Exit
                            Application.Exit();
                            answered = true;
                            break;
                        case 3: // Cancel
                            answered = true;
                            break;
                    }
                }
                catch (Exception exception)
                {
                    Runtime.MessageCollector.AddExceptionMessage(string .Format(Language.ConnectionsFileCouldNotBeLoadedNew, connectionFileName), exception, MessageClass.WarningMsg);
                }
            }
        }

        /// <summary>
        /// Creates a new dialog that allows the user to select an mRemoteNG
        /// connections file path. Don't forget to dispose the dialog when you
        /// are done!
        /// </summary>
        public static SaveFileDialog ConnectionsSaveAsDialog()
        {
            return new SaveFileDialog
            {
                CheckPathExists = true,
                InitialDirectory = ConnectionsFileInfo.DefaultConnectionsPath,
                FileName = ConnectionsFileInfo.DefaultConnectionsFile,
                OverwritePrompt = true,
                Filter = Language.FiltermRemoteXML + @"|*.xml|" + Language.FilterAll + @"|*.*"
            };
        }

        public static CommonOpenFileDialog SelectFolder(string DialogWindowTitle)
        {
            return new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = DialogWindowTitle,
                EnsurePathExists = true,
                InitialDirectory = Properties.OptionsBackupPage.Default.BackupLocation != "" ? Properties.OptionsBackupPage.Default.BackupLocation : Directory.GetCurrentDirectory()
            };
        }

    }
}