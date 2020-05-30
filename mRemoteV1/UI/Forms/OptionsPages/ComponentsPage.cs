using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using Gecko;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Messages;
using mRemoteNG.Themes;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public partial class ComponentsPage
    {
        public ComponentsPage()
        {
            ApplyTheme();
            PageIcon = Resources.ComponentsCheck_Icon;
            InitializeComponent();
            CheckComponents();
            FontOverrider.FontOverride(this);
            ThemeManager.getInstance().ThemeChanged += ApplyTheme;
        }

        public override string PageName
        {
            get => Language.strComponentsCheck;
            set { }
        }

        #region Form Stuff

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            Text = Language.strComponentsCheck;
            btnCheckAgain.Text = Language.strCcCheckAgain;
        }

        private void BtnCheckAgain_Click(object sender, EventArgs e)
        {
            CheckComponents();
        }

        #endregion

        private void CheckComponents()
        {
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Beginning component check", true);
            CheckRdp();
            CheckVnc();
            CheckPutty();
            CheckGeckoBrowser();
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Finished component check", true);
        }

        private void CheckRdp()
        {
            pnlCheck1.Visible = true;
            var rdpProtocolFactory = new RdpProtocolFactory();
            var supportedVersions = rdpProtocolFactory.GetSupportedVersions();

            if (supportedVersions.Any())
            {
                pbCheck1.Image = Resources.Good_Symbol;
                lblCheck1.ForeColor = Color.DarkOliveGreen;
                lblCheck1.Text = "RDP (Remote Desktop) " + Language.strCcCheckSucceeded;
                txtCheck1.Text = string.Format(Language.strCcRDPOK, string.Join(", ", supportedVersions));
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "RDP versions installed: "+ string.Join(",", supportedVersions), true);
            }
            else
            {
                pbCheck1.Image = Resources.Bad_Symbol;
                lblCheck1.ForeColor = Color.Firebrick;
                lblCheck1.Text = "RDP (Remote Desktop) " + Language.strCcCheckFailed;
                txtCheck1.Text = string.Format(Language.strCcRDPFailed, GeneralAppInfo.UrlForum);

                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                                                    "RDP " + Language.strCcNotInstalledProperly, true);
            }
        }

        private void CheckVnc()
        {
            pnlCheck2.Visible = true;

            try
            {
                using (var vnc = new VncSharp.RemoteDesktop())
                {
                    vnc.CreateControl();

                    while (!vnc.Created)
                    {
                        Thread.Sleep(10);
                        System.Windows.Forms.Application.DoEvents();
                    }

                    pbCheck2.Image = Resources.Good_Symbol;
                    lblCheck2.ForeColor = Color.DarkOliveGreen;
                    lblCheck2.Text = "VNC (Virtual Network Computing) " + Language.strCcCheckSucceeded;
                    txtCheck2.Text = string.Format(Language.strCcVNCOK, vnc.ProductVersion);
                    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "VNC installed", true);
                }
            }
            catch (Exception)
            {
                pbCheck2.Image = Resources.Bad_Symbol;
                lblCheck2.ForeColor = Color.Firebrick;
                lblCheck2.Text = "VNC (Virtual Network Computing) " + Language.strCcCheckFailed;
                txtCheck2.Text = string.Format(Language.strCcVNCFailed, GeneralAppInfo.UrlForum);

                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                                                    "VNC " + Language.strCcNotInstalledProperly, true);
            }
        }

        private void CheckPutty()
        {
            pnlCheck3.Visible = true;
            string pPath;
            if (Settings.Default.UseCustomPuttyPath == false)
            {
                pPath = GeneralAppInfo.HomePath + "\\PuTTYNG.exe";
            }
            else
            {
                pPath = Settings.Default.CustomPuttyPath;
            }

            if (File.Exists(pPath))
            {
                var versionInfo = FileVersionInfo.GetVersionInfo(pPath);

                pbCheck3.Image = Resources.Good_Symbol;
                lblCheck3.ForeColor = Color.DarkOliveGreen;
                lblCheck3.Text = "PuTTY (SSH/Telnet/Rlogin/RAW) " + Language.strCcCheckSucceeded;
                txtCheck3.Text =
                    $"{Language.strCcPuttyOK}{Environment.NewLine}Version: {versionInfo.ProductName} Release: {versionInfo.FileVersion}";
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "PuTTY installed", true);
            }
            else
            {
                pbCheck3.Image = Resources.Bad_Symbol;
                lblCheck3.ForeColor = Color.Firebrick;
                lblCheck3.Text = "PuTTY (SSH/Telnet/Rlogin/RAW) " + Language.strCcCheckFailed;
                txtCheck3.Text = Language.strCcPuttyFailed;

                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                                                    "PuTTY " + Language.strCcNotInstalledProperly, true);
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "File " + pPath + " does not exist.",
                                                    true);
            }
        }

        private void CheckGeckoBrowser()
        {
            pnlCheck5.Visible = true;
            var geckoBad = false;
            var geckoFxPath = Path.Combine(GeneralAppInfo.HomePath, "Firefox");

            if (File.Exists(Path.Combine(GeneralAppInfo.HomePath, "Geckofx-Core.dll")))
            {
                if (Directory.Exists(geckoFxPath))
                {
                    if (!File.Exists(Path.Combine(geckoFxPath, "xul.dll")))
                    {
                        geckoBad = true;
                    }
                }
                else
                {
                    geckoBad = true;
                }
            }

            if (geckoBad == false)
            {
                pbCheck5.Image = Resources.Good_Symbol;
                lblCheck5.ForeColor = Color.DarkOliveGreen;
                lblCheck5.Text = @"Gecko (Firefox) Rendering Engine (HTTP/S) " + Language.strCcCheckSucceeded;
                if (!Xpcom.IsInitialized)
                    Xpcom.Initialize("Firefox");
                txtCheck5.Text = Language.strCcGeckoOK + " Version: " + Xpcom.XulRunnerVersion;
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Gecko Browser installed", true);
            }
            else
            {
                pbCheck5.Image = Resources.Bad_Symbol;
                lblCheck5.ForeColor = Color.Firebrick;
                lblCheck5.Text = @"Gecko (Firefox) Rendering Engine (HTTP/S) " + Language.strCcCheckFailed;
                txtCheck5.Text = string.Format(Language.strCcGeckoFailed, GeneralAppInfo.UrlForum);

                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                                                    "Gecko " + Language.strCcNotInstalledProperly, true);
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    "GeckoFx was not found in " + geckoFxPath, true);
            }
        }
    }
}