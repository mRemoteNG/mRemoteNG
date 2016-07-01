using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace mRemoteNG.UI.Controls.FilteredPropertyGrid
{
	/// <summary>This class is a wrapper. It contains the object the propertyGrid has to display.</summary>
	internal class ObjectWrapper : ICustomTypeDescriptor
	{
		/// <summary>Contain a reference to the selected objet that will linked to the parent PropertyGrid.</summary>
		private object m_SelectedObject;
		/// <summary>Contain a reference to the collection of properties to show in the parent PropertyGrid.</summary>
		/// <remarks>By default, m_PropertyDescriptors contain all the properties of the object. </remarks>
		List<PropertyDescriptor> m_PropertyDescriptors = new List<PropertyDescriptor>();

		/// <summary>Simple constructor.</summary>
		/// <param name="obj">A reference to the selected object that will linked to the parent PropertyGrid.</param>
		internal ObjectWrapper(object obj) {
			m_SelectedObject = obj;
		}

		/// <summary>Get or set a reference to the selected objet that will linked to the parent PropertyGrid.</summary>
		public object SelectedObject {
			get { return m_SelectedObject; }
			set { m_SelectedObject = value; } 
		}

		/// <summary>Get or set a reference to the collection of properties to show in the parent PropertyGrid.</summary>
		public List<PropertyDescriptor> PropertyDescriptors {
			get { return m_PropertyDescriptors; }
			set { m_PropertyDescriptors = value; }
		}

	#region ICustomTypeDescriptor Members
		public PropertyDescriptorCollection GetProperties(Attribute[] attributes) {
			return GetProperties();
		}

		public PropertyDescriptorCollection GetProperties() {
			return new PropertyDescriptorCollection(m_PropertyDescriptors.ToArray(),true);
		}

		/// <summary>GetAttributes.</summary>
		/// <returns>AttributeCollection</returns>
		public AttributeCollection GetAttributes() {
			return TypeDescriptor.GetAttributes(m_SelectedObject,true);
		}
		/// <summary>Get Class Name.</summary>
		/// <returns>String</returns>
		public String GetClassName() {
			return TypeDescriptor.GetClassName(m_SelectedObject,true);
		}
		/// <summary>GetComponentName.</summary>
		/// <returns>String</returns>
		public String GetComponentName() {
			return TypeDescriptor.GetComponentName(m_SelectedObject,true);
		}

		/// <summary>GetConverter.</summary>
		/// <returns>TypeConverter</returns>
		public TypeConverter GetConverter() {
			return TypeDescriptor.GetConverter(m_SelectedObject,true);
		}

		/// <summary>GetDefaultEvent.</summary>
		/// <returns>EventDescriptor</returns>
		public EventDescriptor GetDefaultEvent() {
			return TypeDescriptor.GetDefaultEvent(m_SelectedObject,true);
		}

		/// <summary>GetDefaultProperty.</summary>
		/// <returns>PropertyDescriptor</returns>
		public PropertyDescriptor GetDefaultProperty() {
			return TypeDescriptor.GetDefaultProperty(m_SelectedObject,true);
		}

		/// <summary>GetEditor.</summary>
		/// <param name="editorBaseType">editorBaseType</param>
		/// <returns>object</returns>
		public object GetEditor(Type editorBaseType) {
			return TypeDescriptor.GetEditor(this,editorBaseType,true);
		}

		public EventDescriptorCollection GetEvents(Attribute[] attributes) {
			return TypeDescriptor.GetEvents(m_SelectedObject,attributes,true);
		}

		public EventDescriptorCollection GetEvents() {
			return TypeDescriptor.GetEvents(m_SelectedObject,true);
		}

		public object GetPropertyOwner(PropertyDescriptor pd) {
			return m_SelectedObject;
		}

		#endregion

	}
}
