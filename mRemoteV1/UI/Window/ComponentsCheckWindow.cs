using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using AxMSTSCLib;
using AxWFICALib;
using Gecko;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Messages;
using VncSharp;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.UI.Window
{
    public class ComponentsCheckWindow : BaseWindow
    {
        #region Public Methods

        public ComponentsCheckWindow(DockContent Panel)
        {
            WindowType = WindowType.ComponentsCheck;
            DockPnl = Panel;
            InitializeComponent();
        }

        #endregion

        #region Private Methods

        private void CheckComponents()
        {
            pnlCheck1.Visible = true;
            pnlCheck2.Visible = true;
            pnlCheck3.Visible = true;
            pnlCheck4.Visible = true;
            pnlCheck5.Visible = true;

            AxMsRdpClient5NotSafeForScripting rdpClient = null;

            try
            {
                rdpClient = new AxMsRdpClient5NotSafeForScripting();
                rdpClient.CreateControl();

                while (!rdpClient.Created)
                {
                    Thread.Sleep(0);
                    Application.DoEvents();
                }

                if (!(new Version(Convert.ToString(rdpClient.Version)) >= ProtocolRDP.Versions.RDC80))
                    throw new Exception(
                        $"Found RDC Client version {rdpClient.Version} but version {ProtocolRDP.Versions.RDC80} or higher is required.");

                pbCheck1.Image = Resources.Good_Symbol;
                lblCheck1.ForeColor = Color.DarkOliveGreen;
                lblCheck1.Text = "RDP (Remote Desktop) " + Language.strCcCheckSucceeded;
                txtCheck1.Text = string.Format(Language.strCcRDPOK, rdpClient.Version);
            }
            catch (Exception ex)
            {
                pbCheck1.Image = Resources.Bad_Symbol;
                lblCheck1.ForeColor = Color.Firebrick;
                lblCheck1.Text = "RDP (Remote Desktop) " + Language.strCcCheckFailed;
                txtCheck1.Text = Language.strCcRDPFailed;

                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, "RDP " + Language.strCcNotInstalledProperly,
                    true);
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, ex.Message, true);
            }

            rdpClient?.Dispose();


            RemoteDesktop VNC = null;

            try
            {
                VNC = new RemoteDesktop();
                VNC.CreateControl();

                while (!VNC.Created)
                {
                    Thread.Sleep(10);
                    Application.DoEvents();
                }

                pbCheck2.Image = Resources.Good_Symbol;
                lblCheck2.ForeColor = Color.DarkOliveGreen;
                lblCheck2.Text = "VNC (Virtual Network Computing) " + Language.strCcCheckSucceeded;
                txtCheck2.Text = string.Format(Language.strCcVNCOK, VNC.ProductVersion);
            }
            catch (Exception)
            {
                pbCheck2.Image = Resources.Bad_Symbol;
                lblCheck2.ForeColor = Color.Firebrick;
                lblCheck2.Text = "VNC (Virtual Network Computing) " + Language.strCcCheckFailed;
                txtCheck2.Text = Language.strCcVNCFailed;

                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, "VNC " + Language.strCcNotInstalledProperly,
                    true);
            }

            VNC?.Dispose();


            string pPath;
            if (Settings.Default.UseCustomPuttyPath == false)
                pPath = GeneralAppInfo.HomePath + "\\PuTTYNG.exe";
            else
                pPath = Convert.ToString(Settings.Default.CustomPuttyPath);

            if (File.Exists(pPath))
            {
                var versionInfo = FileVersionInfo.GetVersionInfo(pPath);

                pbCheck3.Image = Resources.Good_Symbol;
                lblCheck3.ForeColor = Color.DarkOliveGreen;
                lblCheck3.Text = "PuTTY (SSH/Telnet/Rlogin/RAW) " + Language.strCcCheckSucceeded;
                txtCheck3.Text =
                    $"{Language.strCcPuttyOK}{Environment.NewLine}Version: {versionInfo.ProductName} Release: {versionInfo.FileVersion}";
            }
            else
            {
                pbCheck3.Image = Resources.Bad_Symbol;
                lblCheck3.ForeColor = Color.Firebrick;
                lblCheck3.Text = "PuTTY (SSH/Telnet/Rlogin/RAW) " + Language.strCcCheckFailed;
                txtCheck3.Text = Language.strCcPuttyFailed;

                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                    "PuTTY " + Language.strCcNotInstalledProperly, true);
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "File " + pPath + " does not exist.", true);
            }


            AxICAClient ICA = null;

            try
            {
                ICA = new AxICAClient {Parent = this};
                ICA.CreateControl();

                while (!ICA.Created)
                {
                    Thread.Sleep(10);
                    Application.DoEvents();
                }

                pbCheck4.Image = Resources.Good_Symbol;
                lblCheck4.ForeColor = Color.DarkOliveGreen;
                lblCheck4.Text = "ICA (Citrix ICA) " + Language.strCcCheckSucceeded;
                txtCheck4.Text = string.Format(Language.strCcICAOK, ICA.Version);
            }
            catch (Exception ex)
            {
                pbCheck4.Image = Resources.Bad_Symbol;
                lblCheck4.ForeColor = Color.Firebrick;
                lblCheck4.Text = "ICA (Citrix ICA) " + Language.strCcCheckFailed;
                txtCheck4.Text = Language.strCcICAFailed;

                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, "ICA " + Language.strCcNotInstalledProperly,
                    true);
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, ex.Message, true);
            }

            ICA?.Dispose();


            var GeckoBad = false;
            var GeckoFxPath = Path.Combine(GeneralAppInfo.HomePath, "Firefox");

            if (File.Exists(Path.Combine(GeneralAppInfo.HomePath, "Geckofx-Core.dll")))
                if (Directory.Exists(GeckoFxPath))
                {
                    if (!File.Exists(Path.Combine(GeckoFxPath, "xul.dll")))
                        GeckoBad = true;
                }
                else
                {
                    GeckoBad = true;
                }

            if (GeckoBad == false)
            {
                pbCheck5.Image = Resources.Good_Symbol;
                lblCheck5.ForeColor = Color.DarkOliveGreen;
                lblCheck5.Text = "Gecko (Firefox) Rendering Engine (HTTP/S) " + Language.strCcCheckSucceeded;
                if (!Xpcom.IsInitialized)
                    Xpcom.Initialize("Firefox");
                txtCheck5.Text = Language.strCcGeckoOK + " Version: " + Xpcom.XulRunnerVersion;
            }
            else
            {
                pbCheck5.Image = Resources.Bad_Symbol;
                lblCheck5.ForeColor = Color.Firebrick;
                lblCheck5.Text = "Gecko (Firefox) Rendering Engine (HTTP/S) " + Language.strCcCheckFailed;
                txtCheck5.Text = Language.strCcGeckoFailed;

                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                    "Gecko " + Language.strCcNotInstalledProperly, true);
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "GeckoFx was not found in " + GeckoFxPath,
                    true);
            }
        }

        #endregion

        #region Form Stuff

        internal PictureBox pbCheck1;
        internal Label lblCheck1;
        internal Panel pnlCheck2;
        internal Label lblCheck2;
        internal PictureBox pbCheck2;
        internal Panel pnlCheck3;
        internal Label lblCheck3;
        internal PictureBox pbCheck3;
        internal Panel pnlCheck4;
        internal Label lblCheck4;
        internal PictureBox pbCheck4;
        internal Panel pnlCheck5;
        internal Label lblCheck5;
        internal PictureBox pbCheck5;
        internal Button btnCheckAgain;
        internal TextBox txtCheck1;
        internal TextBox txtCheck2;
        internal TextBox txtCheck3;
        internal TextBox txtCheck4;
        internal TextBox txtCheck5;
        internal CheckBox chkAlwaysShow;
        internal Panel pnlChecks;
        internal Panel pnlCheck1;

        private void InitializeComponent()
        {
            pnlCheck1 = new Panel();
            Load += ComponentsCheck_Load;
            txtCheck1 = new TextBox();
            lblCheck1 = new Label();
            pbCheck1 = new PictureBox();
            pnlCheck2 = new Panel();
            txtCheck2 = new TextBox();
            lblCheck2 = new Label();
            pbCheck2 = new PictureBox();
            pnlCheck3 = new Panel();
            txtCheck3 = new TextBox();
            lblCheck3 = new Label();
            pbCheck3 = new PictureBox();
            pnlCheck4 = new Panel();
            txtCheck4 = new TextBox();
            lblCheck4 = new Label();
            pbCheck4 = new PictureBox();
            pnlCheck5 = new Panel();
            txtCheck5 = new TextBox();
            lblCheck5 = new Label();
            pbCheck5 = new PictureBox();
            btnCheckAgain = new Button();
            btnCheckAgain.Click += btnCheckAgain_Click;
            chkAlwaysShow = new CheckBox();
            chkAlwaysShow.CheckedChanged += chkAlwaysShow_CheckedChanged;
            pnlChecks = new Panel();
            pnlCheck1.SuspendLayout();
            ((ISupportInitialize) pbCheck1).BeginInit();
            pnlCheck2.SuspendLayout();
            ((ISupportInitialize) pbCheck2).BeginInit();
            pnlCheck3.SuspendLayout();
            ((ISupportInitialize) pbCheck3).BeginInit();
            pnlCheck4.SuspendLayout();
            ((ISupportInitialize) pbCheck4).BeginInit();
            pnlCheck5.SuspendLayout();
            ((ISupportInitialize) pbCheck5).BeginInit();
            pnlChecks.SuspendLayout();
            SuspendLayout();
            //
            //pnlCheck1
            //
            pnlCheck1.Anchor = AnchorStyles.Top | AnchorStyles.Left
                               | AnchorStyles.Right;
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
            txtCheck1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                               | AnchorStyles.Left
                               | AnchorStyles.Right;
            txtCheck1.BackColor = SystemColors.Control;
            txtCheck1.BorderStyle = BorderStyle.None;
            txtCheck1.Location = new Point(129, 29);
            txtCheck1.Multiline = true;
            txtCheck1.Name = "txtCheck1";
            txtCheck1.ReadOnly = true;
            txtCheck1.Size = new Size(430, 97);
            txtCheck1.TabIndex = 2;
            //
            //lblCheck1
            //
            lblCheck1.Anchor = AnchorStyles.Top | AnchorStyles.Left
                               | AnchorStyles.Right;
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
            pbCheck1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                              | AnchorStyles.Left;
            pbCheck1.Location = new Point(3, 3);
            pbCheck1.Name = "pbCheck1";
            pbCheck1.Size = new Size(72, 123);
            pbCheck1.TabIndex = 0;
            pbCheck1.TabStop = false;
            //
            //pnlCheck2
            //
            pnlCheck2.Anchor = AnchorStyles.Top | AnchorStyles.Left
                               | AnchorStyles.Right;
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
            txtCheck2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                               | AnchorStyles.Left
                               | AnchorStyles.Right;
            txtCheck2.BackColor = SystemColors.Control;
            txtCheck2.BorderStyle = BorderStyle.None;
            txtCheck2.Location = new Point(129, 29);
            txtCheck2.Multiline = true;
            txtCheck2.Name = "txtCheck2";
            txtCheck2.ReadOnly = true;
            txtCheck2.Size = new Size(430, 97);
            txtCheck2.TabIndex = 2;
            //
            //lblCheck2
            //
            lblCheck2.Anchor = AnchorStyles.Top | AnchorStyles.Left
                               | AnchorStyles.Right;
            lblCheck2.Font = new Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point, Convert.ToByte(0));
            lblCheck2.Location = new Point(112, 3);
            lblCheck2.Name = "lblCheck2";
            lblCheck2.Size = new Size(447, 23);
            lblCheck2.TabIndex = 1;
            lblCheck2.Text = "RDP check succeeded!";
            //
            //pbCheck2
            //
            pbCheck2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                              | AnchorStyles.Left;
            pbCheck2.Location = new Point(3, 3);
            pbCheck2.Name = "pbCheck2";
            pbCheck2.Size = new Size(72, 123);
            pbCheck2.TabIndex = 0;
            pbCheck2.TabStop = false;
            //
            //pnlCheck3
            //
            pnlCheck3.Anchor = AnchorStyles.Top | AnchorStyles.Left
                               | AnchorStyles.Right;
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
            txtCheck3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                               | AnchorStyles.Left
                               | AnchorStyles.Right;
            txtCheck3.BackColor = SystemColors.Control;
            txtCheck3.BorderStyle = BorderStyle.None;
            txtCheck3.Location = new Point(129, 29);
            txtCheck3.Multiline = true;
            txtCheck3.Name = "txtCheck3";
            txtCheck3.ReadOnly = true;
            txtCheck3.Size = new Size(430, 97);
            txtCheck3.TabIndex = 2;
            //
            //lblCheck3
            //
            lblCheck3.Anchor = AnchorStyles.Top | AnchorStyles.Left
                               | AnchorStyles.Right;
            lblCheck3.Font = new Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point, Convert.ToByte(0));
            lblCheck3.Location = new Point(112, 3);
            lblCheck3.Name = "lblCheck3";
            lblCheck3.Size = new Size(447, 23);
            lblCheck3.TabIndex = 1;
            lblCheck3.Text = "RDP check succeeded!";
            //
            //pbCheck3
            //
            pbCheck3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                              | AnchorStyles.Left;
            pbCheck3.Location = new Point(3, 3);
            pbCheck3.Name = "pbCheck3";
            pbCheck3.Size = new Size(72, 123);
            pbCheck3.TabIndex = 0;
            pbCheck3.TabStop = false;
            //
            //pnlCheck4
            //
            pnlCheck4.Anchor = AnchorStyles.Top | AnchorStyles.Left
                               | AnchorStyles.Right;
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
            txtCheck4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                               | AnchorStyles.Left
                               | AnchorStyles.Right;
            txtCheck4.BackColor = SystemColors.Control;
            txtCheck4.BorderStyle = BorderStyle.None;
            txtCheck4.Location = new Point(129, 30);
            txtCheck4.Multiline = true;
            txtCheck4.Name = "txtCheck4";
            txtCheck4.ReadOnly = true;
            txtCheck4.Size = new Size(430, 97);
            txtCheck4.TabIndex = 2;
            //
            //lblCheck4
            //
            lblCheck4.Anchor = AnchorStyles.Top | AnchorStyles.Left
                               | AnchorStyles.Right;
            lblCheck4.Font = new Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point, Convert.ToByte(0));
            lblCheck4.Location = new Point(112, 3);
            lblCheck4.Name = "lblCheck4";
            lblCheck4.Size = new Size(447, 23);
            lblCheck4.TabIndex = 1;
            lblCheck4.Text = "RDP check succeeded!";
            //
            //pbCheck4
            //
            pbCheck4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                              | AnchorStyles.Left;
            pbCheck4.Location = new Point(3, 3);
            pbCheck4.Name = "pbCheck4";
            pbCheck4.Size = new Size(72, 123);
            pbCheck4.TabIndex = 0;
            pbCheck4.TabStop = false;
            //
            //pnlCheck5
            //
            pnlCheck5.Anchor = AnchorStyles.Top | AnchorStyles.Left
                               | AnchorStyles.Right;
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
            txtCheck5.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                               | AnchorStyles.Left
                               | AnchorStyles.Right;
            txtCheck5.BackColor = SystemColors.Control;
            txtCheck5.BorderStyle = BorderStyle.None;
            txtCheck5.Location = new Point(129, 29);
            txtCheck5.Multiline = true;
            txtCheck5.Name = "txtCheck5";
            txtCheck5.ReadOnly = true;
            txtCheck5.Size = new Size(430, 97);
            txtCheck5.TabIndex = 2;
            //
            //lblCheck5
            //
            lblCheck5.Anchor = AnchorStyles.Top | AnchorStyles.Left
                               | AnchorStyles.Right;
            lblCheck5.Font = new Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point, Convert.ToByte(0));
            lblCheck5.Location = new Point(112, 3);
            lblCheck5.Name = "lblCheck5";
            lblCheck5.Size = new Size(447, 23);
            lblCheck5.TabIndex = 1;
            lblCheck5.Text = "RDP check succeeded!";
            //
            //pbCheck5
            //
            pbCheck5.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                              | AnchorStyles.Left;
            pbCheck5.Location = new Point(3, 3);
            pbCheck5.Name = "pbCheck5";
            pbCheck5.Size = new Size(72, 123);
            pbCheck5.TabIndex = 0;
            pbCheck5.TabStop = false;
            //
            //btnCheckAgain
            //
            btnCheckAgain.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCheckAgain.FlatStyle = FlatStyle.Flat;
            btnCheckAgain.Location = new Point(476, 842);
            btnCheckAgain.Name = "btnCheckAgain";
            btnCheckAgain.Size = new Size(104, 23);
            btnCheckAgain.TabIndex = 0;
            btnCheckAgain.Text = "Check again";
            btnCheckAgain.UseVisualStyleBackColor = true;
            //
            //chkAlwaysShow
            //
            chkAlwaysShow.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkAlwaysShow.AutoSize = true;
            chkAlwaysShow.FlatStyle = FlatStyle.Flat;
            chkAlwaysShow.Location = new Point(12, 846);
            chkAlwaysShow.Name = "chkAlwaysShow";
            chkAlwaysShow.Size = new Size(185, 17);
            chkAlwaysShow.TabIndex = 51;
            chkAlwaysShow.Text = "Always show this screen at startup";
            chkAlwaysShow.UseVisualStyleBackColor = true;
            //
            //pnlChecks
            //
            pnlChecks.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                               | AnchorStyles.Left
                               | AnchorStyles.Right;
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
            ((ISupportInitialize) pbCheck1).EndInit();
            pnlCheck2.ResumeLayout(false);
            pnlCheck2.PerformLayout();
            ((ISupportInitialize) pbCheck2).EndInit();
            pnlCheck3.ResumeLayout(false);
            pnlCheck3.PerformLayout();
            ((ISupportInitialize) pbCheck3).EndInit();
            pnlCheck4.ResumeLayout(false);
            pnlCheck4.PerformLayout();
            ((ISupportInitialize) pbCheck4).EndInit();
            pnlCheck5.ResumeLayout(false);
            pnlCheck5.PerformLayout();
            ((ISupportInitialize) pbCheck5).EndInit();
            pnlChecks.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        #region Form Stuff

        private void ComponentsCheck_Load(object sender, EventArgs e)
        {
            ApplyLanguage();

            chkAlwaysShow.Checked = Convert.ToBoolean(Settings.Default.StartupComponentsCheck);
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

        #endregion
    }
}