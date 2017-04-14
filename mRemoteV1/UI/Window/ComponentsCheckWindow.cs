using System;
using System.Diagnostics;
using System.Drawing;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;
using mRemoteNG.App;
using System.Threading;
using AxMSTSCLib;
using AxWFICALib;
using Gecko;
using mRemoteNG.App.Info;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Messages;

namespace mRemoteNG.UI.Window
{
    public class ComponentsCheckWindow : BaseWindow
    {
        #region Form Stuff
        private System.Windows.Forms.PictureBox pbCheck1;
        private System.Windows.Forms.Label lblCheck1;
        private System.Windows.Forms.Panel pnlCheck2;
        private System.Windows.Forms.Label lblCheck2;
        private System.Windows.Forms.PictureBox pbCheck2;
        private System.Windows.Forms.Panel pnlCheck3;
        private System.Windows.Forms.Label lblCheck3;
        private System.Windows.Forms.PictureBox pbCheck3;
        private System.Windows.Forms.Panel pnlCheck4;
        private System.Windows.Forms.Label lblCheck4;
        private System.Windows.Forms.PictureBox pbCheck4;
        private System.Windows.Forms.Panel pnlCheck5;
        private System.Windows.Forms.Label lblCheck5;
        private System.Windows.Forms.PictureBox pbCheck5;
        private System.Windows.Forms.Button btnCheckAgain;
        private System.Windows.Forms.TextBox txtCheck1;
        private System.Windows.Forms.TextBox txtCheck2;
        private System.Windows.Forms.TextBox txtCheck3;
        private System.Windows.Forms.TextBox txtCheck4;
        private System.Windows.Forms.TextBox txtCheck5;
        private System.Windows.Forms.CheckBox chkAlwaysShow;
        private System.Windows.Forms.Panel pnlChecks;
        private System.Windows.Forms.Panel pnlCheck1;

        private void InitializeComponent()
        {
            pnlCheck1 = new System.Windows.Forms.Panel();
            Load += new EventHandler(ComponentsCheck_Load);
            txtCheck1 = new System.Windows.Forms.TextBox();
            lblCheck1 = new System.Windows.Forms.Label();
            pbCheck1 = new System.Windows.Forms.PictureBox();
            pnlCheck2 = new System.Windows.Forms.Panel();
            txtCheck2 = new System.Windows.Forms.TextBox();
            lblCheck2 = new System.Windows.Forms.Label();
            pbCheck2 = new System.Windows.Forms.PictureBox();
            pnlCheck3 = new System.Windows.Forms.Panel();
            txtCheck3 = new System.Windows.Forms.TextBox();
            lblCheck3 = new System.Windows.Forms.Label();
            pbCheck3 = new System.Windows.Forms.PictureBox();
            pnlCheck4 = new System.Windows.Forms.Panel();
            txtCheck4 = new System.Windows.Forms.TextBox();
            lblCheck4 = new System.Windows.Forms.Label();
            pbCheck4 = new System.Windows.Forms.PictureBox();
            pnlCheck5 = new System.Windows.Forms.Panel();
            txtCheck5 = new System.Windows.Forms.TextBox();
            lblCheck5 = new System.Windows.Forms.Label();
            pbCheck5 = new System.Windows.Forms.PictureBox();
            btnCheckAgain = new System.Windows.Forms.Button();
            btnCheckAgain.Click += new EventHandler(btnCheckAgain_Click);
            chkAlwaysShow = new System.Windows.Forms.CheckBox();
            chkAlwaysShow.CheckedChanged += new EventHandler(chkAlwaysShow_CheckedChanged);
            pnlChecks = new System.Windows.Forms.Panel();
            pnlCheck1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) pbCheck1).BeginInit();
            pnlCheck2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) pbCheck2).BeginInit();
            pnlCheck3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) pbCheck3).BeginInit();
            pnlCheck4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) pbCheck4).BeginInit();
            pnlCheck5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) pbCheck5).BeginInit();
            pnlChecks.SuspendLayout();
            SuspendLayout();
            //
            //pnlCheck1
            //
            pnlCheck1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left
                               | System.Windows.Forms.AnchorStyles.Right;
            pnlCheck1.Controls.Add(txtCheck1);
            pnlCheck1.Controls.Add(lblCheck1);
            pnlCheck1.Controls.Add(pbCheck1);
            pnlCheck1.Location = new Point(3, 3);
            pnlCheck1.Name = "pnlCheck1";
            pnlCheck1.Size = new Size(562, 130);
            pnlCheck1.TabIndex = 10;
            pnlCheck1.Visible = false;
            //
            //txtCheck1
            //
            txtCheck1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom
                               | System.Windows.Forms.AnchorStyles.Left
                               | System.Windows.Forms.AnchorStyles.Right;
            txtCheck1.BackColor = SystemColors.Control;
            txtCheck1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtCheck1.Location = new Point(129, 29);
            txtCheck1.Multiline = true;
            txtCheck1.Name = "txtCheck1";
            txtCheck1.ReadOnly = true;
            txtCheck1.Size = new Size(430, 97);
            txtCheck1.TabIndex = 2;
            //
            //lblCheck1
            //
            lblCheck1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left
                               | System.Windows.Forms.AnchorStyles.Right;
            lblCheck1.Font = new Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point, Convert.ToByte(0));
            lblCheck1.ForeColor = SystemColors.ControlText;
            lblCheck1.Location = new Point(108, 3);
            lblCheck1.Name = "lblCheck1";
            lblCheck1.Size = new Size(451, 23);
            lblCheck1.TabIndex = 1;
            lblCheck1.Text = "RDP check succeeded!";
            //
            //pbCheck1
            //
            pbCheck1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom
                              | System.Windows.Forms.AnchorStyles.Left;
            pbCheck1.Location = new Point(3, 3);
            pbCheck1.Name = "pbCheck1";
            pbCheck1.Size = new Size(72, 123);
            pbCheck1.TabIndex = 0;
            pbCheck1.TabStop = false;
            //
            //pnlCheck2
            //
            pnlCheck2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left
                               | System.Windows.Forms.AnchorStyles.Right;
            pnlCheck2.Controls.Add(txtCheck2);
            pnlCheck2.Controls.Add(lblCheck2);
            pnlCheck2.Controls.Add(pbCheck2);
            pnlCheck2.Location = new Point(3, 139);
            pnlCheck2.Name = "pnlCheck2";
            pnlCheck2.Size = new Size(562, 130);
            pnlCheck2.TabIndex = 20;
            pnlCheck2.Visible = false;
            //
            //txtCheck2
            //
            txtCheck2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom
                               | System.Windows.Forms.AnchorStyles.Left
                               | System.Windows.Forms.AnchorStyles.Right;
            txtCheck2.BackColor = SystemColors.Control;
            txtCheck2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtCheck2.Location = new Point(129, 29);
            txtCheck2.Multiline = true;
            txtCheck2.Name = "txtCheck2";
            txtCheck2.ReadOnly = true;
            txtCheck2.Size = new Size(430, 97);
            txtCheck2.TabIndex = 2;
            //
            //lblCheck2
            //
            lblCheck2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left
                               | System.Windows.Forms.AnchorStyles.Right;
            lblCheck2.Font = new Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point, Convert.ToByte(0));
            lblCheck2.Location = new Point(112, 3);
            lblCheck2.Name = "lblCheck2";
            lblCheck2.Size = new Size(447, 23);
            lblCheck2.TabIndex = 1;
            lblCheck2.Text = "RDP check succeeded!";
            //
            //pbCheck2
            //
            pbCheck2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom
                              | System.Windows.Forms.AnchorStyles.Left;
            pbCheck2.Location = new Point(3, 3);
            pbCheck2.Name = "pbCheck2";
            pbCheck2.Size = new Size(72, 123);
            pbCheck2.TabIndex = 0;
            pbCheck2.TabStop = false;
            //
            //pnlCheck3
            //
            pnlCheck3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left
                               | System.Windows.Forms.AnchorStyles.Right;
            pnlCheck3.Controls.Add(txtCheck3);
            pnlCheck3.Controls.Add(lblCheck3);
            pnlCheck3.Controls.Add(pbCheck3);
            pnlCheck3.Location = new Point(3, 275);
            pnlCheck3.Name = "pnlCheck3";
            pnlCheck3.Size = new Size(562, 130);
            pnlCheck3.TabIndex = 30;
            pnlCheck3.Visible = false;
            //
            //txtCheck3
            //
            txtCheck3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom
                               | System.Windows.Forms.AnchorStyles.Left
                               | System.Windows.Forms.AnchorStyles.Right;
            txtCheck3.BackColor = SystemColors.Control;
            txtCheck3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtCheck3.Location = new Point(129, 29);
            txtCheck3.Multiline = true;
            txtCheck3.Name = "txtCheck3";
            txtCheck3.ReadOnly = true;
            txtCheck3.Size = new Size(430, 97);
            txtCheck3.TabIndex = 2;
            //
            //lblCheck3
            //
            lblCheck3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left
                               | System.Windows.Forms.AnchorStyles.Right;
            lblCheck3.Font = new Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point, Convert.ToByte(0));
            lblCheck3.Location = new Point(112, 3);
            lblCheck3.Name = "lblCheck3";
            lblCheck3.Size = new Size(447, 23);
            lblCheck3.TabIndex = 1;
            lblCheck3.Text = "RDP check succeeded!";
            //
            //pbCheck3
            //
            pbCheck3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom
                              | System.Windows.Forms.AnchorStyles.Left;
            pbCheck3.Location = new Point(3, 3);
            pbCheck3.Name = "pbCheck3";
            pbCheck3.Size = new Size(72, 123);
            pbCheck3.TabIndex = 0;
            pbCheck3.TabStop = false;
            //
            //pnlCheck4
            //
            pnlCheck4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left
                               | System.Windows.Forms.AnchorStyles.Right;
            pnlCheck4.Controls.Add(txtCheck4);
            pnlCheck4.Controls.Add(lblCheck4);
            pnlCheck4.Controls.Add(pbCheck4);
            pnlCheck4.Location = new Point(3, 411);
            pnlCheck4.Name = "pnlCheck4";
            pnlCheck4.Size = new Size(562, 130);
            pnlCheck4.TabIndex = 40;
            pnlCheck4.Visible = false;
            //
            //txtCheck4
            //
            txtCheck4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom
                               | System.Windows.Forms.AnchorStyles.Left
                               | System.Windows.Forms.AnchorStyles.Right;
            txtCheck4.BackColor = SystemColors.Control;
            txtCheck4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtCheck4.Location = new Point(129, 30);
            txtCheck4.Multiline = true;
            txtCheck4.Name = "txtCheck4";
            txtCheck4.ReadOnly = true;
            txtCheck4.Size = new Size(430, 97);
            txtCheck4.TabIndex = 2;
            //
            //lblCheck4
            //
            lblCheck4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left
                               | System.Windows.Forms.AnchorStyles.Right;
            lblCheck4.Font = new Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point, Convert.ToByte(0));
            lblCheck4.Location = new Point(112, 3);
            lblCheck4.Name = "lblCheck4";
            lblCheck4.Size = new Size(447, 23);
            lblCheck4.TabIndex = 1;
            lblCheck4.Text = "RDP check succeeded!";
            //
            //pbCheck4
            //
            pbCheck4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom
                              | System.Windows.Forms.AnchorStyles.Left;
            pbCheck4.Location = new Point(3, 3);
            pbCheck4.Name = "pbCheck4";
            pbCheck4.Size = new Size(72, 123);
            pbCheck4.TabIndex = 0;
            pbCheck4.TabStop = false;
            //
            //pnlCheck5
            //
            pnlCheck5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left
                               | System.Windows.Forms.AnchorStyles.Right;
            pnlCheck5.Controls.Add(txtCheck5);
            pnlCheck5.Controls.Add(lblCheck5);
            pnlCheck5.Controls.Add(pbCheck5);
            pnlCheck5.Location = new Point(3, 547);
            pnlCheck5.Name = "pnlCheck5";
            pnlCheck5.Size = new Size(562, 130);
            pnlCheck5.TabIndex = 50;
            pnlCheck5.Visible = false;
            //
            //txtCheck5
            //
            txtCheck5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom
                               | System.Windows.Forms.AnchorStyles.Left
                               | System.Windows.Forms.AnchorStyles.Right;
            txtCheck5.BackColor = SystemColors.Control;
            txtCheck5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtCheck5.Location = new Point(129, 29);
            txtCheck5.Multiline = true;
            txtCheck5.Name = "txtCheck5";
            txtCheck5.ReadOnly = true;
            txtCheck5.Size = new Size(430, 97);
            txtCheck5.TabIndex = 2;
            //
            //lblCheck5
            //
            lblCheck5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left
                               | System.Windows.Forms.AnchorStyles.Right;
            lblCheck5.Font = new Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point, Convert.ToByte(0));
            lblCheck5.Location = new Point(112, 3);
            lblCheck5.Name = "lblCheck5";
            lblCheck5.Size = new Size(447, 23);
            lblCheck5.TabIndex = 1;
            lblCheck5.Text = "RDP check succeeded!";
            //
            //pbCheck5
            //
            pbCheck5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom
                              | System.Windows.Forms.AnchorStyles.Left;
            pbCheck5.Location = new Point(3, 3);
            pbCheck5.Name = "pbCheck5";
            pbCheck5.Size = new Size(72, 123);
            pbCheck5.TabIndex = 0;
            pbCheck5.TabStop = false;
            //
            //btnCheckAgain
            //
            btnCheckAgain.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCheckAgain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCheckAgain.Location = new Point(476, 842);
            btnCheckAgain.Name = "btnCheckAgain";
            btnCheckAgain.Size = new Size(104, 23);
            btnCheckAgain.TabIndex = 0;
            btnCheckAgain.Text = "Check again";
            btnCheckAgain.UseVisualStyleBackColor = true;
            //
            //chkAlwaysShow
            //
            chkAlwaysShow.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            chkAlwaysShow.AutoSize = true;
            chkAlwaysShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            chkAlwaysShow.Location = new Point(12, 846);
            chkAlwaysShow.Name = "chkAlwaysShow";
            chkAlwaysShow.Size = new Size(185, 17);
            chkAlwaysShow.TabIndex = 51;
            chkAlwaysShow.Text = "Always show this screen at startup";
            chkAlwaysShow.UseVisualStyleBackColor = true;
            //
            //pnlChecks
            //
            pnlChecks.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom
                               | System.Windows.Forms.AnchorStyles.Left
                               | System.Windows.Forms.AnchorStyles.Right;
            pnlChecks.AutoScroll = true;
            pnlChecks.Controls.Add(pnlCheck1);
            pnlChecks.Controls.Add(pnlCheck2);
            pnlChecks.Controls.Add(pnlCheck3);
            pnlChecks.Controls.Add(pnlCheck5);
            pnlChecks.Controls.Add(pnlCheck4);
            pnlChecks.Location = new Point(12, 12);
            pnlChecks.Name = "pnlChecks";
            pnlChecks.Size = new Size(568, 824);
            pnlChecks.TabIndex = 52;
            //
            //ComponentsCheck
            //
            ClientSize = new Size(592, 877);
            Controls.Add(pnlChecks);
            Controls.Add(chkAlwaysShow);
            Controls.Add(btnCheckAgain);
            Icon = Resources.ComponentsCheck_Icon;
            Name = "ComponentsCheck";
            TabText = "Components Check";
            Text = "Components Check";
            pnlCheck1.ResumeLayout(false);
            pnlCheck1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) pbCheck1).EndInit();
            pnlCheck2.ResumeLayout(false);
            pnlCheck2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) pbCheck2).EndInit();
            pnlCheck3.ResumeLayout(false);
            pnlCheck3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) pbCheck3).EndInit();
            pnlCheck4.ResumeLayout(false);
            pnlCheck4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) pbCheck4).EndInit();
            pnlCheck5.ResumeLayout(false);
            pnlCheck5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) pbCheck5).EndInit();
            pnlChecks.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion

        #region Public Methods
        public ComponentsCheckWindow()
        {
            WindowType = WindowType.ComponentsCheck;
            DockPnl = new DockContent();
            InitializeComponent();
        }
        #endregion

        #region Form Stuff
        private void ComponentsCheck_Load(object sender, EventArgs e)
        {
            ApplyLanguage();
            chkAlwaysShow.Checked = Settings.Default.StartupComponentsCheck;
            CheckComponents();
        }

        private void ApplyLanguage()
        {
            TabText = Language.strComponentsCheck;
            Text = Language.strComponentsCheck;
            chkAlwaysShow.Text = Language.strCcAlwaysShowScreen;
            btnCheckAgain.Text = Language.strCcCheckAgain;
        }

        private void btnCheckAgain_Click(object sender, EventArgs e)
        {
            CheckComponents();
        }

        private void chkAlwaysShow_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.StartupComponentsCheck = chkAlwaysShow.Checked;
            Settings.Default.Save();
        }

        public new void Show(DockPanel panel)
        {
            try
            {
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Trying to show the components window", true);
                base.Show(panel);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("Failed to properly show the ComponentsWindow", ex);
            }
        }
        #endregion

        private void CheckComponents()
        {
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Beginning component check", true);
            CheckRdp();
            CheckVnc();
            CheckPutty();
            CheckIca();
            CheckGeckoBrowser();
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Finished component check", true);
        }

        private void CheckRdp()
        {
            pnlCheck1.Visible = true;
            
            try
            {
                using (var rdpClient = new AxMsRdpClient8NotSafeForScripting())
                {
                    rdpClient.CreateControl();

                    while (!rdpClient.Created)
                    {
                        Thread.Sleep(10);
                        System.Windows.Forms.Application.DoEvents();
                    }

                    if (!(new Version(rdpClient.Version) >= ProtocolRDP.Versions.RDC80))
                    {
                        throw new Exception(
                            $"Found RDC Client version {rdpClient.Version} but version {ProtocolRDP.Versions.RDC80} or higher is required.");
                    }

                    pbCheck1.Image = Resources.Good_Symbol;
                    lblCheck1.ForeColor = Color.DarkOliveGreen;
                    lblCheck1.Text = "RDP (Remote Desktop) " + Language.strCcCheckSucceeded;
                    txtCheck1.Text = string.Format(Language.strCcRDPOK, rdpClient.Version);
                    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "RDP installed", true);
                }
            }
            catch (Exception ex)
            {
                pbCheck1.Image = Resources.Bad_Symbol;
                lblCheck1.ForeColor = Color.Firebrick;
                lblCheck1.Text = "RDP (Remote Desktop) " + Language.strCcCheckFailed;
                txtCheck1.Text = Language.strCcRDPFailed;

                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                    "RDP " + Language.strCcNotInstalledProperly, true);
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, ex.Message, true);
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
                txtCheck2.Text = Language.strCcVNCFailed;

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
                txtCheck4.Text = Language.strCcICAFailed;

                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, "ICA " + Language.strCcNotInstalledProperly, true);
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, ex.Message, true);
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
                txtCheck5.Text = Language.strCcGeckoFailed;

                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                    "Gecko " + Language.strCcNotInstalledProperly, true);
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                    "GeckoFx was not found in " + geckoFxPath, true);
            }
        }
    }
}