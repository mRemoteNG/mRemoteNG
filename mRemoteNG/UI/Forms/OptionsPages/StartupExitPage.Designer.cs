

using mRemoteNG.UI.Controls;

namespace mRemoteNG.UI.Forms.OptionsPages
{

    public sealed partial class StartupExitPage : OptionsPage
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
            chkReconnectOnStart = new MrngCheckBox();
            chkSaveConsOnExit = new MrngCheckBox();
            chkSingleInstance = new MrngCheckBox();
            chkStartMinimized = new MrngCheckBox();
            chkStartFullScreen = new MrngCheckBox();
            SuspendLayout();
            // 
            // chkReconnectOnStart
            // 
            chkReconnectOnStart._mice = MrngCheckBox.MouseState.OUT;
            chkReconnectOnStart.AutoSize = true;
            chkReconnectOnStart.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkReconnectOnStart.Location = new System.Drawing.Point(8, 31);
            chkReconnectOnStart.Name = "chkReconnectOnStart";
            chkReconnectOnStart.Size = new System.Drawing.Size(295, 17);
            chkReconnectOnStart.TabIndex = 1;
            chkReconnectOnStart.Text = "Reconnect to previously opened sessions on startup";
            chkReconnectOnStart.UseVisualStyleBackColor = true;
            // 
            // chkSaveConsOnExit
            // 
            chkSaveConsOnExit._mice = MrngCheckBox.MouseState.OUT;
            chkSaveConsOnExit.AutoSize = true;
            chkSaveConsOnExit.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkSaveConsOnExit.Location = new System.Drawing.Point(8, 7);
            chkSaveConsOnExit.Name = "chkSaveConsOnExit";
            chkSaveConsOnExit.Size = new System.Drawing.Size(153, 17);
            chkSaveConsOnExit.TabIndex = 0;
            chkSaveConsOnExit.Text = "Save connections on exit";
            chkSaveConsOnExit.UseVisualStyleBackColor = true;
            // 
            // chkSingleInstance
            // 
            chkSingleInstance._mice = MrngCheckBox.MouseState.OUT;
            chkSingleInstance.AutoSize = true;
            chkSingleInstance.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkSingleInstance.Location = new System.Drawing.Point(8, 55);
            chkSingleInstance.Name = "chkSingleInstance";
            chkSingleInstance.Size = new System.Drawing.Size(404, 17);
            chkSingleInstance.TabIndex = 2;
            chkSingleInstance.Text = "Allow only a single instance of the application (mRemote restart required)";
            chkSingleInstance.UseVisualStyleBackColor = true;
            // 
            // chkStartMinimized
            // 
            chkStartMinimized._mice = MrngCheckBox.MouseState.OUT;
            chkStartMinimized.AutoSize = true;
            chkStartMinimized.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkStartMinimized.Location = new System.Drawing.Point(8, 78);
            chkStartMinimized.Name = "chkStartMinimized";
            chkStartMinimized.Size = new System.Drawing.Size(105, 17);
            chkStartMinimized.TabIndex = 3;
            chkStartMinimized.Text = "Start minimized";
            chkStartMinimized.UseVisualStyleBackColor = true;
            chkStartMinimized.CheckedChanged += chkStartMinimized_CheckedChanged;
            // 
            // chkStartFullScreen
            // 
            chkStartFullScreen._mice = MrngCheckBox.MouseState.OUT;
            chkStartFullScreen.AutoSize = true;
            chkStartFullScreen.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkStartFullScreen.Location = new System.Drawing.Point(8, 101);
            chkStartFullScreen.Name = "chkStartFullScreen";
            chkStartFullScreen.Size = new System.Drawing.Size(109, 17);
            chkStartFullScreen.TabIndex = 4;
            chkStartFullScreen.Text = "Start Full Screen";
            chkStartFullScreen.UseVisualStyleBackColor = true;
            chkStartFullScreen.CheckedChanged += chkStartFullScreen_CheckedChanged;
            // 
            // StartupExitPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            Controls.Add(chkStartFullScreen);
            Controls.Add(chkReconnectOnStart);
            Controls.Add(chkSaveConsOnExit);
            Controls.Add(chkSingleInstance);
            Controls.Add(chkStartMinimized);
            Name = "StartupExitPage";
            Size = new System.Drawing.Size(610, 490);
            Load += StartupExitPage_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        internal MrngCheckBox chkReconnectOnStart;
        internal MrngCheckBox chkSaveConsOnExit;
        internal MrngCheckBox chkSingleInstance;
        internal MrngCheckBox chkStartMinimized;
        internal MrngCheckBox chkStartFullScreen;
    }
}
