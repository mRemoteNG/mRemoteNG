using System;
using System.Threading;
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

namespace mRemoteNG.Connection.Protocol.Winbox
{
    public class ProtocolWinbox : ProtocolBase
    {
        public ProtocolWinbox() { }

        public enum Defaults
        {
            Port = 8291
        }

        private const int IDM_RECONF = 0x50; // PuTTY Settings Menu ID
        private readonly DisplayProperties _display = new DisplayProperties();

        #region Public Properties


        public IntPtr WinboxHandle { get; set; }

        private Process WinboxProcess { get; set; }

        public static string WinboxPath { get; set; }

        public bool Focused
        {
            get { return NativeMethods.GetForegroundWindow() == WinboxHandle; }
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
              
                WinboxProcess = new Process
                {
                    StartInfo =
                    {
                        UseShellExecute = false,
                        FileName = WinboxPath,
                        RedirectStandardOutput = true,
                    }
                };

                var arguments = new CommandLineArguments { EscapeForShell = false };

                var username = "";
                var password = "";
                var host = InterfaceControl.Info.Hostname;

                if (!string.IsNullOrEmpty(InterfaceControl.Info?.Username))
                {
                    username = InterfaceControl.Info.Username;
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

                if (!string.IsNullOrEmpty(InterfaceControl.Info?.Password))
                {
                    password = InterfaceControl.Info.Password;
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

                
                if (! InterfaceControl.Info.Port.Equals(Defaults.Port))
                {
                    host += ":" + InterfaceControl.Info.Port.ToString();
                }
                arguments.Add(host, username, password);

                WinboxProcess.StartInfo.Arguments = arguments.ToString();

                WinboxProcess.EnableRaisingEvents = true;
                WinboxProcess.Exited += ProcessExited;

                WinboxProcess.Start();
                WinboxProcess.WaitForInputIdle(Settings.Default.MaxPuttyWaitTime * 1000);
                while (!WinboxProcess.StandardOutput.EndOfStream)
                {
                    var line = WinboxProcess.StandardOutput.ReadLine();
                    Console.WriteLine(line);
                    if (line.Contains("startServices done"))
                    {
                        Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, "Winbox - Find connection done");
                        break;
                    } else if (line.Contains("disconnect"))
                    {
                        Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, "Winbox - Cannot Connect");
                        break;
                    }
                    
                }
                var startTicks = Environment.TickCount;
                while (WinboxHandle.ToInt32() == 0 &
                       Environment.TickCount < startTicks + Settings.Default.MaxPuttyWaitTime * 1000)
                {
                    WinboxHandle = NativeMethods.FindWindowEx(InterfaceControl.Handle, new IntPtr(0), null, null);
                    WinboxProcess.Refresh();
                    WinboxHandle = WinboxProcess.MainWindowHandle;

                    if (WinboxHandle.ToInt32() == 0)
                    {
                        Thread.Sleep(0);
                    }
                }
                NativeMethods.SetParent(WinboxHandle, InterfaceControl.Handle);

                Resize(this, new EventArgs());
                base.Connect();
                return true;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    Language.ConnectionFailed + Environment.NewLine +
                                                    ex.Message);
                return false;
            }
        }

        public override void Focus()
        {
            try
            {
                NativeMethods.SetForegroundWindow(WinboxHandle);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    Language.strPuttyFocusFailed + Environment.NewLine + ex.Message,
                                                    true);
            }
        }

        public override void Resize(object sender, EventArgs e)
        {
            try
            {
                if (InterfaceControl.Size == Size.Empty)
                    return;

               //NativeMethods.MoveWindow(WinboxHandle, 0, 0, InterfaceControl.Width, InterfaceControl.Height, true);
               var scaledFrameBorderHeight = _display.ScaleHeight(SystemInformation.FrameBorderSize.Height);
               var scaledFrameBorderWidth = _display.ScaleWidth(SystemInformation.FrameBorderSize.Width);

               NativeMethods.MoveWindow(WinboxHandle, -scaledFrameBorderWidth,
                                            -(SystemInformation.CaptionHeight + scaledFrameBorderHeight),
                                            InterfaceControl.Width + scaledFrameBorderWidth * 2,
                                            InterfaceControl.Height + SystemInformation.CaptionHeight +
                                            scaledFrameBorderHeight * 2,
                                            true);
              
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
                if (WinboxProcess.HasExited == false)
                {
                    WinboxProcess.Kill();
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    "Winbox - Kill process failed" + Environment.NewLine + ex.Message,
                                                    true);
            }

            try
            {
                WinboxProcess.Dispose();
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
        }

        #endregion

        #region Enums

        protected enum Winbox_Protocol
        {
            winbox = 0,
        }


        #endregion
    }
}
