using System;
using System.Windows.Forms;
using mRemoteNG.Tools;
using System.ComponentModel;


namespace mRemoteNG.Container
{
	[DefaultProperty("Name")]
    public class ContainerInfo
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
			
		private TreeNode _TreeNode;
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
			
		private object _Parent;
        [Category(""), 
            Browsable(false)]
        public object Parent
		{
			get { return _Parent; }
			set { _Parent = value; }
		}
			
		//Private _GlobalID As Integer = 0
		//<Category(""), _
		//    Browsable(False)> _
		//Public Property GlobalID() As Integer
		//    Get
		//        Return _GlobalID
		//    End Get
		//    Set(ByVal value As Integer)
		//        _GlobalID = value
		//    End Set
		//End Property
			
		private bool _IsExpanded;
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
			
		private Connection.ConnectionInfo _ConnectionInfo = new Connection.ConnectionInfo();
        public Connection.ConnectionInfo ConnectionInfo
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