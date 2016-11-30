using System;
using System.Collections.Generic;
using System.ComponentModel;
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
		/// <summary>Contain a reference to the collection of properties to show in the parent PropertyGrid.</summary>
		/// <remarks>By default, m_PropertyDescriptors contain all the properties of the object. </remarks>
		readonly List<PropertyDescriptor> m_PropertyDescriptors = new List<PropertyDescriptor>();
		/// <summary>Contain a reference to the array of properties to display in the PropertyGrid.</summary>
		private AttributeCollection m_HiddenAttributes, m_BrowsableAttributes;
		/// <summary>Contain references to the arrays of properties or categories to hide.</summary>
		private string[] m_BrowsableProperties, m_HiddenProperties;
		/// <summary>Contain a reference to the wrapper that contains the object to be displayed into the PropertyGrid.</summary>
		private ObjectWrapper m_Wrapper;

		/// <summary>Public constructor.</summary>
		public FilteredPropertyGrid() {
			InitializeComponent();
			base.SelectedObject = m_Wrapper;
		}

		public new AttributeCollection BrowsableAttributes {
			get { return m_BrowsableAttributes; }
			set {
			    if (m_BrowsableAttributes == value) return;
			    m_HiddenAttributes = null;
			    m_BrowsableAttributes = value;
			    RefreshProperties();
			}
		}

		/// <summary>Get or set the categories to hide.</summary>
		public AttributeCollection HiddenAttributes {
			get { return m_HiddenAttributes; }
			set {
			    if (value == m_HiddenAttributes) return;
			    m_HiddenAttributes = value;
			    m_BrowsableAttributes = null;
			    RefreshProperties();
			}
		}
		/// <summary>Get or set the properties to show.</summary>
		/// <exception cref="ArgumentException">if one or several properties don't exist.</exception>
		public string[] BrowsableProperties {
			get { return m_BrowsableProperties; }
			set {
			    if (value == m_BrowsableProperties) return;
			    m_BrowsableProperties = value;
			    //m_HiddenProperties = null;
			    RefreshProperties();
			}
		}

		/// <summary>Get or set the properties to hide.</summary>
		public string[] HiddenProperties {
			get { return m_HiddenProperties; }
			set {
			    if (value == m_HiddenProperties) return;
			    //m_BrowsableProperties = null;
			    m_HiddenProperties = value;
			    RefreshProperties();
			}
		}

		/// <summary>Overwrite the PropertyGrid.SelectedObject property.</summary>
		/// <remarks>The object passed to the base PropertyGrid is the wrapper.</remarks>
		public new object SelectedObject {
			get { return m_Wrapper != null ? ((ObjectWrapper)base.SelectedObject).SelectedObject : null; }
			set {
				// Set the new object to the wrapper and create one if necessary.
				if(m_Wrapper == null) {
					m_Wrapper = new ObjectWrapper(value);
					RefreshProperties();
				}
				else if(m_Wrapper.SelectedObject != value) {
					var needrefresh = value.GetType() != m_Wrapper.SelectedObject.GetType();
					m_Wrapper.SelectedObject = value;
					if(needrefresh) RefreshProperties();
				}
				// Set the list of properties to the wrapper.
				m_Wrapper.PropertyDescriptors = m_PropertyDescriptors;
				// Link the wrapper to the parent PropertyGrid.
				base.SelectedObject = m_Wrapper;
			}
		}

        /*
		/// <summary>Called when the browsable properties have changed.</summary>
		private void OnBrowsablePropertiesChanged() {
			if(m_Wrapper == null) return;
		}
        */

		/// <summary>Build the list of the properties to be displayed in the PropertyGrid, following the filters defined the Browsable and Hidden properties.</summary>
		private void RefreshProperties() {
			if(m_Wrapper == null) return;
			// Clear the list of properties to be displayed.
			m_PropertyDescriptors.Clear();
			// Check whether the list is filtered 
			if(m_BrowsableAttributes != null && m_BrowsableAttributes.Count > 0) {
				// Add to the list the attributes that need to be displayed.
				foreach(Attribute attribute in m_BrowsableAttributes) ShowAttribute(attribute);
			} else {
				// Fill the collection with all the properties.
				var originalpropertydescriptors = TypeDescriptor.GetProperties(m_Wrapper.SelectedObject);
				foreach(PropertyDescriptor propertydescriptor in originalpropertydescriptors) m_PropertyDescriptors.Add(propertydescriptor);
				// Remove from the list the attributes that mustn't be displayed.
				if(m_HiddenAttributes != null) foreach(Attribute attribute in m_HiddenAttributes) HideAttribute(attribute);
			}
			// Get all the properties of the SelectedObject
			var allproperties = TypeDescriptor.GetProperties(m_Wrapper.SelectedObject);
			// Hide if necessary, some properties
			if(m_HiddenProperties != null && m_HiddenProperties.Length > 0) {
				// Remove from the list the properties that mustn't be displayed.
				foreach(var propertyname in m_HiddenProperties) {
					try {
						var property = allproperties[propertyname];
						// Remove from the list the property
						HideProperty(property);
					} catch(Exception ex) {
                        Runtime.MessageCollector.AddExceptionMessage("FilteredPropertyGrid: Could not hide Property.", ex);
                    }
				}
			}
			// Display if necessary, some properties
			if(m_BrowsableProperties != null && m_BrowsableProperties.Length > 0) {
				foreach(var propertyname in m_BrowsableProperties) {
					try {
						ShowProperty(allproperties[propertyname]);
					} catch(Exception knfe) {
                        Runtime.MessageCollector.AddExceptionMessage("FilteredPropertyGrid: Property not found", knfe);
					}
				}
			}
		}
        /// <summary>Allows to hide a set of properties to the parent PropertyGrid.</summary>
        /// <param name="attribute">A set of attributes that filter the original collection of properties.</param>
        /// <remarks>For better performance, include the BrowsableAttribute with true value.</remarks>
        private void HideAttribute(Attribute attribute) {
			var filteredoriginalpropertydescriptors = TypeDescriptor.GetProperties(m_Wrapper.SelectedObject,new[] { attribute });
			if(filteredoriginalpropertydescriptors == null || filteredoriginalpropertydescriptors.Count == 0) throw new ArgumentException("Attribute not found",attribute.ToString());
			foreach(PropertyDescriptor propertydescriptor in filteredoriginalpropertydescriptors) HideProperty(propertydescriptor);
		}
        /// <summary>Add all the properties that match an attribute to the list of properties to be displayed in the PropertyGrid.</summary>
        /// <param name="attribute">The attribute to be added.</param>
        private void ShowAttribute(Attribute attribute) {
			var filteredoriginalpropertydescriptors = TypeDescriptor.GetProperties(m_Wrapper.SelectedObject,new[] { attribute });
			if(filteredoriginalpropertydescriptors == null || filteredoriginalpropertydescriptors.Count == 0) throw new ArgumentException("Attribute not found",attribute.ToString());
			foreach(PropertyDescriptor propertydescriptor in filteredoriginalpropertydescriptors) ShowProperty(propertydescriptor);
		}
		/// <summary>Add a property to the list of properties to be displayed in the PropertyGrid.</summary>
		/// <param name="property">The property to be added.</param>
		private void ShowProperty(PropertyDescriptor property) {
			if(!m_PropertyDescriptors.Contains(property)) m_PropertyDescriptors.Add(property);
		}
        /// <summary>Allows to hide a property to the parent PropertyGrid.</summary>
        /// <param name="property">The name of the property to be hidden.</param>
        private void HideProperty(PropertyDescriptor property) {
			if(m_PropertyDescriptors.Contains(property)) m_PropertyDescriptors.Remove(property);
		}
	}
}