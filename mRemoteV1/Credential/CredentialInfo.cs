using mRemoteNG.Tools;
using System.ComponentModel;


namespace mRemoteNG.Credential
{
	public class CredentialInfo
	{
	    #region Public Properties
        [LocalizedAttributes.LocalizedCategory("strCategoryDisplay"), 
            Browsable(true),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameName"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionName")]
        public string Name { get; set; }

	    [LocalizedAttributes.LocalizedCategory("strCategoryDisplay"), 
            Browsable(true),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameDescription"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionDescription")]
        public string Description { get; set; }

	    [LocalizedAttributes.LocalizedCategory("strCategoryCredentials", 2), 
            Browsable(true),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameUsername"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionUsername")]
        public string Username { get; set; }

	    [LocalizedAttributes.LocalizedCategory("strCategoryCredentials", 2), 
            Browsable(true),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNamePassword"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionPassword"), 
            PasswordPropertyText(true)]public string Password { get; set; }

	    [LocalizedAttributes.LocalizedCategory("strCategoryCredentials", 2), 
            Browsable(true),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameDomain"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionDomain")]
        public string Domain { get; set; }

	    #endregion
	}
}