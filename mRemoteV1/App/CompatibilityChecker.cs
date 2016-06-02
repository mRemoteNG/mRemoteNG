using Microsoft.Win32;
using mRemoteNG.App.Info;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.TaskDialog;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace mRemoteNG.App
{
    public class CompatibilityChecker
    {
        public CompatibilityChecker()
        {

        }

        public void CheckCompatibility()
        {
            CheckFipsPolicy();
            CheckLenovoAutoScrollUtility();
        }

        private void CheckFipsPolicy()
        {
            if (FipsPolicyEnabledForServer2003() || FipsPolicyEnabledForServer2008AndNewer())
            {
                MessageBox.Show(frmMain.Default, string.Format(Language.strErrorFipsPolicyIncompatible, GeneralAppInfo.ProdName, GeneralAppInfo.ProdName, MessageBoxButtons.OK, MessageBoxIcon.Error));
                Environment.Exit(1);
            }
        }

        private bool FipsPolicyEnabledForServer2003()
        {
            var regKey = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Control\\Lsa");
            var fipsPolicy = regKey?.GetValue("FIPSAlgorithmPolicy");
            if (fipsPolicy != null && (int)fipsPolicy != 0)
                return true;
            else
                return false;
        }

        private bool FipsPolicyEnabledForServer2008AndNewer()
        {
            var regKey = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Control\\Lsa\\FIPSAlgorithmPolicy");
            var fipsPolicy = regKey?.GetValue("Enabled");
            if (fipsPolicy != null && (int)fipsPolicy != 0)
                return true;
            else
                return false;
        }

        private void CheckLenovoAutoScrollUtility()
        {
            if (!Settings.Default.CompatibilityWarnLenovoAutoScrollUtility)
                return;

            Process[] proccesses = new Process[] { };
            try
            {
                proccesses = Process.GetProcessesByName("virtscrl");
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("Error in CheckLenovoAutoScrollUtility", ex);
            }

            if (proccesses?.Length > 0)
            {
                CTaskDialog.MessageBox(Application.ProductName, Language.strCompatibilityProblemDetected, string.Format(Language.strCompatibilityLenovoAutoScrollUtilityDetected, Application.ProductName), "", "", Language.strCheckboxDoNotShowThisMessageAgain, ETaskDialogButtons.Ok, ESysIcons.Warning, ESysIcons.Warning);
                if (CTaskDialog.VerificationChecked)
                    Settings.Default.CompatibilityWarnLenovoAutoScrollUtility = false;
            }
        }
    }
}
