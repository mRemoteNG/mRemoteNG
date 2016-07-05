using mRemoteNG.Tools;
using System;
using System.ComponentModel;
using System.Security;
using System.Xml.Serialization;
using mRemoteNG.Security;

namespace mRemoteNG.Credential
{
    [Serializable]
    public class CredentialInfo
    {
        [XmlElement("Uuid")]
        public string Uuid { get; set; } = Guid.NewGuid().ToString();

        [LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 1),
         Browsable(true),
         XmlElement("Name"),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameName"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionName")]
        public string Name { get; set; } = Language.strNewCredential;

        [LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 1),
         Browsable(true),
         XmlElement("Description"),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameDescription"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionDescription")]
        public string Description { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryCredentials", 2),
         Browsable(true),
         XmlElement("Username"),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameUsername"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionUsername")]
        public string Username { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryCredentials", 2),
         Browsable(true),
         XmlIgnore,
         LocalizedAttributes.LocalizedDisplayName("strPropertyNamePassword"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionPassword"),
         PasswordPropertyText(true)]
        public SecureString Password { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryCredentials", 2),
         Browsable(true),
         XmlElement("Domain"),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameDomain"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionDomain")]
        public string Domain { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryCredentials", 2),
         Browsable(true),
         XmlElement("CredentialSource"),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameDomain"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionDomain")]
        public CredentialSource CredentialSource { get; set; } = default(CredentialSource);

        [XmlElement("Password")]
        public string EncryptedPassword
        {
            get
            {
                return Password.ConvertToEncryptedString();
            }
            set
            {
                Password = new SecureString().ConvertFromEncryptedString(value);
            }
        } 


        public void SetPasswordFromUnsecureString(string unsecuredPassword)
        {
            Password = unsecuredPassword.ConvertToSecureString();
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