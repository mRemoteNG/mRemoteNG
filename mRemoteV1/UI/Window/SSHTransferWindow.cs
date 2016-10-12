using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Tools;
using mRemoteNG.UI.Forms;
using Renci.SshNet.Common;
using WeifenLuo.WinFormsUI.Docking;
using TextBox = System.Windows.Forms.TextBox;

namespace mRemoteNG.UI.Window
{
    public class SSHTransferWindow : BaseWindow
    {
        #region Private Properties

        private readonly OpenFileDialog oDlg;

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
            var resources = new ComponentResourceManager(typeof(SSHTransferWindow));
            grpFiles = new GroupBox();
            lblLocalFile = new Label();
            txtLocalFile = new TextBox();
            btnTransfer = new Button();
            txtRemoteFile = new TextBox();
            lblRemoteFile = new Label();
            btnBrowse = new Button();
            grpConnection = new GroupBox();
            radProtSFTP = new RadioButton();
            radProtSCP = new RadioButton();
            lblProtocol = new Label();
            lblPassword = new Label();
            lblUser = new Label();
            lblPort = new Label();
            lblHost = new Label();
            txtPort = new TextBox();
            txtHost = new TextBox();
            txtPassword = new TextBox();
            txtUser = new TextBox();
            pbStatus = new ProgressBar();
            grpFiles.SuspendLayout();
            grpConnection.SuspendLayout();
            SuspendLayout();
            // 
            // grpFiles
            // 
            grpFiles.Controls.Add(lblLocalFile);
            grpFiles.Controls.Add(txtLocalFile);
            grpFiles.Controls.Add(btnTransfer);
            grpFiles.Controls.Add(txtRemoteFile);
            grpFiles.Controls.Add(lblRemoteFile);
            grpFiles.Controls.Add(btnBrowse);
            grpFiles.FlatStyle = FlatStyle.Flat;
            grpFiles.Location = new Point(12, 172);
            grpFiles.Name = "grpFiles";
            grpFiles.Size = new Size(668, 175);
            grpFiles.TabIndex = 2000;
            grpFiles.TabStop = false;
            grpFiles.Text = "Files";
            // 
            // lblLocalFile
            // 
            lblLocalFile.AutoSize = true;
            lblLocalFile.Location = new Point(6, 30);
            lblLocalFile.Name = "lblLocalFile";
            lblLocalFile.Size = new Size(55, 13);
            lblLocalFile.TabIndex = 10;
            lblLocalFile.Text = "Local file:";
            // 
            // txtLocalFile
            // 
            txtLocalFile.BorderStyle = BorderStyle.FixedSingle;
            txtLocalFile.Location = new Point(105, 28);
            txtLocalFile.Name = "txtLocalFile";
            txtLocalFile.Size = new Size(455, 22);
            txtLocalFile.TabIndex = 20;
            // 
            // btnTransfer
            // 
            btnTransfer.FlatStyle = FlatStyle.Flat;
            btnTransfer.Image = Resources.SSHTransfer;
            btnTransfer.ImageAlign = ContentAlignment.MiddleLeft;
            btnTransfer.Location = new Point(579, 140);
            btnTransfer.Name = "btnTransfer";
            btnTransfer.Size = new Size(83, 29);
            btnTransfer.TabIndex = 10000;
            btnTransfer.Text = "Transfer";
            btnTransfer.TextAlign = ContentAlignment.MiddleRight;
            btnTransfer.UseVisualStyleBackColor = true;
            btnTransfer.Click += btnTransfer_Click;
            // 
            // txtRemoteFile
            // 
            txtRemoteFile.BorderStyle = BorderStyle.FixedSingle;
            txtRemoteFile.Location = new Point(105, 60);
            txtRemoteFile.Name = "txtRemoteFile";
            txtRemoteFile.Size = new Size(542, 22);
            txtRemoteFile.TabIndex = 50;
            // 
            // lblRemoteFile
            // 
            lblRemoteFile.AutoSize = true;
            lblRemoteFile.Location = new Point(6, 67);
            lblRemoteFile.Name = "lblRemoteFile";
            lblRemoteFile.Size = new Size(68, 13);
            lblRemoteFile.TabIndex = 40;
            lblRemoteFile.Text = "Remote file:";
            // 
            // btnBrowse
            // 
            btnBrowse.FlatStyle = FlatStyle.Flat;
            btnBrowse.Location = new Point(566, 28);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(81, 26);
            btnBrowse.TabIndex = 30;
            btnBrowse.Text = "Browse";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += btnBrowse_Click;
            // 
            // grpConnection
            // 
            grpConnection.Controls.Add(radProtSFTP);
            grpConnection.Controls.Add(radProtSCP);
            grpConnection.Controls.Add(lblProtocol);
            grpConnection.Controls.Add(lblPassword);
            grpConnection.Controls.Add(lblUser);
            grpConnection.Controls.Add(lblPort);
            grpConnection.Controls.Add(lblHost);
            grpConnection.Controls.Add(txtPort);
            grpConnection.Controls.Add(txtHost);
            grpConnection.Controls.Add(txtPassword);
            grpConnection.Controls.Add(txtUser);
            grpConnection.FlatStyle = FlatStyle.Flat;
            grpConnection.Location = new Point(12, 12);
            grpConnection.Name = "grpConnection";
            grpConnection.Size = new Size(668, 154);
            grpConnection.TabIndex = 1000;
            grpConnection.TabStop = false;
            grpConnection.Text = "Connection";
            // 
            // radProtSFTP
            // 
            radProtSFTP.AutoSize = true;
            radProtSFTP.FlatStyle = FlatStyle.Flat;
            radProtSFTP.Location = new Point(164, 113);
            radProtSFTP.Name = "radProtSFTP";
            radProtSFTP.Size = new Size(47, 17);
            radProtSFTP.TabIndex = 90;
            radProtSFTP.Text = "SFTP";
            radProtSFTP.UseVisualStyleBackColor = true;
            // 
            // radProtSCP
            // 
            radProtSCP.AutoSize = true;
            radProtSCP.Checked = true;
            radProtSCP.FlatStyle = FlatStyle.Flat;
            radProtSCP.Location = new Point(105, 113);
            radProtSCP.Name = "radProtSCP";
            radProtSCP.Size = new Size(43, 17);
            radProtSCP.TabIndex = 80;
            radProtSCP.TabStop = true;
            radProtSCP.Text = "SCP";
            radProtSCP.UseVisualStyleBackColor = true;
            // 
            // lblProtocol
            // 
            lblProtocol.AutoSize = true;
            lblProtocol.Location = new Point(6, 117);
            lblProtocol.Name = "lblProtocol";
            lblProtocol.Size = new Size(53, 13);
            lblProtocol.TabIndex = 90;
            lblProtocol.Text = "Protocol:";
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(6, 88);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(59, 13);
            lblPassword.TabIndex = 70;
            lblPassword.Text = "Password:";
            // 
            // lblUser
            // 
            lblUser.AutoSize = true;
            lblUser.Location = new Point(6, 58);
            lblUser.Name = "lblUser";
            lblUser.Size = new Size(33, 13);
            lblUser.TabIndex = 50;
            lblUser.Text = "User:";
            // 
            // lblPort
            // 
            lblPort.AutoSize = true;
            lblPort.Location = new Point(228, 115);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(31, 13);
            lblPort.TabIndex = 30;
            lblPort.Text = "Port:";
            // 
            // lblHost
            // 
            lblHost.AutoSize = true;
            lblHost.Location = new Point(6, 27);
            lblHost.Name = "lblHost";
            lblHost.Size = new Size(34, 13);
            lblHost.TabIndex = 10;
            lblHost.Text = "Host:";
            // 
            // txtPort
            // 
            txtPort.BorderStyle = BorderStyle.FixedSingle;
            txtPort.Location = new Point(271, 110);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(30, 22);
            txtPort.TabIndex = 100;
            txtPort.Text = "22";
            txtPort.TextAlign = HorizontalAlignment.Center;
            // 
            // txtHost
            // 
            txtHost.BorderStyle = BorderStyle.FixedSingle;
            txtHost.Location = new Point(105, 19);
            txtHost.Name = "txtHost";
            txtHost.Size = new Size(471, 22);
            txtHost.TabIndex = 20;
            // 
            // txtPassword
            // 
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.Location = new Point(105, 81);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(471, 22);
            txtPassword.TabIndex = 60;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // txtUser
            // 
            txtUser.BorderStyle = BorderStyle.FixedSingle;
            txtUser.Location = new Point(105, 51);
            txtUser.Name = "txtUser";
            txtUser.Size = new Size(471, 22);
            txtUser.TabIndex = 40;
            // 
            // pbStatus
            // 
            pbStatus.Location = new Point(12, 353);
            pbStatus.Name = "pbStatus";
            pbStatus.Size = new Size(668, 23);
            pbStatus.Style = ProgressBarStyle.Continuous;
            pbStatus.TabIndex = 3000;
            // 
            // SSHTransferWindow
            // 
            ClientSize = new Size(692, 423);
            Controls.Add(grpFiles);
            Controls.Add(grpConnection);
            Controls.Add(pbStatus);
            Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Icon = (Icon) resources.GetObject("$this.Icon");
            Name = "SSHTransferWindow";
            TabText = "SSH File Transfer";
            Text = "SSH File Transfer";
            Load += SSHTransfer_Load;
            grpFiles.ResumeLayout(false);
            grpFiles.PerformLayout();
            grpConnection.ResumeLayout(false);
            grpConnection.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        #region Public Properties

        public string Hostname
        {
            get { return txtHost.Text; }
            set { txtHost.Text = value; }
        }

        public string Port
        {
            get { return txtPort.Text; }
            set { txtPort.Text = value; }
        }

        public string Username
        {
            get { return txtUser.Text; }
            set { txtUser.Text = value; }
        }

        public string Password
        {
            get { return txtPassword.Text; }
            set { txtPassword.Text = value; }
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

                if (Protocol == SecureTransfer.SSHTransferProtocol.SCP)
                    st.ScpClt.Uploading += ScpClt_Uploading;

                if (Protocol == SecureTransfer.SSHTransferProtocol.SFTP)
                    st.asyncCallback = AsyncCallback;

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

        private void ScpClt_Uploading(object sender, ScpUploadEventArgs e)
        {
            // If the file size is over 2 gigs, convert to kb. This means we'll support a 2TB file.
            var max = e.Size > int.MaxValue ? Convert.ToInt32(e.Size/1024) : Convert.ToInt32(e.Size);

            // yes, compare to size since that's the total/original file size
            var cur = e.Size > int.MaxValue ? Convert.ToInt32(e.Uploaded/1024) : Convert.ToInt32(e.Uploaded);

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
                            ? Convert.ToInt32(fi.Length/1024)
                            : Convert.ToInt32(fi.Length);

                        var cur = fi.Length > int.MaxValue
                            ? Convert.ToInt32(st.asyncResult.UploadedBytes/1024)
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
                Runtime.MessageCollector.AddExceptionStackTrace(Language.strSSHStartTransferBG, ex);
                st?.Disconnect();
                st?.Dispose();
            }
        }

        private bool AllFieldsSet()
        {
            if ((txtHost.Text != "") && (txtPort.Text != "") && (txtUser.Text != "") && (txtLocalFile.Text != "") &&
                (txtRemoteFile.Text != ""))
            {
                if (txtPassword.Text == "")
                    if (
                        MessageBox.Show(frmMain.Default, Language.strEmptyPasswordContinue, @"Question?",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return false;

                if (txtRemoteFile.Text.EndsWith("/") || txtRemoteFile.Text.EndsWith("\\"))
                    txtRemoteFile.Text +=
                        txtLocalFile.Text.Substring(txtLocalFile.Text.LastIndexOf("\\", StringComparison.Ordinal) + 1);

                return true;
            }
            return false;
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

        #region Form Stuff

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (oDlg.ShowDialog() == DialogResult.OK)
                if (oDlg.FileName != "")
                    txtLocalFile.Text = oDlg.FileName;
        }

        private void btnTransfer_Click(object sender, EventArgs e)
        {
            if (radProtSCP.Checked)
                StartTransfer(SecureTransfer.SSHTransferProtocol.SCP);
            else if (radProtSFTP.Checked)
                StartTransfer(SecureTransfer.SSHTransferProtocol.SFTP);
        }

        #endregion
    }
}