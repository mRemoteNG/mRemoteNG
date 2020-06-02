using System;
using System.Collections.Generic;
using Microsoft.Deployment.WindowsInstaller;

namespace CustomActions
{
    public class KbInstalledChecker
    {
        private readonly Session _session;
        private readonly InstalledWindowsUpdateChecker _installedUpdateChecker;

        public KbInstalledChecker(Session session)
        {
            _installedUpdateChecker = new InstalledWindowsUpdateChecker();
            _session = session;
        }

        public bool Execute(string acceptedKb, string returnVar) => Execute(new[] {acceptedKb}, returnVar);

        public bool Execute(IEnumerable<string> acceptedKbs, string returnVar)
        {
            try
            {
                _session.Log("Begin KbInstalledChecker");
                var isUpdateInstalled = _installedUpdateChecker.IsUpdateInstalled(acceptedKbs);
                SetReturnValue(isUpdateInstalled, returnVar);
                _session.Log("End KbInstalledChecker");
                return true;
            }
            catch (Exception e)
            {
                _session.Log($"There was an issue executing the KbInstalledChecker. Exception: {e}");
                return false;
            }
        }

        private void SetReturnValue(bool isUpdateInstalled, string returnVar)
        {
            var updateInstalledVal = isUpdateInstalled ? "1" : "0";
            _session[returnVar] = updateInstalledVal;
            _session.Log($"Set property '{returnVar}' to '{updateInstalledVal}'");
        }
    }
}