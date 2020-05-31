using mRemoteNG.Tools;

namespace mRemoteNG.Tree.Root
{
    public class RootPuttySessionsNodeInfo : RootNodeInfo
    {
        private string _name;
        private string _panel;


        public RootPuttySessionsNodeInfo() : base(RootNodeType.PuttySessions)
        {
            _name = Language.PuttySavedSessionsRootName;
            _panel =
                string.IsNullOrEmpty(Settings.Default.PuttySavedSessionsPanel)
                    ? Language.General
                    : Settings.Default.PuttySavedSessionsPanel;
        }

        #region Public Properties

        [LocalizedAttributes.LocalizedDefaultValue("strPuttySavedSessionsRootName")]
        public override string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                //Settings.Default.PuttySavedSessionsName = value;
            }
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Display)),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.Panel)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionPanel))]
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