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
        /// <summary>
        /// Contain a reference to the collection of properties to show in the parent PropertyGrid.
        /// </summary>
        /// <remarks>By default, m_PropertyDescriptors contain all the properties of the object. </remarks>
        readonly List<PropertyDescriptor> _propertyDescriptors = new List<PropertyDescriptor>();

        /// <summary>
        /// Contain a reference to the array of properties to display in the PropertyGrid.
        /// </summary>
        private AttributeCollection _hiddenAttributes;

        private AttributeCollection _browsableAttributes;

        /// <summary>
        /// Contain references to the arrays of properties or categories to hide.
        /// </summary>
        private string[] _mBrowsableProperties;

        private string[] _mHiddenProperties;

        /// <summary>
        /// Contain a reference to the wrapper that contains the object to be displayed into the PropertyGrid.
        /// </summary>
        private ObjectWrapper _mWrapper;

        /// <summary>
        /// Public constructor.
        /// </summary>
        public FilteredPropertyGrid()
        {
            InitializeComponent();
            base.SelectedObject = _mWrapper;
        }

        /// <summary>
        /// A list of all currently properties being shown by the property grid.
        /// </summary>
        public IEnumerable<string> VisibleProperties => _propertyDescriptors.Select(p => p.Name);

        public new AttributeCollection BrowsableAttributes
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
        public AttributeCollection HiddenAttributes
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
        public string[] BrowsableProperties
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
        public string[] HiddenProperties
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
        public new object SelectedObject
        {
            get =>
                _mWrapper != null
                    ? ((ObjectWrapper)base.SelectedObject).SelectedObject
                    : null;
            set
            {
                // Set the new object to the wrapper and create one if necessary.
                if (_mWrapper == null)
                {
                    _mWrapper = new ObjectWrapper(value);
                    RefreshProperties();
                }
                else if (_mWrapper.SelectedObject != value)
                {
                    var needrefresh = value?.GetType() != _mWrapper.SelectedObject?.GetType();
                    _mWrapper.SelectedObject = value ?? new object();
                    if (needrefresh)
                        RefreshProperties();
                }

                // Set the list of properties to the wrapper.
                _mWrapper.PropertyDescriptors = _propertyDescriptors;
                // Link the wrapper to the parent PropertyGrid.
                base.SelectedObject = _mWrapper;
            }
        }

        public List<GridItem> GetVisibleGridItems()
        {
            var gridRoot = SelectedGridItem;
            while (gridRoot.GridItemType != GridItemType.Root)
            {
                gridRoot = gridRoot.Parent;
            }
            return GetVisibleGridItemsRecursive(gridRoot, new List<GridItem>());
        }

        private List<GridItem> GetVisibleGridItemsRecursive(GridItem item, List<GridItem> gridItems)
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

        public GridItem FindPreviousGridItemProperty(GridItem startItem)
        {
            var gridItems = GetVisibleGridItems();

            if (gridItems.Count == 0 || startItem == null)
                return null;

            var startIndex = gridItems.IndexOf(startItem);
            if (startItem.GridItemType == GridItemType.Property)
            {
                startIndex--;
                if (startIndex < 0)
                {
                    startIndex = gridItems.Count - 1;
                }
            }

            var previousIndex = 0;
            var previousIndexValid = false;
            for (var index = startIndex; index >= 0; index--)
            {
                if (gridItems[index].GridItemType != GridItemType.Property) continue;
                previousIndex = index;
                previousIndexValid = true;
                break;
            }

            if (previousIndexValid)
                return gridItems[previousIndex];

            for (var index = gridItems.Count - 1; index >= startIndex + 1; index--)
            {
                if (gridItems[index].GridItemType != GridItemType.Property) continue;
                previousIndex = index;
                previousIndexValid = true;
                break;
            }

            return !previousIndexValid ? null : gridItems[previousIndex];
        }

        public GridItem FindNextGridItemProperty(GridItem startItem)
        {
            var gridItems = GetVisibleGridItems();

            if (gridItems.Count == 0 || startItem == null)
                return null;

            var startIndex = gridItems.IndexOf(startItem);
            if (startItem.GridItemType == GridItemType.Property)
            {
                startIndex++;
                if (startIndex >= gridItems.Count)
                {
                    startIndex = 0;
                }
            }

            var nextIndex = 0;
            var nextIndexValid = false;
            for (var index = startIndex; index <= gridItems.Count - 1; index++)
            {
                if (gridItems[index].GridItemType != GridItemType.Property) continue;
                nextIndex = index;
                nextIndexValid = true;
                break;
            }

            if (nextIndexValid)
                return gridItems[nextIndex];

            for (var index = 0; index <= startIndex - 1; index++)
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
            var nextGridItem = FindNextGridItemProperty(SelectedGridItem);
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
            var previousGridItem = FindPreviousGridItemProperty(SelectedGridItem);
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
            var item = GetVisibleGridItems()
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
            if (_mWrapper == null)
                return;

            // Clear the list of properties to be displayed.
            _propertyDescriptors.Clear();
            // Check whether the list is filtered 
            if (_browsableAttributes != null && _browsableAttributes.Count > 0)
            {
                // Add to the list the attributes that need to be displayed.
                foreach (Attribute attribute in _browsableAttributes)
                    ShowAttribute(attribute);
            }

            // Display if necessary, some properties
            if (_mBrowsableProperties != null && _mBrowsableProperties.Length > 0)
            {
                var allproperties = TypeDescriptor.GetProperties(_mWrapper.SelectedObject);
                foreach (var propertyname in _mBrowsableProperties)
                {
                    var property = allproperties[propertyname];

                    if (property == null)
                        throw new InvalidOperationException($"Property '{propertyname}' not found on object '{_mWrapper.GetClassName()}'");

                    ShowProperty(property);
                }
            }

            if ((_browsableAttributes == null || _browsableAttributes.Count == 0) &&
                (_mBrowsableProperties == null || _mBrowsableProperties.Length == 0))
            {
                // Fill the collection with all the properties.
                var originalPropertyDescriptors = TypeDescriptor
                                                  .GetProperties(_mWrapper.SelectedObject)
                                                  .OfType<PropertyDescriptor>()
                                                  .Where(PropertyDoesntHaveBrowsableFalseAttribute);

                foreach (PropertyDescriptor propertyDescriptor in originalPropertyDescriptors)
                    _propertyDescriptors.Add(propertyDescriptor);
            }

            // Remove from the list the attributes that mustn't be displayed.
            if (_hiddenAttributes != null)
                foreach (Attribute attribute in _hiddenAttributes)
                    HideAttribute(attribute);

            // Hide if necessary, some properties
            if (_mHiddenProperties != null && _mHiddenProperties.Length > 0)
            {
                // Remove from the list the properties that mustn't be displayed.
                foreach (var propertyname in _mHiddenProperties)
                {
                    var property = _propertyDescriptors.FirstOrDefault(p => p.Name == propertyname);

                    // Remove from the list the property
                    HideProperty(property);
                }
            }
        }

        /// <summary>
        /// Predicate to determine if a property has a Browsable(false) attribute
        /// attatched to it. If so, it should not be shown.
        /// </summary>
        /// <param name="propertyDescriptor"></param>
        /// <returns></returns>
        private bool PropertyDoesntHaveBrowsableFalseAttribute(PropertyDescriptor propertyDescriptor)
        {
            return !propertyDescriptor.Attributes.Contains(new BrowsableAttribute(false));
        }

        /// <summary>
        /// Allows to hide a set of properties to the parent PropertyGrid.
        /// </summary>
        /// <param name="attribute">A set of attributes that filter the original collection of properties.</param>
        /// <remarks>For better performance, include the BrowsableAttribute with true value.</remarks>
        private void HideAttribute(Attribute attribute)
        {
            var filteredoriginalpropertydescriptors =
                TypeDescriptor.GetProperties(_mWrapper.SelectedObject, new[] {attribute});
            if (filteredoriginalpropertydescriptors == null || filteredoriginalpropertydescriptors.Count == 0)
                throw new ArgumentException("Attribute not found", attribute.ToString());

            foreach (PropertyDescriptor propertydescriptor in filteredoriginalpropertydescriptors)
                HideProperty(propertydescriptor);
        }

        /// <summary>
        /// Add all the properties that match an attribute to the list of properties to be displayed in the PropertyGrid.
        /// </summary>
        /// <param name="attribute">The attribute to be added.</param>
        private void ShowAttribute(Attribute attribute)
        {
            var filteredoriginalpropertydescriptors =
                TypeDescriptor.GetProperties(_mWrapper.SelectedObject, new[] {attribute});
            if (filteredoriginalpropertydescriptors == null || filteredoriginalpropertydescriptors.Count == 0)
                throw new ArgumentException("Attribute not found", attribute.ToString());

            foreach (PropertyDescriptor propertydescriptor in filteredoriginalpropertydescriptors)
                ShowProperty(propertydescriptor);
        }

        /// <summary>
        /// Add a property to the list of properties to be displayed in the PropertyGrid.
        /// </summary>
        /// <param name="property">The property to be added.</param>
        private void ShowProperty(PropertyDescriptor property)
        {
            if (!_propertyDescriptors.Contains(property))
                _propertyDescriptors.Add(property);
        }

        /// <summary>
        /// Allows to hide a property to the parent PropertyGrid.
        /// </summary>
        /// <param name="property">The name of the property to be hidden.</param>
        private void HideProperty(PropertyDescriptor property)
        {
            if (_propertyDescriptors.Contains(property))
                _propertyDescriptors.Remove(property);
        }
    }
}