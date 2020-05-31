using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using AxWFICALib;
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
            btnCheck.Text = Language.strCheckNow;
        }

        private void BtnCheckAgain_Click(object sender, EventArgs e)
        {
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Beginning component check", true);
            CheckRdp();
            CheckVnc();
            CheckPutty();
            CheckIca();
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Finished component check", true);
        }

        #endregion

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

        private void CheckIca()
        {
            pnlCheck4.Visible = true;

            try
            {
                using (var ica = new AxICAClient())
                {
                    ica.Parent = this;

                    pbCheck4.Image = Resources.Good_Symbol;
                    lblCheck4.ForeColor = Color.DarkOliveGreen;
                    lblCheck4.Text = @"ICA (Citrix ICA) " + Language.strCcCheckSucceeded;
                    txtCheck4.Text = string.Format(Language.strCcICAOK, ica.Version);
                    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "ICA installed", true);
                }
            }
            catch (Exception ex)
            {
                pbCheck4.Image = Resources.Bad_Symbol;
                lblCheck4.ForeColor = Color.Firebrick;
                lblCheck4.Text = @"ICA (Citrix ICA) " + Language.strCcCheckFailed;
                txtCheck4.Text = string.Format(Language.strCcICAFailed, GeneralAppInfo.UrlForum);

                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                                                    "ICA " + Language.strCcNotInstalledProperly, true);
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, ex.Message, true);
            }
        }
    }
}