using System;
using System.ComponentModel;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tools;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.Tree.Root
{
    [SupportedOSPlatform("windows")]
    [DefaultProperty("Name")]
    public class RootNodeInfo(RootNodeType rootType, string uniqueId) : ContainerInfo(uniqueId)
    {
        private string _name = Language.Connections;
        private string _customPassword = "";
        private bool _password;

        public RootNodeInfo(RootNodeType rootType)
            : this(rootType, Guid.NewGuid().ToString())
        {
        }

        #region Public Properties

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Miscellaneous)),
         Browsable(true),
         LocalizedAttributes.LocalizedDefaultValue(nameof(Language.Connections)),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.Name)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionName))]
        public override string Name
        {
            get => _name;
            set => SetField(ref _name, value, nameof(Name));
        }
        
        [LocalizedAttributes.LocalizedCategory(nameof(Language.Miscellaneous)),
         Browsable(true),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.PasswordProtect)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionPasswordProtect)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public new bool Password
        {
            get => _password;
            set => SetField(ref _password, value, nameof(Password));
        }

        [Browsable(false)]
        public string PasswordString
        {
            get => (Password && !string.IsNullOrEmpty(_customPassword)) ? _customPassword : DefaultPassword;
            set
            {
                SetField(ref _customPassword, value, nameof(PasswordString));
                Password = !string.IsNullOrEmpty(value) && _customPassword != DefaultPassword;
            }
        }

        [Browsable(false)] public string DefaultPassword { get; } = "mR3m"; //TODO move password away from code to settings

        [Browsable(false)] public RootNodeType Type { get; set; } = rootType;

        public override TreeNodeType GetTreeNodeType()
        {
            return Type == RootNodeType.Connection
                ? TreeNodeType.Root
                : TreeNodeType.PuttyRoot;
        }
        #endregion
    }
}