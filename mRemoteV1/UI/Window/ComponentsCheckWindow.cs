using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using AxMSTSCLib;
using AxWFICALib;
using Gecko;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Messages;
using mRemoteNG.Themes;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.UI.Window
{
	public class ComponentsCheckWindow : BaseWindow
	{
	    private readonly Image _successImage;
	    private readonly Image _failureImage;

        #region Form Stuff
        private System.Windows.Forms.PictureBox pbCheck1;
        private Controls.Base.NGLabel lblCheck1;
        private System.Windows.Forms.Panel pnlCheck2;
        private Controls.Base.NGLabel lblCheck2;
        private System.Windows.Forms.PictureBox pbCheck2;
        private System.Windows.Forms.Panel pnlCheck3;
        private Controls.Base.NGLabel lblCheck3;
        private System.Windows.Forms.PictureBox pbCheck3;
        private System.Windows.Forms.Panel pnlCheck4;
        private Controls.Base.NGLabel lblCheck4;
        private System.Windows.Forms.PictureBox pbCheck4;
        private System.Windows.Forms.Panel pnlCheck5;
        private Controls.Base.NGLabel lblCheck5;
        private System.Windows.Forms.PictureBox pbCheck5;
        private Controls.Base.NGButton btnCheckAgain;
        private Controls.Base.NGTextBox txtCheck1;
        private Controls.Base.NGTextBox txtCheck2;
        private Controls.Base.NGTextBox txtCheck3;
        private Controls.Base.NGTextBox txtCheck4;
        private Controls.Base.NGTextBox txtCheck5;
        private Controls.Base.NGCheckBox chkAlwaysShow;
        private System.Windows.Forms.Panel pnlChecks;
        private System.Windows.Forms.Panel pnlCheck1;

        private void InitializeComponent()
        {
            this.pnlCheck1 = new System.Windows.Forms.Panel();
            this.txtCheck1 = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.lblCheck1 = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.pbCheck1 = new System.Windows.Forms.PictureBox();
            this.pnlCheck2 = new System.Windows.Forms.Panel();
            this.txtCheck2 = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.lblCheck2 = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.pbCheck2 = new System.Windows.Forms.PictureBox();
            this.pnlCheck3 = new System.Windows.Forms.Panel();
            this.txtCheck3 = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.lblCheck3 = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.pbCheck3 = new System.Windows.Forms.PictureBox();
            this.pnlCheck4 = new System.Windows.Forms.Panel();
            this.txtCheck4 = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.lblCheck4 = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.pbCheck4 = new System.Windows.Forms.PictureBox();
            this.pnlCheck5 = new System.Windows.Forms.Panel();
            this.txtCheck5 = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.lblCheck5 = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.pbCheck5 = new System.Windows.Forms.PictureBox();
            this.btnCheckAgain = new mRemoteNG.UI.Controls.Base.NGButton();
            this.chkAlwaysShow = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.pnlChecks = new System.Windows.Forms.Panel();
            this.pnlCheck1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCheck1)).BeginInit();
            this.pnlCheck2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCheck2)).BeginInit();
            this.pnlCheck3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCheck3)).BeginInit();
            this.pnlCheck4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCheck4)).BeginInit();
            this.pnlCheck5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCheck5)).BeginInit();
            this.pnlChecks.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCheck1
            // 
            this.pnlCheck1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlCheck1.Controls.Add(this.txtCheck1);
            this.pnlCheck1.Controls.Add(this.lblCheck1);
            this.pnlCheck1.Controls.Add(this.pbCheck1);
            this.pnlCheck1.Location = new System.Drawing.Point(3, 3);
            this.pnlCheck1.Name = "pnlCheck1";
            this.pnlCheck1.Size = new System.Drawing.Size(562, 130);
            this.pnlCheck1.TabIndex = 10;
            this.pnlCheck1.Visible = false;
            // 
            // txtCheck1
            // 
            this.txtCheck1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCheck1.BackColor = System.Drawing.SystemColors.Control;
            this.txtCheck1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCheck1.Location = new System.Drawing.Point(129, 29);
            this.txtCheck1.Multiline = true;
            this.txtCheck1.Name = "txtCheck1";
            this.txtCheck1.ReadOnly = true;
            this.txtCheck1.Size = new System.Drawing.Size(430, 97);
            this.txtCheck1.TabIndex = 2;
            // 
            // lblCheck1
            // 
            this.lblCheck1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCheck1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCheck1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblCheck1.Location = new System.Drawing.Point(108, 3);
            this.lblCheck1.Name = "lblCheck1";
            this.lblCheck1.Size = new System.Drawing.Size(451, 23);
            this.lblCheck1.TabIndex = 1;
            this.lblCheck1.Text = "RDP check succeeded!";
            // 
            // pbCheck1
            // 
            this.pbCheck1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pbCheck1.Location = new System.Drawing.Point(3, 3);
            this.pbCheck1.Name = "pbCheck1";
            this.pbCheck1.Size = new System.Drawing.Size(72, 123);
            this.pbCheck1.TabIndex = 0;
            this.pbCheck1.TabStop = false;
            // 
            // pnlCheck2
            // 
            this.pnlCheck2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlCheck2.Controls.Add(this.txtCheck2);
            this.pnlCheck2.Controls.Add(this.lblCheck2);
            this.pnlCheck2.Controls.Add(this.pbCheck2);
            this.pnlCheck2.Location = new System.Drawing.Point(3, 139);
            this.pnlCheck2.Name = "pnlCheck2";
            this.pnlCheck2.Size = new System.Drawing.Size(562, 130);
            this.pnlCheck2.TabIndex = 20;
            this.pnlCheck2.Visible = false;
            // 
            // txtCheck2
            // 
            this.txtCheck2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCheck2.BackColor = System.Drawing.SystemColors.Control;
            this.txtCheck2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCheck2.Location = new System.Drawing.Point(129, 29);
            this.txtCheck2.Multiline = true;
            this.txtCheck2.Name = "txtCheck2";
            this.txtCheck2.ReadOnly = true;
            this.txtCheck2.Size = new System.Drawing.Size(430, 97);
            this.txtCheck2.TabIndex = 2;
            // 
            // lblCheck2
            // 
            this.lblCheck2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCheck2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCheck2.Location = new System.Drawing.Point(112, 3);
            this.lblCheck2.Name = "lblCheck2";
            this.lblCheck2.Size = new System.Drawing.Size(447, 23);
            this.lblCheck2.TabIndex = 1;
            this.lblCheck2.Text = "RDP check succeeded!";
            // 
            // pbCheck2
            // 
            this.pbCheck2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pbCheck2.Location = new System.Drawing.Point(3, 3);
            this.pbCheck2.Name = "pbCheck2";
            this.pbCheck2.Size = new System.Drawing.Size(72, 123);
            this.pbCheck2.TabIndex = 0;
            this.pbCheck2.TabStop = false;
            // 
            // pnlCheck3
            // 
            this.pnlCheck3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlCheck3.Controls.Add(this.txtCheck3);
            this.pnlCheck3.Controls.Add(this.lblCheck3);
            this.pnlCheck3.Controls.Add(this.pbCheck3);
            this.pnlCheck3.Location = new System.Drawing.Point(3, 275);
            this.pnlCheck3.Name = "pnlCheck3";
            this.pnlCheck3.Size = new System.Drawing.Size(562, 130);
            this.pnlCheck3.TabIndex = 30;
            this.pnlCheck3.Visible = false;
            // 
            // txtCheck3
            // 
            this.txtCheck3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCheck3.BackColor = System.Drawing.SystemColors.Control;
            this.txtCheck3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCheck3.Location = new System.Drawing.Point(129, 29);
            this.txtCheck3.Multiline = true;
            this.txtCheck3.Name = "txtCheck3";
            this.txtCheck3.ReadOnly = true;
            this.txtCheck3.Size = new System.Drawing.Size(430, 97);
            this.txtCheck3.TabIndex = 2;
            // 
            // lblCheck3
            // 
            this.lblCheck3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCheck3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCheck3.Location = new System.Drawing.Point(112, 3);
            this.lblCheck3.Name = "lblCheck3";
            this.lblCheck3.Size = new System.Drawing.Size(447, 23);
            this.lblCheck3.TabIndex = 1;
            this.lblCheck3.Text = "RDP check succeeded!";
            // 
            // pbCheck3
            // 
            this.pbCheck3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pbCheck3.Location = new System.Drawing.Point(3, 3);
            this.pbCheck3.Name = "pbCheck3";
            this.pbCheck3.Size = new System.Drawing.Size(72, 123);
            this.pbCheck3.TabIndex = 0;
            this.pbCheck3.TabStop = false;
            // 
            // pnlCheck4
            // 
            this.pnlCheck4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlCheck4.Controls.Add(this.txtCheck4);
            this.pnlCheck4.Controls.Add(this.lblCheck4);
            this.pnlCheck4.Controls.Add(this.pbCheck4);
            this.pnlCheck4.Location = new System.Drawing.Point(3, 411);
            this.pnlCheck4.Name = "pnlCheck4";
            this.pnlCheck4.Size = new System.Drawing.Size(562, 130);
            this.pnlCheck4.TabIndex = 40;
            this.pnlCheck4.Visible = false;
            // 
            // txtCheck4
            // 
            this.txtCheck4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCheck4.BackColor = System.Drawing.SystemColors.Control;
            this.txtCheck4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCheck4.Location = new System.Drawing.Point(129, 30);
            this.txtCheck4.Multiline = true;
            this.txtCheck4.Name = "txtCheck4";
            this.txtCheck4.ReadOnly = true;
            this.txtCheck4.Size = new System.Drawing.Size(430, 97);
            this.txtCheck4.TabIndex = 2;
            // 
            // lblCheck4
            // 
            this.lblCheck4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCheck4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCheck4.Location = new System.Drawing.Point(112, 3);
            this.lblCheck4.Name = "lblCheck4";
            this.lblCheck4.Size = new System.Drawing.Size(447, 23);
            this.lblCheck4.TabIndex = 1;
            this.lblCheck4.Text = "RDP check succeeded!";
            // 
            // pbCheck4
            // 
            this.pbCheck4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pbCheck4.Location = new System.Drawing.Point(3, 3);
            this.pbCheck4.Name = "pbCheck4";
            this.pbCheck4.Size = new System.Drawing.Size(72, 123);
            this.pbCheck4.TabIndex = 0;
            this.pbCheck4.TabStop = false;
            // 
            // pnlCheck5
            // 
            this.pnlCheck5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlCheck5.Controls.Add(this.txtCheck5);
            this.pnlCheck5.Controls.Add(this.lblCheck5);
            this.pnlCheck5.Controls.Add(this.pbCheck5);
            this.pnlCheck5.Location = new System.Drawing.Point(3, 547);
            this.pnlCheck5.Name = "pnlCheck5";
            this.pnlCheck5.Size = new System.Drawing.Size(562, 130);
            this.pnlCheck5.TabIndex = 50;
            this.pnlCheck5.Visible = false;
            // 
            // txtCheck5
            // 
            this.txtCheck5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCheck5.BackColor = System.Drawing.SystemColors.Control;
            this.txtCheck5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCheck5.Location = new System.Drawing.Point(129, 29);
            this.txtCheck5.Multiline = true;
            this.txtCheck5.Name = "txtCheck5";
            this.txtCheck5.ReadOnly = true;
            this.txtCheck5.Size = new System.Drawing.Size(430, 97);
            this.txtCheck5.TabIndex = 2;
            // 
            // lblCheck5
            // 
            this.lblCheck5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCheck5.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCheck5.Location = new System.Drawing.Point(112, 3);
            this.lblCheck5.Name = "lblCheck5";
            this.lblCheck5.Size = new System.Drawing.Size(447, 23);
            this.lblCheck5.TabIndex = 1;
            this.lblCheck5.Text = "RDP check succeeded!";
            // 
            // pbCheck5
            // 
            this.pbCheck5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pbCheck5.Location = new System.Drawing.Point(3, 3);
            this.pbCheck5.Name = "pbCheck5";
            this.pbCheck5.Size = new System.Drawing.Size(72, 123);
            this.pbCheck5.TabIndex = 0;
            this.pbCheck5.TabStop = false;
            // 
            // btnCheckAgain
            // 
            this.btnCheckAgain._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.btnCheckAgain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCheckAgain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheckAgain.Location = new System.Drawing.Point(476, 810);
            this.btnCheckAgain.Name = "btnCheckAgain";
            this.btnCheckAgain.Size = new System.Drawing.Size(104, 23);
            this.btnCheckAgain.TabIndex = 0;
            this.btnCheckAgain.Text = "Check again";
            this.btnCheckAgain.UseVisualStyleBackColor = true;
            this.btnCheckAgain.Click += new System.EventHandler(this.btnCheckAgain_Click);
            // 
            // chkAlwaysShow
            // 
            this.chkAlwaysShow._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.HOVER;
            this.chkAlwaysShow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkAlwaysShow.AutoSize = true;
            this.chkAlwaysShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkAlwaysShow.Location = new System.Drawing.Point(12, 814);
            this.chkAlwaysShow.Name = "chkAlwaysShow";
            this.chkAlwaysShow.Size = new System.Drawing.Size(200, 17);
            this.chkAlwaysShow.TabIndex = 51;
            this.chkAlwaysShow.Text = "Always show this screen at startup";
            this.chkAlwaysShow.UseVisualStyleBackColor = true;
            this.chkAlwaysShow.CheckedChanged += new System.EventHandler(this.chkAlwaysShow_CheckedChanged);
            // 
            // pnlChecks
            // 
            this.pnlChecks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlChecks.AutoScroll = true;
            this.pnlChecks.Controls.Add(this.pnlCheck1);
            this.pnlChecks.Controls.Add(this.pnlCheck2);
            this.pnlChecks.Controls.Add(this.pnlCheck3);
            this.pnlChecks.Controls.Add(this.pnlCheck5);
            this.pnlChecks.Controls.Add(this.pnlCheck4);
            this.pnlChecks.Location = new System.Drawing.Point(12, 12);
            this.pnlChecks.Name = "pnlChecks";
            this.pnlChecks.Size = new System.Drawing.Size(568, 792);
            this.pnlChecks.TabIndex = 52;
            // 
            // ComponentsCheckWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(592, 845);
            this.Controls.Add(this.pnlChecks);
            this.Controls.Add(this.chkAlwaysShow);
            this.Controls.Add(this.btnCheckAgain);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::mRemoteNG.Resources.ComponentsCheck_Icon;
            this.Name = "ComponentsCheckWindow";
            this.TabText = "Components Check";
            this.Text = "Components Check";
            this.Load += new System.EventHandler(this.ComponentsCheck_Load);
            this.pnlCheck1.ResumeLayout(false);
            this.pnlCheck1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCheck1)).EndInit();
            this.pnlCheck2.ResumeLayout(false);
            this.pnlCheck2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCheck2)).EndInit();
            this.pnlCheck3.ResumeLayout(false);
            this.pnlCheck3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCheck3)).EndInit();
            this.pnlCheck4.ResumeLayout(false);
            this.pnlCheck4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCheck4)).EndInit();
            this.pnlCheck5.ResumeLayout(false);
            this.pnlCheck5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCheck5)).EndInit();
            this.pnlChecks.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #region Public Methods
        public ComponentsCheckWindow()
        {
            WindowType = WindowType.ComponentsCheck;
            DockPnl = new DockContent();
            InitializeComponent();
            var display = new DisplayProperties();
            _successImage = display.ScaleImage(Resources.Good_Symbol);
            _failureImage = display.ScaleImage(Resources.Bad_Symbol);
            FontOverrider.FontOverride(this);
            ThemeManager.getInstance().ThemeChanged += ApplyTheme;
        }
        #endregion

        #region Form Stuff
        private void ComponentsCheck_Load(object sender, EventArgs e)
        {
            ApplyLanguage();
            ApplyTheme();
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

        private new void ApplyTheme()
        {
            if (!ThemeManager.getInstance().ThemingActive) return;
            base.ApplyTheme();

            if (!ThemeManager.getInstance().ActiveAndExtended) return;
            pnlCheck1.BackColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            pnlCheck1.ForeColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
            pnlCheck2.BackColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            pnlCheck2.ForeColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
            pnlCheck3.BackColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            pnlCheck3.ForeColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
            pnlCheck4.BackColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            pnlCheck4.ForeColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
            pnlCheck5.BackColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            pnlCheck5.ForeColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
            pnlChecks.BackColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            pnlChecks.ForeColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
        }

        private void btnCheckAgain_Click(object sender, EventArgs e)
        {
            CheckComponents();
        }

        private void chkAlwaysShow_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.StartupComponentsCheck = chkAlwaysShow.Checked;
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

                    if (!(new Version(rdpClient.Version) >= RdpProtocol.Versions.RDC80))
                    {
                        throw new Exception(
                            $"Found RDC Client version {rdpClient.Version} but version {RdpProtocol.Versions.RDC80} or higher is required.");
                    }

                    pbCheck1.Image = _successImage;
                    lblCheck1.ForeColor = Color.DarkOliveGreen;
                    lblCheck1.Text = "RDP (Remote Desktop) " + Language.strCcCheckSucceeded;
                    txtCheck1.Text = string.Format(Language.strCcRDPOK, rdpClient.Version);
                    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "RDP installed", true);
                }
            }
            catch (Exception ex)
            {
                pbCheck1.Image = _failureImage;
                lblCheck1.ForeColor = Color.Firebrick;
                lblCheck1.Text = "RDP (Remote Desktop) " + Language.strCcCheckFailed;
                txtCheck1.Text = string.Format(Language.strCcRDPFailed, GeneralAppInfo.UrlForum);

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

                    pbCheck2.Image = _successImage;
                    lblCheck2.ForeColor = Color.DarkOliveGreen;
                    lblCheck2.Text = "VNC (Virtual Network Computing) " + Language.strCcCheckSucceeded;
                    txtCheck2.Text = string.Format(Language.strCcVNCOK, vnc.ProductVersion);
                    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "VNC installed", true);
                }
            }
            catch (Exception)
            {
                pbCheck2.Image = _failureImage;
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

                pbCheck3.Image = _successImage;
                lblCheck3.ForeColor = Color.DarkOliveGreen;
                lblCheck3.Text = "PuTTY (SSH/Telnet/Rlogin/RAW) " + Language.strCcCheckSucceeded;
                txtCheck3.Text =
                    $"{Language.strCcPuttyOK}{Environment.NewLine}Version: {versionInfo.ProductName} Release: {versionInfo.FileVersion}";
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "PuTTY installed", true);
            }
            else
            {
                pbCheck3.Image = _failureImage;
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

                    pbCheck4.Image = _successImage;
                    lblCheck4.ForeColor = Color.DarkOliveGreen;
                    lblCheck4.Text = @"ICA (Citrix ICA) " + Language.strCcCheckSucceeded;
                    txtCheck4.Text = string.Format(Language.strCcICAOK, ica.Version);
                    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "ICA installed", true);
                }
            }
            catch (Exception ex)
            {
                pbCheck4.Image = _failureImage;
                lblCheck4.ForeColor = Color.Firebrick;
                lblCheck4.Text = @"ICA (Citrix ICA) " + Language.strCcCheckFailed;
                txtCheck4.Text = string.Format(Language.strCcICAFailed, GeneralAppInfo.UrlForum);

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
                pbCheck5.Image = _successImage;
                lblCheck5.ForeColor = Color.DarkOliveGreen;
                lblCheck5.Text = @"Gecko (Firefox) Rendering Engine (HTTP/S) " + Language.strCcCheckSucceeded;
                if (!Xpcom.IsInitialized)
                    Xpcom.Initialize("Firefox");
                txtCheck5.Text = Language.strCcGeckoOK + " Version: " + Xpcom.XulRunnerVersion;
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Gecko Browser installed", true);
            }
            else
            {
                pbCheck5.Image = _failureImage;
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