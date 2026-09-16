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
            string searchTextLower = searchText.ToLowerInvariant();
            foreach (ConnectionInfo node in nodes)
            {
                if (node.Name.ToLowerInvariant().Contains(searchTextLower) ||
                    node.Description.ToLowerInvariant().Contains(searchTextLower) ||
                    node.Hostname.ToLowerInvariant().Contains(searchTextLower))
                    Matches.Add(node);
            }

            if (Matches.Count > 0)
                CurrentMatch = Matches.First();
            return Matches;
        }

        public ConnectionInfo? NextMatch()
        {
            int currentMatchIndex = CurrentMatchIndex();
            if (!CurrentMatchIsTheLastMatchInTheList())
                CurrentMatch = Matches[currentMatchIndex + 1];
            return CurrentMatch;
        }

        private bool CurrentMatchIsTheLastMatchInTheList()
        {
            return CurrentMatchIndex() >= Matches.Count - 1;
        }

        public ConnectionInfo? PreviousMatch()
        {
            int currentMatchIndex = CurrentMatchIndex();
            if (!CurrentMatchIsTheFirstMatchInTheList())
                CurrentMatch = Matches[currentMatchIndex - 1];
            return CurrentMatch;
        }

        private bool CurrentMatchIsTheFirstMatchInTheList()
        {
            return CurrentMatchIndex() <= 0;
        }

        // IndexOf of a null match is -1; guard so the non-null IndexOf overload is satisfied.
        private int CurrentMatchIndex()
        {
            return CurrentMatch is null ? -1 : Matches.IndexOf(CurrentMatch);
        }

        private void ResetMatches()
        {
            Matches = [];
            CurrentMatch = null;
        }
    }
}