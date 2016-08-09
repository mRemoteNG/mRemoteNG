using mRemoteNG.App;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.Tools;
using mRemoteNG.UI.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.UI.Window
{
    public class SSHTransferWindow : BaseWindow
    {
        #region Form Init
        private ProgressBar pbStatus;
        private Button btnTransfer;
        private TextBox txtUser;
        private TextBox txtPassword;
        private TextBox txtHost;
        private TextBox txtPort;
        private Label lblHost;
        private Label lblPort;
        private Label lblUser;
        private Label lblPassword;
        private Label lblProtocol;
        private RadioButton radProtSCP;
        private RadioButton radProtSFTP;
        private GroupBox grpConnection;
        private Button btnBrowse;
        private Label lblRemoteFile;
        private TextBox txtRemoteFile;
        private TextBox txtLocalFile;
        private Label lblLocalFile;
        private GroupBox grpFiles;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SSHTransferWindow));
            this.grpFiles = new System.Windows.Forms.GroupBox();
            this.lblLocalFile = new System.Windows.Forms.Label();
            this.txtLocalFile = new System.Windows.Forms.TextBox();
            this.btnTransfer = new System.Windows.Forms.Button();
            this.txtRemoteFile = new System.Windows.Forms.TextBox();
            this.lblRemoteFile = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.grpConnection = new System.Windows.Forms.GroupBox();
            this.radProtSFTP = new System.Windows.Forms.RadioButton();
            this.radProtSCP = new System.Windows.Forms.RadioButton();
            this.lblProtocol = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblHost = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.pbStatus = new System.Windows.Forms.ProgressBar();
            this.grpFiles.SuspendLayout();
            this.grpConnection.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpFiles
            // 
            this.grpFiles.Controls.Add(this.lblLocalFile);
            this.grpFiles.Controls.Add(this.txtLocalFile);
            this.grpFiles.Controls.Add(this.btnTransfer);
            this.grpFiles.Controls.Add(this.txtRemoteFile);
            this.grpFiles.Controls.Add(this.lblRemoteFile);
            this.grpFiles.Controls.Add(this.btnBrowse);
            this.grpFiles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.grpFiles.Location = new System.Drawing.Point(12, 172);
            this.grpFiles.Name = "grpFiles";
            this.grpFiles.Size = new System.Drawing.Size(668, 175);
            this.grpFiles.TabIndex = 2000;
            this.grpFiles.TabStop = false;
            this.grpFiles.Text = "Files";
            // 
            // lblLocalFile
            // 
            this.lblLocalFile.AutoSize = true;
            this.lblLocalFile.Location = new System.Drawing.Point(6, 30);
            this.lblLocalFile.Name = "lblLocalFile";
            this.lblLocalFile.Size = new System.Drawing.Size(64, 19);
            this.lblLocalFile.TabIndex = 10;
            this.lblLocalFile.Text = "Local file:";
            // 
            // txtLocalFile
            // 
            this.txtLocalFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLocalFile.Location = new System.Drawing.Point(105, 28);
            this.txtLocalFile.Name = "txtLocalFile";
            this.txtLocalFile.Size = new System.Drawing.Size(455, 26);
            this.txtLocalFile.TabIndex = 20;
            // 
            // btnTransfer
            // 
            this.btnTransfer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTransfer.Image = global::mRemoteNG.Resources.SSHTransfer;
            this.btnTransfer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTransfer.Location = new System.Drawing.Point(579, 140);
            this.btnTransfer.Name = "btnTransfer";
            this.btnTransfer.Size = new System.Drawing.Size(83, 29);
            this.btnTransfer.TabIndex = 10000;
            this.btnTransfer.Text = "Transfer";
            this.btnTransfer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnTransfer.UseVisualStyleBackColor = true;
            this.btnTransfer.Click += new System.EventHandler(this.btnTransfer_Click);
            // 
            // txtRemoteFile
            // 
            this.txtRemoteFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRemoteFile.Location = new System.Drawing.Point(105, 60);
            this.txtRemoteFile.Name = "txtRemoteFile";
            this.txtRemoteFile.Size = new System.Drawing.Size(542, 26);
            this.txtRemoteFile.TabIndex = 50;
            // 
            // lblRemoteFile
            // 
            this.lblRemoteFile.AutoSize = true;
            this.lblRemoteFile.Location = new System.Drawing.Point(6, 67);
            this.lblRemoteFile.Name = "lblRemoteFile";
            this.lblRemoteFile.Size = new System.Drawing.Size(80, 19);
            this.lblRemoteFile.TabIndex = 40;
            this.lblRemoteFile.Text = "Remote file:";
            // 
            // btnBrowse
            // 
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowse.Location = new System.Drawing.Point(566, 28);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(81, 26);
            this.btnBrowse.TabIndex = 30;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // grpConnection
            // 
            this.grpConnection.Controls.Add(this.radProtSFTP);
            this.grpConnection.Controls.Add(this.radProtSCP);
            this.grpConnection.Controls.Add(this.lblProtocol);
            this.grpConnection.Controls.Add(this.lblPassword);
            this.grpConnection.Controls.Add(this.lblUser);
            this.grpConnection.Controls.Add(this.lblPort);
            this.grpConnection.Controls.Add(this.lblHost);
            this.grpConnection.Controls.Add(this.txtPort);
            this.grpConnection.Controls.Add(this.txtHost);
            this.grpConnection.Controls.Add(this.txtPassword);
            this.grpConnection.Controls.Add(this.txtUser);
            this.grpConnection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.grpConnection.Location = new System.Drawing.Point(12, 12);
            this.grpConnection.Name = "grpConnection";
            this.grpConnection.Size = new System.Drawing.Size(668, 154);
            this.grpConnection.TabIndex = 1000;
            this.grpConnection.TabStop = false;
            this.grpConnection.Text = "Connection";
            // 
            // radProtSFTP
            // 
            this.radProtSFTP.AutoSize = true;
            this.radProtSFTP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radProtSFTP.Location = new System.Drawing.Point(164, 113);
            this.radProtSFTP.Name = "radProtSFTP";
            this.radProtSFTP.Size = new System.Drawing.Size(58, 23);
            this.radProtSFTP.TabIndex = 90;
            this.radProtSFTP.Text = "SFTP";
            this.radProtSFTP.UseVisualStyleBackColor = true;
            // 
            // radProtSCP
            // 
            this.radProtSCP.AutoSize = true;
            this.radProtSCP.Checked = true;
            this.radProtSCP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radProtSCP.Location = new System.Drawing.Point(105, 113);
            this.radProtSCP.Name = "radProtSCP";
            this.radProtSCP.Size = new System.Drawing.Size(53, 23);
            this.radProtSCP.TabIndex = 80;
            this.radProtSCP.TabStop = true;
            this.radProtSCP.Text = "SCP";
            this.radProtSCP.UseVisualStyleBackColor = true;
            // 
            // lblProtocol
            // 
            this.lblProtocol.AutoSize = true;
            this.lblProtocol.Location = new System.Drawing.Point(6, 117);
            this.lblProtocol.Name = "lblProtocol";
            this.lblProtocol.Size = new System.Drawing.Size(63, 19);
            this.lblProtocol.TabIndex = 90;
            this.lblProtocol.Text = "Protocol:";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(6, 88);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(70, 19);
            this.lblPassword.TabIndex = 70;
            this.lblPassword.Text = "Password:";
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(6, 58);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(40, 19);
            this.lblUser.TabIndex = 50;
            this.lblUser.Text = "User:";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(228, 115);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(37, 19);
            this.lblPort.TabIndex = 30;
            this.lblPort.Text = "Port:";
            // 
            // lblHost
            // 
            this.lblHost.AutoSize = true;
            this.lblHost.Location = new System.Drawing.Point(6, 27);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(41, 19);
            this.lblHost.TabIndex = 10;
            this.lblHost.Text = "Host:";
            // 
            // txtPort
            // 
            this.txtPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPort.Location = new System.Drawing.Point(271, 110);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(30, 26);
            this.txtPort.TabIndex = 100;
            this.txtPort.Text = "22";
            this.txtPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtHost
            // 
            this.txtHost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHost.Location = new System.Drawing.Point(105, 19);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(471, 26);
            this.txtHost.TabIndex = 20;
            // 
            // txtPassword
            // 
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.Location = new System.Drawing.Point(105, 81);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(471, 26);
            this.txtPassword.TabIndex = 60;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // txtUser
            // 
            this.txtUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUser.Location = new System.Drawing.Point(105, 51);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(471, 26);
            this.txtUser.TabIndex = 40;
            // 
            // pbStatus
            // 
            this.pbStatus.Location = new System.Drawing.Point(12, 353);
            this.pbStatus.Name = "pbStatus";
            this.pbStatus.Size = new System.Drawing.Size(668, 23);
            this.pbStatus.TabIndex = 3000;
            // 
            // SSHTransferWindow
            // 
            this.ClientSize = new System.Drawing.Size(692, 423);
            this.Controls.Add(this.grpFiles);
            this.Controls.Add(this.grpConnection);
            this.Controls.Add(this.pbStatus);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            this.Name = "SSHTransferWindow";
            this.TabText = "SSH File Transfer";
            this.Text = "SSH File Transfer";
            this.Load += new System.EventHandler(this.SSHTransfer_Load);
            this.grpFiles.ResumeLayout(false);
            this.grpFiles.PerformLayout();
            this.grpConnection.ResumeLayout(false);
            this.grpConnection.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        #region Private Properties
        private OpenFileDialog oDlg;
        #endregion

        #region Public Properties
        public string Hostname
        {
            get
            {
                return txtHost.Text;
            }
            set
            {
                txtHost.Text = value;
            }
        }

        public string Port
        {
            get
            {
                return txtPort.Text;
            }
            set
            {
                txtPort.Text = value;
            }
        }

        public string Username
        {
            get
            {
                return txtUser.Text;
            }
            set
            {
                txtUser.Text = value;
            }
        }

        public string Password
        {
            get
            {
                return txtPassword.Text;
            }
            set
            {
                txtPassword.Text = value;
            }
        }
        #endregion

        #region Form Stuff
        private void SSHTransfer_Load(object sender, EventArgs e)
        {
            ApplyLanguage();
        }

        private void ApplyLanguage()
        {
            grpFiles.Text = Language.strGroupboxFiles;
            lblLocalFile.Text = Language.strLocalFile + ":";
            lblRemoteFile.Text = Language.strRemoteFile + ":";
            btnBrowse.Text = Language.strButtonBrowse;
            grpConnection.Text = Language.strGroupboxConnection;
            lblProtocol.Text = Language.strLabelProtocol;
            lblPassword.Text = Language.strLabelPassword;
            lblUser.Text = Language.strUser + ":";
            lblPort.Text = Language.strLabelPort;
            lblHost.Text = Language.strHost + ":";
            btnTransfer.Text = Language.strTransfer;
            TabText = Language.strMenuSSHFileTransfer;
            Text = Language.strMenuSSHFileTransfer;
        }
        #endregion

        #region Private Methods
        private SecureTransfer st;
        private void StartTransfer(SecureTransfer.SSHTransferProtocol Protocol)
        {
            if (AllFieldsSet() == false)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.strPleaseFillAllFields);
                return;
            }

            if (File.Exists(txtLocalFile.Text) == false)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, Language.strLocalFileDoesNotExist);
                return;
            }

            try
            {
                st = new SecureTransfer(txtHost.Text, txtUser.Text, txtPassword.Text, int.Parse(txtPort.Text), Protocol);
                st.SrcFile = txtLocalFile.Text;
                st.DstFile = txtRemoteFile.Text;
                st.Connect();

                if (Protocol == SecureTransfer.SSHTransferProtocol.SCP)
                {
                    st.ScpClt.Uploading += ScpClt_Uploading;
                }

                Thread t = new Thread(StartTransferBG);
                t.SetApartmentState(ApartmentState.STA);
                t.IsBackground = true;
                t.Start();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.strSSHTransferFailed + Environment.NewLine + ex.Message);
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.strSSHTransferFailed + Environment.NewLine + ex.StackTrace, true);
                st?.Disconnect();
                st?.Dispose();
            }
        }

        private void ScpClt_Uploading(object sender, Renci.SshNet.Common.ScpUploadEventArgs e)
        {
            // If the file size is over 2 gigs, convert to kb. This means we'll support a 2TB file.
            int max = e.Size > int.MaxValue ? Convert.ToInt32(e.Size / 1024) : Convert.ToInt32(e.Size);

            // yes, compare to size since that's the total/original file size
            int cur = e.Size > int.MaxValue ? Convert.ToInt32(e.Uploaded / 1024) : Convert.ToInt32(e.Uploaded);

            SshTransfer_Progress(cur, max);

            // Upload should be complete
            /*
            if (e.Size == e.Uploaded)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, $"Transfer of {e.Filename} completed.", true);
            }
            */
        }

        private void StartTransferBG()
        {
            try
            {
                DisableButtons();
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, $"Transfer of {Path.GetFileName(st.SrcFile)} started.", true);
                st.Upload();
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, $"Transfer of {Path.GetFileName(st.SrcFile)} completed.", true);
                st.Disconnect();
                st.Dispose();
                EnableButtons();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.strSSHStartTransferBG + Environment.NewLine + ex.StackTrace, true);
            }
        }

        private bool AllFieldsSet()
        {
            if (txtHost.Text != "" && txtPort.Text != "" && txtUser.Text != "" && txtLocalFile.Text != "" && txtRemoteFile.Text != "")
            {
                if (txtPassword.Text == "")
                {
                    if (MessageBox.Show(frmMain.Default, Language.strEmptyPasswordContinue, @"Question?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return false;
                    }
                }

                if (txtRemoteFile.Text.EndsWith("/") || txtRemoteFile.Text.EndsWith("\\"))
                {
                    txtRemoteFile.Text += txtLocalFile.Text.Substring(txtLocalFile.Text.LastIndexOf("\\", StringComparison.Ordinal) + 1);
                }

                return true;
            }
            else
            {
                return false;
            }
        }


        private int maxVal;
        private int curVal;

        delegate void SetStatusCB();
        private void SetStatus()
        {
            if (pbStatus.InvokeRequired)
            {
                SetStatusCB d = SetStatus;
                pbStatus.Invoke(d);
            }
            else
            {
                pbStatus.Maximum = maxVal;
                pbStatus.Value = curVal;
            }
        }

        delegate void EnableButtonsCB();
        private void EnableButtons()
        {
            if (btnTransfer.InvokeRequired)
            {
                EnableButtonsCB d = EnableButtons;
                btnTransfer.Invoke(d);
            }
            else
            {
                btnTransfer.Enabled = true;
            }
        }

        delegate void DisableButtonsCB();
        private void DisableButtons()
        {
            if (btnTransfer.InvokeRequired)
            {
                DisableButtonsCB d = DisableButtons;
                btnTransfer.Invoke(d);
            }
            else
            {
                btnTransfer.Enabled = false;
            }
        }

        /*	
		private void SshTransfer_Start(int transferredBytes, int totalBytes)
		{
			maxVal = totalBytes;
			curVal = transferredBytes;
					
			SetStatus();
		}
		*/

        private void SshTransfer_Progress(int transferredBytes, int totalBytes)
        {
            maxVal = totalBytes;
            curVal = transferredBytes;

            SetStatus();
        }

        /*		
		private void SshTransfer_End(int transferredBytes, int totalBytes)
		{
			try
			{
				maxVal = totalBytes;
				curVal = transferredBytes;
						
				EnableButtons();
				SetStatus();
						
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, Language.strSSHTransferFailed);
						
				if (st != null)
				{
                    st?.Disconnect();
                    st?.Dispose();
                }
				
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.strSSHTransferEndFailed + Environment.NewLine + ex.StackTrace, true);
			}
		}
        */
        #endregion

        #region Public Methods
        public SSHTransferWindow(DockContent Panel)
        {
            WindowType = WindowType.SSHTransfer;
            DockPnl = Panel;
            InitializeComponent();

            oDlg = new OpenFileDialog
            {
                Filter = @"All Files (*.*)|*.*",
                CheckFileExists = true
            };
        }
        #endregion

        #region Form Stuff
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (oDlg.ShowDialog() == DialogResult.OK)
            {
                if (oDlg.FileName != "")
                {
                    txtLocalFile.Text = oDlg.FileName;
                }
            }
        }

        private void btnTransfer_Click(object sender, EventArgs e)
        {
            if (radProtSCP.Checked)
            {
                StartTransfer(SecureTransfer.SSHTransferProtocol.SCP);
            }
            else if (radProtSFTP.Checked)
            {
                StartTransfer(SecureTransfer.SSHTransferProtocol.SFTP);
            }
        }
        #endregion
    }
}