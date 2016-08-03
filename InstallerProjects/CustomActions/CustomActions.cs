using Microsoft.Deployment.WindowsInstaller;

namespace CustomActions
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult IsKBInstalled(Session session)
        {
            session.Log("Begin IsKBInstalled");
            var kb = session["KB"];
            session.Log("Checking if '{0}' is installed", kb);
            var updateGatherer = new InstalledWindowsUpdateGatherer();
            var isUpdateInstalled = updateGatherer.IsUpdateInstalled(kb);
            session.Log("KB is installed = '{0}'", isUpdateInstalled);
            if (isUpdateInstalled)
            {
                session[kb] = "1";
                session.Log("Set property '{0}' to '1'", kb);
            }
            else
            {
                session[kb] = "0";
                session.Log("Set property '{0}' to '0'", kb);
            }

            session.Log("End IsKBInstalled");
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