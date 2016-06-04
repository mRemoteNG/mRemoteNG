using mRemoteNG.App;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.UI.Forms;
using Tamir.SharpSsh;
using WeifenLuo.WinFormsUI.Docking;


namespace mRemoteNG.UI.Window
{
	public class SSHTransferWindow : BaseWindow
	{
        #region Form Init
		internal ProgressBar pbStatus;
		internal Button btnTransfer;
		internal TextBox txtUser;
		internal TextBox txtPassword;
		internal TextBox txtHost;
		internal TextBox txtPort;
		internal Label lblHost;
		internal Label lblPort;
		internal Label lblUser;
		internal Label lblPassword;
		internal Label lblProtocol;
		internal RadioButton radProtSCP;
		internal RadioButton radProtSFTP;
		internal GroupBox grpConnection;
		internal Button btnBrowse;
		internal Label lblRemoteFile;
		internal TextBox txtRemoteFile;
		internal TextBox txtLocalFile;
		internal Label lblLocalFile;
		internal GroupBox grpFiles;
				
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SSHTransferWindow));
            grpFiles = new GroupBox();
            lblLocalFile = new Label();
            txtLocalFile = new TextBox();
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
            btnTransfer = new Button();
            pbStatus = new ProgressBar();
            grpFiles.SuspendLayout();
            grpConnection.SuspendLayout();
            SuspendLayout();
            // 
            // grpFiles
            // 
            grpFiles.Anchor = (((AnchorStyles.Top | AnchorStyles.Bottom)
            | AnchorStyles.Left)
            | AnchorStyles.Right);
            grpFiles.Controls.Add(lblLocalFile);
            grpFiles.Controls.Add(txtLocalFile);
            grpFiles.Controls.Add(txtRemoteFile);
            grpFiles.Controls.Add(lblRemoteFile);
            grpFiles.Controls.Add(btnBrowse);
            grpFiles.FlatStyle = FlatStyle.Flat;
            grpFiles.Location = new System.Drawing.Point(12, 153);
            grpFiles.Name = "grpFiles";
            grpFiles.Size = new System.Drawing.Size(668, 194);
            grpFiles.TabIndex = 2000;
            grpFiles.TabStop = false;
            grpFiles.Text = "Files";
            // 
            // lblLocalFile
            // 
            lblLocalFile.AutoSize = true;
            lblLocalFile.Location = new System.Drawing.Point(20, 25);
            lblLocalFile.Name = "lblLocalFile";
            lblLocalFile.Size = new System.Drawing.Size(55, 13);
            lblLocalFile.TabIndex = 10;
            lblLocalFile.Text = "Local file:";
            // 
            // txtLocalFile
            // 
            txtLocalFile.Anchor = ((AnchorStyles.Top | AnchorStyles.Left)
            | AnchorStyles.Right);
            txtLocalFile.BorderStyle = BorderStyle.FixedSingle;
            txtLocalFile.Location = new System.Drawing.Point(105, 23);
            txtLocalFile.Name = "txtLocalFile";
            txtLocalFile.Size = new System.Drawing.Size(455, 22);
            txtLocalFile.TabIndex = 20;
            // 
            // txtRemoteFile
            // 
            txtRemoteFile.Anchor = ((AnchorStyles.Top | AnchorStyles.Left)
            | AnchorStyles.Right);
            txtRemoteFile.BorderStyle = BorderStyle.FixedSingle;
            txtRemoteFile.Location = new System.Drawing.Point(105, 49);
            txtRemoteFile.Name = "txtRemoteFile";
            txtRemoteFile.Size = new System.Drawing.Size(542, 22);
            txtRemoteFile.TabIndex = 50;
            // 
            // lblRemoteFile
            // 
            lblRemoteFile.AutoSize = true;
            lblRemoteFile.Location = new System.Drawing.Point(20, 51);
            lblRemoteFile.Name = "lblRemoteFile";
            lblRemoteFile.Size = new System.Drawing.Size(68, 13);
            lblRemoteFile.TabIndex = 40;
            lblRemoteFile.Text = "Remote file:";
            // 
            // btnBrowse
            // 
            btnBrowse.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
            btnBrowse.FlatStyle = FlatStyle.Flat;
            btnBrowse.Location = new System.Drawing.Point(566, 21);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new System.Drawing.Size(81, 23);
            btnBrowse.TabIndex = 30;
            btnBrowse.Text = "Browse";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += new EventHandler(btnBrowse_Click);
            // 
            // grpConnection
            // 
            grpConnection.Anchor = ((AnchorStyles.Top | AnchorStyles.Left)
            | AnchorStyles.Right);
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
            grpConnection.Location = new System.Drawing.Point(12, 12);
            grpConnection.Name = "grpConnection";
            grpConnection.Size = new System.Drawing.Size(668, 135);
            grpConnection.TabIndex = 1000;
            grpConnection.TabStop = false;
            grpConnection.Text = "Connection";
            // 
            // radProtSFTP
            // 
            radProtSFTP.AutoSize = true;
            radProtSFTP.FlatStyle = FlatStyle.Flat;
            radProtSFTP.Location = new System.Drawing.Point(153, 103);
            radProtSFTP.Name = "radProtSFTP";
            radProtSFTP.Size = new System.Drawing.Size(47, 17);
            radProtSFTP.TabIndex = 110;
            radProtSFTP.Text = "SFTP";
            radProtSFTP.UseVisualStyleBackColor = true;
            // 
            // radProtSCP
            // 
            radProtSCP.AutoSize = true;
            radProtSCP.Checked = true;
            radProtSCP.FlatStyle = FlatStyle.Flat;
            radProtSCP.Location = new System.Drawing.Point(92, 103);
            radProtSCP.Name = "radProtSCP";
            radProtSCP.Size = new System.Drawing.Size(43, 17);
            radProtSCP.TabIndex = 100;
            radProtSCP.TabStop = true;
            radProtSCP.Text = "SCP";
            radProtSCP.UseVisualStyleBackColor = true;
            // 
            // lblProtocol
            // 
            lblProtocol.AutoSize = true;
            lblProtocol.Location = new System.Drawing.Point(20, 105);
            lblProtocol.Name = "lblProtocol";
            lblProtocol.Size = new System.Drawing.Size(53, 13);
            lblProtocol.TabIndex = 90;
            lblProtocol.Text = "Protocol:";
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new System.Drawing.Point(20, 79);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new System.Drawing.Size(59, 13);
            lblPassword.TabIndex = 70;
            lblPassword.Text = "Password:";
            // 
            // lblUser
            // 
            lblUser.AutoSize = true;
            lblUser.Location = new System.Drawing.Point(20, 53);
            lblUser.Name = "lblUser";
            lblUser.Size = new System.Drawing.Size(33, 13);
            lblUser.TabIndex = 50;
            lblUser.Text = "User:";
            // 
            // lblPort
            // 
            lblPort.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
            lblPort.AutoSize = true;
            lblPort.Location = new System.Drawing.Point(582, 27);
            lblPort.Name = "lblPort";
            lblPort.Size = new System.Drawing.Size(31, 13);
            lblPort.TabIndex = 30;
            lblPort.Text = "Port:";
            // 
            // lblHost
            // 
            lblHost.AutoSize = true;
            lblHost.Location = new System.Drawing.Point(20, 27);
            lblHost.Name = "lblHost";
            lblHost.Size = new System.Drawing.Size(34, 13);
            lblHost.TabIndex = 10;
            lblHost.Text = "Host:";
            // 
            // txtPort
            // 
            txtPort.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
            txtPort.BorderStyle = BorderStyle.FixedSingle;
            txtPort.Location = new System.Drawing.Point(617, 25);
            txtPort.Name = "txtPort";
            txtPort.Size = new System.Drawing.Size(30, 22);
            txtPort.TabIndex = 40;
            // 
            // txtHost
            // 
            txtHost.Anchor = ((AnchorStyles.Top | AnchorStyles.Left)
            | AnchorStyles.Right);
            txtHost.BorderStyle = BorderStyle.FixedSingle;
            txtHost.Location = new System.Drawing.Point(105, 25);
            txtHost.Name = "txtHost";
            txtHost.Size = new System.Drawing.Size(471, 22);
            txtHost.TabIndex = 20;
            // 
            // txtPassword
            // 
            txtPassword.Anchor = ((AnchorStyles.Top | AnchorStyles.Left)
            | AnchorStyles.Right);
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.Location = new System.Drawing.Point(105, 77);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new System.Drawing.Size(471, 22);
            txtPassword.TabIndex = 80;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // txtUser
            // 
            txtUser.Anchor = ((AnchorStyles.Top | AnchorStyles.Left)
            | AnchorStyles.Right);
            txtUser.BorderStyle = BorderStyle.FixedSingle;
            txtUser.Location = new System.Drawing.Point(105, 51);
            txtUser.Name = "txtUser";
            txtUser.Size = new System.Drawing.Size(471, 22);
            txtUser.TabIndex = 60;
            // 
            // btnTransfer
            // 
            btnTransfer.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            btnTransfer.FlatStyle = FlatStyle.Flat;
            btnTransfer.Image = Resources.SSHTransfer;
            btnTransfer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnTransfer.Location = new System.Drawing.Point(597, 382);
            btnTransfer.Name = "btnTransfer";
            btnTransfer.Size = new System.Drawing.Size(83, 29);
            btnTransfer.TabIndex = 10000;
            btnTransfer.Text = "Transfer";
            btnTransfer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            btnTransfer.UseVisualStyleBackColor = true;
            btnTransfer.Click += new EventHandler(btnTransfer_Click);
            // 
            // pbStatus
            // 
            pbStatus.Anchor = ((AnchorStyles.Bottom | AnchorStyles.Left)
            | AnchorStyles.Right);
            pbStatus.Location = new System.Drawing.Point(12, 353);
            pbStatus.Name = "pbStatus";
            pbStatus.Size = new System.Drawing.Size(668, 23);
            pbStatus.TabIndex = 3000;
            // 
            // SSHTransferWindow
            // 
            ClientSize = new System.Drawing.Size(692, 423);
            Controls.Add(grpFiles);
            Controls.Add(grpConnection);
            Controls.Add(btnTransfer);
            Controls.Add(pbStatus);
            Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            Name = "SSHTransferWindow";
            TabText = "SSH File Transfer";
            Text = "SSH File Transfer";
            Load += new EventHandler(SSHTransfer_Load);
            grpFiles.ResumeLayout(false);
            grpFiles.PerformLayout();
            grpConnection.ResumeLayout(false);
            grpConnection.PerformLayout();
            ResumeLayout(false);

		}
        #endregion
		
        #region Private Properties
		private SshTransferProtocolBase sshT;
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
		private void StartTransfer(SSHTransferProtocol Protocol)
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
				if (Protocol == SSHTransferProtocol.SCP)
				{
                    sshT = new Scp(txtHost.Text, txtUser.Text, txtPassword.Text);
				}
				else if (Protocol == SSHTransferProtocol.SFTP)
				{
                    sshT = new Sftp(txtHost.Text, txtUser.Text, txtPassword.Text);
				}
						
				sshT.OnTransferStart += SshTransfer_Start;
				sshT.OnTransferProgress += SshTransfer_Progress;
				sshT.OnTransferEnd += SshTransfer_End;

                sshT.Connect(Convert.ToInt32(txtPort.Text));
						
				LocalFile = txtLocalFile.Text;
				RemoteFile = txtRemoteFile.Text;
						
				Thread t = new Thread(new ThreadStart(StartTransferBG));
				t.SetApartmentState(ApartmentState.STA);
				t.IsBackground = true;
				t.Start();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.strSSHTransferFailed + Environment.NewLine + ex.Message);
                sshT.Close();
			}
		}
				
		private string LocalFile;
		private string RemoteFile;
				
		private void StartTransferBG()
		{
			try
			{
				DisableButtons();
                sshT.Put(LocalFile, RemoteFile);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.strSSHStartTransferBG + Environment.NewLine + ex.Message, true);
			}
		}
				
		private bool AllFieldsSet()
		{
			if (txtHost.Text != "" && txtPort.Text != "" && txtUser.Text != "" && txtLocalFile.Text != "" && txtRemoteFile.Text != "")
			{
				if (txtPassword.Text == "")
				{
                    if (MessageBox.Show(frmMain.Default, Language.strEmptyPasswordContinue, "Question?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
					{
						return false;
					}
				}
						
				if (txtRemoteFile.Text.EndsWith("/") || txtRemoteFile.Text.EndsWith("\\"))
				{
                    txtRemoteFile.Text += txtLocalFile.Text.Substring(txtLocalFile.Text.LastIndexOf("\\") + 1);
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
				SetStatusCB d = new SetStatusCB(SetStatus);
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
				EnableButtonsCB d = new EnableButtonsCB(EnableButtons);
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
				DisableButtonsCB d = new DisableButtonsCB(DisableButtons);
                btnTransfer.Invoke(d);
			}
			else
			{
				btnTransfer.Enabled = false;
			}
		}
				
		private void SshTransfer_Start(string src, string dst, int transferredBytes, int totalBytes, string message)
		{
			maxVal = totalBytes;
			curVal = transferredBytes;
					
			SetStatus();
		}
				
		private void SshTransfer_Progress(string src, string dst, int transferredBytes, int totalBytes, string message)
		{
			maxVal = totalBytes;
			curVal = transferredBytes;
					
			SetStatus();
		}
				
		private void SshTransfer_End(string src, string dst, int transferredBytes, int totalBytes, string message)
		{
			try
			{
				maxVal = totalBytes;
				curVal = transferredBytes;
						
				EnableButtons();
				SetStatus();
						
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, Language.strSSHTransferFailed);
						
				if (sshT != null)
				{
                    sshT.Close();
				}
				else if (sshT != null)
				{
                    sshT.Close();
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.strSSHTransferEndFailed + Environment.NewLine + ex.Message, true);
			}
		}
        #endregion
		
        #region Public Methods
		public SSHTransferWindow(DockContent Panel)
		{
            WindowType = WindowType.SSHTransfer;
            DockPnl = Panel;
            InitializeComponent();

            oDlg = new OpenFileDialog();
            oDlg.Filter = "All Files (*.*)|*.*";
            oDlg.CheckFileExists = true;
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
                StartTransfer(SSHTransferProtocol.SCP);
			}
			else if (radProtSFTP.Checked)
			{
                StartTransfer(SSHTransferProtocol.SFTP);
			}
		}
        #endregion
		
		private enum SSHTransferProtocol
		{
			SCP = 0,
			SFTP = 1
		}
	}
}