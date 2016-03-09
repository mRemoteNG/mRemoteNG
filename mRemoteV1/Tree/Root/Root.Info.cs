using System.Windows.Forms;
using mRemoteNG.Tools;
using System.ComponentModel;


namespace mRemoteNG.Root
{
	[DefaultProperty("Name")]
    public class Info
	{
        #region Constructors
		public Info(RootType rootType)
		{
			// VBConversions Note: Non-static class variable initialization is below.  Class variables cannot be initially assigned non-static values in C#.
			_name = My.Language.strConnections;
				
			Type = rootType;
		}
        #endregion
			
        #region Public Properties
		private string _name; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 1), 
            Browsable(true),
            LocalizedAttributes.LocalizedDefaultValue("strConnections"),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameName"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionName")]
        public virtual string Name
		{
			get
			{
				return _name;
			}
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
            TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
        public bool Password { get; set; }
			
		[Browsable(false)]
        public string PasswordString {get; set;}
			
		[Browsable(false)]
        public RootType Type {get; set;}
			
		[Browsable(false)]
        public TreeNode TreeNode {get; set;}
        #endregion
			
        #region Public Enumerations
		public enum RootType
		{
			Connection,
			Credential,
			PuttySessions
		}
        #endregion
	}
}
