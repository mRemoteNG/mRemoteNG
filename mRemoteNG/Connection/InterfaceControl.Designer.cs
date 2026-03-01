namespace mRemoteNG.Connection
{
    public sealed partial class InterfaceControl : System.Windows.Forms.Panel
	{
		//UserControl overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]
        		protected override void Dispose(bool disposing)
        		{
        			try
        			{
        				if (disposing)
        				{
        					Protocol?.Dispose();
        					if (components != null)
        					{
        						components.Dispose();
        					}
        				}
        			}
        			finally
        			{
        				try
        				{
        					base.Dispose(disposing);
        				}
        				catch (System.Runtime.InteropServices.InvalidComObjectException)
        				{
        					// RDP ActiveX control (MSTSCLib) may already be detached from its RCW
        					// when the protocol was closed before Dispose runs.
        				}
        			}
        		}			
		//Required by the Windows Form Designer
		private System.ComponentModel.Container components = null;

        //NOTE: The following procedure is required by the Windows Form Designer
        //It can be modified using the Windows Form Designer.
        //Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent() => components = new System.ComponentModel.Container();
    }
}
