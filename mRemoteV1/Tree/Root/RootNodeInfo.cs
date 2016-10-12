using System.ComponentModel;
using mRemoteNG.Container;
using mRemoteNG.Tools;

namespace mRemoteNG.Tree.Root
{
    [DefaultProperty("Name")]
    public class RootNodeInfo : ContainerInfo
    {
        private string _name;

        public RootNodeInfo(RootNodeType rootType)
        {
            _name = Language.strConnections;
            Type = rootType;
        }

        #region Public Properties

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryDisplay")]
        [Browsable(true)]
        [LocalizedAttributes.LocalizedDefaultValueAttribute("strConnections")]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameName")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionName")]
        public override string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryDisplay")]
        [Browsable(true)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPasswordProtect")]
        [TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public new bool Password { get; set; }

        [Browsable(false)]
        public string PasswordString { get; set; }

        [Browsable(false)]
        public RootNodeType Type { get; set; }

        public override TreeNodeType GetTreeNodeType()
        {
            return TreeNodeType.Root;
        }

        #endregion
    }
}