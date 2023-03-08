using System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Win32;
using mRemoteNG.App.Info;
using mRemoteNG.Messages;
using mRemoteNG.Properties;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.TaskDialog;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.App
{
    [SupportedOSPlatform("windows")]
    public static class CompatibilityChecker
    {
        public static void CheckCompatibility(MessageCollector messageCollector)
        {
            CheckFipsPolicy(messageCollector);
            CheckLenovoAutoScrollUtility(messageCollector);
        }

        private static void CheckFipsPolicy(MessageCollector messageCollector)
        {
            if (Settings.Default.OverrideFIPSCheck)
            {
                messageCollector.AddMessage(MessageClass.InformationMsg, "OverrideFIPSCheck is set. Will skip check", true);
                return;
            }

            messageCollector.AddMessage(MessageClass.InformationMsg, "Checking FIPS policy...", true);
            messageCollector.AddMessage(MessageClass.InformationMsg, $"FIPS2003: {FipsPolicyEnabledForServer2003()}", true);
            messageCollector.AddMessage(MessageClass.InformationMsg, $"FIPS2008+: {FipsPolicyEnabledForServer2008AndNewer()}", true);

            if (!FipsPolicyEnabledForServer2003() && !FipsPolicyEnabledForServer2008AndNewer()) return;

            var errorText = string.Format(Language.ErrorFipsPolicyIncompatible, GeneralAppInfo.ProductName);
            messageCollector.AddMessage(MessageClass.ErrorMsg, errorText, true);

            //About to pop up a message, let's not block it...
            FrmSplashScreenNew.GetInstance().Close();

            var ShouldIStayOrShouldIGo = CTaskDialog.MessageBox(Application.ProductName, Language.CompatibilityProblemDetected, errorText, "", "", Language.CheckboxDoNotShowThisMessageAgain, ETaskDialogButtons.OkCancel, ESysIcons.Warning, ESysIcons.Warning);
            if (CTaskDialog.VerificationChecked && ShouldIStayOrShouldIGo == DialogResult.OK)
            {
                messageCollector.AddMessage(MessageClass.ErrorMsg, "User requests that FIPS check be overridden", true);
                Settings.Default.OverrideFIPSCheck = true;
                Settings.Default.Save();
                return;
            }

            if (ShouldIStayOrShouldIGo == DialogResult.Cancel)
                Environment.Exit(1);
        }

        private static bool FipsPolicyEnabledForServer2003()
        {
            var regKey = Registry.LocalMachine.OpenSubKey(@"System\CurrentControlSet\Control\Lsa");
            if (!(regKey?.GetValue("FIPSAlgorithmPolicy") is int fipsPolicy))
                return false;
            return fipsPolicy != 0;
        }

        private static bool FipsPolicyEnabledForServer2008AndNewer()
        {
            var regKey = Registry.LocalMachine.OpenSubKey(@"System\CurrentControlSet\Control\Lsa\FIPSAlgorithmPolicy");
            if (!(regKey?.GetValue("Enabled") is int fipsPolicy))
                return false;
            return fipsPolicy != 0;
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

            if (proccesses.Length <= 0)
            {
                messageCollector.AddMessage(MessageClass.InformationMsg, "Lenovo AutoScroll Utility not found", true);
                return;
            }

            messageCollector.AddMessage(MessageClass.WarningMsg, "Lenovo AutoScroll Utility found", true);

            CTaskDialog.MessageBox(Application.ProductName, Language.CompatibilityProblemDetected,
                                   string.Format(Language.CompatibilityLenovoAutoScrollUtilityDetected,
                                                 Application.ProductName), "",
                                   "", Language.CheckboxDoNotShowThisMessageAgain, ETaskDialogButtons.Ok,
                                   ESysIcons.Warning,
                                   ESysIcons.Warning);
            if (CTaskDialog.VerificationChecked)
                Settings.Default.CompatibilityWarnLenovoAutoScrollUtility = false;
        }
    }
}