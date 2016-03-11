using mRemoteNG.My;
using mRemoteNG.Tools;
using System.Windows.Forms;


namespace mRemoteNG.Root.PuttySessions
{
	public class Info : Root.Info
    {
        #region Private Variables
        private string _name;
        private string _panel;
        #endregion

        #region Constructors
        public Info() : base(RootType.PuttySessions)
		{
			_name = Language.strPuttySavedSessionsRootName;
			_panel = My.Language.strGeneral;
		}
        #endregion

        #region Public Properties
        [LocalizedAttributes.LocalizedDefaultValue("strPuttySavedSessionsRootName")]
        public override string Name
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
					TreeNode.Text = value;
				}
                My.Settings.Default.PuttySavedSessionsName = value;
			}
		}
				
        [LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 1),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNamePanel"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionPanel")]
        public string Panel
		{
			get { return _panel; }
			set
			{
				if (_panel == value)
				{
					return ;
				}
				_panel = value;
				My.Settings.Default.PuttySavedSessionsPanel = value;
			}
        }
        #endregion
    }
}