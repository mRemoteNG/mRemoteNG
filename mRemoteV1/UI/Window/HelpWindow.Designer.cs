
namespace mRemoteNG.UI.Window
{
    partial class HelpWindow
    {
        #region  Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.cefBrwoser = new CefSharp.WinForms.ChromiumWebBrowser();
            this.SuspendLayout();
            // 
            // chromiumWebBrowser1
            // 
            this.cefBrwoser.ActivateBrowserOnCreation = false;
            this.cefBrwoser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cefBrwoser.Location = new System.Drawing.Point(0, 0);
            this.cefBrwoser.Name = "chromiumWebBrowser1";
            this.cefBrwoser.Size = new System.Drawing.Size(1117, 705);
            this.cefBrwoser.TabIndex = 0;
            // 
            // HelpWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1117, 705);
            this.Controls.Add(this.cefBrwoser);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = global::mRemoteNG.Resources.Help_Icon;
            this.Name = "HelpWindow";
            this.TabText = "Help";
            this.Text = "Help";
            this.Load += new System.EventHandler(this.HelpWindow_Load);
            this.ResumeLayout(false);

        }
        #endregion

        private CefSharp.WinForms.ChromiumWebBrowser cefBrwoser;
    }
}
