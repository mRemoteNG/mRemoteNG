using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;
using BrightIdeasSoftware;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.UI.Controls.ConnectionTree
{
    [SupportedOSPlatform("windows")]
    public class ConnectionTreeSearchTextFilter : IModelFilter
    {
        public string FilterText { get; set; } = "";

        /// <summary>
        /// Optional protocol type filter. When set, only connections
        /// matching this protocol are shown.
        /// </summary>
        public ProtocolType? FilterProtocol { get; set; }

        /// <summary>
        /// A list of <see cref="ConnectionInfo"/> objects that should
        /// always be included in the output, regardless of matching
        /// the desired <see cref="FilterText"/>.
        /// </summary>
        public IList<ConnectionInfo> SpecialInclusionList { get; } = [];

        public bool Filter(object modelObject)
        {
            if (!(modelObject is ConnectionInfo objectAsConnectionInfo))
                return false;

            if (SpecialInclusionList.Contains(objectAsConnectionInfo))
                return true;

            // Exclude nodes that belong to a folder marked ExcludeFromSearch
            if (IsUnderExcludedContainer(objectAsConnectionInfo))
                return false;

            if (NodeMatchesFilter(objectAsConnectionInfo))
                return true;

            // Show all descendants of a folder whose name directly matches the
            // filter (issue #2178: "search folder name not the connections").
            if (AnyAncestorContainerMatchesFilter(objectAsConnectionInfo))
                return true;

            // For containers, keep visible if any descendant matches so that
            // search finds connections inside collapsed/non-matching folders.
            if (objectAsConnectionInfo is ContainerInfo container)
                return container.GetRecursiveChildList()
                                .Where(child => !IsUnderExcludedContainer(child))
                                .Any(NodeMatchesFilter);

            return false;
        }

        /// <summary>
        /// Returns true if any ancestor folder (non-root <see cref="ContainerInfo"/>)
        /// directly matches the filter text by name, so that all descendants of a
        /// matching folder are included in the search results (issues #2178, #2293).
        /// Root nodes are excluded because they have generic names (e.g.
        /// "Connections") that would otherwise match many unrelated searches.
        /// Only the folder's <em>name</em> is checked — hostname, description and
        /// tags are connection-specific fields and are not meaningful on folders.
        /// </summary>
        private bool AnyAncestorContainerMatchesFilter(ConnectionInfo node)
        {
            ContainerInfo? parent = node.Parent;
            while (parent != null && parent is not RootNodeInfo)
            {
                if (NodeMatchesFolderNameFilter(parent))
                    return true;
                parent = parent.Parent;
            }
            return false;
        }

        /// <summary>
        /// Checks whether <paramref name="node"/> matches the filter by name only,
        /// ignoring protocol, hostname, description and tags.  Used for the ancestor
        /// folder check (issue #2293) so that typing a folder name reveals all
        /// connections inside it without accidentally matching on connection-only fields.
        /// </summary>
        private bool NodeMatchesFolderNameFilter(ConnectionInfo node)
        {
            string filterTextLower = FilterText.ToLowerInvariant();

            // regex: prefix — match against folder name only
            if (filterTextLower.StartsWith("regex:", StringComparison.Ordinal))
            {
                try
                {
                    string pattern = FilterText.Substring(6);
                    return new Regex(pattern, RegexOptions.IgnoreCase).IsMatch(node.Name);
                }
                catch (ArgumentException)
                {
                    return false;
                }
            }

            // protocol: and tag: prefixes are connection-specific — no folder match
            if (filterTextLower.StartsWith("protocol:", StringComparison.Ordinal) ||
                filterTextLower.StartsWith("tag:", StringComparison.Ordinal))
                return false;

            // AND-logic across space-separated terms, name only
            string[] terms = filterTextLower.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (terms.Length == 0)
                return true;

            string nameLower = node.Name.ToLowerInvariant();
            return terms.All(term => nameLower.Contains(term));
        }

        /// <summary>
        /// Returns true if <paramref name="node"/> itself is a container with
        /// <see cref="ContainerInfo.ExcludeFromSearch"/> set, or if any ancestor
        /// container has that flag set.
        /// </summary>
        private static bool IsUnderExcludedContainer(ConnectionInfo node)
        {
            if (node is ContainerInfo self && self.ExcludeFromSearch)
                return true;

            ContainerInfo? parent = node.Parent;
            while (parent != null)
            {
                if (parent.ExcludeFromSearch)
                    return true;
                parent = parent.Parent;
            }
            return false;
        }

        private bool NodeMatchesFilter(ConnectionInfo node)
        {
            // Protocol filter: exclude connections that don't match
            if (FilterProtocol.HasValue && node.Protocol != FilterProtocol.Value)
                return false;

            string filterTextLower = FilterText.ToLowerInvariant();

            // Support "regex:" prefix syntax
            if (filterTextLower.StartsWith("regex:", StringComparison.Ordinal))
            {
                try
                {
                    string pattern = FilterText.Substring(6);
                    Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                    return regex.IsMatch(node.Name) ||
                           regex.IsMatch(node.Hostname) ||
                           regex.IsMatch(node.Description) ||
                           regex.IsMatch(node.EnvironmentTags ?? "");
                }
                catch (ArgumentException)
                {
                    return false;
                }
            }

            // Support "protocol:RDP" and "tag:production" prefix syntax
            if (filterTextLower.StartsWith("protocol:", StringComparison.Ordinal))
            {
                string protocolFilter = filterTextLower.Substring(9).Trim();
                return node.Protocol.ToString().Contains(protocolFilter, StringComparison.OrdinalIgnoreCase);
            }

            if (filterTextLower.StartsWith("tag:", StringComparison.Ordinal))
            {
                string tagFilter = filterTextLower.Substring(4).Trim();
                return (node.EnvironmentTags ?? "").Contains(tagFilter, StringComparison.OrdinalIgnoreCase);
            }

            // Multiple space-separated terms are treated as AND criteria:
            // all terms must match at least one field for the node to pass.
            string[] terms = filterTextLower.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (terms.Length == 0)
                return true;

            string nameLower = node.Name.ToLowerInvariant();
            string hostnameLower = node.Hostname.ToLowerInvariant();
            string descriptionLower = node.Description.ToLowerInvariant();
            string tagsLower = (node.EnvironmentTags ?? "").ToLowerInvariant();

            return terms.All(term =>
                nameLower.Contains(term) ||
                hostnameLower.Contains(term) ||
                descriptionLower.Contains(term) ||
                tagsLower.Contains(term));
        }
    }
}