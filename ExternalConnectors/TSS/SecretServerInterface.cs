using Microsoft.Win32;
using SecretServerAuthentication.TSS;
using SecretServerRestClient.TSS;

namespace ExternalConnectors.TSS
{
    public class SecretServerInterface
    {
        private static class SSConnectionData
        {
            public static string ssUsername = "";
            public static string ssPassword = "";
            public static string ssUrl = "";
            public static bool ssSSO = false;

            public static bool initdone = false;

            public static void Init()
            {
                if (ssPassword != "" || initdone == true)
                    return;

                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\mRemoteSSInterface");
                try
                {
                    // display gui and ask for data
                    SSConnectionForm f = new SSConnectionForm();
                    string? un = key.GetValue("Username") as string;
                    f.tbUsername.Text = un ?? "";

                    string? url = key.GetValue("URL") as string;
                    if (url == null || !url.Contains("://"))
                        url = "https://cred.domain.local/SecretServer";
                    f.tbSSURL.Text = url;

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
                    ssUrl = f.tbSSURL.Text;
                    ssSSO = f.cbUseSSO.Checked;

                    // write values to registry
                    key.SetValue("Username", ssUsername);
                    key.SetValue("URL", ssUrl);
                    key.SetValue("SSO", ssSSO);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    key.Close();
                }

            }
        }

        private static void FetchSecret(int secretID, out string secretUsername, out string secretPassword, out string secretDomain)
        {
            string authUsername = SSConnectionData.ssUsername;
            string authPassword = SSConnectionData.ssPassword;
            string baseURL = SSConnectionData.ssUrl;

            SecretModel secret;
            if (SSConnectionData.ssSSO)
            {
                // REQUIRES IIS CONFIG! https://docs.thycotic.com/ss/11.0.0/api-scripting/webservice-iwa-powershell
                var handler = new HttpClientHandler() { UseDefaultCredentials = true };
                using (var httpClient = new HttpClient(handler))
                {
                    // Call REST API:
                    var client = new SecretsServiceClient($"{baseURL}/winauthwebservices/api", httpClient);
                    secret = client.GetSecretAsync(false, true, secretID, null).Result;
                }
            }
            else
            {
                using (var httpClient = new HttpClient())
                {
                    // Authenticate:
                    var tokenClient = new OAuth2ServiceClient(baseURL, httpClient);
                    var token = tokenClient.AuthorizeAsync(Grant_type.Password, authUsername, authPassword, null).Result;
                    var tokenResult = token.Access_token;

                    // Set credentials (token):
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenResult);

                    // Call REST API:
                    var client = new SecretsServiceClient($"{baseURL}/api", httpClient);
                    secret = client.GetSecretAsync(false, true, secretID, null).Result;
                }
            }

            // clear return variables
            secretDomain = "";
            secretUsername = "";
            secretPassword = "";

            // parse data and extract what we need
            foreach (var item in secret.Items)
            {
                if (item.FieldName.ToLower().Equals("domain"))
                    secretDomain = item.ItemValue;
                else if (item.FieldName.ToLower().Equals("username"))
                    secretUsername = item.ItemValue;
                else if (item.FieldName.ToLower().Equals("password"))
                    secretPassword = item.ItemValue;
            }

        }

        // input must be in form "SSAPI:xxxx" where xxx is the secret id to fetch
        public static void FetchSecretFromServer(string input, out string username, out string password, out string domain)
        {
            // get secret id
            if (!input.StartsWith("SSAPI:"))
                throw new Exception("calling this function requires SSAPI: input");
            int secretID = Int32.Parse(input.Substring(6));

            // init connection credentials, display popup if necessary
            SSConnectionData.Init();

            // get the secret
            FetchSecret(secretID, out username, out password, out domain);
        }
    }
}
