using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Versioning;

namespace mRemoteNG.Connection
{
    [SupportedOSPlatform("windows")]
    public class ConnectionInfoComparer<TProperty> : IComparer<ConnectionInfo> where TProperty : IComparable<TProperty>
    {
        private readonly Func<ConnectionInfo, TProperty> _sortExpression;
        public ListSortDirection SortDirection { get; set; } = ListSortDirection.Ascending;

        public ConnectionInfoComparer(Func<ConnectionInfo, TProperty> sortExpression)
        {
            _sortExpression = sortExpression;
        }

        public int Compare(ConnectionInfo x, ConnectionInfo y)
        {
            return SortDirection == ListSortDirection.Ascending ? CompareAscending(x, y) : CompareDescending(x, y);
        }

        private int CompareAscending(ConnectionInfo x, ConnectionInfo y)
        {
            return _sortExpression(x).CompareTo(_sortExpression(y));
        }

        private int CompareDescending(ConnectionInfo x, ConnectionInfo y)
        {
            return _sortExpression(y).CompareTo(_sortExpression(x));
        }
    }
}