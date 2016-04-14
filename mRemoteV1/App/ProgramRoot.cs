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
            mRemoteNG.frmMain mainForm = new frmMain();
            mRemoteNG.frmMain.Default = mainForm;
            Application.Run(mainForm);
        }
    }
}