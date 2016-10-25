using mRemoteNG.Tools;
using System.ComponentModel;
using System.Security;
using mRemoteNG.Container;
using mRemoteNG.Security;


namespace mRemoteNG.Tree.Root
{
	[DefaultProperty("Name")]
    public class RootNodeInfo : ContainerInfo
	{
	    private string _name;
	    private string _customPassword = "";

	    public RootNodeInfo(RootNodeType rootType)
		{
            _name = Language.strConnections;
			Type = rootType;
		}
		
        #region Public Properties

	    [LocalizedAttributes.LocalizedCategory("strCategoryDisplay"),
	     Browsable(true),
	     LocalizedAttributes.LocalizedDefaultValue("strConnections"),
	     LocalizedAttributes.LocalizedDisplayName("strPropertyNameName"),
	     LocalizedAttributes.LocalizedDescription("strPropertyDescriptionName")]
	    public override string Name
	    {
	        get { return _name; }
            set { _name = value; }
	    }

	    [LocalizedAttributes.LocalizedCategory("strCategoryDisplay"),
            Browsable(true),
            LocalizedAttributes.LocalizedDisplayName("strPasswordProtect"),
            TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public new bool Password { get; set; }

	    [Browsable(false)]
	    public string PasswordString
	    {
	        get
	        {
	            return Password ? _customPassword : DefaultPassword;
	        }
	        set
	        {
	            _customPassword = value;
	            Password = !string.IsNullOrEmpty(value) && _customPassword != DefaultPassword;
	        }
	    }

        [Browsable(false)]
        public string DefaultPassword { get; } = "mR3m";
			
		[Browsable(false)]
        public RootNodeType Type {get; set;}

        public override TreeNodeType GetTreeNodeType()
        {
            return TreeNodeType.Root;
        }
        #endregion
    }
}