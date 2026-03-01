using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Runtime.Versioning;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Security;
using mRemoteNG.Resources.Language;
using mRemoteNG.Tools;
using mRemoteNG.PluginSystem;
using mRemoteNG.Tree;

namespace mRemoteNG.Container
{
    /// <summary>
    /// Represents a folder in the connection tree that can contain child
    /// <see cref="ConnectionInfo"/> nodes and other <see cref="ContainerInfo"/> folders.
    /// Extends <see cref="ConnectionInfo"/> so that folder-level defaults can be
    /// inherited by child connections via the inheritance system.
    /// Implements <see cref="INotifyCollectionChanged"/> to notify the tree view
    /// when children are added, removed, or reordered.
    /// </summary>
    [SupportedOSPlatform("windows")]
    [DefaultProperty("Name")]
    public class ContainerInfo : ConnectionInfo, INotifyCollectionChanged, IConnectionNode, ITreeNodeContainer
    {
        private bool _isExpanded;
        private bool _autoSort;
        private bool _excludeFromSearch;
        private SecureString? _containerPassword;

        #region IConnectionNode Implementation
        IEnumerable<IConnectionNode> IConnectionNode.Children => Children;
        #endregion

        [Browsable(false)] public List<ConnectionInfo> Children { get; } = [];

        [LocalizedAttributes.LocalizedCategory(nameof(Language.General)),
         DisplayName("Folder Password"),
         Description("Password to protect this folder."),
         PasswordPropertyText(true),
         Browsable(true)]
        public string ContainerPassword
        {
            get => _containerPassword?.ConvertToUnsecureString() ?? string.Empty;
            set
            {
                string password = value ?? string.Empty;
                if (string.Equals(_containerPassword?.ConvertToUnsecureString() ?? string.Empty, password, StringComparison.Ordinal))
                    return;

                _containerPassword?.Dispose();
                _containerPassword = password.ConvertToSecureString();
            }
        }

        [Browsable(false)]
        public bool IsUnlocked { get; set; }

        [Category(""), Browsable(false), ReadOnly(false), Bindable(false), DefaultValue(""), DesignOnly(false)]
        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetField(ref _isExpanded, value, nameof(IsExpanded));
        }

        /// <summary>
        /// When true, this folder and all its children are excluded from connection tree search results.
        /// </summary>
        [Browsable(false)]
        public bool ExcludeFromSearch
        {
            get => _excludeFromSearch;
            set => SetField(ref _excludeFromSearch, value, nameof(ExcludeFromSearch));
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.General)),
         DisplayName("Automatic Sort"),
         Description("Automatically sort child nodes by name when items are added, moved, or renamed."),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool AutoSort
        {
            get => GetPropertyValue(nameof(AutoSort), _autoSort);
            set
            {
                bool wasAutoSortEnabled = AutoSort;
                SetField(ref _autoSort, value, nameof(AutoSort));
                if (!wasAutoSortEnabled && AutoSort)
                    Sort();
            }
        }

        [Browsable(false)]
        public override bool IsContainer
        {
            get => true;
            set { }
        }

        [Browsable(false)]
        public bool IsEntity { get; set; }

        [Category(""), Browsable(false), ReadOnly(false), Bindable(false), DefaultValue(DynamicSourceType.None), DesignOnly(false)]
        public DynamicSourceType DynamicSource
        {
            get => _dynamicSource;
            set => SetField(ref _dynamicSource, value, nameof(DynamicSource));
        }
        private DynamicSourceType _dynamicSource;

        [Category(""), Browsable(false), ReadOnly(false), Bindable(false), DefaultValue(""), DesignOnly(false)]
        public string DynamicSourceValue
        {
            get => _dynamicSourceValue;
            set => SetField(ref _dynamicSourceValue, value, nameof(DynamicSourceValue));
        }
        private string _dynamicSourceValue = string.Empty;

        [Category(""), Browsable(false), ReadOnly(false), Bindable(false), DefaultValue(0), DesignOnly(false)]
        public int DynamicRefreshInterval
        {
            get => _dynamicRefreshInterval;
            set => SetField(ref _dynamicRefreshInterval, value, nameof(DynamicRefreshInterval));
        }
        private int _dynamicRefreshInterval;

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
            return IsEntity ? TreeNodeType.Entity : TreeNodeType.Container;
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
            int newChildIndex = Children.IndexOf(reference);
            if (newChildIndex < 0)
                newChildIndex = Children.Count;
            AddChildAt(newChildItem, newChildIndex);
        }

        public void AddChildBelow(ConnectionInfo newChildItem, ConnectionInfo reference)
        {
            int newChildIndex = Children.IndexOf(reference) + 1;
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

            if (AutoSort)
            {
                Sort();
                return;
            }

            RaiseCollectionChangedEvent(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newChildItem));
        }

        public void AddChildRange(IEnumerable<ConnectionInfo> newChildren)
        {
            foreach (ConnectionInfo child in newChildren)
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
            foreach (ConnectionInfo child in removalTargets)
            {
                RemoveChild(child);
            }
        }

        public void SetChildPosition(ConnectionInfo child, int newIndex)
        {
            int originalIndex = Children.IndexOf(child);
            if (originalIndex < 0 || originalIndex == newIndex || newIndex < 0) return;
            Children.Remove(child);
            if (newIndex > Children.Count) newIndex = Children.Count;
            Children.Insert(newIndex, child);

            if (AutoSort)
            {
                Sort();
                return;
            }

            RaiseCollectionChangedEvent(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, child, newIndex, originalIndex));
        }

        public void SetChildAbove(ConnectionInfo childToPromote, ConnectionInfo reference)
        {
            int newIndex = GetNewChildIndexAboveReference(childToPromote, reference);
            SetChildPosition(childToPromote, newIndex);
        }

        private int GetNewChildIndexAboveReference(ConnectionInfo childToPromote, ConnectionInfo reference)
        {
            int originalIndex = Children.IndexOf(childToPromote);
            int newIndex = Children.IndexOf(reference);
            if (originalIndex < newIndex)
                newIndex -= 1;
            return newIndex < 0 ? 0 : newIndex;
        }

        public void SetChildBelow(ConnectionInfo childToPromote, ConnectionInfo reference)
        {
            int newIndex = GetNewChildIndexBelowReference(childToPromote, reference);
            SetChildPosition(childToPromote, newIndex);
        }

        private int GetNewChildIndexBelowReference(ConnectionInfo childToPromote, ConnectionInfo reference)
        {
            int originalIndex = Children.IndexOf(childToPromote);
            int newIndex = Children.IndexOf(reference);
            if (originalIndex > newIndex)
                newIndex += 1;
            return newIndex < 0 ? 0 : newIndex;
        }

        public void PromoteChild(ConnectionInfo child)
        {
            int originalIndex = Children.IndexOf(child);
            SetChildPosition(child, originalIndex - 1);
        }

        public void DemoteChild(ConnectionInfo child)
        {
            int originalIndex = Children.IndexOf(child);
            SetChildPosition(child, originalIndex + 1);
        }

        public void Sort(ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            SortOn(connectionInfo => connectionInfo.Name, sortDirection);
        }

        public void SortOn<TProperty>(Func<ConnectionInfo, TProperty> propertyToCompare, ListSortDirection sortDirection = ListSortDirection.Ascending)
            where TProperty : IComparable<TProperty>
        {
            ConnectionInfoComparer<TProperty> connectionComparer = new(propertyToCompare)
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
            foreach (ContainerInfo child in Children.OfType<ContainerInfo>())
                child.SortOnRecursive(propertyToCompare, sortDirection);
            SortOn(propertyToCompare, sortDirection);
        }

        // Deep clone, recursive
        public override ConnectionInfo Clone()
        {
            ContainerInfo newContainer = new();
            newContainer.CopyFrom(this);
            newContainer._autoSort = _autoSort;
            newContainer._excludeFromSearch = _excludeFromSearch;
            newContainer.OpenConnections = [];
            newContainer.Inheritance = Inheritance.Clone(newContainer);
            foreach (ConnectionInfo child in Children.ToArray())
            {
                ConnectionInfo newChild = child.Clone();
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
            foreach (ConnectionInfo child in Children)
            {
                yield return child;
                if (child is ContainerInfo childContainer)
                {
                    foreach (ConnectionInfo descendant in childContainer.GetRecursiveChildList())
                        yield return descendant;
                }
            }
        }

        public IEnumerable<ConnectionInfo> GetRecursiveFavoriteChildList()
        {
            foreach (ConnectionInfo child in Children)
            {
                if (child.Favorite && child.GetTreeNodeType() == TreeNodeType.Connection)
                    yield return child;
                if (child is ContainerInfo childContainer)
                {
                    foreach (ConnectionInfo descendant in childContainer.GetRecursiveFavoriteChildList())
                        yield return descendant;
                }
            }
        }

        /// <summary>
        /// Pushes the connection properties of this container to all
        /// children recursively.
        /// </summary>
        public void ApplyConnectionPropertiesToChildren()
        {
            // Materialize the list to avoid "Collection was modified" when
            // CopyFrom triggers PropertyChanged -> auto-sort on parent containers.
            List<ConnectionInfo> children = GetRecursiveChildList().ToList();

            foreach (ConnectionInfo child in children)
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
            // Materialize the list to avoid "Collection was modified" during iteration.
            List<ConnectionInfo> children = GetRecursiveChildList().ToList();

            foreach (ConnectionInfo child in children)
            {
                child.Inheritance = Inheritance.Clone(child);
            }
        }

        protected virtual void SubscribeToChildEvents(ConnectionInfo child)
        {
            child.PropertyChanged += OnChildPropertyChanged;
            ContainerInfo? childAsContainer = child as ContainerInfo;
            if (childAsContainer == null) return;
            childAsContainer.CollectionChanged += RaiseCollectionChangedEvent;
        }

        protected virtual void UnsubscribeToChildEvents(ConnectionInfo child)
        {
            child.PropertyChanged -= OnChildPropertyChanged;
            ContainerInfo? childAsContainer = child as ContainerInfo;
            if (childAsContainer == null) return;
            childAsContainer.CollectionChanged -= RaiseCollectionChangedEvent;
        }

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        private void OnChildPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            RaisePropertyChangedEvent(sender, args);

            if (args.PropertyName != nameof(ConnectionInfo.Name))
                return;
            if (sender is not ConnectionInfo child || !Children.Contains(child))
                return;
            if (!AutoSort)
                return;

            Sort();
        }

        private void RaiseCollectionChangedEvent(object sender, NotifyCollectionChangedEventArgs args)
        {
            CollectionChanged?.Invoke(this, args);
        }
    }
}
