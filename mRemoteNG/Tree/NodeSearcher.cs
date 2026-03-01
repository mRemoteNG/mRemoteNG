using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using mRemoteNG.Connection;


namespace mRemoteNG.Tree
{
    [SupportedOSPlatform("windows")]
    public class NodeSearcher(ConnectionTreeModel connectionTreeModel)
    {
        private readonly ConnectionTreeModel _connectionTreeModel = connectionTreeModel;

        private List<ConnectionInfo> Matches { get; set; } = [];
        public ConnectionInfo? CurrentMatch { get; private set; }

        public IEnumerable<ConnectionInfo> SearchByName(string searchText)
        {
            ResetMatches();
            if (searchText == "") return Matches;
            IReadOnlyList<ConnectionInfo> nodes = _connectionTreeModel.GetRecursiveChildList();
            foreach (ConnectionInfo node in nodes)
            {
                if (node.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    node.Description.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    node.Hostname.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    (node.EnvironmentTags ?? "").Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    Matches.Add(node);
            }

            Matches = Matches.OrderByDescending(node =>
                node.Name.Equals(searchText, System.StringComparison.OrdinalIgnoreCase) ||
                node.Hostname.Equals(searchText, System.StringComparison.OrdinalIgnoreCase)
            ).ToList();

            if (Matches.Count > 0)
                CurrentMatch = Matches.First();
            return Matches;
        }

        public ConnectionInfo? NextMatch()
        {
            if (CurrentMatch is null) return CurrentMatch;
            int currentMatchIndex = Matches.IndexOf(CurrentMatch);
            if (!CurrentMatchIsTheLastMatchInTheList())
                CurrentMatch = Matches[currentMatchIndex + 1];
            return CurrentMatch;
        }

        private bool CurrentMatchIsTheLastMatchInTheList()
        {
            if (CurrentMatch is null) return true;
            int currentMatchIndex = Matches.IndexOf(CurrentMatch);
            return currentMatchIndex >= Matches.Count - 1;
        }

        public ConnectionInfo? PreviousMatch()
        {
            if (CurrentMatch is null) return CurrentMatch;
            int currentMatchIndex = Matches.IndexOf(CurrentMatch);
            if (!CurrentMatchIsTheFirstMatchInTheList())
                CurrentMatch = Matches[currentMatchIndex - 1];
            return CurrentMatch;
        }

        private bool CurrentMatchIsTheFirstMatchInTheList()
        {
            if (CurrentMatch is null) return true;
            int currentMatchIndex = Matches.IndexOf(CurrentMatch);
            return currentMatchIndex <= 0;
        }

        private void ResetMatches()
        {
            Matches = [];
            CurrentMatch = null;
        }
    }
}