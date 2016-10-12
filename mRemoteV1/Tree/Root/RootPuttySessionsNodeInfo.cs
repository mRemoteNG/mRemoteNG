using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Root.PuttySessions
{
    public class RootPuttySessionsNodeInfo : RootNodeInfo
    {
        private string _name;
        private string _panel;


        public RootPuttySessionsNodeInfo() : base(RootNodeType.PuttySessions)
        {
            _name = Language.strPuttySavedSessionsRootName;
            _panel =
                string.IsNullOrEmpty(Settings.Default.PuttySavedSessionsPanel)
                    ? Language.strGeneral
                    : Settings.Default.PuttySavedSessionsPanel;
        }

        #region Public Properties

        [LocalizedAttributes.LocalizedDefaultValueAttribute("strPuttySavedSessionsRootName")]
        public override string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                //Settings.Default.PuttySavedSessionsName = value;
            }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryDisplay")]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNamePanel")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionPanel")]
        public override string Panel
        {
            get { return _panel; }
            set
            {
                _panel = value;
                Settings.Default.PuttySavedSessionsPanel = value;
            }
        }

        public override TreeNodeType GetTreeNodeType()
        {
            return TreeNodeType.PuttyRoot;
        }

        #endregion
    }
}