using Microsoft.Deployment.WindowsInstaller;

namespace CustomActions
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult IsKBInstalled(Session session)
        {
            session.Log("Begin IsKBInstalled");
            string kb = session["KB"];
            session.Log("Checking if '{0}' is installed", kb);
            InstalledWindowsUpdateGatherer updateGatherer = new InstalledWindowsUpdateGatherer();
            bool isUpdateInstalled = updateGatherer.IsUpdateInstalled(kb);
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
    }
}