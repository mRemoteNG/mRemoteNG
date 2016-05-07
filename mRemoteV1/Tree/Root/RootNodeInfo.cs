using mRemoteNG.Tools;
using System.ComponentModel;
using System.Windows.Forms;


namespace mRemoteNG.Tree.Root
{
	[DefaultProperty("Name")]
    public class RootNodeInfo
    {
        private string _name;


        public RootNodeInfo(RootNodeType rootType)
		{
			_name = My.Language.strConnections;
			Type = rootType;
		}
			
        #region Public Properties
        [LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 1), 
            Browsable(true),
            LocalizedAttributes.LocalizedDefaultValue("strConnections"),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameName"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionName")]
        public virtual string Name
		{
			get { return _name; }
			set
			{
				if (_name == value)
				{
					return ;
				}
				_name = value;
				if (TreeNode != null)
				{
					TreeNode.Name = value;
					TreeNode.Text = value;
				}
			}
		}

        [LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 1),
            Browsable(true),
            LocalizedAttributes.LocalizedDisplayName("strPasswordProtect"),
            TypeConverter(typeof(Tools.MiscTools.YesNoTypeConverter))]
        public bool Password { get; set; }
			
		[Browsable(false)]
        public string PasswordString {get; set;}
			
		[Browsable(false)]
        public RootNodeType Type {get; set;}
			
		[Browsable(false)]
        public TreeNode TreeNode {get; set;}
        #endregion
	}
}