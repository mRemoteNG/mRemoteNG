using System.ComponentModel;
using mRemoteNG.Tools;

namespace mRemoteNG.Credential
{
    public class CredentialInfo
    {
        #region Public Properties

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryDisplay", 1)]
        [Browsable(true)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameName")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionName")]
        public string Name { get; set; }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryDisplay", 1)]
        [Browsable(true)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameDescription")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionDescription")]
        public string Description { get; set; }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryCredentials", 2)]
        [Browsable(true)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameUsername")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionUsername")]
        public string Username { get; set; }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryCredentials", 2)]
        [Browsable(true)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNamePassword")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionPassword")]
        [PasswordPropertyText(true)]
        public string Password { get; set; }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryCredentials", 2)]
        [Browsable(true)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameDomain")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionDomain")]
        public string Domain { get; set; }

        #endregion
    }
}