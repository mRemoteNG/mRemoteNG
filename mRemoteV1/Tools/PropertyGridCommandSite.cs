using System;
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

        public DesignerVerbCollection Verbs
        {
            get
            {
                var objectVerbs = new DesignerVerbCollection();
                // ReSharper disable VBPossibleMistakenCallToGetType.2
                var methods = TheObject.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
                // ReSharper restore VBPossibleMistakenCallToGetType.2
                foreach (var method in methods)
                {
                    var commandAttributes = method.GetCustomAttributes(typeof(CommandAttribute), true);
                    if ((commandAttributes == null) || (commandAttributes.Length == 0))
                        continue;

                    var commandAttribute = (CommandAttribute) commandAttributes[0];
                    if (!commandAttribute.Command)
                        continue;

                    var displayName = method.Name;
                    var displayNameAttributes = method.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                    if (!((displayNameAttributes == null) || (displayNameAttributes.Length == 0)))
                    {
                        var displayNameAttribute = (DisplayNameAttribute) displayNameAttributes[0];
                        if (!string.IsNullOrEmpty(displayNameAttribute.DisplayName))
                            displayName = displayNameAttribute.DisplayName;
                    }
                    objectVerbs.Add(new DesignerVerb(displayName, VerbEventHandler));
                }

                return objectVerbs;
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

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IMenuCommandService))
                return this;
            return null;
        }

        public IComponent Component
        {
            get { throw new NotImplementedException(); }
        }

        public IContainer Container
        {
            get { return null; }
        }

        public bool DesignMode
        {
            get { return true; }
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        private void VerbEventHandler(object sender, EventArgs e)
        {
            var verb = sender as DesignerVerb;
            if (verb == null)
                return;
            // ReSharper disable VBPossibleMistakenCallToGetType.2
            var methods = TheObject.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
            // ReSharper restore VBPossibleMistakenCallToGetType.2
            foreach (var method in methods)
            {
                var commandAttributes = method.GetCustomAttributes(typeof(CommandAttribute), true);
                if ((commandAttributes == null) || (commandAttributes.Length == 0))
                    continue;

                var commandAttribute = (CommandAttribute) commandAttributes[0];
                if (!commandAttribute.Command)
                    continue;

                var displayName = method.Name;
                var displayNameAttributes = method.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                if (!((displayNameAttributes == null) || (displayNameAttributes.Length == 0)))
                {
                    var displayNameAttribute = (DisplayNameAttribute) displayNameAttributes[0];
                    if (!string.IsNullOrEmpty(displayNameAttribute.DisplayName))
                        displayName = displayNameAttribute.DisplayName;
                }

                if (verb.Text == displayName)
                {
                    method.Invoke(TheObject, null);
                    return;
                }
            }
        }
    }

    public class CommandAttribute : Attribute
    {
        public CommandAttribute(bool isCommand = true)
        {
            Command = isCommand;
        }

        public bool Command { get; set; }
    }
}