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
            // Re-set name after base ContainerInfo constructor overrides it via SetDefaults()
            _name = Language.Connections;
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

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Miscellaneous)),
         Browsable(true),
         DisplayName("Auto lock on minimize"),
         Description("Require master password when restoring the app after minimize."),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool AutoLockOnMinimize { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Miscellaneous)),
         Browsable(true),
         DisplayName("Two-Factor Authentication (TOTP)"),
         Description("Require a TOTP code from an authenticator app in addition to the master password."),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool TotpEnabled { get; set; }

        [Browsable(false)]
        public string TotpSecret { get; set; } = "";

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

        [Browsable(false)] public string DefaultPassword { get; } = Security.ConnectionFileDefaults.LegacyEncryptionKey;

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

        [Browsable(false)]
        public string Filename { get; set; } = string.Empty;
        #endregion
    }
}
