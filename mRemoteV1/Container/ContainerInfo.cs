using mRemoteNG.Connection;
using mRemoteNG.Tools;
using System.ComponentModel;
using System.Windows.Forms;

namespace mRemoteNG.Container
{
    [DefaultProperty("Name")]
    public class ContainerInfo : Parent,IInheritable
	{
        private TreeNode _TreeNode;
        private ContainerInfo _Parent;
        private ConnectionInfo _ConnectionInfo = new ConnectionInfo();
        private bool _IsExpanded;

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
        public TreeNode TreeNode
		{
			get { return _TreeNode; }
			set { _TreeNode = value; }
		}
		
        [Category(""), 
            Browsable(false)]
        public ContainerInfo Parent
		{
			get { return _Parent; }
			set { _Parent = value; }
		}

        [Category(""),
            Browsable(false)]
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
        public bool IsExpanded
		{
			get { return _IsExpanded; }
			set { _IsExpanded = value; }
		}
		
        public ConnectionInfo ConnectionInfo
		{
			get { return _ConnectionInfo; }
			set { _ConnectionInfo = value; }
		}
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