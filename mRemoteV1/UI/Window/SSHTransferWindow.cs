using mRemoteNG.App;
using System;
using System.IO;
using System.Threading;
using mRemoteNG.Tools;
using WeifenLuo.WinFormsUI.Docking;
using System.Windows.Forms;
using mRemoteNG.Messages;
using mRemoteNG.UI.Forms;

namespace mRemoteNG.UI.Window
{
    public class SSHTransferWindow : BaseWindow
    {
        #region Form Init

        private Controls.Base.NGProgressBar pbStatus;
        private Controls.Base.NGButton btnTransfer;
        private Controls.Base.NGTextBox txtUser;
        private Controls.Base.NGTextBox txtPassword;
        private Controls.Base.NGTextBox txtHost;
        private Controls.Base.NGTextBox txtPort;
        private Controls.Base.NGLabel lblHost;
        private Controls.Base.NGLabel lblPort;
        private Controls.Base.NGLabel lblUser;
        private Controls.Base.NGLabel lblPassword;
        private Controls.Base.NGLabel lblProtocol;
        private Controls.Base.NGRadioButton radProtSCP;
        private Controls.Base.NGRadioButton radProtSFTP;
        private Controls.Base.NGGroupBox grpConnection;
        private Controls.Base.NGButton btnBrowse;
        private Controls.Base.NGLabel lblRemoteFile;
        private Controls.Base.NGTextBox txtRemoteFile;
        private Controls.Base.NGTextBox txtLocalFile;
        private Controls.Base.NGLabel lblLocalFile;
        private Controls.Base.NGGroupBox grpFiles;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(SSHTransferWindow));
            this.grpFiles = new mRemoteNG.UI.Controls.Base.NGGroupBox();
            this.lblLocalFile = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.txtLocalFile = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.btnTransfer = new mRemoteNG.UI.Controls.Base.NGButton();
            this.txtRemoteFile = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.lblRemoteFile = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.btnBrowse = new mRemoteNG.UI.Controls.Base.NGButton();
            this.grpConnection = new mRemoteNG.UI.Controls.Base.NGGroupBox();
            this.radProtSFTP = new mRemoteNG.UI.Controls.Base.NGRadioButton();
            this.radProtSCP = new mRemoteNG.UI.Controls.Base.NGRadioButton();
            this.lblProtocol = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.lblPassword = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.lblUser = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.lblPort = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.lblHost = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.txtPort = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.txtHost = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.txtPassword = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.txtUser = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.pbStatus = new mRemoteNG.UI.Controls.Base.NGProgressBar();
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
            this.lblLocalFile.Size = new System.Drawing.Size(55, 13);
            this.lblLocalFile.TabIndex = 10;
            this.lblLocalFile.Text = "Local file:";
            // 
            // txtLocalFile
            // 
            this.txtLocalFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLocalFile.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular,
                                                             System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLocalFile.Location = new System.Drawing.Point(105, 28);
            this.txtLocalFile.Name = "txtLocalFile";
            this.txtLocalFile.Size = new System.Drawing.Size(455, 22);
            this.txtLocalFile.TabIndex = 20;
            // 
            // btnTransfer
            // 
            this.btnTransfer._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.btnTransfer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTransfer.Image = global::mRemoteNG.Resources.SSHTransfer;
            this.btnTransfer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTransfer.Location = new System.Drawing.Point(562, 145);
            this.btnTransfer.Name = "btnTransfer";
            this.btnTransfer.Size = new System.Drawing.Size(100, 24);
            this.btnTransfer.TabIndex = 10000;
            this.btnTransfer.Text = "Transfer";
            this.btnTransfer.UseVisualStyleBackColor = true;
            this.btnTransfer.Click += new System.EventHandler(this.btnTransfer_Click);
            // 
            // txtRemoteFile
            // 
            this.txtRemoteFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRemoteFile.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular,
                                                              System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRemoteFile.Location = new System.Drawing.Point(105, 60);
            this.txtRemoteFile.Name = "txtRemoteFile";
            this.txtRemoteFile.Size = new System.Drawing.Size(542, 22);
            this.txtRemoteFile.TabIndex = 50;
            // 
            // lblRemoteFile
            // 
            this.lblRemoteFile.AutoSize = true;
            this.lblRemoteFile.Location = new System.Drawing.Point(6, 67);
            this.lblRemoteFile.Name = "lblRemoteFile";
            this.lblRemoteFile.Size = new System.Drawing.Size(68, 13);
            this.lblRemoteFile.TabIndex = 40;
            this.lblRemoteFile.Text = "Remote file:";
            // 
            // btnBrowse
            // 
            this.btnBrowse._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowse.Location = new System.Drawing.Point(566, 28);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(81, 22);
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
            this.radProtSFTP.Size = new System.Drawing.Size(47, 17);
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
            this.radProtSCP.Size = new System.Drawing.Size(43, 17);
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
            this.lblProtocol.Size = new System.Drawing.Size(53, 13);
            this.lblProtocol.TabIndex = 90;
            this.lblProtocol.Text = "Protocol:";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(6, 88);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(59, 13);
            this.lblPassword.TabIndex = 70;
            this.lblPassword.Text = "Password:";
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(6, 58);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(33, 13);
            this.lblUser.TabIndex = 50;
            this.lblUser.Text = "User:";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(228, 115);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(31, 13);
            this.lblPort.TabIndex = 30;
            this.lblPort.Text = "Port:";
            // 
            // lblHost
            // 
            this.lblHost.AutoSize = true;
            this.lblHost.Location = new System.Drawing.Point(6, 27);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(34, 13);
            this.lblHost.TabIndex = 10;
            this.lblHost.Text = "Host:";
            // 
            // txtPort
            // 
            this.txtPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPort.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular,
                                                        System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPort.Location = new System.Drawing.Point(271, 110);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(30, 22);
            this.txtPort.TabIndex = 100;
            this.txtPort.Text = "22";
            this.txtPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtHost
            // 
            this.txtHost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHost.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular,
                                                        System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHost.Location = new System.Drawing.Point(105, 19);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(471, 22);
            this.txtHost.TabIndex = 20;
            // 
            // txtPassword
            // 
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular,
                                                            System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Location = new System.Drawing.Point(105, 81);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(471, 22);
            this.txtPassword.TabIndex = 60;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // txtUser
            // 
            this.txtUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUser.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular,
                                                        System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUser.Location = new System.Drawing.Point(105, 51);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(471, 22);
            this.txtUser.TabIndex = 40;
            // 
            // pbStatus
            // 
            this.pbStatus.Location = new System.Drawing.Point(12, 353);
            this.pbStatus.Name = "pbStatus";
            this.pbStatus.Size = new System.Drawing.Size(668, 23);
            this.pbStatus.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbStatus.TabIndex = 3000;
            // 
            // SSHTransferWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(692, 423);
            this.Controls.Add(this.grpFiles);
            this.Controls.Add(this.grpConnection);
            this.Controls.Add(this.pbStatus);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular,
                                                System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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

        private readonly OpenFileDialog oDlg;

        #endregion

        #region Public Properties

        public string Hostname
        {
            get => txtHost.Text;
            set => txtHost.Text = value;
        }

        public string Port
        {
            get => txtPort.Text;
            set => txtPort.Text = value;
        }

        public string Username
        {
            get => txtUser.Text;
            set => txtUser.Text = value;
        }

        public string Password
        {
            get => txtPassword.Text;
            set => txtPassword.Text = value;
        }

        #endregion

        #region Form Stuff

        private void SSHTransfer_Load(object sender, EventArgs e)
        {
            ApplyTheme();
            ApplyLanguage();
            var display = new DisplayProperties();
            btnTransfer.Image = display.ScaleImage(btnTransfer.Image);
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
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strPleaseFillAllFields);
                return;
            }

            if (File.Exists(txtLocalFile.Text) == false)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, Language.strLocalFileDoesNotExist);
                return;
            }

            try
            {
                st = new SecureTransfer(txtHost.Text, txtUser.Text, txtPassword.Text, int.Parse(txtPort.Text), Protocol,
                                        txtLocalFile.Text, txtRemoteFile.Text);

                // Connect creates the protocol objects and makes the initial connection.
                st.Connect();

                switch (Protocol)
                {
                    case SecureTransfer.SSHTransferProtocol.SCP:
                        st.ScpClt.Uploading += ScpClt_Uploading;
                        break;
                    case SecureTransfer.SSHTransferProtocol.SFTP:
                        st.asyncCallback = AsyncCallback;
                        break;
                }

                var t = new Thread(StartTransferBG);
                t.SetApartmentState(ApartmentState.STA);
                t.IsBackground = true;
                t.Start();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.strSSHTransferFailed, ex);
                st?.Disconnect();
                st?.Dispose();
            }
        }

        private void AsyncCallback(IAsyncResult ar)
        {
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, $"SFTP AsyncCallback completed.", true);
        }

        private void ScpClt_Uploading(object sender, Renci.SshNet.Common.ScpUploadEventArgs e)
        {
            // If the file size is over 2 gigs, convert to kb. This means we'll support a 2TB file.
            var max = e.Size > int.MaxValue ? Convert.ToInt32(e.Size / 1024) : Convert.ToInt32(e.Size);

            // yes, compare to size since that's the total/original file size
            var cur = e.Size > int.MaxValue ? Convert.ToInt32(e.Uploaded / 1024) : Convert.ToInt32(e.Uploaded);

            SshTransfer_Progress(cur, max);
        }

        private void StartTransferBG()
        {
            try
            {
                DisableButtons();
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                                                    $"Transfer of {Path.GetFileName(st.SrcFile)} started.", true);
                st.Upload();

                // SftpClient is Asynchronous, so we need to wait here after the upload and handle the status directly since no status events are raised.
                if (st.Protocol == SecureTransfer.SSHTransferProtocol.SFTP)
                {
                    var fi = new FileInfo(st.SrcFile);
                    while (!st.asyncResult.IsCompleted)
                    {
                        var max = fi.Length > int.MaxValue
                            ? Convert.ToInt32(fi.Length / 1024)
                            : Convert.ToInt32(fi.Length);

                        var cur = fi.Length > int.MaxValue
                            ? Convert.ToInt32(st.asyncResult.UploadedBytes / 1024)
                            : Convert.ToInt32(st.asyncResult.UploadedBytes);
                        SshTransfer_Progress(cur, max);
                        Thread.Sleep(50);
                    }
                }

                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                                                    $"Transfer of {Path.GetFileName(st.SrcFile)} completed.", true);
                st.Disconnect();
                st.Dispose();
                EnableButtons();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.strSSHStartTransferBG, ex,
                                                                MessageClass.ErrorMsg, false);
                st?.Disconnect();
                st?.Dispose();
            }
        }

        private bool AllFieldsSet()
        {
            if (txtHost.Text != "" && txtPort.Text != "" && txtUser.Text != "" && txtLocalFile.Text != "" &&
                txtRemoteFile.Text != "")
            {
                if (txtPassword.Text == "")
                {
                    if (MessageBox.Show(FrmMain.Default, Language.strEmptyPasswordContinue, @"Question?",
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return false;
                    }
                }

                if (txtRemoteFile.Text.EndsWith("/") || txtRemoteFile.Text.EndsWith("\\"))
                {
                    txtRemoteFile.Text +=
                        txtLocalFile.Text.Substring(txtLocalFile.Text.LastIndexOf("\\", StringComparison.Ordinal) + 1);
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

        private delegate void SetStatusCB();

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

        private delegate void EnableButtonsCB();

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

        private delegate void DisableButtonsCB();

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

        private void SshTransfer_Progress(int transferredBytes, int totalBytes)
        {
            maxVal = totalBytes;
            curVal = transferredBytes;

            SetStatus();
        }

        #endregion

        #region Public Methods

        public SSHTransferWindow()
        {
            WindowType = WindowType.SSHTransfer;
            DockPnl = new DockContent();
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
            if (oDlg.ShowDialog() != DialogResult.OK) return;
            if (oDlg.FileName != "")
            {
                txtLocalFile.Text = oDlg.FileName;
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