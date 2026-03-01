using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
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
            // .NET 5+ uses CNG crypto implementations that are FIPS-validated.
            // The legacy FIPS warning only applied to .NET Framework's managed implementations
            // (e.g. RijndaelManaged, MD5CryptoServiceProvider) which are no longer used.
            if (Environment.Version.Major >= 5)
            {
                messageCollector.AddMessage(MessageClass.InformationMsg, "FIPS check skipped: .NET 5+ uses FIPS-validated CNG implementations", true);
                return;
            }

            if (Settings.Default.OverrideFIPSCheck)
            {
                messageCollector.AddMessage(MessageClass.InformationMsg, "OverrideFIPSCheck is set. Will skip check", true);
                return;
            }

            messageCollector.AddMessage(MessageClass.InformationMsg, "Checking FIPS policy...", true);
            messageCollector.AddMessage(MessageClass.InformationMsg, $"FIPS2003: {FipsPolicyEnabledForServer2003()}", true);
            messageCollector.AddMessage(MessageClass.InformationMsg, $"FIPS2008+: {FipsPolicyEnabledForServer2008AndNewer()}", true);

            if (!FipsPolicyEnabledForServer2003() && !FipsPolicyEnabledForServer2008AndNewer()) return;

            string errorText = string.Format(CultureInfo.CurrentCulture, Language.ErrorFipsPolicyIncompatible, GeneralAppInfo.ProductName);
            messageCollector.AddMessage(MessageClass.ErrorMsg, errorText, true);

            //About to pop up a message, let's not block it...
            try
            {
                var splash = FrmSplashScreenNew.GetInstance();
                if (!splash.Dispatcher.HasShutdownStarted)
                    splash.Dispatcher.Invoke(() => { splash.Close(); splash.Dispatcher.InvokeShutdown(); });
            }
            catch (TaskCanceledException)

            {

                _ = 0; // Intentionally empty

            }
            catch (OperationCanceledException)

            {

                _ = 0; // Intentionally empty

            }

            DialogResult ShouldIStayOrShouldIGo = CTaskDialog.MessageBox(Application.ProductName ?? string.Empty, Language.CompatibilityProblemDetected, errorText, "", "", Language.CheckboxDoNotShowThisMessageAgain, ETaskDialogButtons.OkCancel, ESysIcons.Warning, ESysIcons.Warning);
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
            RegistryKey? regKey = Registry.LocalMachine.OpenSubKey(@"System\CurrentControlSet\Control\Lsa");
            if (!(regKey?.GetValue("FIPSAlgorithmPolicy") is int fipsPolicy))
                return false;
            return fipsPolicy != 0;
        }

        private static bool FipsPolicyEnabledForServer2008AndNewer()
        {
            RegistryKey? regKey = Registry.LocalMachine.OpenSubKey(@"System\CurrentControlSet\Control\Lsa\FIPSAlgorithmPolicy");
            if (!(regKey?.GetValue("Enabled") is int fipsPolicy))
                return false;
            return fipsPolicy != 0;
        }

        private static void CheckLenovoAutoScrollUtility(MessageCollector messageCollector)
        {
            messageCollector.AddMessage(MessageClass.InformationMsg, "Checking Lenovo AutoScroll Utility...", true);

            if (!Settings.Default.CompatibilityWarnLenovoAutoScrollUtility)
                return;

            Process[] proccesses = Array.Empty<Process>();
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

            CTaskDialog.MessageBox(Application.ProductName ?? string.Empty, Language.CompatibilityProblemDetected,
                                   string.Format(CultureInfo.CurrentCulture, Language.CompatibilityLenovoAutoScrollUtilityDetected,
                                                 Application.ProductName), "",
                                   "", Language.CheckboxDoNotShowThisMessageAgain, ETaskDialogButtons.Ok,
                                   ESysIcons.Warning,
                                   ESysIcons.Warning);
            if (CTaskDialog.VerificationChecked)
                Settings.Default.CompatibilityWarnLenovoAutoScrollUtility = false;
        }
    }
}