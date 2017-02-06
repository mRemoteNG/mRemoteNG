using Microsoft.Win32;
using mRemoteNG.App.Info;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.TaskDialog;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using mRemoteNG.Messages;

namespace mRemoteNG.App
{
    public static class CompatibilityChecker
    {
        public static void CheckCompatibility(MessageCollector messageCollector)
        {
            CheckFipsPolicy(messageCollector);
            CheckLenovoAutoScrollUtility(messageCollector);
        }

        private static void CheckFipsPolicy(MessageCollector messageCollector)
        {
            messageCollector.AddMessage(MessageClass.InformationMsg, "Checking FIPS Policy...", true);
            if (!FipsPolicyEnabledForServer2003() && !FipsPolicyEnabledForServer2008AndNewer()) return;
            var errorText = string.Format(Language.strErrorFipsPolicyIncompatible, GeneralAppInfo.ProductName,
                GeneralAppInfo.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            messageCollector.AddMessage(MessageClass.ErrorMsg, errorText, true);
            MessageBox.Show(FrmMain.Default, errorText);
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

        private static void CheckLenovoAutoScrollUtility(MessageCollector messageCollector)
        {
            messageCollector.AddMessage(MessageClass.InformationMsg, "Checking Lenovo AutoScroll Utility...", true);

            if (!Settings.Default.CompatibilityWarnLenovoAutoScrollUtility)
                return;

            var proccesses = new Process[] { };
            try
            {
                proccesses = Process.GetProcessesByName("virtscrl");
            }
            catch (InvalidOperationException ex)
            {
                messageCollector.AddExceptionMessage("Error in CheckLenovoAutoScrollUtility", ex);
            }

            if (proccesses.Length <= 0) return;
            CTaskDialog.MessageBox(Application.ProductName, Language.strCompatibilityProblemDetected, string.Format(Language.strCompatibilityLenovoAutoScrollUtilityDetected, Application.ProductName), "", "", Language.strCheckboxDoNotShowThisMessageAgain, ETaskDialogButtons.Ok, ESysIcons.Warning, ESysIcons.Warning);
            if (CTaskDialog.VerificationChecked)
                Settings.Default.CompatibilityWarnLenovoAutoScrollUtility = false;
        }
    }
}
