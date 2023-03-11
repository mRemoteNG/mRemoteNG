using System.Collections.Generic;
using System.Runtime.Versioning;
using BrightIdeasSoftware;
using mRemoteNG.Connection;

namespace mRemoteNG.UI.Controls.ConnectionTree
{
    [SupportedOSPlatform("windows")]
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
            if (!(modelObject is ConnectionInfo objectAsConnectionInfo))
                return false;

            if (SpecialInclusionList.Contains(objectAsConnectionInfo))
                return true;

            var filterTextLower = FilterText.ToLowerInvariant();

            return objectAsConnectionInfo.Name.ToLowerInvariant().Contains(filterTextLower) ||
                   objectAsConnectionInfo.Hostname.ToLowerInvariant().Contains(filterTextLower) ||
                   objectAsConnectionInfo.Description.ToLowerInvariant().Contains(filterTextLower);
        }
    }
}