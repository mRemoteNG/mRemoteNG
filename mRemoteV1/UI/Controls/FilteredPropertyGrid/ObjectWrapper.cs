using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace mRemoteNG.UI.Controls.FilteredPropertyGrid
{
    /// <summary>
    /// This class is a wrapper. It contains the object the PropertyGrid has to display.
    /// </summary>
    internal class ObjectWrapper : ICustomTypeDescriptor
	{
	    /// <summary>
	    /// Creates a new instance of an <see cref="ObjectWrapper"/> with the given object to be wrapped.
	    /// </summary>
		/// <param name="obj">A reference to the selected object that will linked to the parent PropertyGrid.</param>
		internal ObjectWrapper(object obj)
	    {
			SelectedObject = obj;
		}

		/// <summary>
		/// Get or set a reference to the selected objet that will linked to the parent PropertyGrid.
		/// </summary>
		public object SelectedObject { get; set; }

	    /// <summary>
		/// Get or set a reference to the collection of properties to show in the parent PropertyGrid
		/// </summary>
		public List<PropertyDescriptor> PropertyDescriptors { get; set; } = new List<PropertyDescriptor>();

	    #region ICustomTypeDescriptor Members
		public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			return GetProperties();
		}

		public PropertyDescriptorCollection GetProperties()
		{
			return new PropertyDescriptorCollection(PropertyDescriptors.ToArray(), true);
		}

		/// <summary>
		/// GetAttributes
		/// </summary>
		/// <returns>AttributeCollection</returns>
		public AttributeCollection GetAttributes()
		{
			return TypeDescriptor.GetAttributes(SelectedObject, true);
		}

		/// <summary>
		/// Get Class Name
		/// </summary>
		/// <returns>String</returns>
		public string GetClassName()
		{
			return TypeDescriptor.GetClassName(SelectedObject, true);
		}

		/// <summary>
		/// GetComponentName
		/// </summary>
		/// <returns>String</returns>
		public string GetComponentName()
		{
			return TypeDescriptor.GetComponentName(SelectedObject, true);
		}

		/// <summary>
		/// GetConverter
		/// </summary>
		/// <returns>TypeConverter</returns>
		public TypeConverter GetConverter()
		{
			return TypeDescriptor.GetConverter(SelectedObject, true);
		}

		/// <summary>
		/// GetDefaultEvent
		/// </summary>
		/// <returns>EventDescriptor</returns>
		public EventDescriptor GetDefaultEvent()
		{
			return TypeDescriptor.GetDefaultEvent(SelectedObject, true);
		}

		/// <summary>
		/// GetDefaultProperty
		/// </summary>
		/// <returns>PropertyDescriptor</returns>
		public PropertyDescriptor GetDefaultProperty()
		{
			return TypeDescriptor.GetDefaultProperty(SelectedObject, true);
		}

		/// <summary>
		/// GetEditor
		/// </summary>
		/// <param name="editorBaseType">editorBaseType</param>
		/// <returns>object</returns>
		public object GetEditor(Type editorBaseType)
		{
			return TypeDescriptor.GetEditor(this,editorBaseType, true);
		}

		public EventDescriptorCollection GetEvents(Attribute[] attributes)
		{
			return TypeDescriptor.GetEvents(SelectedObject, attributes, true);
		}

		public EventDescriptorCollection GetEvents()
		{
			return TypeDescriptor.GetEvents(SelectedObject, true);
		}

		public object GetPropertyOwner(PropertyDescriptor pd)
		{
			return SelectedObject;
		}
		#endregion
	}
}
