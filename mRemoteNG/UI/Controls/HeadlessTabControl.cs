using System;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.App;

namespace mRemoteNG.UI.Controls
{
    [SupportedOSPlatform("windows")]
    public class HeadlessTabControl : TabControl
    {
        protected override void WndProc(ref Message m)
        {
            // Hide tabs by trapping the TCM_ADJUSTRECT message
            if (m.Msg == NativeMethods.TCM_ADJUSTRECT && !DesignMode)
                m.Result = (IntPtr)1;
            else
                base.WndProc(ref m);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // HeadlessTabControl
            // 
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular,
                                                System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ResumeLayout(false);
        }
    }
}