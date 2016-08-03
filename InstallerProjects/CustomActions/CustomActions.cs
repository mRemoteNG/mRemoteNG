using Microsoft.Deployment.WindowsInstaller;

namespace CustomActions
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult IsMinimumRdpVersionInstalleds(Session session)
        {
            var rdpVersionChecker = new RdpVersionChecker(session);
            rdpVersionChecker.Execute();
            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult IsMinimumRdpVersionInstalled(Session session)
        {
            const string MinimumVersionInstalledReturnVar = "MINIMUM_RDP_VERSION_INSTALLED";
            const string MinimumRdpKbVariable = "MINIMUM_RDP_KB";
            session.Log("Begin IsMinimumRdpVersionInstalled");
            var minimumKb = session[MinimumRdpKbVariable];
            session.Log("Checking if '{0}' is installed", minimumKb);
            var updateGatherer = new InstalledWindowsUpdateGatherer();
            var isUpdateInstalled = updateGatherer.IsUpdateInstalled(minimumKb);
            session.Log("KB is installed = '{0}'", isUpdateInstalled);
            var updateInstalledVal = isUpdateInstalled ? "1" : "0";
            session[MinimumVersionInstalledReturnVar] = updateInstalledVal;
            session.Log($"Set property '{MinimumVersionInstalledReturnVar}' to '{updateInstalledVal}'");
            session.Log("End IsMinimumRdpVersionInstalled");

            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult IsLegacyVersionInstalled(Session session)
        {
            session.Log("Begin IsLegacyVersionInstalled");
            var uninstaller = new UninstallNSISVersions();
            if (uninstaller.IsLegacymRemoteNGInstalled())
            {
                session["LEGACYVERSIONINSTALLED"] = "1";
            }
            else
            {
                session["LEGACYVERSIONINSTALLED"] = "0";
            }

            session.Log("End IsLegacyVersionInstalled");
            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult UninstallLegacyVersion(Session session)
        {
            session.Log("Begin UninstallLegacyVersion");
            var uninstaller = new UninstallNSISVersions();
            var uninstallString = uninstaller.GetLegacyUninstallString();
            uninstaller.UninstallLegacyVersion(true);
            session.Log("End UninstallLegacyVersion");
            return ActionResult.Success;
        }
    }
}