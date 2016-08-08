using Microsoft.Deployment.WindowsInstaller;

namespace CustomActions
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult IsMinimumRdpVersionInstalled(Session session)
        {
            var rdpVersionChecker = new InstalledKbChecker("MINIMUM_RDP_KB", "MINIMUM_RDP_VERSION_INSTALLED", session);
            rdpVersionChecker.Execute();
            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult IsRdpDtlsUpdateInstalled(Session session)
        {
            var rdpVersionChecker = new InstalledKbChecker("RDP_DTLS_KB", "RDP_DTLS_UPDATE_INSTALLED", session);
            rdpVersionChecker.Execute();
            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult IsLegacyVersionInstalled(Session session)
        {
            session.Log("Begin IsLegacyVersionInstalled");
            var uninstaller = new UninstallNsisVersions();
            if (uninstaller.IsLegacymRemoteNgInstalled())
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
            var uninstaller = new UninstallNsisVersions();
            uninstaller.GetLegacyUninstallString();
            uninstaller.UninstallLegacyVersion(true);
            session.Log("End UninstallLegacyVersion");
            return ActionResult.Success;
        }
    }
}