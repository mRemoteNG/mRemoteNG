using BrightIdeasSoftware;
using mRemoteNG.Connection;

namespace mRemoteNG.UI.Controls
{
    public class ConnectionTreeSearchTextFilter : IModelFilter
    {
        public string FilterText { get; set; } = "";

        public bool Filter(object modelObject)
        {
            var objectAsConnectionInfo = modelObject as ConnectionInfo;
            if (objectAsConnectionInfo == null)
                return false;

            var filterTextLower = FilterText.ToLowerInvariant();

            if (objectAsConnectionInfo.Name.ToLowerInvariant().Contains(filterTextLower) ||
                objectAsConnectionInfo.Hostname.ToLowerInvariant().Contains(filterTextLower) ||
                objectAsConnectionInfo.Description.ToLowerInvariant().Contains(filterTextLower))
                return true;

            return false;
        }
    }
}
