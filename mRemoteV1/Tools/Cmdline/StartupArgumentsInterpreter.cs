using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using mRemoteNG.App.Info;
using mRemoteNG.Messages;


namespace mRemoteNG.Tools.Cmdline
{
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
                _messageCollector.AddExceptionMessage(Language.strCommandLineArgsCouldNotBeParsed, ex, logOnly: false);
            }
        }

        private void ParseResetPositionArg(CmdArgumentsInterpreter args)
        {
            if (args["resetpos"] == null && args["rp"] == null && args["reset"] == null) return;
            _messageCollector.AddMessage(MessageClass.DebugMsg, "Cmdline arg: Resetting window positions.");
            Settings.Default.MainFormKiosk = false;
            Settings.Default.MainFormLocation = new Point(999, 999);
            Settings.Default.MainFormSize = new Size(900, 600);
            Settings.Default.MainFormState = FormWindowState.Normal;
        }

        private void ParseResetPanelsArg(CmdArgumentsInterpreter args)
        {
            if (args["resetpanels"] == null && args["rpnl"] == null && args["reset"] == null) return;
            _messageCollector.AddMessage(MessageClass.DebugMsg, "Cmdline arg: Resetting panels");
            Settings.Default.ResetPanels = true;
        }

        private void ParseResetToolbarArg(CmdArgumentsInterpreter args)
        {
            if (args["resettoolbar"] == null && args["rtbr"] == null && args["reset"] == null) return;
            _messageCollector.AddMessage(MessageClass.DebugMsg, "Cmdline arg: Resetting toolbar position");
            Settings.Default.ResetToolbars = true;
        }

        private void ParseNoReconnectArg(CmdArgumentsInterpreter args)
        {
            if (args["noreconnect"] == null && args["norc"] == null) return;
            _messageCollector.AddMessage(MessageClass.DebugMsg, "Cmdline arg: Disabling reconnection to previously connected hosts");
            Settings.Default.NoReconnect = true;
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
                if (File.Exists(GeneralAppInfo.HomePath + "\\" + args[consParam]))
                {
                    Settings.Default.LoadConsFromCustomLocation = true;
                    Settings.Default.CustomConsPath = GeneralAppInfo.HomePath + "\\" + args[consParam];
                    return;
                }
                if (!File.Exists(ConnectionsFileInfo.DefaultConnectionsPath + "\\" + args[consParam])) return;
                Settings.Default.LoadConsFromCustomLocation = true;
                Settings.Default.CustomConsPath = ConnectionsFileInfo.DefaultConnectionsPath + "\\" + args[consParam];
            }
            else
            {
                Settings.Default.LoadConsFromCustomLocation = true;
                Settings.Default.CustomConsPath = args[consParam];
            }
        }
    }
}