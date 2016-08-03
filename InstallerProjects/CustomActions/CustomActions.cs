using Microsoft.Deployment.WindowsInstaller;

namespace CustomActions
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult IsMinimumRdpVersionInstalled(Session session)
        {
            var rdpVersionChecker = new RdpVersionChecker(session);
            rdpVersionChecker.Execute();
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