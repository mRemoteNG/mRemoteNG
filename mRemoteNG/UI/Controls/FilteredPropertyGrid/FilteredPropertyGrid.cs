using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace mRemoteNG.UI.Controls.FilteredPropertyGrid
{
    /// <summary>
    /// This class overrides the standard PropertyGrid provided by Microsoft.
    /// It also allows to hide (or filter) the properties of the SelectedObject displayed by the PropertyGrid.
    /// </summary>
    public partial class FilteredPropertyGrid : PropertyGrid
    {
        private ObjectWrapper[]? _mWrappers;

        /// <summary>
        /// Contain a reference to the array of properties to display in the PropertyGrid.
        /// </summary>
        private AttributeCollection? _hiddenAttributes;

        private AttributeCollection? _browsableAttributes;

        /// <summary>
        /// Contain references to the arrays of properties or categories to hide.
        /// </summary>
        private string[]? _mBrowsableProperties;

        private string[]? _mHiddenProperties;

        /// <summary>
        /// Public constructor.
        /// </summary>
        public FilteredPropertyGrid()
        {
            InitializeComponent();
        }

        /// <summary>
        /// A list of all currently properties being shown by the property grid.
        /// </summary>
        public IEnumerable<string> VisibleProperties =>
            _mWrappers?.FirstOrDefault()?.PropertyDescriptors.Select(p => p.Name) ?? Enumerable.Empty<string>();

        public new AttributeCollection? BrowsableAttributes
        {
            get => _browsableAttributes;
            set
            {
                if (_browsableAttributes == value) return;
                _hiddenAttributes = null;
                _browsableAttributes = value;
                RefreshProperties();
            }
        }

        /// <summary>
        /// Get or set the categories to hide.
        /// </summary>
        public AttributeCollection? HiddenAttributes
        {
            get => _hiddenAttributes;
            set
            {
                if (value == _hiddenAttributes) return;
                _hiddenAttributes = value;
                _browsableAttributes = null;
                RefreshProperties();
            }
        }

        /// <summary>
        /// Get or set the properties to show.
        /// </summary>
        /// <exception cref="ArgumentException">if one or several properties don't exist.</exception>
        public string[]? BrowsableProperties
        {
            get => _mBrowsableProperties;
            set
            {
                if (value == _mBrowsableProperties) return;
                _mBrowsableProperties = value;
                RefreshProperties();
            }
        }

        /// <summary>Get or set the properties to hide.</summary>
        public string[]? HiddenProperties
        {
            get => _mHiddenProperties;
            set
            {
                if (value == _mHiddenProperties) return;
                _mHiddenProperties = value;
                RefreshProperties();
            }
        }

        /// <summary>
        /// Overwrite the PropertyGrid.SelectedObject property.
        /// </summary>
        /// <remarks>The object passed to the base PropertyGrid is the wrapper.</remarks>
        public new object? SelectedObject
        {
            get => _mWrappers?.FirstOrDefault()?.SelectedObject;
            set
            {
                if (value == null)
                {
                    _mWrappers = null;
                    base.SelectedObject = null;
                }
                else
                {
                    _mWrappers = new[] { new ObjectWrapper(value) };
                    RefreshProperties();
                    base.SelectedObject = _mWrappers[0];
                }
            }
        }

        public new object[]? SelectedObjects
        {
            get => _mWrappers?.Select(w => w.SelectedObject).ToArray();
            set
            {
                if (value == null || value.Length == 0)
                {
                    _mWrappers = null;
                    base.SelectedObjects = null;
                }
                else
                {
                    _mWrappers = value.Select(o => new ObjectWrapper(o)).ToArray();
                    RefreshProperties();
                    base.SelectedObjects = _mWrappers;
                }
            }
        }

        public IList<GridItem> GetVisibleGridItems()
        {
            GridItem? gridRoot = SelectedGridItem;
            while (gridRoot != null && gridRoot.GridItemType != GridItemType.Root)
            {
                gridRoot = gridRoot.Parent;
            }

            if (gridRoot == null)
                return [];

            return GetVisibleGridItemsRecursive(gridRoot, []);
        }

        private static List<GridItem> GetVisibleGridItemsRecursive(GridItem item, List<GridItem> gridItems)
        {
            if (item.GridItemType == GridItemType.Property && !gridItems.Contains(item))
                gridItems.Add(item);

            if (item.Expandable && !item.Expanded)
                return gridItems;

            foreach (GridItem child in item.GridItems)
            {
                GetVisibleGridItemsRecursive(child, gridItems);
            }

            return gridItems;
        }

        public GridItem? FindPreviousGridItemProperty(GridItem? startItem)
        {
            IList<GridItem> gridItems = GetVisibleGridItems();

            if (gridItems.Count == 0 || startItem == null)
                return null;

            int startIndex = gridItems.IndexOf(startItem);
            if (startItem.GridItemType == GridItemType.Property)
            {
                startIndex--;
                if (startIndex < 0)
                {
                    startIndex = gridItems.Count - 1;
                }
            }

            int previousIndex = 0;
            bool previousIndexValid = false;
            for (int index = startIndex; index >= 0; index--)
            {
                if (gridItems[index].GridItemType != GridItemType.Property) continue;
                previousIndex = index;
                previousIndexValid = true;
                break;
            }

            if (previousIndexValid)
                return gridItems[previousIndex];

            for (int index = gridItems.Count - 1; index >= startIndex + 1; index--)
            {
                if (gridItems[index].GridItemType != GridItemType.Property) continue;
                previousIndex = index;
                previousIndexValid = true;
                break;
            }

            return !previousIndexValid ? null : gridItems[previousIndex];
        }

        public GridItem? FindNextGridItemProperty(GridItem? startItem)
        {
            IList<GridItem> gridItems = GetVisibleGridItems();

            if (gridItems.Count == 0 || startItem == null)
                return null;

            int startIndex = gridItems.IndexOf(startItem);
            if (startItem.GridItemType == GridItemType.Property)
            {
                startIndex++;
                if (startIndex >= gridItems.Count)
                {
                    startIndex = 0;
                }
            }

            int nextIndex = 0;
            bool nextIndexValid = false;
            for (int index = startIndex; index <= gridItems.Count - 1; index++)
            {
                if (gridItems[index].GridItemType != GridItemType.Property) continue;
                nextIndex = index;
                nextIndexValid = true;
                break;
            }

            if (nextIndexValid)
                return gridItems[nextIndex];

            for (int index = 0; index <= startIndex - 1; index++)
            {
                if (gridItems[index].GridItemType != GridItemType.Property) continue;
                nextIndex = index;
                nextIndexValid = true;
                break;
            }

            return !nextIndexValid ? null : gridItems[nextIndex];
        }

        /// <summary>
        /// Selects the next grid item in the property grid
        /// using the currently selected grid item as a reference.
        /// Does nothing if there is no next item.
        /// </summary>
        public void SelectNextGridItem()
        {
            GridItem? nextGridItem = FindNextGridItemProperty(SelectedGridItem);
            if (nextGridItem != null)
                SelectedGridItem = nextGridItem;
        }

        /// <summary>
        /// Selects the previous grid item in the property grid
        /// using the currently selected grid item as a reference.
        /// Does nothing if there is no previous item.
        /// </summary>
        public void SelectPreviousGridItem()
        {
            GridItem? previousGridItem = FindPreviousGridItemProperty(SelectedGridItem);
            if (previousGridItem != null)
                SelectedGridItem = previousGridItem;
        }

        /// <summary>
        /// Select the grid item whose backing property name
        /// matches the given <see cref="propertyName"/>.
        /// </summary>
        /// <param name="propertyName"></param>
        public void SelectGridItem(string propertyName)
        {
            GridItem? item = GetVisibleGridItems()
                .FirstOrDefault(gridItem => gridItem.PropertyDescriptor?.Name == propertyName);

            if (item != null)
                SelectedGridItem = item;
        }

        public void ClearFilters()
        {
            _mBrowsableProperties = null;
            _mHiddenProperties = null;
            RefreshProperties();
        }

        /// <summary>
        /// Build the list of the properties to be displayed in the PropertyGrid, following the filters defined the Browsable and Hidden properties.
        /// </summary>
        private void RefreshProperties()
        {
            if (_mWrappers == null) return;

            foreach (var wrapper in _mWrappers)
            {
                var propertyDescriptors = new List<PropertyDescriptor>();

                // 1. BrowsableAttributes
                if (_browsableAttributes != null && _browsableAttributes.Count > 0)
                {
                    foreach (Attribute attribute in _browsableAttributes)
                        AddAttributesToDescriptors(propertyDescriptors, wrapper.SelectedObject, attribute);
                }

                // 2. BrowsableProperties
                if (_mBrowsableProperties != null && _mBrowsableProperties.Length > 0)
                {
                    PropertyDescriptorCollection allproperties = TypeDescriptor.GetProperties(wrapper.SelectedObject);
                    foreach (string propertyname in _mBrowsableProperties)
                    {
                        PropertyDescriptor? property = allproperties[propertyname];
                        if (property == null)
                            continue; // Skip if property missing (mixed selection support)

                        if (!propertyDescriptors.Contains(property))
                            propertyDescriptors.Add(property);
                    }
                }

                // 3. Default (All properties) if no inclusive filters
                if ((_browsableAttributes == null || _browsableAttributes.Count == 0) &&
                    (_mBrowsableProperties == null || _mBrowsableProperties.Length == 0))
                {
                    IEnumerable<PropertyDescriptor> originalPropertyDescriptors = TypeDescriptor
                                                      .GetProperties(wrapper.SelectedObject)
                                                      .OfType<PropertyDescriptor>()
                                                      .Where(PropertyDoesntHaveBrowsableFalseAttribute);

                    foreach (PropertyDescriptor propertyDescriptor in originalPropertyDescriptors)
                        propertyDescriptors.Add(propertyDescriptor);
                }

                // 4. HiddenAttributes
                if (_hiddenAttributes != null)
                {
                    foreach (Attribute attribute in _hiddenAttributes)
                        RemoveAttributesFromDescriptors(propertyDescriptors, wrapper.SelectedObject, attribute);
                }

                // 5. HiddenProperties
                if (_mHiddenProperties != null && _mHiddenProperties.Length > 0)
                {
                    foreach (string propertyname in _mHiddenProperties)
                    {
                        PropertyDescriptor? property = propertyDescriptors.FirstOrDefault(p => p.Name == propertyname);
                        if (property != null)
                            propertyDescriptors.Remove(property);
                    }
                }

                wrapper.PropertyDescriptors = propertyDescriptors;
            }

            if (_mWrappers.Length == 1)
                base.SelectedObject = _mWrappers[0];
            else
                base.SelectedObjects = _mWrappers;
            
            Refresh();
        }

        /// <summary>
        /// Predicate to determine if a property has a Browsable(false) attribute
        /// attatched to it. If so, it should not be shown.
        /// </summary>
        /// <param name="propertyDescriptor"></param>
        /// <returns></returns>
        private static bool PropertyDoesntHaveBrowsableFalseAttribute(PropertyDescriptor propertyDescriptor)
        {
            return !propertyDescriptor.Attributes.Contains(new BrowsableAttribute(false));
        }

        private static void AddAttributesToDescriptors(List<PropertyDescriptor> descriptors, object obj, Attribute attribute)
        {
            PropertyDescriptorCollection filtered = TypeDescriptor.GetProperties(obj, new[] { attribute });
            foreach (PropertyDescriptor pd in filtered)
            {
                if (!descriptors.Contains(pd)) descriptors.Add(pd);
            }
        }

        private static void RemoveAttributesFromDescriptors(List<PropertyDescriptor> descriptors, object obj, Attribute attribute)
        {
            PropertyDescriptorCollection filtered = TypeDescriptor.GetProperties(obj, new[] { attribute });
            foreach (PropertyDescriptor pd in filtered)
            {
                descriptors.Remove(pd);
            }
        }
    }
}