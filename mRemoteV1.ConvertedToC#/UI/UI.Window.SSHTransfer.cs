using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Tamir.SharpSsh;
using System.IO;
using System.Threading;
using mRemoteNG.App.Runtime;

namespace mRemoteNG.UI
{
	namespace Window
	{
		public class SSHTransfer : UI.Window.Base
		{

			#region "Form Init"
			internal System.Windows.Forms.ProgressBar pbStatus;
			private System.Windows.Forms.Button withEventsField_btnTransfer;
			internal System.Windows.Forms.Button btnTransfer {
				get { return withEventsField_btnTransfer; }
				set {
					if (withEventsField_btnTransfer != null) {
						withEventsField_btnTransfer.Click -= btnTransfer_Click;
					}
					withEventsField_btnTransfer = value;
					if (withEventsField_btnTransfer != null) {
						withEventsField_btnTransfer.Click += btnTransfer_Click;
					}
				}
			}
			internal System.Windows.Forms.TextBox txtUser;
			internal System.Windows.Forms.TextBox txtPassword;
			internal System.Windows.Forms.TextBox txtHost;
			internal System.Windows.Forms.TextBox txtPort;
			internal System.Windows.Forms.Label lblHost;
			internal System.Windows.Forms.Label lblPort;
			internal System.Windows.Forms.Label lblUser;
			internal System.Windows.Forms.Label lblPassword;
			internal System.Windows.Forms.Label lblProtocol;
			internal System.Windows.Forms.RadioButton radProtSCP;
			internal System.Windows.Forms.RadioButton radProtSFTP;
			internal System.Windows.Forms.GroupBox grpConnection;
			private System.Windows.Forms.Button withEventsField_btnBrowse;
			internal System.Windows.Forms.Button btnBrowse {
				get { return withEventsField_btnBrowse; }
				set {
					if (withEventsField_btnBrowse != null) {
						withEventsField_btnBrowse.Click -= btnBrowse_Click;
					}
					withEventsField_btnBrowse = value;
					if (withEventsField_btnBrowse != null) {
						withEventsField_btnBrowse.Click += btnBrowse_Click;
					}
				}
			}
			internal System.Windows.Forms.Label lblRemoteFile;
			internal System.Windows.Forms.TextBox txtRemoteFile;
			internal System.Windows.Forms.TextBox txtLocalFile;
			internal System.Windows.Forms.Label lblLocalFile;

			internal System.Windows.Forms.GroupBox grpFiles;
			private void InitializeComponent()
			{
				System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SSHTransfer));
				this.grpFiles = new System.Windows.Forms.GroupBox();
				this.lblLocalFile = new System.Windows.Forms.Label();
				this.txtLocalFile = new System.Windows.Forms.TextBox();
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
				this.btnTransfer = new System.Windows.Forms.Button();
				this.pbStatus = new System.Windows.Forms.ProgressBar();
				this.grpFiles.SuspendLayout();
				this.grpConnection.SuspendLayout();
				this.SuspendLayout();
				//
				//grpFiles
				//
				this.grpFiles.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
				this.grpFiles.Controls.Add(this.lblLocalFile);
				this.grpFiles.Controls.Add(this.txtLocalFile);
				this.grpFiles.Controls.Add(this.txtRemoteFile);
				this.grpFiles.Controls.Add(this.lblRemoteFile);
				this.grpFiles.Controls.Add(this.btnBrowse);
				this.grpFiles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
				this.grpFiles.Location = new System.Drawing.Point(12, 153);
				this.grpFiles.Name = "grpFiles";
				this.grpFiles.Size = new System.Drawing.Size(668, 194);
				this.grpFiles.TabIndex = 2000;
				this.grpFiles.TabStop = false;
				this.grpFiles.Text = "Files";
				//
				//lblLocalFile
				//
				this.lblLocalFile.AutoSize = true;
				this.lblLocalFile.Location = new System.Drawing.Point(20, 25);
				this.lblLocalFile.Name = "lblLocalFile";
				this.lblLocalFile.Size = new System.Drawing.Size(52, 13);
				this.lblLocalFile.TabIndex = 10;
				this.lblLocalFile.Text = "Local file:";
				//
				//txtLocalFile
				//
				this.txtLocalFile.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
				this.txtLocalFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
				this.txtLocalFile.Location = new System.Drawing.Point(105, 23);
				this.txtLocalFile.Name = "txtLocalFile";
				this.txtLocalFile.Size = new System.Drawing.Size(455, 20);
				this.txtLocalFile.TabIndex = 20;
				//
				//txtRemoteFile
				//
				this.txtRemoteFile.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
				this.txtRemoteFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
				this.txtRemoteFile.Location = new System.Drawing.Point(105, 49);
				this.txtRemoteFile.Name = "txtRemoteFile";
				this.txtRemoteFile.Size = new System.Drawing.Size(542, 20);
				this.txtRemoteFile.TabIndex = 50;
				//
				//lblRemoteFile
				//
				this.lblRemoteFile.AutoSize = true;
				this.lblRemoteFile.Location = new System.Drawing.Point(20, 51);
				this.lblRemoteFile.Name = "lblRemoteFile";
				this.lblRemoteFile.Size = new System.Drawing.Size(63, 13);
				this.lblRemoteFile.TabIndex = 40;
				this.lblRemoteFile.Text = "Remote file:";
				//
				//btnBrowse
				//
				this.btnBrowse.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
				this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
				this.btnBrowse.Location = new System.Drawing.Point(566, 21);
				this.btnBrowse.Name = "btnBrowse";
				this.btnBrowse.Size = new System.Drawing.Size(81, 23);
				this.btnBrowse.TabIndex = 30;
				this.btnBrowse.Text = "Browse";
				this.btnBrowse.UseVisualStyleBackColor = true;
				//
				//grpConnection
				//
				this.grpConnection.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
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
				this.grpConnection.Size = new System.Drawing.Size(668, 135);
				this.grpConnection.TabIndex = 1000;
				this.grpConnection.TabStop = false;
				this.grpConnection.Text = "Connection";
				//
				//radProtSFTP
				//
				this.radProtSFTP.AutoSize = true;
				this.radProtSFTP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
				this.radProtSFTP.Location = new System.Drawing.Point(153, 103);
				this.radProtSFTP.Name = "radProtSFTP";
				this.radProtSFTP.Size = new System.Drawing.Size(51, 17);
				this.radProtSFTP.TabIndex = 110;
				this.radProtSFTP.Text = "SFTP";
				this.radProtSFTP.UseVisualStyleBackColor = true;
				//
				//radProtSCP
				//
				this.radProtSCP.AutoSize = true;
				this.radProtSCP.Checked = true;
				this.radProtSCP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
				this.radProtSCP.Location = new System.Drawing.Point(92, 103);
				this.radProtSCP.Name = "radProtSCP";
				this.radProtSCP.Size = new System.Drawing.Size(45, 17);
				this.radProtSCP.TabIndex = 100;
				this.radProtSCP.TabStop = true;
				this.radProtSCP.Text = "SCP";
				this.radProtSCP.UseVisualStyleBackColor = true;
				//
				//lblProtocol
				//
				this.lblProtocol.AutoSize = true;
				this.lblProtocol.Location = new System.Drawing.Point(20, 105);
				this.lblProtocol.Name = "lblProtocol";
				this.lblProtocol.Size = new System.Drawing.Size(49, 13);
				this.lblProtocol.TabIndex = 90;
				this.lblProtocol.Text = "Protocol:";
				//
				//lblPassword
				//
				this.lblPassword.AutoSize = true;
				this.lblPassword.Location = new System.Drawing.Point(20, 79);
				this.lblPassword.Name = "lblPassword";
				this.lblPassword.Size = new System.Drawing.Size(56, 13);
				this.lblPassword.TabIndex = 70;
				this.lblPassword.Text = "Password:";
				//
				//lblUser
				//
				this.lblUser.AutoSize = true;
				this.lblUser.Location = new System.Drawing.Point(20, 53);
				this.lblUser.Name = "lblUser";
				this.lblUser.Size = new System.Drawing.Size(32, 13);
				this.lblUser.TabIndex = 50;
				this.lblUser.Text = "User:";
				//
				//lblPort
				//
				this.lblPort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
				this.lblPort.AutoSize = true;
				this.lblPort.Location = new System.Drawing.Point(582, 27);
				this.lblPort.Name = "lblPort";
				this.lblPort.Size = new System.Drawing.Size(29, 13);
				this.lblPort.TabIndex = 30;
				this.lblPort.Text = "Port:";
				//
				//lblHost
				//
				this.lblHost.AutoSize = true;
				this.lblHost.Location = new System.Drawing.Point(20, 27);
				this.lblHost.Name = "lblHost";
				this.lblHost.Size = new System.Drawing.Size(32, 13);
				this.lblHost.TabIndex = 10;
				this.lblHost.Text = "Host:";
				//
				//txtPort
				//
				this.txtPort.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
				this.txtPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
				this.txtPort.Location = new System.Drawing.Point(617, 25);
				this.txtPort.Name = "txtPort";
				this.txtPort.Size = new System.Drawing.Size(30, 20);
				this.txtPort.TabIndex = 40;
				//
				//txtHost
				//
				this.txtHost.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
				this.txtHost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
				this.txtHost.Location = new System.Drawing.Point(105, 25);
				this.txtHost.Name = "txtHost";
				this.txtHost.Size = new System.Drawing.Size(471, 20);
				this.txtHost.TabIndex = 20;
				//
				//txtPassword
				//
				this.txtPassword.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
				this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
				this.txtPassword.Location = new System.Drawing.Point(105, 77);
				this.txtPassword.Name = "txtPassword";
				this.txtPassword.PasswordChar = Strings.ChrW(42);
				this.txtPassword.Size = new System.Drawing.Size(471, 20);
				this.txtPassword.TabIndex = 80;
				//
				//txtUser
				//
				this.txtUser.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
				this.txtUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
				this.txtUser.Location = new System.Drawing.Point(105, 51);
				this.txtUser.Name = "txtUser";
				this.txtUser.Size = new System.Drawing.Size(471, 20);
				this.txtUser.TabIndex = 60;
				//
				//btnTransfer
				//
				this.btnTransfer.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
				this.btnTransfer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
				this.btnTransfer.Image = global::mRemoteNG.My.Resources.Resources.SSHTransfer;
				this.btnTransfer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
				this.btnTransfer.Location = new System.Drawing.Point(597, 382);
				this.btnTransfer.Name = "btnTransfer";
				this.btnTransfer.Size = new System.Drawing.Size(83, 29);
				this.btnTransfer.TabIndex = 10000;
				this.btnTransfer.Text = "Transfer";
				this.btnTransfer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
				this.btnTransfer.UseVisualStyleBackColor = true;
				//
				//pbStatus
				//
				this.pbStatus.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
				this.pbStatus.Location = new System.Drawing.Point(12, 353);
				this.pbStatus.Name = "pbStatus";
				this.pbStatus.Size = new System.Drawing.Size(668, 23);
				this.pbStatus.TabIndex = 3000;
				//
				//SSHTransfer
				//
				this.ClientSize = new System.Drawing.Size(692, 423);
				this.Controls.Add(this.grpFiles);
				this.Controls.Add(this.grpConnection);
				this.Controls.Add(this.btnTransfer);
				this.Controls.Add(this.pbStatus);
				this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
				this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
				this.Name = "SSHTransfer";
				this.TabText = "SSH File Transfer";
				this.Text = "SSH File Transfer";
				this.grpFiles.ResumeLayout(false);
				this.grpFiles.PerformLayout();
				this.grpConnection.ResumeLayout(false);
				this.grpConnection.PerformLayout();
				this.ResumeLayout(false);

			}
			#endregion

			#region "Private Properties"
			private SshTransferProtocolBase sshT;
				#endregion
			private OpenFileDialog oDlg;

			#region "Public Properties"
			public string Hostname {
				get { return this.txtHost.Text; }
				set { this.txtHost.Text = value; }
			}

			public string Port {
				get { return this.txtPort.Text; }
				set { this.txtPort.Text = value; }
			}

			public string Username {
				get { return this.txtUser.Text; }
				set { this.txtUser.Text = value; }
			}

			public string Password {
				get { return this.txtPassword.Text; }
				set { this.txtPassword.Text = value; }
			}
			#endregion

			#region "Form Stuff"
			private void SSHTransfer_Load(object sender, System.EventArgs e)
			{
				ApplyLanguage();
			}

			private void ApplyLanguage()
			{
				grpFiles.Text = mRemoteNG.My.Language.strGroupboxFiles;
				lblLocalFile.Text = mRemoteNG.My.Language.strLocalFile + ":";
				lblRemoteFile.Text = mRemoteNG.My.Language.strRemoteFile + ":";
				btnBrowse.Text = mRemoteNG.My.Language.strButtonBrowse;
				grpConnection.Text = mRemoteNG.My.Language.strGroupboxConnection;
				lblProtocol.Text = mRemoteNG.My.Language.strLabelProtocol;
				lblPassword.Text = mRemoteNG.My.Language.strLabelPassword;
				lblUser.Text = mRemoteNG.My.Language.strUser + ":";
				lblPort.Text = mRemoteNG.My.Language.strLabelPort;
				lblHost.Text = mRemoteNG.My.Language.strHost + ":";
				btnTransfer.Text = mRemoteNG.My.Language.strTransfer;
				TabText = mRemoteNG.My.Language.strMenuSSHFileTransfer;
				Text = mRemoteNG.My.Language.strMenuSSHFileTransfer;
			}
			#endregion

			#region "Private Methods"
			private void StartTransfer(SSHTransferProtocol Protocol)
			{
				if (AllFieldsSet() == false) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strPleaseFillAllFields);
					return;
				}

				if (File.Exists(this.txtLocalFile.Text) == false) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.WarningMsg, mRemoteNG.My.Language.strLocalFileDoesNotExist);
					return;
				}

				try {
					if (Protocol == SSHTransferProtocol.SCP) {
						this.sshT = new Scp(txtHost.Text, txtUser.Text, txtPassword.Text);
					} else if (Protocol == SSHTransferProtocol.SFTP) {
						this.sshT = new Sftp(txtHost.Text, txtUser.Text, txtPassword.Text);
					}

					sshT.OnTransferStart += SshTransfer_Start;
					sshT.OnTransferProgress += SshTransfer_Progress;
					sshT.OnTransferEnd += SshTransfer_End;

					this.sshT.Connect(this.txtPort.Text);

					LocalFile = this.txtLocalFile.Text;
					RemoteFile = this.txtRemoteFile.Text;

					Thread t = new Thread(StartTransferBG);
					t.SetApartmentState(ApartmentState.STA);
					t.IsBackground = true;
					t.Start();
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strSSHTransferFailed + Constants.vbNewLine + ex.Message);
					this.sshT.Close();
				}
			}

			private string LocalFile;

			private string RemoteFile;
			private void StartTransferBG()
			{
				try {
					DisableButtons();
					this.sshT.Put(LocalFile, RemoteFile);
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strSSHStartTransferBG + Constants.vbNewLine + ex.Message, true);
				}
			}

			private bool AllFieldsSet()
			{
				if (!string.IsNullOrEmpty(this.txtHost.Text) & !string.IsNullOrEmpty(this.txtPort.Text) & !string.IsNullOrEmpty(this.txtUser.Text) & !string.IsNullOrEmpty(this.txtLocalFile.Text) & !string.IsNullOrEmpty(this.txtRemoteFile.Text)) {
					if (string.IsNullOrEmpty(this.txtPassword.Text)) {
						if (Interaction.MsgBox(mRemoteNG.My.Language.strEmptyPasswordContinue, MsgBoxStyle.Question | MsgBoxStyle.YesNo) == MsgBoxResult.No) {
							return false;
						}
					}

					if (this.txtRemoteFile.Text.EndsWith("/") | this.txtRemoteFile.Text.EndsWith("\\")) {
						this.txtRemoteFile.Text += this.txtLocalFile.Text.Substring(this.txtLocalFile.Text.LastIndexOf("\\") + 1);
					}

					return true;
				} else {
					return false;
				}
			}


			private long maxVal;

			private long curVal;
			public delegate void SetStatusCB();
			private void SetStatus()
			{
				if (pbStatus.InvokeRequired) {
					SetStatusCB d = new SetStatusCB(SetStatus);
					this.pbStatus.Invoke(d);
				} else {
					pbStatus.Maximum = maxVal;
					pbStatus.Value = curVal;
				}
			}

			public delegate void EnableButtonsCB();
			private void EnableButtons()
			{
				if (btnTransfer.InvokeRequired) {
					EnableButtonsCB d = new EnableButtonsCB(EnableButtons);
					this.btnTransfer.Invoke(d);
				} else {
					btnTransfer.Enabled = true;
				}
			}

			public delegate void DisableButtonsCB();
			private void DisableButtons()
			{
				if (btnTransfer.InvokeRequired) {
					DisableButtonsCB d = new DisableButtonsCB(DisableButtons);
					this.btnTransfer.Invoke(d);
				} else {
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
				try {
					maxVal = totalBytes;
					curVal = transferredBytes;

					EnableButtons();
					SetStatus();

					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.InformationMsg, mRemoteNG.My.Language.strSSHTransferFailed);

					if (this.sshT != null) {
						this.sshT.Close();
					} else if (this.sshT != null) {
						this.sshT.Close();
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strSSHTransferEndFailed + Constants.vbNewLine + ex.Message, true);
				}
			}
			#endregion

			#region "Public Methods"
			public SSHTransfer(DockContent Panel)
			{
				Load += SSHTransfer_Load;
				this.WindowType = Type.SSHTransfer;
				this.DockPnl = Panel;
				this.InitializeComponent();

				this.oDlg = new OpenFileDialog();
				this.oDlg.Filter = "All Files (*.*)|*.*";
				this.oDlg.CheckFileExists = true;
			}
			#endregion

			#region "Form Stuff"
			private void btnBrowse_Click(System.Object sender, System.EventArgs e)
			{
				if (this.oDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
					if (!string.IsNullOrEmpty(this.oDlg.FileName)) {
						this.txtLocalFile.Text = this.oDlg.FileName;
					}
				}
			}

			private void btnTransfer_Click(System.Object sender, System.EventArgs e)
			{
				if (this.radProtSCP.Checked) {
					this.StartTransfer(SSHTransferProtocol.SCP);
				} else if (this.radProtSFTP.Checked) {
					this.StartTransfer(SSHTransferProtocol.SFTP);
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
}
