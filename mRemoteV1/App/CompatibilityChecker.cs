using Microsoft.Win32;
using mRemoteNG.App.Info;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.TaskDialog;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace mRemoteNG.App
{
    public static class CompatibilityChecker
    {
        public static void CheckCompatibility()
        {
            CheckFipsPolicy();
            CheckLenovoAutoScrollUtility();
        }

        private static void CheckFipsPolicy()
        {
            Logger.Instance.InfoFormat("Checking FIPS Policy...");
            if (!FipsPolicyEnabledForServer2003() && !FipsPolicyEnabledForServer2008AndNewer()) return;
            MessageBox.Show(frmMain.Default, string.Format(Language.strErrorFipsPolicyIncompatible, GeneralAppInfo.ProductName, GeneralAppInfo.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error));
            Environment.Exit(1);
        }

        private static bool FipsPolicyEnabledForServer2003()
        {
            var regKey = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Control\\Lsa");
            var fipsPolicy = regKey?.GetValue("FIPSAlgorithmPolicy");
            if (fipsPolicy == null) return false;
            fipsPolicy = Convert.ToInt32(fipsPolicy);
            return (int)fipsPolicy != 0;
        }

        private static bool FipsPolicyEnabledForServer2008AndNewer()
        {
            var regKey = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Control\\Lsa\\FIPSAlgorithmPolicy");
            var fipsPolicy = regKey?.GetValue("Enabled");
            if (fipsPolicy == null) return false;
            fipsPolicy = Convert.ToInt32(fipsPolicy);
            return (int)fipsPolicy != 0;
        }

        private static void CheckLenovoAutoScrollUtility()
        {
            Logger.Instance.InfoFormat("Checking Lenovo AutoScroll Utility...");

            if (!Settings.Default.CompatibilityWarnLenovoAutoScrollUtility)
                return;

            var proccesses = new Process[] { };
            try
            {
                proccesses = Process.GetProcessesByName("virtscrl");
            }
            catch (InvalidOperationException ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("Error in CheckLenovoAutoScrollUtility", ex);
            }

            if (proccesses.Length <= 0) return;
            CTaskDialog.MessageBox(Application.ProductName, Language.strCompatibilityProblemDetected, string.Format(Language.strCompatibilityLenovoAutoScrollUtilityDetected, Application.ProductName), "", "", Language.strCheckboxDoNotShowThisMessageAgain, ETaskDialogButtons.Ok, ESysIcons.Warning, ESysIcons.Warning);
            if (CTaskDialog.VerificationChecked)
                Settings.Default.CompatibilityWarnLenovoAutoScrollUtility = false;
        }
    }
}
