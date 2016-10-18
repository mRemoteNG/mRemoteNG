using System.Collections.Generic;
using System.Linq;
using mRemoteNG.Connection;


namespace mRemoteNG.Tree
{
    internal class NodeSearcher
    {
        private readonly ConnectionTreeModel _connectionTreeModel;

        private List<ConnectionInfo> Matches { get; set; }
        public ConnectionInfo CurrentMatch { get; private set; }


        public NodeSearcher(ConnectionTreeModel connectionTreeModel)
        {
            _connectionTreeModel = connectionTreeModel;
        }

        internal IEnumerable<ConnectionInfo> SearchByName(string searchText)
        {
            ResetMatches();
            if (searchText == "") return Matches;
            var nodes = (List<ConnectionInfo>)_connectionTreeModel.GetRecursiveChildList();
            foreach (var node in nodes)
            {
                if (node.Name.ToLowerInvariant().Contains(searchText.ToLowerInvariant()))
                    Matches.Add(node);
            }
            if (Matches.Count > 0)
                CurrentMatch = Matches.First();
            return Matches;
        }

        internal ConnectionInfo NextMatch()
        {
            var currentMatchIndex = Matches.IndexOf(CurrentMatch);
            if (currentMatchIndex < Matches.Count-1)
                CurrentMatch = Matches[currentMatchIndex + 1];
            return CurrentMatch;
        }

        internal ConnectionInfo PreviousMatch()
        {
            var currentMatchIndex = Matches.IndexOf(CurrentMatch);
            if (currentMatchIndex > 0)
                CurrentMatch = Matches[currentMatchIndex - 1];
            return CurrentMatch;
        }

        private void ResetMatches()
        {
            Matches = new List<ConnectionInfo>();
            CurrentMatch = null;
        }
    }
}