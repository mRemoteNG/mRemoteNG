using mRemoteNG.Tools;
using System.ComponentModel;
using System.Windows.Forms;


namespace mRemoteNG.Root
{
	[DefaultProperty("Name")]
    public class Info
    {
        #region Private Properties
        private string _name;
        #endregion

        #region Constructors
        public Info(RootType rootType)
		{
			_name = My.Language.strConnections;
			Type = rootType;
		}
        #endregion
			
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