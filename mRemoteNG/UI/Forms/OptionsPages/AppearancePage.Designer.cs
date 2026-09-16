

using System.Windows.Forms;
using mRemoteNG.UI.Controls;

namespace mRemoteNG.UI.Forms.OptionsPages
{

    public sealed partial class AppearancePage : OptionsPage
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
            lblLanguageRestartRequired = new MrngLabel();
            cboLanguage = new ComboBox();
            lblLanguage = new MrngLabel();
            chkShowFullConnectionsFilePathInTitle = new MrngCheckBox();
            chkShowDescriptionTooltipsInTree = new MrngCheckBox();
            chkShowSystemTrayIcon = new MrngCheckBox();
            chkMinimizeToSystemTray = new MrngCheckBox();
            chkCloseToSystemTray = new MrngCheckBox();
            chkEnableConnectionTreeAnimations = new MrngCheckBox();
            pnlOptions = new Panel();
            tableLayoutPanelAppearance = new TableLayoutPanel();
            lblRegistrySettingsUsedInfo = new System.Windows.Forms.Label();
            pnlOptions.SuspendLayout();
            tableLayoutPanelAppearance.SuspendLayout();
            SuspendLayout();
            // 
            // lblLanguageRestartRequired
            // 
            lblLanguageRestartRequired.AutoSize = true;
            lblLanguageRestartRequired.Dock = DockStyle.Fill;
            lblLanguageRestartRequired.Location = new System.Drawing.Point(6, 54);
            lblLanguageRestartRequired.Margin = new Padding(6, 0, 3, 12);
            lblLanguageRestartRequired.Name = "lblLanguageRestartRequired";
            lblLanguageRestartRequired.Size = new System.Drawing.Size(571, 13);
            lblLanguageRestartRequired.TabIndex = 2;
            lblLanguageRestartRequired.Text = "mRemoteNG must be restarted before changes to the language will take effect.";
            // 
            // cboLanguage
            // 
            cboLanguage.Dock = DockStyle.Fill;
            cboLanguage.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLanguage.FormattingEnabled = true;
            cboLanguage.IntegralHeight = false;
            cboLanguage.Margin = new Padding(6, 3, 24, 6);
            cboLanguage.Name = "cboLanguage";
            cboLanguage.Size = new System.Drawing.Size(550, 23);
            cboLanguage.Sorted = true;
            cboLanguage.TabIndex = 1;
            // 
            // lblLanguage
            // 
            lblLanguage.AutoSize = true;
            lblLanguage.Dock = DockStyle.Fill;
            lblLanguage.Location = new System.Drawing.Point(6, 6);
            lblLanguage.Margin = new Padding(6, 6, 3, 6);
            lblLanguage.Name = "lblLanguage";
            lblLanguage.Size = new System.Drawing.Size(571, 13);
            lblLanguage.TabIndex = 0;
            lblLanguage.Text = "Language";
            // 
            // chkShowFullConnectionsFilePathInTitle
            // 
            chkShowFullConnectionsFilePathInTitle._mice = MrngCheckBox.MouseState.OUT;
            chkShowFullConnectionsFilePathInTitle.AutoSize = true;
            chkShowFullConnectionsFilePathInTitle.Dock = DockStyle.Fill;
            chkShowFullConnectionsFilePathInTitle.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkShowFullConnectionsFilePathInTitle.Location = new System.Drawing.Point(6, 107);
            chkShowFullConnectionsFilePathInTitle.Margin = new Padding(6, 3, 3, 6);
            chkShowFullConnectionsFilePathInTitle.Name = "chkShowFullConnectionsFilePathInTitle";
            chkShowFullConnectionsFilePathInTitle.Size = new System.Drawing.Size(571, 17);
            chkShowFullConnectionsFilePathInTitle.TabIndex = 4;
            chkShowFullConnectionsFilePathInTitle.Text = "Show full connections file path in window title";
            chkShowFullConnectionsFilePathInTitle.UseVisualStyleBackColor = true;
            // 
            // chkShowDescriptionTooltipsInTree
            // 
            chkShowDescriptionTooltipsInTree._mice = MrngCheckBox.MouseState.OUT;
            chkShowDescriptionTooltipsInTree.AutoSize = true;
            chkShowDescriptionTooltipsInTree.Dock = DockStyle.Fill;
            chkShowDescriptionTooltipsInTree.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkShowDescriptionTooltipsInTree.Location = new System.Drawing.Point(6, 81);
            chkShowDescriptionTooltipsInTree.Margin = new Padding(6, 3, 3, 6);
            chkShowDescriptionTooltipsInTree.Name = "chkShowDescriptionTooltipsInTree";
            chkShowDescriptionTooltipsInTree.Size = new System.Drawing.Size(571, 17);
            chkShowDescriptionTooltipsInTree.TabIndex = 3;
            chkShowDescriptionTooltipsInTree.Text = "Show description tooltips in connection tree";
            chkShowDescriptionTooltipsInTree.UseVisualStyleBackColor = true;
            // 
            // chkShowSystemTrayIcon
            // 
            chkShowSystemTrayIcon._mice = MrngCheckBox.MouseState.OUT;
            chkShowSystemTrayIcon.AutoSize = true;
            chkShowSystemTrayIcon.Dock = DockStyle.Fill;
            chkShowSystemTrayIcon.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkShowSystemTrayIcon.Location = new System.Drawing.Point(6, 133);
            chkShowSystemTrayIcon.Margin = new Padding(6, 3, 3, 6);
            chkShowSystemTrayIcon.Name = "chkShowSystemTrayIcon";
            chkShowSystemTrayIcon.Size = new System.Drawing.Size(571, 17);
            chkShowSystemTrayIcon.TabIndex = 5;
            chkShowSystemTrayIcon.Text = "Always show System Tray Icon";
            chkShowSystemTrayIcon.UseVisualStyleBackColor = true;
            // 
            // chkMinimizeToSystemTray
            // 
            chkMinimizeToSystemTray._mice = MrngCheckBox.MouseState.OUT;
            chkMinimizeToSystemTray.AutoSize = true;
            chkMinimizeToSystemTray.Dock = DockStyle.Fill;
            chkMinimizeToSystemTray.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkMinimizeToSystemTray.Location = new System.Drawing.Point(6, 159);
            chkMinimizeToSystemTray.Margin = new Padding(6, 3, 3, 6);
            chkMinimizeToSystemTray.Name = "chkMinimizeToSystemTray";
            chkMinimizeToSystemTray.Size = new System.Drawing.Size(571, 17);
            chkMinimizeToSystemTray.TabIndex = 6;
            chkMinimizeToSystemTray.Text = "Minimize to System Tray";
            chkMinimizeToSystemTray.UseVisualStyleBackColor = true;
            // 
            // chkCloseToSystemTray
            // 
            chkCloseToSystemTray._mice = MrngCheckBox.MouseState.OUT;
            chkCloseToSystemTray.AutoSize = true;
            chkCloseToSystemTray.Dock = DockStyle.Fill;
            chkCloseToSystemTray.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkCloseToSystemTray.Location = new System.Drawing.Point(6, 185);
            chkCloseToSystemTray.Margin = new Padding(6, 3, 3, 6);
            chkCloseToSystemTray.Name = "chkCloseToSystemTray";
            chkCloseToSystemTray.Size = new System.Drawing.Size(571, 17);
            chkCloseToSystemTray.TabIndex = 7;
            chkCloseToSystemTray.Text = "Close to System Tray";
            chkCloseToSystemTray.UseVisualStyleBackColor = true;
            // 
            // chkEnableConnectionTreeAnimations
            // 
            chkEnableConnectionTreeAnimations._mice = MrngCheckBox.MouseState.OUT;
            chkEnableConnectionTreeAnimations.AutoSize = true;
            chkEnableConnectionTreeAnimations.Dock = DockStyle.Fill;
            chkEnableConnectionTreeAnimations.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkEnableConnectionTreeAnimations.Location = new System.Drawing.Point(6, 211);
            chkEnableConnectionTreeAnimations.Margin = new Padding(6, 3, 3, 6);
            chkEnableConnectionTreeAnimations.Name = "chkEnableConnectionTreeAnimations";
            chkEnableConnectionTreeAnimations.Size = new System.Drawing.Size(571, 17);
            chkEnableConnectionTreeAnimations.TabIndex = 8;
            chkEnableConnectionTreeAnimations.Text = "Enable connection tree expand/collapse animations";
            chkEnableConnectionTreeAnimations.UseVisualStyleBackColor = true;
            // 
            // pnlOptions
            // 
            pnlOptions.Controls.Add(tableLayoutPanelAppearance);
            pnlOptions.Dock = DockStyle.Top;
            pnlOptions.Location = new System.Drawing.Point(0, 30);
            pnlOptions.Name = "pnlOptions";
            pnlOptions.Padding = new Padding(0, 0, 0, 8);
            pnlOptions.Size = new System.Drawing.Size(610, 260);
            pnlOptions.TabIndex = 8;
            // 
            // tableLayoutPanelAppearance
            // 
            tableLayoutPanelAppearance.ColumnCount = 1;
            tableLayoutPanelAppearance.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanelAppearance.Controls.Add(lblLanguage, 0, 0);
            tableLayoutPanelAppearance.Controls.Add(cboLanguage, 0, 1);
            tableLayoutPanelAppearance.Controls.Add(lblLanguageRestartRequired, 0, 2);
            tableLayoutPanelAppearance.Controls.Add(chkShowDescriptionTooltipsInTree, 0, 3);
            tableLayoutPanelAppearance.Controls.Add(chkShowFullConnectionsFilePathInTitle, 0, 4);
            tableLayoutPanelAppearance.Controls.Add(chkShowSystemTrayIcon, 0, 5);
            tableLayoutPanelAppearance.Controls.Add(chkMinimizeToSystemTray, 0, 6);
            tableLayoutPanelAppearance.Controls.Add(chkCloseToSystemTray, 0, 7);
            tableLayoutPanelAppearance.Controls.Add(chkEnableConnectionTreeAnimations, 0, 8);
            tableLayoutPanelAppearance.Dock = DockStyle.Fill;
            tableLayoutPanelAppearance.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelAppearance.Name = "tableLayoutPanelAppearance";
            tableLayoutPanelAppearance.RowCount = 9;
            tableLayoutPanelAppearance.RowStyles.Add(new RowStyle());
            tableLayoutPanelAppearance.RowStyles.Add(new RowStyle());
            tableLayoutPanelAppearance.RowStyles.Add(new RowStyle());
            tableLayoutPanelAppearance.RowStyles.Add(new RowStyle());
            tableLayoutPanelAppearance.RowStyles.Add(new RowStyle());
            tableLayoutPanelAppearance.RowStyles.Add(new RowStyle());
            tableLayoutPanelAppearance.RowStyles.Add(new RowStyle());
            tableLayoutPanelAppearance.RowStyles.Add(new RowStyle());
            tableLayoutPanelAppearance.RowStyles.Add(new RowStyle());
            tableLayoutPanelAppearance.Size = new System.Drawing.Size(610, 252);
            tableLayoutPanelAppearance.TabIndex = 0;
            // 
            // lblRegistrySettingsUsedInfo
            // 
            lblRegistrySettingsUsedInfo.BackColor = System.Drawing.SystemColors.ControlLight;
            lblRegistrySettingsUsedInfo.Dock = DockStyle.Top;
            lblRegistrySettingsUsedInfo.ForeColor = System.Drawing.SystemColors.ControlText;
            lblRegistrySettingsUsedInfo.Location = new System.Drawing.Point(0, 0);
            lblRegistrySettingsUsedInfo.Name = "lblRegistrySettingsUsedInfo";
            lblRegistrySettingsUsedInfo.Padding = new Padding(0, 2, 0, 0);
            lblRegistrySettingsUsedInfo.Size = new System.Drawing.Size(610, 30);
            lblRegistrySettingsUsedInfo.TabIndex = 9;
            lblRegistrySettingsUsedInfo.Text = "Some settings are configured by your Administrator. Please contact your administrator for more information.";
            lblRegistrySettingsUsedInfo.Visible = false;
            // 
            // AppearancePage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            Controls.Add(pnlOptions);
            Controls.Add(lblRegistrySettingsUsedInfo);
            Name = "AppearancePage";
            Size = new System.Drawing.Size(610, 490);
            pnlOptions.ResumeLayout(false);
            tableLayoutPanelAppearance.ResumeLayout(false);
            tableLayoutPanelAppearance.PerformLayout();
            ResumeLayout(false);
        }

        internal Controls.MrngLabel lblLanguageRestartRequired;
        internal ComboBox cboLanguage;
        internal Controls.MrngLabel lblLanguage;
        internal MrngCheckBox chkShowFullConnectionsFilePathInTitle;
        internal MrngCheckBox chkShowDescriptionTooltipsInTree;
        internal MrngCheckBox chkShowSystemTrayIcon;
        internal MrngCheckBox chkMinimizeToSystemTray;
        internal MrngCheckBox chkCloseToSystemTray;
        internal MrngCheckBox chkEnableConnectionTreeAnimations;
        private Panel pnlOptions;
        private TableLayoutPanel tableLayoutPanelAppearance;
        internal System.Windows.Forms.Label lblRegistrySettingsUsedInfo;
    }
}
