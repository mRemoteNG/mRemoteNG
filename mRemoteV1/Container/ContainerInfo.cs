using System;
using System.Collections.Generic;
using mRemoteNG.Connection;
using System.ComponentModel;
using System.Linq;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Tools;
using mRemoteNG.Tree;

namespace mRemoteNG.Container
{
    [DefaultProperty("Name")]
    public class ContainerInfo : ConnectionInfo
	{
        [Browsable(false)]
        public List<ConnectionInfo> Children { get; set; } = new List<ConnectionInfo>();

        [Category(""), Browsable(false), ReadOnly(false), Bindable(false), DefaultValue(""), DesignOnly(false)]
        public bool IsExpanded { get; set; }

        public ContainerInfo()
        {
            SetDefaults();
            IsContainer = true;
        }

        public override TreeNodeType GetTreeNodeType()
        {
            return TreeNodeType.Container;
        }

        public bool HasChildren()
        {
            return Children.Count > 0;
        }

        public void AddChild(ConnectionInfo newChildItem)
        {
            newChildItem.Parent = this;
            Children.Add(newChildItem);
        }

        public void AddChildRange(IEnumerable<ConnectionInfo> newChildren)
        {
            foreach (var child in newChildren)
            {
                AddChild(child);
            }
        }

        public void RemoveChild(ConnectionInfo removalTarget)
        {
            removalTarget.Parent = null;
            Children.Remove(removalTarget);
        }

        public void RemoveChildRange(IEnumerable<ConnectionInfo> removalTargets)
        {
            foreach (var child in removalTargets)
            {
                RemoveChild(child);
            }
        }

        public void SetChildPosition(ConnectionInfo child, int newIndex)
        {
            var originalIndex = Children.IndexOf(child);
            if (originalIndex < 0 || originalIndex == newIndex || newIndex < 0) return;
            Children.Remove(child);
            if (newIndex > Children.Count) newIndex = Children.Count;
            Children.Insert(newIndex, child);
        }

        public void SetChildAbove(ConnectionInfo childToPromote, ConnectionInfo reference)
        {
            var originalIndex = Children.IndexOf(childToPromote);
            var newIndex = Children.IndexOf(reference);
            if (newIndex < 0) return;
            if (originalIndex < newIndex)
                SetChildPosition(childToPromote, newIndex - 1);
            else if (originalIndex > newIndex)
                SetChildPosition(childToPromote, newIndex);
        }

        public void SetChildBelow(ConnectionInfo childToPromote, ConnectionInfo reference)
        {
            var originalIndex = Children.IndexOf(childToPromote);
            var newIndex = Children.IndexOf(reference);
            if (newIndex < 0) return;
            if (originalIndex < newIndex)
                SetChildPosition(childToPromote, newIndex);
            else if (originalIndex  > newIndex)
                SetChildPosition(childToPromote, newIndex + 1);
        }

        public void PromoteChild(ConnectionInfo child)
        {
            var originalIndex = Children.IndexOf(child);
            SetChildPosition(child, originalIndex - 1);
        }

        public void DemoteChild(ConnectionInfo child)
        {
            var originalIndex = Children.IndexOf(child);
            SetChildPosition(child, originalIndex + 1);
        }

        public void Sort(ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            SortOn(connectionInfo => connectionInfo.Name, sortDirection);
        }

        public void SortOn<TProperty>(Func<ConnectionInfo, TProperty> propertyToCompare, ListSortDirection sortDirection = ListSortDirection.Ascending) 
            where TProperty : IComparable<TProperty>
        {
            var connectionComparer = new ConnectionInfoComparer<TProperty>(propertyToCompare)
            {
                SortDirection = sortDirection
            };
            Children.Sort(connectionComparer);
        }

        public void SortRecursive(ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            SortOnRecursive(connectionInfo => connectionInfo.Name, sortDirection);
        }

        public void SortOnRecursive<TProperty>(Func<ConnectionInfo, TProperty> propertyToCompare, ListSortDirection sortDirection = ListSortDirection.Ascending)
            where TProperty : IComparable<TProperty>
        {
            foreach (var child in Children.OfType<ContainerInfo>())
                child.SortOnRecursive(propertyToCompare, sortDirection);
            SortOn(propertyToCompare, sortDirection);
        }

        public override void Dispose()
        {
            var tempChildList = Children.ToArray();
            foreach (var child in tempChildList)
                child.Dispose();
            RemoveParent();
        }

        // Deep clone, recursive
        public override ConnectionInfo Clone()
		{
            var newContainer = new ContainerInfo();
            newContainer.CopyFrom(this);
            newContainer.ConstantID = MiscTools.CreateConstantID();
            newContainer.SetParent(Parent);
            newContainer.OpenConnections = new ProtocolList();
            newContainer.Inheritance = Inheritance.Clone();
            foreach (var child in Children.ToArray())
            {
                var newChild = child.Clone();
                newChild.RemoveParent();
                newContainer.AddChild(newChild);
            }
            return newContainer;
		}
			
		private new void SetDefaults()
		{
		    Name = "New Folder";
            IsExpanded = true;
		}

        public IEnumerable<ConnectionInfo> GetRecursiveChildList()
        {
            var childList = new List<ConnectionInfo>();
            foreach (var child in Children)
            {
                childList.Add(child);
                var childContainer = child as ContainerInfo;
                if (childContainer != null)
                    childList.AddRange(GetRecursiveChildList(childContainer));
            }
            return childList;
        }

        private IEnumerable<ConnectionInfo> GetRecursiveChildList(ContainerInfo container)
        {
            var childList = new List<ConnectionInfo>();
            foreach (var child in container.Children)
            {
                childList.Add(child);
                var childContainer = child as ContainerInfo;
                if (childContainer != null)
                    childList.AddRange(GetRecursiveChildList(childContainer));
            }
            return childList;
        }
    }
}