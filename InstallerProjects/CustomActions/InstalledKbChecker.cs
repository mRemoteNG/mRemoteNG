using System;
using Microsoft.Deployment.WindowsInstaller;

namespace CustomActions
{
    public class InstalledKbChecker
    {
        private readonly Session _session;
        private readonly string _kbVariable;
        private readonly string _returnVar;

        public InstalledKbChecker(string kbVariable, string returnVar, Session session)
        {
            _kbVariable = kbVariable;
            _returnVar = returnVar;
            _session = session;
        }

        public bool Execute()
        {
            try
            {
                _session.Log("Begin InstalledKbChecker");
                var minimumKb = _session[_kbVariable];
                var isUpdateInstalled = IsUpdateInstalled(minimumKb);
                SetReturnValue(isUpdateInstalled);
                _session.Log("End InstalledKbChecker");
                return true;
            }
            catch (Exception e)
            {
                _session.Log($"There was an issue executing the RdpVersionChecker. Exception: {e}");
                return false;
            }
        }

        private bool IsUpdateInstalled(string kb)
        {
            _session.Log($"Checking if '{kb}' is installed");
            var updateGatherer = new InstalledWindowsUpdateGatherer();
            var isUpdateInstalled = updateGatherer.IsUpdateInstalled(kb);
            _session.Log($"KB is installed = '{isUpdateInstalled}'");
            return isUpdateInstalled;
        }

        private void SetReturnValue(bool isUpdateInstalled)
        {
            var updateInstalledVal = isUpdateInstalled ? "1" : "0";
            _session[_returnVar] = updateInstalledVal;
            _session.Log($"Set property '{_returnVar}' to '{updateInstalledVal}'");
        }
    }
}