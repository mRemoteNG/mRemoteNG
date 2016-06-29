using mRemoteNG.Tools;
using System;
using System.ComponentModel;
using System.Security;

namespace mRemoteNG.Credential
{
	public class CredentialInfo
	{
	    public string Uuid { get; set; } = Guid.NewGuid().ToString();

	    [LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 1),
	     Browsable(true),
	     LocalizedAttributes.LocalizedDisplayName("strPropertyNameName"),
	     LocalizedAttributes.LocalizedDescription("strPropertyDescriptionName")]
	    public string Name { get; set; } = Language.strNewCredential;
		

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
        public CredentialSource CredentialSource { get; set; }


	    public void SetPasswordFromUnsecureString(string unsecuredPassword)
	    {
            var secureString = new SecureString();
	        foreach (var character in unsecuredPassword.ToCharArray())
                secureString.AppendChar(character);
	        // ReSharper disable once RedundantAssignment
	        unsecuredPassword = null;
	        Password = secureString;
	    }

	    public CredentialInfo Clone()
	    {
	        var clone = new CredentialInfo
	        {
                Uuid = Uuid,
	            Name = Name,
	            Description = Description,
                Username = Username,
                Password = Password,
                Domain = Domain,
                CredentialSource = CredentialSource
	        };
	        return clone;
	    }
	}
}