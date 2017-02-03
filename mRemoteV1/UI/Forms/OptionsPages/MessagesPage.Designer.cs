

namespace mRemoteNG.UI.Forms.OptionsPages
{
	
    public partial class MessagesPage : OptionsPage
	{
			
		//UserControl overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && components != null)
				{
					components.Dispose();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}
			
		//Required by the Windows Form Designer
		private System.ComponentModel.Container components = null;
			
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessagesPage));
            this.lblSwitchToErrorsAndInfos = new System.Windows.Forms.Label();
            this.chkSwitchToMCInformation = new System.Windows.Forms.CheckBox();
            this.chkSwitchToMCErrors = new System.Windows.Forms.CheckBox();
            this.chkSwitchToMCWarnings = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkShowErrorInMC = new System.Windows.Forms.CheckBox();
            this.chkShowWarningInMC = new System.Windows.Forms.CheckBox();
            this.chkShowInfoInMC = new System.Windows.Forms.CheckBox();
            this.chkShowDebugInMC = new System.Windows.Forms.CheckBox();
            this.groupBoxLogging = new System.Windows.Forms.GroupBox();
            this.buttonOpenLogFile = new System.Windows.Forms.Button();
            this.buttonSelectLogPath = new System.Windows.Forms.Button();
            this.labelLogTheseMsgTypes = new System.Windows.Forms.Label();
            this.chkLogErrorMsgs = new System.Windows.Forms.CheckBox();
            this.labelLogFilePath = new System.Windows.Forms.Label();
            this.chkLogWarningMsgs = new System.Windows.Forms.CheckBox();
            this.textBoxLogPath = new System.Windows.Forms.TextBox();
            this.chkLogInfoMsgs = new System.Windows.Forms.CheckBox();
            this.chkLogDebugMsgs = new System.Windows.Forms.CheckBox();
            this.saveFileDialogLogging = new System.Windows.Forms.SaveFileDialog();
            this.buttonRestoreDefaultLogPath = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBoxLogging.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSwitchToErrorsAndInfos
            // 
            this.lblSwitchToErrorsAndInfos.AutoSize = true;
            this.lblSwitchToErrorsAndInfos.Location = new System.Drawing.Point(177, 26);
            this.lblSwitchToErrorsAndInfos.Name = "lblSwitchToErrorsAndInfos";
            this.lblSwitchToErrorsAndInfos.Size = new System.Drawing.Size(159, 13);
            this.lblSwitchToErrorsAndInfos.TabIndex = 24;
            this.lblSwitchToErrorsAndInfos.Text = "Switch to Notifications panel on:";
            // 
            // chkSwitchToMCInformation
            // 
            this.chkSwitchToMCInformation.AutoSize = true;
            this.chkSwitchToMCInformation.Location = new System.Drawing.Point(195, 68);
            this.chkSwitchToMCInformation.Name = "chkSwitchToMCInformation";
            this.chkSwitchToMCInformation.Size = new System.Drawing.Size(78, 17);
            this.chkSwitchToMCInformation.TabIndex = 25;
            this.chkSwitchToMCInformation.Text = "Information";
            this.chkSwitchToMCInformation.UseVisualStyleBackColor = true;
            // 
            // chkSwitchToMCErrors
            // 
            this.chkSwitchToMCErrors.AutoSize = true;
            this.chkSwitchToMCErrors.Location = new System.Drawing.Point(195, 114);
            this.chkSwitchToMCErrors.Name = "chkSwitchToMCErrors";
            this.chkSwitchToMCErrors.Size = new System.Drawing.Size(48, 17);
            this.chkSwitchToMCErrors.TabIndex = 27;
            this.chkSwitchToMCErrors.Text = "Error";
            this.chkSwitchToMCErrors.UseVisualStyleBackColor = true;
            // 
            // chkSwitchToMCWarnings
            // 
            this.chkSwitchToMCWarnings.AutoSize = true;
            this.chkSwitchToMCWarnings.Location = new System.Drawing.Point(195, 91);
            this.chkSwitchToMCWarnings.Name = "chkSwitchToMCWarnings";
            this.chkSwitchToMCWarnings.Size = new System.Drawing.Size(66, 17);
            this.chkSwitchToMCWarnings.TabIndex = 26;
            this.chkSwitchToMCWarnings.Text = "Warning";
            this.chkSwitchToMCWarnings.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lblSwitchToErrorsAndInfos);
            this.groupBox1.Controls.Add(this.chkSwitchToMCErrors);
            this.groupBox1.Controls.Add(this.chkShowErrorInMC);
            this.groupBox1.Controls.Add(this.chkSwitchToMCInformation);
            this.groupBox1.Controls.Add(this.chkShowWarningInMC);
            this.groupBox1.Controls.Add(this.chkSwitchToMCWarnings);
            this.groupBox1.Controls.Add(this.chkShowInfoInMC);
            this.groupBox1.Controls.Add(this.chkShowDebugInMC);
            this.groupBox1.Location = new System.Drawing.Point(6, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(601, 141);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Notifications Panel";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Show these message types:";
            // 
            // chkShowErrorInMC
            // 
            this.chkShowErrorInMC.AutoSize = true;
            this.chkShowErrorInMC.Location = new System.Drawing.Point(20, 114);
            this.chkShowErrorInMC.Name = "chkShowErrorInMC";
            this.chkShowErrorInMC.Size = new System.Drawing.Size(48, 17);
            this.chkShowErrorInMC.TabIndex = 32;
            this.chkShowErrorInMC.Text = "Error";
            this.chkShowErrorInMC.UseVisualStyleBackColor = true;
            // 
            // chkShowWarningInMC
            // 
            this.chkShowWarningInMC.AutoSize = true;
            this.chkShowWarningInMC.Location = new System.Drawing.Point(20, 91);
            this.chkShowWarningInMC.Name = "chkShowWarningInMC";
            this.chkShowWarningInMC.Size = new System.Drawing.Size(66, 17);
            this.chkShowWarningInMC.TabIndex = 31;
            this.chkShowWarningInMC.Text = "Warning";
            this.chkShowWarningInMC.UseVisualStyleBackColor = true;
            // 
            // chkShowInfoInMC
            // 
            this.chkShowInfoInMC.AutoSize = true;
            this.chkShowInfoInMC.Location = new System.Drawing.Point(20, 68);
            this.chkShowInfoInMC.Name = "chkShowInfoInMC";
            this.chkShowInfoInMC.Size = new System.Drawing.Size(78, 17);
            this.chkShowInfoInMC.TabIndex = 30;
            this.chkShowInfoInMC.Text = "Information";
            this.chkShowInfoInMC.UseVisualStyleBackColor = true;
            // 
            // chkShowDebugInMC
            // 
            this.chkShowDebugInMC.AutoSize = true;
            this.chkShowDebugInMC.Location = new System.Drawing.Point(20, 45);
            this.chkShowDebugInMC.Name = "chkShowDebugInMC";
            this.chkShowDebugInMC.Size = new System.Drawing.Size(58, 17);
            this.chkShowDebugInMC.TabIndex = 29;
            this.chkShowDebugInMC.Text = "Debug";
            this.chkShowDebugInMC.UseVisualStyleBackColor = true;
            // 
            // groupBoxLogging
            // 
            this.groupBoxLogging.Controls.Add(this.buttonRestoreDefaultLogPath);
            this.groupBoxLogging.Controls.Add(this.buttonOpenLogFile);
            this.groupBoxLogging.Controls.Add(this.buttonSelectLogPath);
            this.groupBoxLogging.Controls.Add(this.labelLogTheseMsgTypes);
            this.groupBoxLogging.Controls.Add(this.chkLogErrorMsgs);
            this.groupBoxLogging.Controls.Add(this.labelLogFilePath);
            this.groupBoxLogging.Controls.Add(this.chkLogWarningMsgs);
            this.groupBoxLogging.Controls.Add(this.textBoxLogPath);
            this.groupBoxLogging.Controls.Add(this.chkLogInfoMsgs);
            this.groupBoxLogging.Controls.Add(this.chkLogDebugMsgs);
            this.groupBoxLogging.Location = new System.Drawing.Point(6, 150);
            this.groupBoxLogging.Name = "groupBoxLogging";
            this.groupBoxLogging.Size = new System.Drawing.Size(601, 140);
            this.groupBoxLogging.TabIndex = 29;
            this.groupBoxLogging.TabStop = false;
            this.groupBoxLogging.Text = "Logging";
            // 
            // buttonOpenLogFile
            // 
            this.buttonOpenLogFile.Location = new System.Drawing.Point(274, 68);
            this.buttonOpenLogFile.Name = "buttonOpenLogFile";
            this.buttonOpenLogFile.Size = new System.Drawing.Size(105, 23);
            this.buttonOpenLogFile.TabIndex = 30;
            this.buttonOpenLogFile.Text = "Open File";
            this.buttonOpenLogFile.UseVisualStyleBackColor = true;
            // 
            // buttonSelectLogPath
            // 
            this.buttonSelectLogPath.Location = new System.Drawing.Point(385, 68);
            this.buttonSelectLogPath.Name = "buttonSelectLogPath";
            this.buttonSelectLogPath.Size = new System.Drawing.Size(105, 23);
            this.buttonSelectLogPath.TabIndex = 30;
            this.buttonSelectLogPath.Text = "Choose Path";
            this.buttonSelectLogPath.UseVisualStyleBackColor = true;
            this.buttonSelectLogPath.Click += new System.EventHandler(this.buttonSelectLogPath_Click);
            // 
            // labelLogTheseMsgTypes
            // 
            this.labelLogTheseMsgTypes.AutoSize = true;
            this.labelLogTheseMsgTypes.Location = new System.Drawing.Point(6, 26);
            this.labelLogTheseMsgTypes.Name = "labelLogTheseMsgTypes";
            this.labelLogTheseMsgTypes.Size = new System.Drawing.Size(130, 13);
            this.labelLogTheseMsgTypes.TabIndex = 34;
            this.labelLogTheseMsgTypes.Text = "Log these message types:";
            // 
            // chkLogErrorMsgs
            // 
            this.chkLogErrorMsgs.AutoSize = true;
            this.chkLogErrorMsgs.Location = new System.Drawing.Point(20, 114);
            this.chkLogErrorMsgs.Name = "chkLogErrorMsgs";
            this.chkLogErrorMsgs.Size = new System.Drawing.Size(48, 17);
            this.chkLogErrorMsgs.TabIndex = 38;
            this.chkLogErrorMsgs.Text = "Error";
            this.chkLogErrorMsgs.UseVisualStyleBackColor = true;
            // 
            // labelLogFilePath
            // 
            this.labelLogFilePath.AutoSize = true;
            this.labelLogFilePath.Location = new System.Drawing.Point(193, 26);
            this.labelLogFilePath.Name = "labelLogFilePath";
            this.labelLogFilePath.Size = new System.Drawing.Size(68, 13);
            this.labelLogFilePath.TabIndex = 30;
            this.labelLogFilePath.Text = "Log file path:";
            // 
            // chkLogWarningMsgs
            // 
            this.chkLogWarningMsgs.AutoSize = true;
            this.chkLogWarningMsgs.Location = new System.Drawing.Point(20, 91);
            this.chkLogWarningMsgs.Name = "chkLogWarningMsgs";
            this.chkLogWarningMsgs.Size = new System.Drawing.Size(66, 17);
            this.chkLogWarningMsgs.TabIndex = 37;
            this.chkLogWarningMsgs.Text = "Warning";
            this.chkLogWarningMsgs.UseVisualStyleBackColor = true;
            // 
            // textBoxLogPath
            // 
            this.textBoxLogPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxLogPath.Location = new System.Drawing.Point(196, 43);
            this.textBoxLogPath.Name = "textBoxLogPath";
            this.textBoxLogPath.ReadOnly = true;
            this.textBoxLogPath.Size = new System.Drawing.Size(399, 20);
            this.textBoxLogPath.TabIndex = 0;
            // 
            // chkLogInfoMsgs
            // 
            this.chkLogInfoMsgs.AutoSize = true;
            this.chkLogInfoMsgs.Location = new System.Drawing.Point(20, 68);
            this.chkLogInfoMsgs.Name = "chkLogInfoMsgs";
            this.chkLogInfoMsgs.Size = new System.Drawing.Size(78, 17);
            this.chkLogInfoMsgs.TabIndex = 36;
            this.chkLogInfoMsgs.Text = "Information";
            this.chkLogInfoMsgs.UseVisualStyleBackColor = true;
            // 
            // chkLogDebugMsgs
            // 
            this.chkLogDebugMsgs.AutoSize = true;
            this.chkLogDebugMsgs.Location = new System.Drawing.Point(20, 45);
            this.chkLogDebugMsgs.Name = "chkLogDebugMsgs";
            this.chkLogDebugMsgs.Size = new System.Drawing.Size(58, 17);
            this.chkLogDebugMsgs.TabIndex = 35;
            this.chkLogDebugMsgs.Text = "Debug";
            this.chkLogDebugMsgs.UseVisualStyleBackColor = true;
            // 
            // buttonRestoreDefaultLogPath
            // 
            this.buttonRestoreDefaultLogPath.Location = new System.Drawing.Point(496, 69);
            this.buttonRestoreDefaultLogPath.Name = "buttonRestoreDefaultLogPath";
            this.buttonRestoreDefaultLogPath.Size = new System.Drawing.Size(99, 23);
            this.buttonRestoreDefaultLogPath.TabIndex = 39;
            this.buttonRestoreDefaultLogPath.Text = "Use Default";
            this.buttonRestoreDefaultLogPath.UseVisualStyleBackColor = true;
            this.buttonRestoreDefaultLogPath.Click += new System.EventHandler(this.buttonRestoreDefaultLogPath_Click);
            // 
            // MessagesPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxLogging);
            this.Controls.Add(this.groupBox1);
            this.Name = "MessagesPage";
            this.PageIcon = ((System.Drawing.Icon)(resources.GetObject("$this.PageIcon")));
            this.Size = new System.Drawing.Size(610, 489);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxLogging.ResumeLayout(false);
            this.groupBoxLogging.PerformLayout();
            this.ResumeLayout(false);

		}

        internal System.Windows.Forms.Label lblSwitchToErrorsAndInfos;
        internal System.Windows.Forms.CheckBox chkSwitchToMCInformation;
        internal System.Windows.Forms.CheckBox chkSwitchToMCErrors;
        internal System.Windows.Forms.CheckBox chkSwitchToMCWarnings;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkShowErrorInMC;
        private System.Windows.Forms.CheckBox chkShowWarningInMC;
        private System.Windows.Forms.CheckBox chkShowInfoInMC;
        private System.Windows.Forms.CheckBox chkShowDebugInMC;
        private System.Windows.Forms.GroupBox groupBoxLogging;
        private System.Windows.Forms.SaveFileDialog saveFileDialogLogging;
        private System.Windows.Forms.Label labelLogFilePath;
        private System.Windows.Forms.TextBox textBoxLogPath;
        private System.Windows.Forms.Button buttonSelectLogPath;
        private System.Windows.Forms.Label labelLogTheseMsgTypes;
        private System.Windows.Forms.CheckBox chkLogErrorMsgs;
        private System.Windows.Forms.CheckBox chkLogWarningMsgs;
        private System.Windows.Forms.CheckBox chkLogInfoMsgs;
        private System.Windows.Forms.CheckBox chkLogDebugMsgs;
        private System.Windows.Forms.Button buttonOpenLogFile;
        private System.Windows.Forms.Button buttonRestoreDefaultLogPath;
    }
}
