

using mRemoteNG.UI.Controls;

namespace mRemoteNG.UI.Forms.OptionsPages
{

    public sealed partial class NotificationsPage : OptionsPage
    {

        //UserControl overrides dispose to clean up the component list.
        [System.Diagnostics.DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
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
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            labelSwitchToErrorsAndInfos = new MrngLabel();
            chkSwitchToMCInformation = new MrngCheckBox();
            chkSwitchToMCErrors = new MrngCheckBox();
            chkSwitchToMCWarnings = new MrngCheckBox();
            groupBoxNotifications = new MrngGroupBox();
            labelNotificationsShowTypes = new MrngLabel();
            chkShowErrorInMC = new MrngCheckBox();
            chkShowWarningInMC = new MrngCheckBox();
            chkShowInfoInMC = new MrngCheckBox();
            chkShowDebugInMC = new MrngCheckBox();
            groupBoxLogging = new MrngGroupBox();
            tblLogging = new System.Windows.Forms.TableLayoutPanel();
            chkLogDebugMsgs = new MrngCheckBox();
            chkLogInfoMsgs = new MrngCheckBox();
            chkLogWarningMsgs = new MrngCheckBox();
            chkLogErrorMsgs = new MrngCheckBox();
            chkLogToCurrentDir = new MrngCheckBox();
            buttonRestoreDefaultLogPath = new MrngButton();
            buttonOpenLogFile = new MrngButton();
            buttonSelectLogPath = new MrngButton();
            labelLogTheseMsgTypes = new MrngLabel();
            labelLogFilePath = new MrngLabel();
            textBoxLogPath = new MrngTextBox();
            saveFileDialogLogging = new System.Windows.Forms.SaveFileDialog();
            groupBoxPopups = new MrngGroupBox();
            tblPopups = new System.Windows.Forms.TableLayoutPanel();
            chkPopupDebug = new MrngCheckBox();
            chkPopupError = new MrngCheckBox();
            chkPopupInfo = new MrngCheckBox();
            chkPopupWarning = new MrngCheckBox();
            labelPopupShowTypes = new MrngLabel();
            lblRegistrySettingsUsedInfo = new System.Windows.Forms.Label();
            groupBoxNotifications.SuspendLayout();
            groupBoxLogging.SuspendLayout();
            tblLogging.SuspendLayout();
            groupBoxPopups.SuspendLayout();
            tblPopups.SuspendLayout();
            SuspendLayout();
            // 
            // labelSwitchToErrorsAndInfos
            // 
            labelSwitchToErrorsAndInfos.AutoSize = true;
            labelSwitchToErrorsAndInfos.Location = new System.Drawing.Point(177, 25);
            labelSwitchToErrorsAndInfos.Name = "labelSwitchToErrorsAndInfos";
            labelSwitchToErrorsAndInfos.Size = new System.Drawing.Size(176, 13);
            labelSwitchToErrorsAndInfos.TabIndex = 5;
            labelSwitchToErrorsAndInfos.Text = "Switch to Notifications panel on:";
            // 
            // chkSwitchToMCInformation
            // 
            chkSwitchToMCInformation._mice = MrngCheckBox.MouseState.OUT;
            chkSwitchToMCInformation.AutoSize = true;
            chkSwitchToMCInformation.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkSwitchToMCInformation.Location = new System.Drawing.Point(195, 64);
            chkSwitchToMCInformation.Name = "chkSwitchToMCInformation";
            chkSwitchToMCInformation.Size = new System.Drawing.Size(87, 17);
            chkSwitchToMCInformation.TabIndex = 6;
            chkSwitchToMCInformation.Text = "Information";
            chkSwitchToMCInformation.UseVisualStyleBackColor = true;
            // 
            // chkSwitchToMCErrors
            // 
            chkSwitchToMCErrors._mice = MrngCheckBox.MouseState.OUT;
            chkSwitchToMCErrors.AutoSize = true;
            chkSwitchToMCErrors.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkSwitchToMCErrors.Location = new System.Drawing.Point(195, 110);
            chkSwitchToMCErrors.Name = "chkSwitchToMCErrors";
            chkSwitchToMCErrors.Size = new System.Drawing.Size(51, 17);
            chkSwitchToMCErrors.TabIndex = 8;
            chkSwitchToMCErrors.Text = "Error";
            chkSwitchToMCErrors.UseVisualStyleBackColor = true;
            // 
            // chkSwitchToMCWarnings
            // 
            chkSwitchToMCWarnings._mice = MrngCheckBox.MouseState.OUT;
            chkSwitchToMCWarnings.AutoSize = true;
            chkSwitchToMCWarnings.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkSwitchToMCWarnings.Location = new System.Drawing.Point(195, 87);
            chkSwitchToMCWarnings.Name = "chkSwitchToMCWarnings";
            chkSwitchToMCWarnings.Size = new System.Drawing.Size(71, 17);
            chkSwitchToMCWarnings.TabIndex = 7;
            chkSwitchToMCWarnings.Text = "Warning";
            chkSwitchToMCWarnings.UseVisualStyleBackColor = true;
            // 
            // groupBoxNotifications
            // 
            groupBoxNotifications.Controls.Add(labelNotificationsShowTypes);
            groupBoxNotifications.Controls.Add(labelSwitchToErrorsAndInfos);
            groupBoxNotifications.Controls.Add(chkSwitchToMCErrors);
            groupBoxNotifications.Controls.Add(chkShowErrorInMC);
            groupBoxNotifications.Controls.Add(chkSwitchToMCInformation);
            groupBoxNotifications.Controls.Add(chkShowWarningInMC);
            groupBoxNotifications.Controls.Add(chkSwitchToMCWarnings);
            groupBoxNotifications.Controls.Add(chkShowInfoInMC);
            groupBoxNotifications.Controls.Add(chkShowDebugInMC);
            groupBoxNotifications.Dock = System.Windows.Forms.DockStyle.Top;
            groupBoxNotifications.Location = new System.Drawing.Point(0, 0);
            groupBoxNotifications.Name = "groupBoxNotifications";
            groupBoxNotifications.Size = new System.Drawing.Size(610, 132);
            groupBoxNotifications.TabIndex = 0;
            groupBoxNotifications.TabStop = false;
            groupBoxNotifications.Text = "Notifications Panel";
            // 
            // labelNotificationsShowTypes
            // 
            labelNotificationsShowTypes.AutoSize = true;
            labelNotificationsShowTypes.Location = new System.Drawing.Point(6, 25);
            labelNotificationsShowTypes.Name = "labelNotificationsShowTypes";
            labelNotificationsShowTypes.Size = new System.Drawing.Size(147, 13);
            labelNotificationsShowTypes.TabIndex = 0;
            labelNotificationsShowTypes.Text = "Show these message types:";
            // 
            // chkShowErrorInMC
            // 
            chkShowErrorInMC._mice = MrngCheckBox.MouseState.OUT;
            chkShowErrorInMC.AutoSize = true;
            chkShowErrorInMC.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkShowErrorInMC.Location = new System.Drawing.Point(20, 110);
            chkShowErrorInMC.Name = "chkShowErrorInMC";
            chkShowErrorInMC.Size = new System.Drawing.Size(51, 17);
            chkShowErrorInMC.TabIndex = 4;
            chkShowErrorInMC.Text = "Error";
            chkShowErrorInMC.UseVisualStyleBackColor = true;
            // 
            // chkShowWarningInMC
            // 
            chkShowWarningInMC._mice = MrngCheckBox.MouseState.OUT;
            chkShowWarningInMC.AutoSize = true;
            chkShowWarningInMC.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkShowWarningInMC.Location = new System.Drawing.Point(20, 87);
            chkShowWarningInMC.Name = "chkShowWarningInMC";
            chkShowWarningInMC.Size = new System.Drawing.Size(71, 17);
            chkShowWarningInMC.TabIndex = 3;
            chkShowWarningInMC.Text = "Warning";
            chkShowWarningInMC.UseVisualStyleBackColor = true;
            // 
            // chkShowInfoInMC
            // 
            chkShowInfoInMC._mice = MrngCheckBox.MouseState.OUT;
            chkShowInfoInMC.AutoSize = true;
            chkShowInfoInMC.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkShowInfoInMC.Location = new System.Drawing.Point(20, 64);
            chkShowInfoInMC.Name = "chkShowInfoInMC";
            chkShowInfoInMC.Size = new System.Drawing.Size(87, 17);
            chkShowInfoInMC.TabIndex = 2;
            chkShowInfoInMC.Text = "Information";
            chkShowInfoInMC.UseVisualStyleBackColor = true;
            // 
            // chkShowDebugInMC
            // 
            chkShowDebugInMC._mice = MrngCheckBox.MouseState.OUT;
            chkShowDebugInMC.AutoSize = true;
            chkShowDebugInMC.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkShowDebugInMC.Location = new System.Drawing.Point(20, 41);
            chkShowDebugInMC.Name = "chkShowDebugInMC";
            chkShowDebugInMC.Size = new System.Drawing.Size(61, 17);
            chkShowDebugInMC.TabIndex = 1;
            chkShowDebugInMC.Text = "Debug";
            chkShowDebugInMC.UseVisualStyleBackColor = true;
            // 
            // groupBoxLogging
            // 
            groupBoxLogging.Controls.Add(tblLogging);
            groupBoxLogging.Controls.Add(chkLogToCurrentDir);
            groupBoxLogging.Controls.Add(buttonRestoreDefaultLogPath);
            groupBoxLogging.Controls.Add(buttonOpenLogFile);
            groupBoxLogging.Controls.Add(buttonSelectLogPath);
            groupBoxLogging.Controls.Add(labelLogTheseMsgTypes);
            groupBoxLogging.Controls.Add(labelLogFilePath);
            groupBoxLogging.Controls.Add(textBoxLogPath);
            groupBoxLogging.Dock = System.Windows.Forms.DockStyle.Top;
            groupBoxLogging.Location = new System.Drawing.Point(0, 132);
            groupBoxLogging.Name = "groupBoxLogging";
            groupBoxLogging.Size = new System.Drawing.Size(610, 173);
            groupBoxLogging.TabIndex = 1;
            groupBoxLogging.TabStop = false;
            groupBoxLogging.Text = "Logging";
            // 
            // tblLogging
            // 
            tblLogging.ColumnCount = 4;
            tblLogging.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tblLogging.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tblLogging.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tblLogging.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tblLogging.Controls.Add(chkLogDebugMsgs, 0, 0);
            tblLogging.Controls.Add(chkLogInfoMsgs, 1, 0);
            tblLogging.Controls.Add(chkLogWarningMsgs, 2, 0);
            tblLogging.Controls.Add(chkLogErrorMsgs, 3, 0);
            tblLogging.Location = new System.Drawing.Point(9, 138);
            tblLogging.Name = "tblLogging";
            tblLogging.RowCount = 1;
            tblLogging.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tblLogging.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            tblLogging.Size = new System.Drawing.Size(585, 25);
            tblLogging.TabIndex = 7;
            // 
            // chkLogDebugMsgs
            // 
            chkLogDebugMsgs._mice = MrngCheckBox.MouseState.OUT;
            chkLogDebugMsgs.AutoSize = true;
            chkLogDebugMsgs.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkLogDebugMsgs.Location = new System.Drawing.Point(3, 3);
            chkLogDebugMsgs.Name = "chkLogDebugMsgs";
            chkLogDebugMsgs.Size = new System.Drawing.Size(61, 17);
            chkLogDebugMsgs.TabIndex = 0;
            chkLogDebugMsgs.Text = "Debug";
            chkLogDebugMsgs.UseVisualStyleBackColor = true;
            // 
            // chkLogInfoMsgs
            // 
            chkLogInfoMsgs._mice = MrngCheckBox.MouseState.OUT;
            chkLogInfoMsgs.AutoSize = true;
            chkLogInfoMsgs.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkLogInfoMsgs.Location = new System.Drawing.Point(149, 3);
            chkLogInfoMsgs.Name = "chkLogInfoMsgs";
            chkLogInfoMsgs.Size = new System.Drawing.Size(87, 17);
            chkLogInfoMsgs.TabIndex = 1;
            chkLogInfoMsgs.Text = "Information";
            chkLogInfoMsgs.UseVisualStyleBackColor = true;
            // 
            // chkLogWarningMsgs
            // 
            chkLogWarningMsgs._mice = MrngCheckBox.MouseState.OUT;
            chkLogWarningMsgs.AutoSize = true;
            chkLogWarningMsgs.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkLogWarningMsgs.Location = new System.Drawing.Point(295, 3);
            chkLogWarningMsgs.Name = "chkLogWarningMsgs";
            chkLogWarningMsgs.Size = new System.Drawing.Size(71, 17);
            chkLogWarningMsgs.TabIndex = 2;
            chkLogWarningMsgs.Text = "Warning";
            chkLogWarningMsgs.UseVisualStyleBackColor = true;
            // 
            // chkLogErrorMsgs
            // 
            chkLogErrorMsgs._mice = MrngCheckBox.MouseState.OUT;
            chkLogErrorMsgs.AutoSize = true;
            chkLogErrorMsgs.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkLogErrorMsgs.Location = new System.Drawing.Point(441, 3);
            chkLogErrorMsgs.Name = "chkLogErrorMsgs";
            chkLogErrorMsgs.Size = new System.Drawing.Size(51, 17);
            chkLogErrorMsgs.TabIndex = 3;
            chkLogErrorMsgs.Text = "Error";
            chkLogErrorMsgs.UseVisualStyleBackColor = true;
            // 
            // chkLogToCurrentDir
            // 
            chkLogToCurrentDir._mice = MrngCheckBox.MouseState.OUT;
            chkLogToCurrentDir.AutoSize = true;
            chkLogToCurrentDir.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkLogToCurrentDir.Location = new System.Drawing.Point(9, 18);
            chkLogToCurrentDir.Name = "chkLogToCurrentDir";
            chkLogToCurrentDir.Size = new System.Drawing.Size(168, 17);
            chkLogToCurrentDir.TabIndex = 0;
            chkLogToCurrentDir.Text = "Log to application directory";
            chkLogToCurrentDir.UseVisualStyleBackColor = true;
            chkLogToCurrentDir.CheckedChanged += chkLogToCurrentDir_CheckedChanged;
            // 
            // buttonRestoreDefaultLogPath
            // 
            buttonRestoreDefaultLogPath._mice = MrngButton.MouseState.OUT;
            buttonRestoreDefaultLogPath.Location = new System.Drawing.Point(9, 85);
            buttonRestoreDefaultLogPath.Name = "buttonRestoreDefaultLogPath";
            buttonRestoreDefaultLogPath.Size = new System.Drawing.Size(179, 25);
            buttonRestoreDefaultLogPath.TabIndex = 5;
            buttonRestoreDefaultLogPath.Text = "Use Default";
            buttonRestoreDefaultLogPath.UseVisualStyleBackColor = true;
            buttonRestoreDefaultLogPath.Click += buttonRestoreDefaultLogPath_Click;
            // 
            // buttonOpenLogFile
            // 
            buttonOpenLogFile._mice = MrngButton.MouseState.OUT;
            buttonOpenLogFile.Location = new System.Drawing.Point(378, 85);
            buttonOpenLogFile.Name = "buttonOpenLogFile";
            buttonOpenLogFile.Size = new System.Drawing.Size(105, 25);
            buttonOpenLogFile.TabIndex = 3;
            buttonOpenLogFile.Text = "Open File";
            buttonOpenLogFile.UseVisualStyleBackColor = true;
            buttonOpenLogFile.Click += buttonOpenLogFile_Click;
            // 
            // buttonSelectLogPath
            // 
            buttonSelectLogPath._mice = MrngButton.MouseState.OUT;
            buttonSelectLogPath.Location = new System.Drawing.Point(489, 85);
            buttonSelectLogPath.Name = "buttonSelectLogPath";
            buttonSelectLogPath.Size = new System.Drawing.Size(105, 25);
            buttonSelectLogPath.TabIndex = 4;
            buttonSelectLogPath.Text = "Choose Path";
            buttonSelectLogPath.UseVisualStyleBackColor = true;
            buttonSelectLogPath.Click += buttonSelectLogPath_Click;
            // 
            // labelLogTheseMsgTypes
            // 
            labelLogTheseMsgTypes.AutoSize = true;
            labelLogTheseMsgTypes.Location = new System.Drawing.Point(6, 122);
            labelLogTheseMsgTypes.Name = "labelLogTheseMsgTypes";
            labelLogTheseMsgTypes.Size = new System.Drawing.Size(137, 13);
            labelLogTheseMsgTypes.TabIndex = 6;
            labelLogTheseMsgTypes.Text = "Log these message types:";
            // 
            // labelLogFilePath
            // 
            labelLogFilePath.AutoSize = true;
            labelLogFilePath.Location = new System.Drawing.Point(6, 38);
            labelLogFilePath.Name = "labelLogFilePath";
            labelLogFilePath.Size = new System.Drawing.Size(75, 13);
            labelLogFilePath.TabIndex = 1;
            labelLogFilePath.Text = "Log file path:";
            // 
            // textBoxLogPath
            // 
            textBoxLogPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            textBoxLogPath.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            textBoxLogPath.Location = new System.Drawing.Point(9, 57);
            textBoxLogPath.Name = "textBoxLogPath";
            textBoxLogPath.ReadOnly = true;
            textBoxLogPath.Size = new System.Drawing.Size(585, 22);
            textBoxLogPath.TabIndex = 2;
            // 
            // groupBoxPopups
            // 
            groupBoxPopups.Controls.Add(tblPopups);
            groupBoxPopups.Controls.Add(labelPopupShowTypes);
            groupBoxPopups.Dock = System.Windows.Forms.DockStyle.Top;
            groupBoxPopups.Location = new System.Drawing.Point(0, 305);
            groupBoxPopups.Name = "groupBoxPopups";
            groupBoxPopups.Size = new System.Drawing.Size(610, 74);
            groupBoxPopups.TabIndex = 2;
            groupBoxPopups.TabStop = false;
            groupBoxPopups.Text = "Pop-ups";
            // 
            // tblPopups
            // 
            tblPopups.ColumnCount = 4;
            tblPopups.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tblPopups.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tblPopups.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tblPopups.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tblPopups.Controls.Add(chkPopupDebug, 0, 0);
            tblPopups.Controls.Add(chkPopupError, 3, 0);
            tblPopups.Controls.Add(chkPopupInfo, 1, 0);
            tblPopups.Controls.Add(chkPopupWarning, 2, 0);
            tblPopups.Location = new System.Drawing.Point(11, 40);
            tblPopups.Name = "tblPopups";
            tblPopups.RowCount = 1;
            tblPopups.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tblPopups.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            tblPopups.Size = new System.Drawing.Size(584, 25);
            tblPopups.TabIndex = 1;
            // 
            // chkPopupDebug
            // 
            chkPopupDebug._mice = MrngCheckBox.MouseState.OUT;
            chkPopupDebug.AutoSize = true;
            chkPopupDebug.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkPopupDebug.Location = new System.Drawing.Point(3, 3);
            chkPopupDebug.Name = "chkPopupDebug";
            chkPopupDebug.Size = new System.Drawing.Size(61, 17);
            chkPopupDebug.TabIndex = 0;
            chkPopupDebug.Text = "Debug";
            chkPopupDebug.UseVisualStyleBackColor = true;
            // 
            // chkPopupError
            // 
            chkPopupError._mice = MrngCheckBox.MouseState.OUT;
            chkPopupError.AutoSize = true;
            chkPopupError.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkPopupError.Location = new System.Drawing.Point(441, 3);
            chkPopupError.Name = "chkPopupError";
            chkPopupError.Size = new System.Drawing.Size(51, 17);
            chkPopupError.TabIndex = 3;
            chkPopupError.Text = "Error";
            chkPopupError.UseVisualStyleBackColor = true;
            // 
            // chkPopupInfo
            // 
            chkPopupInfo._mice = MrngCheckBox.MouseState.OUT;
            chkPopupInfo.AutoSize = true;
            chkPopupInfo.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkPopupInfo.Location = new System.Drawing.Point(149, 3);
            chkPopupInfo.Name = "chkPopupInfo";
            chkPopupInfo.Size = new System.Drawing.Size(87, 17);
            chkPopupInfo.TabIndex = 1;
            chkPopupInfo.Text = "Information";
            chkPopupInfo.UseVisualStyleBackColor = true;
            // 
            // chkPopupWarning
            // 
            chkPopupWarning._mice = MrngCheckBox.MouseState.OUT;
            chkPopupWarning.AutoSize = true;
            chkPopupWarning.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkPopupWarning.Location = new System.Drawing.Point(295, 3);
            chkPopupWarning.Name = "chkPopupWarning";
            chkPopupWarning.Size = new System.Drawing.Size(71, 17);
            chkPopupWarning.TabIndex = 2;
            chkPopupWarning.Text = "Warning";
            chkPopupWarning.UseVisualStyleBackColor = true;
            // 
            // labelPopupShowTypes
            // 
            labelPopupShowTypes.AutoSize = true;
            labelPopupShowTypes.Location = new System.Drawing.Point(8, 24);
            labelPopupShowTypes.Name = "labelPopupShowTypes";
            labelPopupShowTypes.Size = new System.Drawing.Size(147, 13);
            labelPopupShowTypes.TabIndex = 0;
            labelPopupShowTypes.Text = "Show these message types:";
            // 
            // lblRegistrySettingsUsedInfo
            // 
            lblRegistrySettingsUsedInfo.BackColor = System.Drawing.SystemColors.ControlLight;
            lblRegistrySettingsUsedInfo.Dock = System.Windows.Forms.DockStyle.Top;
            lblRegistrySettingsUsedInfo.ForeColor = System.Drawing.SystemColors.ControlText;
            lblRegistrySettingsUsedInfo.Location = new System.Drawing.Point(0, 379);
            lblRegistrySettingsUsedInfo.Name = "lblRegistrySettingsUsedInfo";
            lblRegistrySettingsUsedInfo.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            lblRegistrySettingsUsedInfo.Size = new System.Drawing.Size(610, 30);
            lblRegistrySettingsUsedInfo.TabIndex = 3;
            lblRegistrySettingsUsedInfo.Text = "Some settings are configured by your Administrator. Please contact your administrator for more information.";
            lblRegistrySettingsUsedInfo.Visible = false;
            // 
            // NotificationsPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            Controls.Add(groupBoxPopups);
            Controls.Add(groupBoxLogging);
            Controls.Add(groupBoxNotifications);
            Controls.Add(lblRegistrySettingsUsedInfo);
            Margin = new System.Windows.Forms.Padding(4);
            Name = "NotificationsPage";
            Size = new System.Drawing.Size(610, 490);
            groupBoxNotifications.ResumeLayout(false);
            groupBoxNotifications.PerformLayout();
            groupBoxLogging.ResumeLayout(false);
            groupBoxLogging.PerformLayout();
            tblLogging.ResumeLayout(false);
            tblLogging.PerformLayout();
            groupBoxPopups.ResumeLayout(false);
            groupBoxPopups.PerformLayout();
            tblPopups.ResumeLayout(false);
            tblPopups.PerformLayout();
            ResumeLayout(false);
        }

        internal Controls.MrngLabel labelSwitchToErrorsAndInfos;
        internal MrngCheckBox chkSwitchToMCInformation;
        internal MrngCheckBox chkSwitchToMCErrors;
        internal MrngCheckBox chkSwitchToMCWarnings;
        private Controls.MrngLabel labelNotificationsShowTypes;
        private MrngCheckBox chkShowErrorInMC;
        private MrngCheckBox chkShowWarningInMC;
        private MrngCheckBox chkShowInfoInMC;
        private MrngCheckBox chkShowDebugInMC;
        private System.Windows.Forms.SaveFileDialog saveFileDialogLogging;
        private Controls.MrngLabel labelLogFilePath;
        private Controls.MrngTextBox textBoxLogPath;
        private MrngButton buttonSelectLogPath;
        private Controls.MrngLabel labelLogTheseMsgTypes;
        private MrngCheckBox chkLogErrorMsgs;
        private MrngCheckBox chkLogWarningMsgs;
        private MrngCheckBox chkLogInfoMsgs;
        private MrngCheckBox chkLogDebugMsgs;
        private MrngButton buttonOpenLogFile;
        private MrngButton buttonRestoreDefaultLogPath;
        private MrngCheckBox chkPopupError;
        private Controls.MrngLabel labelPopupShowTypes;
        private MrngCheckBox chkPopupWarning;
        private MrngCheckBox chkPopupDebug;
        private MrngCheckBox chkPopupInfo;
        private MrngCheckBox chkLogToCurrentDir;
        private System.Windows.Forms.TableLayoutPanel tblLogging;
        private System.Windows.Forms.TableLayoutPanel tblPopups;
        private MrngGroupBox groupBoxNotifications;
        private MrngGroupBox groupBoxLogging;
        private MrngGroupBox groupBoxPopups;
        internal System.Windows.Forms.Label lblRegistrySettingsUsedInfo;
    }
}
