using Microsoft.Win32;
using SecretServerInterface.SSWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SecretServerInterface
{
    public class SecretServerInterface
    {
        static void fetchSecret(int secretID,
           out string secretUsername, out string secretPassword, out string secretDomain)
        {
            string authUsername = SSConnectionData.ssUsername;
            string authPassword = SSConnectionData.ssPassword;
            string authDomain = SSConnectionData.ssDomain;
            string authOrg = SSConnectionData.ssOrg;
            string url = SSConnectionData.ssUrl;
            string waurl = SSConnectionData.ssWAUrl;

            if(SSConnectionData.ssSSO)
            {
                using (var service = new SSWebServiceWinAuth.SSWinAuthWebService())
                {
                    // url where to retrieve secrets from
                    service.Url = waurl;

                    // try to authenticate to server
                    service.UseDefaultCredentials = true;

                    var secret = service.GetSecret(secretID, false, null);

                    // clear return variables
                    secretDomain = "";
                    secretUsername = "";
                    secretPassword = "";

                    // parse data and extract what we need
                    for (int i = 0; i < secret.Secret.Items.Count(); i++)
                    {
                        if (secret.Secret.Items[i].FieldName.ToLower().Equals("domain"))
                            secretDomain = secret.Secret.Items[i].Value;
                        else if (secret.Secret.Items[i].FieldName.ToLower().Equals("username"))
                            secretUsername = secret.Secret.Items[i].Value;
                        else if (secret.Secret.Items[i].FieldName.ToLower().Equals("password"))
                            secretPassword = secret.Secret.Items[i].Value;
                    }
                }
            }
            else
            {
                using (var service = new SSWebService.SSWebService())
                {
                    // url where to retrieve secrets from
                    service.Url = url;

                    // try to authenticate to server
                    var authResult = service.Authenticate(authUsername, authPassword, authOrg, authDomain);
                    if (authResult.Errors.Count() > 0)
                    {
                        throw new Exception($"authentication error: {authResult.Errors[0]}");
                    }

                    // set up the coderesponse structure
                    SSWebService.CodeResponse[] codeResponses = new SSWebService.CodeResponse[1];
                    codeResponses[0] = new SSWebService.CodeResponse();
                    codeResponses[0].ErrorCode = "COMMENT";
                    codeResponses[0].Comment = $"accessing secret from mRemote, username: {authUsername}";

                    // fetch the secret
                    var secret = service.GetSecret(authResult.Token, secretID, false, codeResponses);
                    if (secret.Errors.Count() > 0)
                    {
                        throw new Exception($"getSecret error {secret.Errors[0]}");
                    }

                    // clear return variables
                    secretDomain = "";
                    secretUsername = "";
                    secretPassword = "";

                    // parse data and extract what we need
                    for (int i = 0; i < secret.Secret.Items.Count(); i++)
                    {
                        if (secret.Secret.Items[i].FieldName.ToLower().Equals("domain"))
                            secretDomain = secret.Secret.Items[i].Value;
                        else if (secret.Secret.Items[i].FieldName.ToLower().Equals("username"))
                            secretUsername = secret.Secret.Items[i].Value;
                        else if (secret.Secret.Items[i].FieldName.ToLower().Equals("password"))
                            secretPassword = secret.Secret.Items[i].Value;
                    }
                }
            }

            
        }

        public static class SSConnectionData
        {
            public static string ssUsername = "";
            public static string ssPassword = "";
            public static string ssUrl = "";
            public static string ssWAUrl = "";
            public static string ssOrg = "";
            public static string ssDomain = "";
            public static bool ssSSO = false;

            public static bool initdone = false;

            public static void init()
            {
                if (ssPassword != "" || initdone == true)
                    return;

                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\StrongITmRemoteSSInterface");
                try
                {
                    // display gui and ask for data
                    SSConnectionForm f = new SSConnectionForm();
                    f.tbDomain.Text = (string)key.GetValue("Domain");
                    f.tbOrganization.Text = (string)key.GetValue("Organization");
                    f.tbUsername.Text = (string)key.GetValue("Username");

                    string url;
                    url = (string)key.GetValue("URL");
                    if (url == null || !url.Contains("://"))
                        url = "https://cred.domain.local/SecretServer/webservices/SSWebservice.asmx";
                    f.tbSSURL.Text = url;

                    url = (string)key.GetValue("WAURL");
                    if (url == null || !url.Contains("://"))
                        url = "https://cred.domain.local/SecretServer/winauthwebservices/SSWinAuthWebService.asmx";
                    f.tbWinAuthURL.Text = url;

                    var b = key.GetValue("SSO");
                    if (b == null || (string)b != "True")
                        ssSSO = false;
                    else
                    {
                        ssSSO = true;
                        initdone = true;
                    }
                    f.cbUseSSO.Checked = ssSSO;

                    // show dialog
                    _ = f.ShowDialog();

                    if (f.DialogResult != DialogResult.OK)
                        return;

                    // store values to memory
                    ssUsername = f.tbUsername.Text;
                    ssPassword = f.tbPassword.Text;
                    ssOrg = f.tbOrganization.Text;
                    ssDomain = f.tbDomain.Text;
                    ssUrl = f.tbSSURL.Text;
                    ssWAUrl = f.tbWinAuthURL.Text;
                    ssSSO = f.cbUseSSO.Checked;

                    // write values to registry
                    key.SetValue("Username", ssUsername);
                    key.SetValue("Domain", ssDomain);
                    key.SetValue("Organization", ssOrg);
                    key.SetValue("URL", ssUrl);
                    key.SetValue("WAURL", ssWAUrl);
                    key.SetValue("SSO", ssSSO);
                }
                catch(Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    key.Close();
                }
                
            }


        }

        public static void fetchSecretFromServer(string input, out string username, out string password, out string domain)
        {
            // get secret id
            if (!input.StartsWith("SSAPI:"))
                throw new Exception("calling this function requires SSAPI: input");
            int secretID = Int32.Parse(input.Substring(6));

            // init connection credentials, display popup if necessary
            SSConnectionData.init();

            // get the secret
            fetchSecret(secretID, out username, out password, out domain);
        }
    }
}
