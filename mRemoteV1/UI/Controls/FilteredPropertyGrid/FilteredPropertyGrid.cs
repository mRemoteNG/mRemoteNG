using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.App;

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

        public new AttributeCollection BrowsableAttributes {
			get { return _browsableAttributes; }
			set {
			    if (_browsableAttributes == value) return;
			    _hiddenAttributes = null;
			    _browsableAttributes = value;
			    RefreshProperties();
			}
		}

		/// <summary>
		/// Get or set the categories to hide.
		/// </summary>
		public AttributeCollection HiddenAttributes {
			get { return _hiddenAttributes; }
			set {
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
		public string[] BrowsableProperties {
			get { return _mBrowsableProperties; }
			set {
			    if (value == _mBrowsableProperties) return;
			    _mBrowsableProperties = value;
			    RefreshProperties();
			}
		}

		/// <summary>Get or set the properties to hide.</summary>
		public string[] HiddenProperties {
			get { return _mHiddenProperties; }
			set {
			    if (value == _mHiddenProperties) return;
			    _mHiddenProperties = value;
			    RefreshProperties();
			}
		}

		/// <summary>
		/// Overwrite the PropertyGrid.SelectedObject property.
		/// </summary>
		/// <remarks>The object passed to the base PropertyGrid is the wrapper.</remarks>
		public new object SelectedObject {
		    get
		    {
		        return _mWrapper != null 
		            ? ((ObjectWrapper)base.SelectedObject).SelectedObject 
		            : null;
		    }
			set {
				// Set the new object to the wrapper and create one if necessary.
				if(_mWrapper == null)
				{
					_mWrapper = new ObjectWrapper(value);
					RefreshProperties();
				}
				else if(_mWrapper.SelectedObject != value)
				{
					var needrefresh = value.GetType() != _mWrapper.SelectedObject.GetType();
					_mWrapper.SelectedObject = value;
					if(needrefresh)
					    RefreshProperties();
				}
				// Set the list of properties to the wrapper.
				_mWrapper.PropertyDescriptors = _propertyDescriptors;
				// Link the wrapper to the parent PropertyGrid.
				base.SelectedObject = _mWrapper;
			}
		}

		/// <summary>
		/// Build the list of the properties to be displayed in the PropertyGrid, following the filters defined the Browsable and Hidden properties.
		/// </summary>
		private void RefreshProperties()
		{
			if(_mWrapper == null)
			    return;

			// Clear the list of properties to be displayed.
			_propertyDescriptors.Clear();
			// Check whether the list is filtered 
			if(_browsableAttributes != null && _browsableAttributes.Count > 0)
			{
				// Add to the list the attributes that need to be displayed.
				foreach(Attribute attribute in _browsableAttributes)
				    ShowAttribute(attribute);
			}
			else
			{
				// Fill the collection with all the properties.
				var originalPropertyDescriptors = TypeDescriptor
				    .GetProperties(_mWrapper.SelectedObject)
				    .OfType<PropertyDescriptor>()
			        .Where(PropertyDoesntHaveBrowsableFalseAttribute);

				foreach(PropertyDescriptor propertyDescriptor in originalPropertyDescriptors)
				    _propertyDescriptors.Add(propertyDescriptor);

				// Remove from the list the attributes that mustn't be displayed.
				if(_hiddenAttributes != null)
				    foreach (Attribute attribute in _hiddenAttributes)
				        HideAttribute(attribute);
			}

			// Get all the properties of the SelectedObject
			var allproperties = TypeDescriptor.GetProperties(_mWrapper.SelectedObject);
			
		    // Hide if necessary, some properties
			if(_mHiddenProperties != null && _mHiddenProperties.Length > 0)
			{
				// Remove from the list the properties that mustn't be displayed.
				foreach(var propertyname in _mHiddenProperties)
				{
					try
					{
						var property = allproperties[propertyname];
						// Remove from the list the property
						HideProperty(property);
					}
					catch (Exception ex)
					{
                        Runtime.MessageCollector.AddExceptionMessage("FilteredPropertyGrid: Could not hide Property.", ex);
                    }
				}
			}

			// Display if necessary, some properties
			if(_mBrowsableProperties != null && _mBrowsableProperties.Length > 0)
			{
				foreach(var propertyname in _mBrowsableProperties)
				{
					try
					{
						ShowProperty(allproperties[propertyname]);
					}
					catch (Exception ex)
					{
                        Runtime.MessageCollector.AddExceptionMessage("FilteredPropertyGrid: Property not found", ex);
					}
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
			var filteredoriginalpropertydescriptors = TypeDescriptor.GetProperties(_mWrapper.SelectedObject,new[] { attribute });
			if(filteredoriginalpropertydescriptors == null || filteredoriginalpropertydescriptors.Count == 0)
			    throw new ArgumentException("Attribute not found", attribute.ToString());

			foreach(PropertyDescriptor propertydescriptor in filteredoriginalpropertydescriptors)
			    HideProperty(propertydescriptor);
		}

        /// <summary>
        /// Add all the properties that match an attribute to the list of properties to be displayed in the PropertyGrid.
        /// </summary>
        /// <param name="attribute">The attribute to be added.</param>
        private void ShowAttribute(Attribute attribute)
        {
			var filteredoriginalpropertydescriptors = TypeDescriptor.GetProperties(_mWrapper.SelectedObject,new[] { attribute });
			if(filteredoriginalpropertydescriptors == null || filteredoriginalpropertydescriptors.Count == 0)
			    throw new ArgumentException("Attribute not found", attribute.ToString());

			foreach(PropertyDescriptor propertydescriptor in filteredoriginalpropertydescriptors)
			    ShowProperty(propertydescriptor);
		}

		/// <summary>
		/// Add a property to the list of properties to be displayed in the PropertyGrid.
		/// </summary>
		/// <param name="property">The property to be added.</param>
		private void ShowProperty(PropertyDescriptor property)
		{
			if(!_propertyDescriptors.Contains(property))
			    _propertyDescriptors.Add(property);
		}

        /// <summary>
        /// Allows to hide a property to the parent PropertyGrid.
        /// </summary>
        /// <param name="property">The name of the property to be hidden.</param>
        private void HideProperty(PropertyDescriptor property)
        {
			if(_propertyDescriptors.Contains(property))
			    _propertyDescriptors.Remove(property);
		}
	}
}