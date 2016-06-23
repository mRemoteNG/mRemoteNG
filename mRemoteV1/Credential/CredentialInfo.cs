using mRemoteNG.Tools;
using System;
using System.ComponentModel;
using System.Security;

namespace mRemoteNG.Credential
{
	public class CredentialInfo
	{
	    public string Uuid { get; } = Guid.NewGuid().ToString();

	    [LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 1), 
            Browsable(true),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameName"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionName")]
        public string Name { get; set; }
		

        [LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 1),
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
            PasswordPropertyText(true)]
        public SecureString Password { get; set; }

		
        [LocalizedAttributes.LocalizedCategory("strCategoryCredentials", 2),
            Browsable(true),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameDomain"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionDomain")]
        public string Domain { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryCredentials", 2),
            Browsable(true),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameDomain"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionDomain")]
        public string CredentialSource { get; set; }
    }
}