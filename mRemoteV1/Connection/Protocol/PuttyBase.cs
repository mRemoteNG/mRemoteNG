using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tools;
using mRemoteNG.Tools.Cmdline;
using mRemoteNG.UI;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.Connection.Protocol
{
    public class PuttyBase : ExternalProcessProtocolBase
    {
        private const int IDM_RECONF = 0x50; // PuTTY Settings Menu ID
        private bool _isPuttyNg;
        private readonly DisplayProperties _display = new DisplayProperties();

        #region Public Properties
        protected Putty_Protocol PuttyProtocol { private get; set; }

        protected Putty_SSHVersion PuttySSHVersion { private get; set; }

        public static string PuttyPath { get; set; }

        public bool Focused
        {
            get { return NativeMethods.GetForegroundWindow() == ProcessHandle; }
        }

        #endregion

        #region Private Events & Handlers

        private void ProcessExited(object sender, EventArgs e)
        {
            Event_Closed(this);
        }

        #endregion

        #region Public Methods

        public override bool Connect()
        {
            try
            {
                base.Connect();
                var arguments = BuildPuttyCommandLineArguments(InterfaceControl.Info);

                ProtocolProcess = new Process
                {
                    StartInfo =
                    {
                        UseShellExecute = false,
                        FileName = PuttyPath,
                        Arguments = arguments.ToString()
                    },
                    EnableRaisingEvents = true
                };

                ProtocolProcess.Exited += ProcessExited;

                ProtocolProcess.Start();
                ProtocolProcess.WaitForInputIdle(Settings.Default.MaxPuttyWaitTime * 1000);

                var startTicks = Environment.TickCount;
                while (ProcessHandle.ToInt32() == 0 &
                       Environment.TickCount < startTicks + Settings.Default.MaxPuttyWaitTime * 1000)
                {
                    if (_isPuttyNg)
                    {
                        ProcessHandle = NativeMethods.FindWindowEx(InterfaceControl.Handle, new IntPtr(0), null, null);
                    }
                    else
                    {
                        ProtocolProcess.Refresh();
                        ProcessHandle = ProtocolProcess.MainWindowHandle;
                    }

                    if (ProcessHandle.ToInt32() == 0)
                    {
                        Thread.Sleep(0);
                    }
                }

                if (!_isPuttyNg)
                {
                    NativeMethods.SetParent(ProcessHandle, InterfaceControl.Handle);
                }

                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.strPuttyStuff, true);
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                                                    string.Format(Language.strPuttyHandle, ProcessHandle), true);
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                                                    string.Format(Language.strPuttyTitle, ProtocolProcess.MainWindowTitle),
                                                    true);
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                                                    string.Format(Language.strPuttyParentHandle,
                                                                  InterfaceControl.Parent.Handle), true);

                Resize(this, new EventArgs());
                return true;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    Language.strPuttyConnectionFailed + Environment.NewLine +
                                                    ex.Message);
                return false;
            }
        }

        private CommandLineArguments BuildPuttyCommandLineArguments(AbstractConnectionRecord connectionInfo)
        {
            var arguments = new CommandLineArguments { EscapeForShell = false };

            arguments.Add("-load", connectionInfo.PuttySession);

            if (!(connectionInfo is PuttySessionInfo))
            {
                arguments.Add("-" + PuttyProtocol);

                if (PuttyProtocol == Putty_Protocol.ssh)
                {
                    var username = "";
                    var password = "";

                    if (!string.IsNullOrEmpty(connectionInfo.Username))
                    {
                        username = connectionInfo.Username;
                    }
                    else
                    {
                        // ReSharper disable once SwitchStatementMissingSomeCases
                        switch (Settings.Default.EmptyCredentials)
                        {
                            case "windows":
                                username = Environment.UserName;
                                break;
                            case "custom":
                                username = Settings.Default.DefaultUsername;
                                break;
                        }
                    }

                    if (!string.IsNullOrEmpty(connectionInfo.Password))
                    {
                        password = connectionInfo.Password;
                    }
                    else
                    {
                        if (Settings.Default.EmptyCredentials == "custom")
                        {
                            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
                            password = cryptographyProvider.Decrypt(Settings.Default.DefaultPassword,
                                                                    Runtime.EncryptionKey);
                        }
                    }

                    arguments.Add("-" + (int)PuttySSHVersion);

                    if (!Force.HasFlag(ConnectionInfo.Force.NoCredentials))
                    {
                        if (!string.IsNullOrEmpty(username))
                        {
                            arguments.Add("-l", username);
                        }

                        if (!string.IsNullOrEmpty(password))
                        {
                            arguments.Add("-pw", password);
                        }
                    }
                }

                arguments.Add("-P", connectionInfo.Port.ToString());
                arguments.Add(connectionInfo.Hostname);
            }

            _isPuttyNg = PuttyTypeDetector.GetPuttyType() == PuttyTypeDetector.PuttyType.PuttyNg;
            if (_isPuttyNg)
            {
                arguments.Add("-hwndparent", InterfaceControl.Handle.ToString());
            }

            return arguments;
        }

        public override void Focus()
        {
            //try
            //{
            //    if (NativeMethods.GetForegroundWindow() == PuttyHandle)
            //    {
            //        Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, $"Putty window already focused, ignoring focus request '{InterfaceControl.Info.Name}' (pid:{PuttyProcess.Id})");
            //        return;
            //    }

            //    NativeMethods.SetForegroundWindow(PuttyHandle);
            //    Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, $"Putty window focused '{InterfaceControl.Info.Name}' (pid:{PuttyProcess.Id})");
            //}
            //catch (Exception ex)
            //{
            //    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
            //                                        Language.strPuttyFocusFailed + Environment.NewLine + ex.Message,
            //                                        true);
            //}
            base.Focus();
        }

        public override void Resize(object sender, EventArgs e)
        {
            try
            {
                if (InterfaceControl.Size == Size.Empty)
                    return;

                if (_isPuttyNg)
                {
                    // PuTTYNG 0.70.0.1 and later doesn't have any window borders
                    NativeMethods.MoveWindow(ProcessHandle, 0, 0, InterfaceControl.Width, InterfaceControl.Height, true);
                }
                else
                {
                    var scaledFrameBorderHeight = _display.ScaleHeight(SystemInformation.FrameBorderSize.Height);
                    var scaledFrameBorderWidth = _display.ScaleWidth(SystemInformation.FrameBorderSize.Width);

                    NativeMethods.MoveWindow(ProcessHandle, -scaledFrameBorderWidth,
                                             -(SystemInformation.CaptionHeight + scaledFrameBorderHeight),
                                             InterfaceControl.Width + scaledFrameBorderWidth * 2,
                                             InterfaceControl.Height + SystemInformation.CaptionHeight +
                                             scaledFrameBorderHeight * 2,
                                             true);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    Language.strPuttyResizeFailed + Environment.NewLine + ex.Message,
                                                    true);
            }
        }

        public override void Close()
        {
            try
            {
                if (ProtocolProcess.HasExited == false)
                {
                    ProtocolProcess.Kill();
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    Language.strPuttyKillFailed + Environment.NewLine + ex.Message,
                                                    true);
            }

            try
            {
                ProtocolProcess.Dispose();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    Language.strPuttyDisposeFailed + Environment.NewLine + ex.Message,
                                                    true);
            }

            base.Close();
        }

        public void ShowSettingsDialog()
        {
            try
            {
                NativeMethods.PostMessage(ProcessHandle, NativeMethods.WM_SYSCOMMAND, (IntPtr)IDM_RECONF, IntPtr.Zero);
                Focus();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    Language.strPuttyShowSettingsDialogFailed + Environment.NewLine +
                                                    ex.Message, true);
            }
        }

        /// <summary>
        /// Sends an individual key stroke to this PuTTY session.
        /// </summary>
        public void SendKeyStroke(int keyType, int keyData)
        {
            NativeMethods.PostMessage(ProcessHandle, keyType, new IntPtr(keyData), new IntPtr(0));
        }

        #endregion

        #region Enums

        protected enum Putty_Protocol
        {
            ssh = 0,
            telnet = 1,
            rlogin = 2,
            raw = 3,
            serial = 4
        }

        protected enum Putty_SSHVersion
        {
            ssh1 = 1,
            ssh2 = 2
        }

        #endregion
    }
}