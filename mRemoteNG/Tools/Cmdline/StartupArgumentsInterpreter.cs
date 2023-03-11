using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.App.Info;
using mRemoteNG.Messages;
using mRemoteNG.Properties;
using mRemoteNG.Resources.Language;


namespace mRemoteNG.Tools.Cmdline
{
    [SupportedOSPlatform("windows")]
    public class StartupArgumentsInterpreter
    {
        private readonly MessageCollector _messageCollector;

        public StartupArgumentsInterpreter(MessageCollector messageCollector)
        {
            if (messageCollector == null)
                throw new ArgumentNullException(nameof(messageCollector));

            _messageCollector = messageCollector;
        }

        public void ParseArguments(IEnumerable<string> cmdlineArgs)
        {
            //if (!cmdlineArgs.Any()) return;
            _messageCollector.AddMessage(MessageClass.DebugMsg, "Parsing cmdline arguments");

            try
            {
                var args = new CmdArgumentsInterpreter(cmdlineArgs);

                ParseResetPositionArg(args);
                ParseResetPanelsArg(args);
                ParseResetToolbarArg(args);
                ParseNoReconnectArg(args);
                ParseCustomConnectionPathArg(args);
            }
            catch (Exception ex)
            {
                _messageCollector.AddExceptionMessage(Language.CommandLineArgsCouldNotBeParsed, ex, logOnly: false);
            }
        }

        private void ParseResetPositionArg(CmdArgumentsInterpreter args)
        {
            if (args["resetpos"] == null && args["rp"] == null && args["reset"] == null) return;
            _messageCollector.AddMessage(MessageClass.DebugMsg, "Cmdline arg: Resetting window positions.");
            Properties.App.Default.MainFormKiosk = false;
            var newWidth = 900;
            var newHeight = 600;
            var newX = Screen.PrimaryScreen.WorkingArea.Width / 2 - newWidth / 2;
            var newY = Screen.PrimaryScreen.WorkingArea.Height / 2 - newHeight / 2;
            Properties.App.Default.MainFormLocation = new Point(newX, newY);
            Properties.App.Default.MainFormSize = new Size(newWidth, newHeight);
            Properties.App.Default.MainFormState = FormWindowState.Normal;
        }

        private void ParseResetPanelsArg(CmdArgumentsInterpreter args)
        {
            if (args["resetpanels"] == null && args["rpnl"] == null && args["reset"] == null) return;
            _messageCollector.AddMessage(MessageClass.DebugMsg, "Cmdline arg: Resetting panels");
            Properties.App.Default.ResetPanels = true;
        }

        private void ParseResetToolbarArg(CmdArgumentsInterpreter args)
        {
            if (args["resettoolbar"] == null && args["rtbr"] == null && args["reset"] == null) return;
            _messageCollector.AddMessage(MessageClass.DebugMsg, "Cmdline arg: Resetting toolbar position");
            Properties.App.Default.ResetToolbars = true;
        }

        private void ParseNoReconnectArg(CmdArgumentsInterpreter args)
        {
            if (args["noreconnect"] == null && args["norc"] == null) return;
            _messageCollector.AddMessage(MessageClass.DebugMsg,
                                         "Cmdline arg: Disabling reconnection to previously connected hosts");
            Properties.OptionsAdvancedPage.Default.NoReconnect = true;
        }

        private void ParseCustomConnectionPathArg(CmdArgumentsInterpreter args)
        {
            var consParam = "";
            if (args["cons"] != null)
                consParam = "cons";
            if (args["c"] != null)
                consParam = "c";

            if (string.IsNullOrEmpty(consParam)) return;
            _messageCollector.AddMessage(MessageClass.DebugMsg, "Cmdline arg: loading connections from a custom path");
            if (File.Exists(args[consParam]) == false)
            {
                if (File.Exists(Path.Combine(GeneralAppInfo.HomePath, args[consParam])))
                {
                    Properties.OptionsBackupPage.Default.LoadConsFromCustomLocation = true;
                    Properties.OptionsBackupPage.Default.BackupLocation = Path.Combine(GeneralAppInfo.HomePath, args[consParam]);
                    return;
                }

                if (!File.Exists(Path.Combine(ConnectionsFileInfo.DefaultConnectionsPath, args[consParam]))) return;
                Properties.OptionsBackupPage.Default.LoadConsFromCustomLocation = true;
                Properties.OptionsBackupPage.Default.BackupLocation = Path.Combine(ConnectionsFileInfo.DefaultConnectionsPath, args[consParam]);
            }
            else
            {
                Properties.OptionsBackupPage.Default.LoadConsFromCustomLocation = true;
                Properties.OptionsBackupPage.Default.BackupLocation = args[consParam];
            }
        }
    }
}