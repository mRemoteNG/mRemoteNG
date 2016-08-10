using mRemoteNG.Connection;
using mRemoteNG.Tools;
using System.ComponentModel;
using System.Windows.Forms;

namespace mRemoteNG.Container
{
    [DefaultProperty("Name")]
    public class ContainerInfo : Parent,IInheritable
	{
        #region Properties
        [LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 1), 
            Browsable(true), 
            ReadOnly(false), 
            Bindable(false), 
            DefaultValue(""), 
            DesignOnly(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameName"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionName")]
        public string Name
		{
			get { return ConnectionInfo.Name; }
			set { ConnectionInfo.Name = value; }
		}
		
        [Category(""), 
            Browsable(false), 
            ReadOnly(false), 
            Bindable(false), 
            DefaultValue(""), 
            DesignOnly(false)]
        public TreeNode TreeNode { get; set; }

        [Category(""), Browsable(false)]
        public ContainerInfo Parent { get; set; }

        [Category(""), Browsable(false)]
        public ConnectionInfoInheritance Inheritance
        {
            get { return ConnectionInfo.Inheritance; }
            set { ConnectionInfo.Inheritance = value; }
        }

        [Category(""), 
            Browsable(false), 
            ReadOnly(false), 
            Bindable(false), 
            DefaultValue(""), 
            DesignOnly(false)]
        public bool IsExpanded { get; set; }

        public ConnectionInfo ConnectionInfo { get; set; } = new ConnectionInfo();

        #endregion
			
        #region Methods
		public ContainerInfo Copy()
		{
			return (ContainerInfo)MemberwiseClone();
		}
			
		public ContainerInfo()
		{
            SetDefaults();
		}
			
		public void SetDefaults()
		{
            IsExpanded = true;
		}
        #endregion
	}
}