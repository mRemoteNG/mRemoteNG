using System;
using System.ComponentModel;
using System.Security;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Security;
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
            set => _name = value;
        }
        
        [LocalizedAttributes.LocalizedCategory(nameof(Language.Miscellaneous)),
         Browsable(true),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.PasswordProtect)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionPasswordProtect)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public new bool Password { get; set; }

        [Browsable(false)]
        public string PasswordString
        {
            get => (Password && !string.IsNullOrEmpty(_customPassword)) ? _customPassword : DefaultPassword;
            set
            {
                _customPassword = value;
                Password = !string.IsNullOrEmpty(value) && _customPassword != DefaultPassword;
            }
        }

        [Browsable(false)] public string DefaultPassword { get; } = "mR3m"; //TODO move password away from code to settings

        [Browsable(false)]
        public bool IsPasswordMatch(SecureString? providedPassword)
        {
            if (providedPassword == null)
                return false;

            string expectedPassword = string.IsNullOrEmpty(_customPassword) ? DefaultPassword : _customPassword;
            string suppliedPassword = providedPassword.ConvertToUnsecureString();
            return string.Equals(expectedPassword, suppliedPassword, StringComparison.Ordinal);
        }

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
