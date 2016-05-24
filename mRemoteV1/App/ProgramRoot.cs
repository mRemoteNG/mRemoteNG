using System;
using System.Windows.Forms;
using mRemoteNG.UI.Forms;

namespace mRemoteNG.App
{
    public static class ProgramRoot
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            Startup.InitializeProgram();
            Application.Run(frmMain.Default);
        }
    }
}
