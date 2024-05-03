using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using mRemoteNG.Connection;


namespace mRemoteNG.Tree
{
    [SupportedOSPlatform("windows")]
    public class NodeSearcher
    {
        private readonly ConnectionTreeModel _connectionTreeModel;

        private List<ConnectionInfo> Matches { get; set; }
        public ConnectionInfo CurrentMatch { get; private set; }


        public NodeSearcher(ConnectionTreeModel connectionTreeModel)
        {
            _connectionTreeModel = connectionTreeModel;
        }

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

        public ConnectionInfo NextMatch()
        {
            int currentMatchIndex = Matches.IndexOf(CurrentMatch);
            if (!CurrentMatchIsTheLastMatchInTheList())
                CurrentMatch = Matches[currentMatchIndex + 1];
            return CurrentMatch;
        }

        private bool CurrentMatchIsTheLastMatchInTheList()
        {
            int currentMatchIndex = Matches.IndexOf(CurrentMatch);
            return currentMatchIndex >= Matches.Count - 1;
        }

        public ConnectionInfo PreviousMatch()
        {
            int currentMatchIndex = Matches.IndexOf(CurrentMatch);
            if (!CurrentMatchIsTheFirstMatchInTheList())
                CurrentMatch = Matches[currentMatchIndex - 1];
            return CurrentMatch;
        }

        private bool CurrentMatchIsTheFirstMatchInTheList()
        {
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