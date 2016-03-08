using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace mRemoteNG
{
    public static class ProgramRoot
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            // Create main form
            mRemoteNG.frmMain mainForm = new frmMain();
            // Set default form
            mRemoteNG.frmMain.Default = mainForm;
            // Run the default form
            Application.Run(mainForm);
        }
    }
}