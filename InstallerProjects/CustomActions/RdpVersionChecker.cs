using System;
using Microsoft.Deployment.WindowsInstaller;

namespace CustomActions
{
    public class RdpVersionChecker
    {
        private readonly Session _session;
        private const string MinimumVersionInstalledReturnVar = "MINIMUM_RDP_VERSION_INSTALLED";
        private const string MinimumRdpKbVariable = "MINIMUM_RDP_KB";

        public RdpVersionChecker(Session session)
        {
            _session = session;
        }

        public bool Execute()
        {
            try
            {
                _session.Log("Begin IsMinimumRdpVersionInstalled");
                var minimumKb = _session[MinimumRdpKbVariable];
                var isUpdateInstalled = IsUpdateInstalled(minimumKb);
                SetReturnValue(isUpdateInstalled);
                _session.Log("End IsMinimumRdpVersionInstalled");
                return true;
            }
            catch (Exception e)
            {
                _session.Log($"There was an issue executing the RdpVersionChecker. Exception: {e}");
                return false;
            }
        }

        private bool IsUpdateInstalled(string minimumKb)
        {
            _session.Log("Checking if '{0}' is installed", minimumKb);
            var updateGatherer = new InstalledWindowsUpdateGatherer();
            var isUpdateInstalled = updateGatherer.IsUpdateInstalled(minimumKb);
            _session.Log("KB is installed = '{0}'", isUpdateInstalled);
            return isUpdateInstalled;
        }

        private void SetReturnValue(bool isUpdateInstalled)
        {
            var updateInstalledVal = isUpdateInstalled ? "1" : "0";
            _session[MinimumVersionInstalledReturnVar] = updateInstalledVal;
            _session.Log($"Set property '{MinimumVersionInstalledReturnVar}' to '{updateInstalledVal}'");
        }
    }
}