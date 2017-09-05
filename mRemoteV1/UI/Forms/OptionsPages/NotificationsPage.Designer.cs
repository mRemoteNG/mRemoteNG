

namespace mRemoteNG.UI.Forms.OptionsPages
{
	
    public partial class NotificationsPage : OptionsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotificationsPage));
            this.labelSwitchToErrorsAndInfos = new Controls.Base.NGLabel();
            this.chkSwitchToMCInformation = new Controls.Base.NGCheckBox();
            this.chkSwitchToMCErrors = new Controls.Base.NGCheckBox();
            this.chkSwitchToMCWarnings = new Controls.Base.NGCheckBox();
            this.groupBoxNotifications = new System.Windows.Forms.GroupBox();
            this.labelNotificationsShowTypes = new Controls.Base.NGLabel();
            this.chkShowErrorInMC = new Controls.Base.NGCheckBox();
            this.chkShowWarningInMC = new Controls.Base.NGCheckBox();
            this.chkShowInfoInMC = new Controls.Base.NGCheckBox();
            this.chkShowDebugInMC = new Controls.Base.NGCheckBox();
            this.groupBoxLogging = new System.Windows.Forms.GroupBox();
            this.chkLogToCurrentDir = new Controls.Base.NGCheckBox();
            this.buttonRestoreDefaultLogPath = new Controls.Base.NGButton();
            this.buttonOpenLogFile = new Controls.Base.NGButton();
            this.buttonSelectLogPath = new Controls.Base.NGButton();
            this.labelLogTheseMsgTypes = new Controls.Base.NGLabel();
            this.chkLogErrorMsgs = new Controls.Base.NGCheckBox();
            this.labelLogFilePath = new Controls.Base.NGLabel();
            this.chkLogWarningMsgs = new Controls.Base.NGCheckBox();
            this.textBoxLogPath = new Controls.Base.NGTextBox();
            this.chkLogInfoMsgs = new Controls.Base.NGCheckBox();
            this.chkLogDebugMsgs = new Controls.Base.NGCheckBox();
            this.saveFileDialogLogging = new System.Windows.Forms.SaveFileDialog();
            this.groupBoxPopups = new System.Windows.Forms.GroupBox();
            this.chkPopupError = new Controls.Base.NGCheckBox();
            this.labelPopupShowTypes = new Controls.Base.NGLabel();
            this.chkPopupWarning = new Controls.Base.NGCheckBox();
            this.chkPopupDebug = new Controls.Base.NGCheckBox();
            this.chkPopupInfo = new Controls.Base.NGCheckBox();
            this.groupBoxNotifications.SuspendLayout();
            this.groupBoxLogging.SuspendLayout();
            this.groupBoxPopups.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelSwitchToErrorsAndInfos
            // 
            this.labelSwitchToErrorsAndInfos.AutoSize = true;
            this.labelSwitchToErrorsAndInfos.Location = new System.Drawing.Point(177, 26);
            this.labelSwitchToErrorsAndInfos.Name = "labelSwitchToErrorsAndInfos";
            this.labelSwitchToErrorsAndInfos.Size = new System.Drawing.Size(159, 13);
            this.labelSwitchToErrorsAndInfos.TabIndex = 24;
            this.labelSwitchToErrorsAndInfos.Text = "Switch to Notifications panel on:";
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
            // groupBoxNotifications
            // 
            this.groupBoxNotifications.Controls.Add(this.labelNotificationsShowTypes);
            this.groupBoxNotifications.Controls.Add(this.labelSwitchToErrorsAndInfos);
            this.groupBoxNotifications.Controls.Add(this.chkSwitchToMCErrors);
            this.groupBoxNotifications.Controls.Add(this.chkShowErrorInMC);
            this.groupBoxNotifications.Controls.Add(this.chkSwitchToMCInformation);
            this.groupBoxNotifications.Controls.Add(this.chkShowWarningInMC);
            this.groupBoxNotifications.Controls.Add(this.chkSwitchToMCWarnings);
            this.groupBoxNotifications.Controls.Add(this.chkShowInfoInMC);
            this.groupBoxNotifications.Controls.Add(this.chkShowDebugInMC);
            this.groupBoxNotifications.Location = new System.Drawing.Point(6, 3);
            this.groupBoxNotifications.Name = "groupBoxNotifications";
            this.groupBoxNotifications.Size = new System.Drawing.Size(601, 141);
            this.groupBoxNotifications.TabIndex = 28;
            this.groupBoxNotifications.TabStop = false;
            this.groupBoxNotifications.Text = "Notifications Panel";
            // 
            // labelNotificationsShowTypes
            // 
            this.labelNotificationsShowTypes.AutoSize = true;
            this.labelNotificationsShowTypes.Location = new System.Drawing.Point(6, 26);
            this.labelNotificationsShowTypes.Name = "labelNotificationsShowTypes";
            this.labelNotificationsShowTypes.Size = new System.Drawing.Size(139, 13);
            this.labelNotificationsShowTypes.TabIndex = 29;
            this.labelNotificationsShowTypes.Text = "Show these message types:";
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
            this.groupBoxLogging.Controls.Add(this.chkLogToCurrentDir);
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
            // chkLogToCurrentDir
            // 
            this.chkLogToCurrentDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkLogToCurrentDir.AutoSize = true;
            this.chkLogToCurrentDir.Location = new System.Drawing.Point(441, 20);
            this.chkLogToCurrentDir.Name = "chkLogToCurrentDir";
            this.chkLogToCurrentDir.Size = new System.Drawing.Size(153, 17);
            this.chkLogToCurrentDir.TabIndex = 40;
            this.chkLogToCurrentDir.Text = "Log to application directory";
            this.chkLogToCurrentDir.UseVisualStyleBackColor = true;
            this.chkLogToCurrentDir.CheckedChanged += new System.EventHandler(this.chkLogToCurrentDir_CheckedChanged);
            // 
            // buttonRestoreDefaultLogPath
            // 
            this.buttonRestoreDefaultLogPath.Location = new System.Drawing.Point(495, 69);
            this.buttonRestoreDefaultLogPath.Name = "buttonRestoreDefaultLogPath";
            this.buttonRestoreDefaultLogPath.Size = new System.Drawing.Size(99, 23);
            this.buttonRestoreDefaultLogPath.TabIndex = 39;
            this.buttonRestoreDefaultLogPath.Text = "Use Default";
            this.buttonRestoreDefaultLogPath.UseVisualStyleBackColor = true;
            this.buttonRestoreDefaultLogPath.Click += new System.EventHandler(this.buttonRestoreDefaultLogPath_Click);
            // 
            // buttonOpenLogFile
            // 
            this.buttonOpenLogFile.Location = new System.Drawing.Point(273, 68);
            this.buttonOpenLogFile.Name = "buttonOpenLogFile";
            this.buttonOpenLogFile.Size = new System.Drawing.Size(105, 23);
            this.buttonOpenLogFile.TabIndex = 30;
            this.buttonOpenLogFile.Text = "Open File";
            this.buttonOpenLogFile.UseVisualStyleBackColor = true;
            this.buttonOpenLogFile.Click += new System.EventHandler(this.buttonOpenLogFile_Click);
            // 
            // buttonSelectLogPath
            // 
            this.buttonSelectLogPath.Location = new System.Drawing.Point(384, 68);
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
            this.labelLogFilePath.Location = new System.Drawing.Point(192, 26);
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
            this.textBoxLogPath.Location = new System.Drawing.Point(195, 43);
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
            // groupBoxPopups
            // 
            this.groupBoxPopups.Controls.Add(this.chkPopupError);
            this.groupBoxPopups.Controls.Add(this.labelPopupShowTypes);
            this.groupBoxPopups.Controls.Add(this.chkPopupWarning);
            this.groupBoxPopups.Controls.Add(this.chkPopupDebug);
            this.groupBoxPopups.Controls.Add(this.chkPopupInfo);
            this.groupBoxPopups.Location = new System.Drawing.Point(4, 297);
            this.groupBoxPopups.Name = "groupBoxPopups";
            this.groupBoxPopups.Size = new System.Drawing.Size(603, 135);
            this.groupBoxPopups.TabIndex = 30;
            this.groupBoxPopups.TabStop = false;
            this.groupBoxPopups.Text = "Pop-ups";
            // 
            // chkPopupError
            // 
            this.chkPopupError.AutoSize = true;
            this.chkPopupError.Location = new System.Drawing.Point(22, 110);
            this.chkPopupError.Name = "chkPopupError";
            this.chkPopupError.Size = new System.Drawing.Size(48, 17);
            this.chkPopupError.TabIndex = 43;
            this.chkPopupError.Text = "Error";
            this.chkPopupError.UseVisualStyleBackColor = true;
            // 
            // labelPopupShowTypes
            // 
            this.labelPopupShowTypes.AutoSize = true;
            this.labelPopupShowTypes.Location = new System.Drawing.Point(8, 25);
            this.labelPopupShowTypes.Name = "labelPopupShowTypes";
            this.labelPopupShowTypes.Size = new System.Drawing.Size(139, 13);
            this.labelPopupShowTypes.TabIndex = 33;
            this.labelPopupShowTypes.Text = "Show these message types:";
            // 
            // chkPopupWarning
            // 
            this.chkPopupWarning.AutoSize = true;
            this.chkPopupWarning.Location = new System.Drawing.Point(22, 87);
            this.chkPopupWarning.Name = "chkPopupWarning";
            this.chkPopupWarning.Size = new System.Drawing.Size(66, 17);
            this.chkPopupWarning.TabIndex = 42;
            this.chkPopupWarning.Text = "Warning";
            this.chkPopupWarning.UseVisualStyleBackColor = true;
            // 
            // chkPopupDebug
            // 
            this.chkPopupDebug.AutoSize = true;
            this.chkPopupDebug.Location = new System.Drawing.Point(22, 41);
            this.chkPopupDebug.Name = "chkPopupDebug";
            this.chkPopupDebug.Size = new System.Drawing.Size(58, 17);
            this.chkPopupDebug.TabIndex = 40;
            this.chkPopupDebug.Text = "Debug";
            this.chkPopupDebug.UseVisualStyleBackColor = true;
            // 
            // chkPopupInfo
            // 
            this.chkPopupInfo.AutoSize = true;
            this.chkPopupInfo.Location = new System.Drawing.Point(22, 64);
            this.chkPopupInfo.Name = "chkPopupInfo";
            this.chkPopupInfo.Size = new System.Drawing.Size(78, 17);
            this.chkPopupInfo.TabIndex = 41;
            this.chkPopupInfo.Text = "Information";
            this.chkPopupInfo.UseVisualStyleBackColor = true;
            // 
            // NotificationsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxPopups);
            this.Controls.Add(this.groupBoxLogging);
            this.Controls.Add(this.groupBoxNotifications);
            this.Name = "NotificationsPage";
            this.PageIcon = ((System.Drawing.Icon)(resources.GetObject("$this.PageIcon")));
            this.Size = new System.Drawing.Size(610, 489);
            this.groupBoxNotifications.ResumeLayout(false);
            this.groupBoxNotifications.PerformLayout();
            this.groupBoxLogging.ResumeLayout(false);
            this.groupBoxLogging.PerformLayout();
            this.groupBoxPopups.ResumeLayout(false);
            this.groupBoxPopups.PerformLayout();
            this.ResumeLayout(false);

		}

        internal Controls.Base.NGLabel labelSwitchToErrorsAndInfos;
        internal Controls.Base.NGCheckBox chkSwitchToMCInformation;
        internal Controls.Base.NGCheckBox chkSwitchToMCErrors;
        internal Controls.Base.NGCheckBox chkSwitchToMCWarnings;
        private System.Windows.Forms.GroupBox groupBoxNotifications;
        private Controls.Base.NGLabel labelNotificationsShowTypes;
        private Controls.Base.NGCheckBox chkShowErrorInMC;
        private Controls.Base.NGCheckBox chkShowWarningInMC;
        private Controls.Base.NGCheckBox chkShowInfoInMC;
        private Controls.Base.NGCheckBox chkShowDebugInMC;
        private System.Windows.Forms.GroupBox groupBoxLogging;
        private System.Windows.Forms.SaveFileDialog saveFileDialogLogging;
        private Controls.Base.NGLabel labelLogFilePath;
        private Controls.Base.NGTextBox textBoxLogPath;
        private Controls.Base.NGButton buttonSelectLogPath;
        private Controls.Base.NGLabel labelLogTheseMsgTypes;
        private Controls.Base.NGCheckBox chkLogErrorMsgs;
        private Controls.Base.NGCheckBox chkLogWarningMsgs;
        private Controls.Base.NGCheckBox chkLogInfoMsgs;
        private Controls.Base.NGCheckBox chkLogDebugMsgs;
        private Controls.Base.NGButton buttonOpenLogFile;
        private Controls.Base.NGButton buttonRestoreDefaultLogPath;
        private System.Windows.Forms.GroupBox groupBoxPopups;
        private Controls.Base.NGCheckBox chkPopupError;
        private Controls.Base.NGLabel labelPopupShowTypes;
        private Controls.Base.NGCheckBox chkPopupWarning;
        private Controls.Base.NGCheckBox chkPopupDebug;
        private Controls.Base.NGCheckBox chkPopupInfo;
        private Controls.Base.NGCheckBox chkLogToCurrentDir;
    }
}
