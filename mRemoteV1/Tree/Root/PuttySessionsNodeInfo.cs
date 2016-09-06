using mRemoteNG.My;
using mRemoteNG.Tools;
using mRemoteNG.Tree.Root;
using System.Windows.Forms;
using mRemoteNG.Tree;


namespace mRemoteNG.Root.PuttySessions
{
	public class PuttySessionsNodeInfo : RootNodeInfo
    {
        private string _name;
        private string _panel;


        public PuttySessionsNodeInfo() : base(RootNodeType.PuttySessions)
		{
			_name = Language.strPuttySavedSessionsRootName;
			_panel = Language.strGeneral;
		}

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
                mRemoteNG.Settings.Default.PuttySavedSessionsName = value;
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
                mRemoteNG.Settings.Default.PuttySavedSessionsPanel = value;
			}
        }

        public override TreeNodeType GetTreeNodeType()
        {
            return TreeNodeType.PuttyRoot;
        }
        #endregion
    }
}