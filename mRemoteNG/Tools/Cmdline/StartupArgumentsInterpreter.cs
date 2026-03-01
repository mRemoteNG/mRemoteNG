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

        /// <summary>
        /// Connection name or ID specified via --connect command-line argument.
        /// Used to auto-open a connection on startup (e.g. for desktop shortcuts).
        /// </summary>
        public static string? ConnectTo { get; private set; }

        /// <summary>
        /// Connection name or ID specified via --startup command-line argument.
        /// Opened after the main form has finished startup initialization.
        /// </summary>
        public static string? StartupConnectTo { get; private set; }

        /// <summary>
        /// Hostname (and optional port) for an ad-hoc quick connect specified via --quickconnect.
        /// Supports the same formats as the Quick Connect toolbar: "host", "host:port", "user@host:port".
        /// Opened after the main form has finished startup initialization.
        /// </summary>
        public static string? QuickConnectTo { get; private set; }

        /// <summary>
        /// Protocol to use with --quickconnect (e.g. "RDP", "SSH2", "VNC").
        /// Defaults to the user's QuickConnectProtocol setting when null.
        /// </summary>
        public static string? QuickConnectProtocol { get; private set; }

        /// <summary>
        /// When true, mRemoteNG will exit automatically after the last connection
        /// opened via --connect, --startup, or --quickconnect is closed.
        /// Set by the --exitafter command-line argument.
        /// </summary>
        public static bool ExitAfterLastConnection { get; private set; }

        public static void ResetConnectionArgs()
        {
            ConnectTo = null;
            StartupConnectTo = null;
            QuickConnectTo = null;
            QuickConnectProtocol = null;
            ExitAfterLastConnection = false;
        }

        public StartupArgumentsInterpreter(MessageCollector messageCollector)
        {
            ArgumentNullException.ThrowIfNull(messageCollector);
            _messageCollector = messageCollector;
        }

        public void ParseArguments(IEnumerable<string> cmdlineArgs)
        {
            //if (!cmdlineArgs.Any()) return;
            _messageCollector.AddMessage(MessageClass.DebugMsg, "Parsing cmdline arguments");

            try
            {
                CmdArgumentsInterpreter args = new(cmdlineArgs);

                ParseResetPositionArg(args);
                ParseResetPanelsArg(args);
                ParseResetToolbarArg(args);
                ParseNoReconnectArg(args);
                ParseCustomConnectionPathArg(args);
                ParseConnectArg(args);
                ParseStartupConnectArg(args);
                ParseQuickConnectArg(args);
                ParseExitAfterArg(args);
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
            int newWidth = 900;
            int newHeight = 600;
            var workingArea = Screen.PrimaryScreen?.WorkingArea ?? new Rectangle(0, 0, newWidth, newHeight);
            int newX = workingArea.Width / 2 - newWidth / 2;
            int newY = workingArea.Height / 2 - newHeight / 2;
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
            string consParam = "";
            if (args["cons"] != null)
                consParam = "cons";
            if (args["c"] != null)
                consParam = "c";

            if (string.IsNullOrEmpty(consParam)) return;
            _messageCollector.AddMessage(MessageClass.DebugMsg, "Cmdline arg: loading connections from a custom path");
            string? consValue = args[consParam];
            if (consValue == null) return;

            if (File.Exists(consValue) == false)
            {
                if (File.Exists(Path.Combine(GeneralAppInfo.HomePath, consValue)))
                {
                    Properties.OptionsBackupPage.Default.LoadConsFromCustomLocation = true;
                    Properties.OptionsBackupPage.Default.BackupLocation = Path.Combine(GeneralAppInfo.HomePath, consValue);
                    return;
                }

                if (!File.Exists(Path.Combine(ConnectionsFileInfo.DefaultConnectionsPath, consValue))) return;
                Properties.OptionsBackupPage.Default.LoadConsFromCustomLocation = true;
                Properties.OptionsBackupPage.Default.BackupLocation = Path.Combine(ConnectionsFileInfo.DefaultConnectionsPath, consValue);
            }
            else
            {
                Properties.OptionsBackupPage.Default.LoadConsFromCustomLocation = true;
                Properties.OptionsBackupPage.Default.BackupLocation = consValue;
            }
        }

        private void ParseConnectArg(CmdArgumentsInterpreter args)
        {
            string? connectValue = args["connect"];
            if (string.IsNullOrEmpty(connectValue))
                return;

            _messageCollector.AddMessage(MessageClass.DebugMsg, $"Cmdline arg: auto-connect to \"{connectValue}\"");
            ConnectTo = connectValue;
        }

        private void ParseStartupConnectArg(CmdArgumentsInterpreter args)
        {
            string? startupConnectValue = args["startup"];
            if (string.IsNullOrEmpty(startupConnectValue))
                return;

            _messageCollector.AddMessage(MessageClass.DebugMsg, $"Cmdline arg: startup auto-connect to \"{startupConnectValue}\"");
            StartupConnectTo = startupConnectValue;
        }

        private void ParseQuickConnectArg(CmdArgumentsInterpreter args)
        {
            string? quickConnectValue = args["quickconnect"] ?? args["qc"];
            if (string.IsNullOrEmpty(quickConnectValue))
                return;

            _messageCollector.AddMessage(MessageClass.DebugMsg, $"Cmdline arg: quick connect to \"{quickConnectValue}\"");
            QuickConnectTo = quickConnectValue;

            string? protocolValue = args["protocol"] ?? args["p"];
            if (!string.IsNullOrEmpty(protocolValue))
            {
                _messageCollector.AddMessage(MessageClass.DebugMsg, $"Cmdline arg: quick connect protocol \"{protocolValue}\"");
                QuickConnectProtocol = protocolValue;
            }
        }

        private void ParseExitAfterArg(CmdArgumentsInterpreter args)
        {
            if (args["exitafter"] == null) return;
            _messageCollector.AddMessage(MessageClass.DebugMsg, "Cmdline arg: exit after last connection closes");
            ExitAfterLastConnection = true;
        }
    }
}