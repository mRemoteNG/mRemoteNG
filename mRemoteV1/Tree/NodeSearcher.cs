using System.Collections.Generic;
using System.Linq;
using mRemoteNG.Connection;


namespace mRemoteNG.Tree
{
    internal class NodeSearcher
    {
        private readonly ConnectionTreeModel _connectionTreeModel;
        private ConnectionInfo _currentMatch;

        public List<ConnectionInfo> Matches { get; private set; }

        public NodeSearcher(ConnectionTreeModel connectionTreeModel)
        {
            _connectionTreeModel = connectionTreeModel;
        }

        internal IEnumerable<ConnectionInfo> SearchByName(string searchText)
        {
            if (searchText == "")
                ResetMatches();
            else
            {
                Matches = (List<ConnectionInfo>)_connectionTreeModel.GetRecursiveChildList().Where(node => node.Name.Contains(searchText));
                _currentMatch = Matches.First();
            }
            return Matches;
        }

        internal ConnectionInfo NextMatch()
        {
            var currentMatchIndex = Matches.IndexOf(_currentMatch);
            if (currentMatchIndex < Matches.Count-1)
                _currentMatch = Matches[currentMatchIndex + 1];
            return _currentMatch;
        }

        internal ConnectionInfo PreviousMatch()
        {
            var currentMatchIndex = Matches.IndexOf(_currentMatch);
            if (currentMatchIndex > 0)
                _currentMatch = Matches[currentMatchIndex - 1];
            return _currentMatch;
        }

        private void ResetMatches()
        {
            Matches = new List<ConnectionInfo>();
            _currentMatch = null;
        }
    }
}