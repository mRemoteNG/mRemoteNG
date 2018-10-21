using System.Collections.Generic;
using BrightIdeasSoftware;
using mRemoteNG.Connection;

namespace mRemoteNG.UI.Controls
{
    public class ConnectionTreeSearchTextFilter : IModelFilter
    {
        public string FilterText { get; set; } = "";

        /// <summary>
        /// A list of <see cref="ConnectionInfo"/> objects that should
        /// always be included in the output, regardless of matching
        /// the desired <see cref="FilterText"/>.
        /// </summary>
        public List<ConnectionInfo> SpecialInclusionList { get; } = new List<ConnectionInfo>();

        public bool Filter(object modelObject)
        {
            var objectAsConnectionInfo = modelObject as ConnectionInfo;
            if (objectAsConnectionInfo == null)
                return false;

            if (SpecialInclusionList.Contains(objectAsConnectionInfo))
                return true;

            var filterTextLower = FilterText.ToLowerInvariant();

            if (objectAsConnectionInfo.Name.ToLowerInvariant().Contains(filterTextLower) ||
                objectAsConnectionInfo.Hostname.ToLowerInvariant().Contains(filterTextLower) ||
                objectAsConnectionInfo.Description.ToLowerInvariant().Contains(filterTextLower))
                return true;

            return false;
        }
    }
}
