using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Versioning;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Tree;

namespace mRemoteNG.Container
{
    [SupportedOSPlatform("windows")]
    [DefaultProperty("Name")]
    public class ContainerInfo : ConnectionInfo, INotifyCollectionChanged
    {
        private bool _isExpanded;

        [Browsable(false)] public List<ConnectionInfo> Children { get; } = new List<ConnectionInfo>();

        [Category(""), Browsable(false), ReadOnly(false), Bindable(false), DefaultValue(""), DesignOnly(false)]
        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetField(ref _isExpanded, value, "IsExpanded");
        }

        [Browsable(false)]
        public override bool IsContainer
        {
            get => true;
            set { }
        }

        public ContainerInfo(string uniqueId)
            : base(uniqueId)
        {
            SetDefaults();
        }

        public ContainerInfo()
            : this(Guid.NewGuid().ToString())
        {
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
            AddChildAt(newChildItem, Children.Count);
        }

        public void AddChildAbove(ConnectionInfo newChildItem, ConnectionInfo reference)
        {
            var newChildIndex = Children.IndexOf(reference);
            if (newChildIndex < 0)
                newChildIndex = Children.Count;
            AddChildAt(newChildItem, newChildIndex);
        }

        public void AddChildBelow(ConnectionInfo newChildItem, ConnectionInfo reference)
        {
            var newChildIndex = Children.IndexOf(reference) + 1;
            if (newChildIndex > Children.Count || newChildIndex < 1)
                newChildIndex = Children.Count;
            AddChildAt(newChildItem, newChildIndex);
        }

        public virtual void AddChildAt(ConnectionInfo newChildItem, int index)
        {
            if (Children.Contains(newChildItem)) return;
            newChildItem.Parent?.RemoveChild(newChildItem);
            newChildItem.Parent = this;
            Children.Insert(index, newChildItem);
            SubscribeToChildEvents(newChildItem);
            RaiseCollectionChangedEvent(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newChildItem));
        }

        public void AddChildRange(IEnumerable<ConnectionInfo> newChildren)
        {
            foreach (var child in newChildren)
            {
                AddChild(child);
            }
        }

        public virtual void RemoveChild(ConnectionInfo removalTarget)
        {
            if (!Children.Contains(removalTarget)) return;
            removalTarget.Parent = null;
            Children.Remove(removalTarget);
            UnsubscribeToChildEvents(removalTarget);
            RaiseCollectionChangedEvent(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removalTarget));
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
            RaiseCollectionChangedEvent(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, child, newIndex, originalIndex));
        }

        public void SetChildAbove(ConnectionInfo childToPromote, ConnectionInfo reference)
        {
            var newIndex = GetNewChildIndexAboveReference(childToPromote, reference);
            SetChildPosition(childToPromote, newIndex);
        }

        private int GetNewChildIndexAboveReference(ConnectionInfo childToPromote, ConnectionInfo reference)
        {
            var originalIndex = Children.IndexOf(childToPromote);
            var newIndex = Children.IndexOf(reference);
            if (originalIndex < newIndex)
                newIndex -= 1;
            return newIndex < 0 ? 0 : newIndex;
        }

        public void SetChildBelow(ConnectionInfo childToPromote, ConnectionInfo reference)
        {
            var newIndex = GetNewChildIndexBelowReference(childToPromote, reference);
            SetChildPosition(childToPromote, newIndex);
        }

        private int GetNewChildIndexBelowReference(ConnectionInfo childToPromote, ConnectionInfo reference)
        {
            var originalIndex = Children.IndexOf(childToPromote);
            var newIndex = Children.IndexOf(reference);
            if (originalIndex > newIndex)
                newIndex += 1;
            return newIndex < 0 ? 0 : newIndex;
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
            RaiseCollectionChangedEvent(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
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

        // Deep clone, recursive
        public override ConnectionInfo Clone()
        {
            var newContainer = new ContainerInfo();
            newContainer.CopyFrom(this);
            newContainer.OpenConnections = new ProtocolList();
            newContainer.Inheritance = Inheritance.Clone(newContainer);
            foreach (var child in Children.ToArray())
            {
                var newChild = child.Clone();
                newChild.RemoveParent();
                newContainer.AddChild(newChild);
            }

            return newContainer;
        }

        private void SetDefaults()
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

        public IEnumerable<ConnectionInfo> GetRecursiveFavoriteChildList()
        {
            var childList = new List<ConnectionInfo>();
            foreach (var child in Children)
            {
                if (child.Favorite && child.GetTreeNodeType() == TreeNodeType.Connection)
                    childList.Add(child);
                var childContainer = child as ContainerInfo;
                if (childContainer != null)
                    childList.AddRange(GetRecursiveFavoritChildList(childContainer));
            }
            return childList;
        }

        /// <summary>
        /// Pushes the connection properties of this container to all
        /// children recursively.
        /// </summary>
        public void ApplyConnectionPropertiesToChildren()
        {
            var children = GetRecursiveChildList();

            foreach (var child in children)
            {
                child.CopyFrom(this);
            }
        }

        /// <summary>
        /// Pushes the inheritance settings of this container to all
        /// children recursively.
        /// </summary>
        public void ApplyInheritancePropertiesToChildren()
        {
            var children = GetRecursiveChildList();

            foreach (var child in children)
            {
                child.Inheritance = Inheritance.Clone(child);
            }
        }

        private IEnumerable<ConnectionInfo> GetRecursiveFavoritChildList(ContainerInfo container)
        {
            var childList = new List<ConnectionInfo>();
            foreach (var child in container.Children)
            {
                if (child.Favorite && child.GetTreeNodeType() == TreeNodeType.Connection)
                    childList.Add(child);
                var childContainer = child as ContainerInfo;
                if (childContainer != null)
                    childList.AddRange(GetRecursiveFavoritChildList(childContainer));
            }
            return childList;
        }

        protected virtual void SubscribeToChildEvents(ConnectionInfo child)
        {
            child.PropertyChanged += RaisePropertyChangedEvent;
            var childAsContainer = child as ContainerInfo;
            if (childAsContainer == null) return;
            childAsContainer.CollectionChanged += RaiseCollectionChangedEvent;
        }

        protected virtual void UnsubscribeToChildEvents(ConnectionInfo child)
        {
            child.PropertyChanged -= RaisePropertyChangedEvent;
            var childAsContainer = child as ContainerInfo;
            if (childAsContainer == null) return;
            childAsContainer.CollectionChanged -= RaiseCollectionChangedEvent;
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void RaiseCollectionChangedEvent(object sender, NotifyCollectionChangedEventArgs args)
        {
            CollectionChanged?.Invoke(sender, args);
        }
    }
}