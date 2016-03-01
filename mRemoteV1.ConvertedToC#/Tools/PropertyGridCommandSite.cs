using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reflection;

namespace mRemoteNG.Tools
{
	public class PropertyGridCommandSite : IMenuCommandService, ISite
	{

		protected object TheObject;
		public PropertyGridCommandSite(object @object)
		{
			TheObject = @object;
		}

		public DesignerVerbCollection Verbs {
			get {
				DesignerVerbCollection objectVerbs = new DesignerVerbCollection();
				// ReSharper disable VBPossibleMistakenCallToGetType.2
				MethodInfo[] methods = TheObject.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
				// ReSharper restore VBPossibleMistakenCallToGetType.2
				foreach (MethodInfo method in methods) {
					object[] commandAttributes = method.GetCustomAttributes(typeof(CommandAttribute), true);
					if (commandAttributes == null || commandAttributes.Length == 0)
						continue;

					CommandAttribute commandAttribute = (CommandAttribute)commandAttributes[0];
					if (!commandAttribute.Command)
						continue;

					string displayName = method.Name;
					object[] displayNameAttributes = method.GetCustomAttributes(typeof(DisplayNameAttribute), true);
					if (!(displayNameAttributes == null || displayNameAttributes.Length == 0)) {
						DisplayNameAttribute displayNameAttribute = (DisplayNameAttribute)displayNameAttributes[0];
						if (!string.IsNullOrEmpty(displayNameAttribute.DisplayName)) {
							displayName = displayNameAttribute.DisplayName;
						}
					}
					objectVerbs.Add(new DesignerVerb(displayName, new EventHandler(VerbEventHandler)));
				}

				return objectVerbs;
			}
		}

		private void VerbEventHandler(object sender, EventArgs e)
		{
			DesignerVerb verb = sender as DesignerVerb;
			if (verb == null)
				return;
			// ReSharper disable VBPossibleMistakenCallToGetType.2
			MethodInfo[] methods = TheObject.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
			// ReSharper restore VBPossibleMistakenCallToGetType.2
			foreach (MethodInfo method in methods) {
				object[] commandAttributes = method.GetCustomAttributes(typeof(CommandAttribute), true);
				if (commandAttributes == null || commandAttributes.Length == 0)
					continue;

				CommandAttribute commandAttribute = (CommandAttribute)commandAttributes[0];
				if (!commandAttribute.Command)
					continue;

				string displayName = method.Name;
				object[] displayNameAttributes = method.GetCustomAttributes(typeof(DisplayNameAttribute), true);
				if (!(displayNameAttributes == null || displayNameAttributes.Length == 0)) {
					DisplayNameAttribute displayNameAttribute = (DisplayNameAttribute)displayNameAttributes[0];
					if (!string.IsNullOrEmpty(displayNameAttribute.DisplayName)) {
						displayName = displayNameAttribute.DisplayName;
					}
				}

				if (verb.Text == displayName) {
					method.Invoke(TheObject, null);
					return;
				}
			}
		}

		public object GetService(Type serviceType)
		{
			if (object.ReferenceEquals(serviceType, typeof(IMenuCommandService))) {
				return this;
			} else {
				return null;
			}
		}

		public System.ComponentModel.IComponent Component {
			get {
				throw new NotImplementedException();
			}
		}

		public System.ComponentModel.IContainer Container {
			get { return null; }
		}

		public bool DesignMode {
			get { return true; }
		}

		public string Name {
			get {
				throw new NotImplementedException();
			}
			set {
				throw new NotImplementedException();
			}
		}

		public void AddCommand(MenuCommand command)
		{
			throw new NotImplementedException();
		}

		public void AddVerb(DesignerVerb verb)
		{
			throw new NotImplementedException();
		}

		public MenuCommand FindCommand(CommandID commandId)
		{
			throw new NotImplementedException();
		}

		public bool GlobalInvoke(CommandID commandId)
		{
			throw new NotImplementedException();
		}

		public void RemoveCommand(MenuCommand command)
		{
			throw new NotImplementedException();
		}

		public void RemoveVerb(DesignerVerb verb)
		{
			throw new NotImplementedException();
		}

		public void ShowContextMenu(CommandID menuId, int x, int y)
		{
			throw new NotImplementedException();
		}

	}

	public class CommandAttribute : Attribute
	{
		public bool Command { get; set; }
		public CommandAttribute(bool isCommand = true)
		{
			Command = isCommand;
		}
	}
}
