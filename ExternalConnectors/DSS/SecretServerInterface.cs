using Microsoft.Win32;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using SecretServerAuthentication.DSS;
using SecretServerRestClient.DSS;
using System.Security.Cryptography;

namespace ExternalConnectors.DSS;

public class SecretServerInterface
{
    private static class SSConnectionData
    {
        public static string ssUsername = "";
        public static string ssPassword = "";
        public static string ssUrl = "";
        public static string ssOTP = "";
        public static bool ssSSO = false;
        public static bool initdone = false;

        //token 
        public static string ssTokenBearer = "";
        public static DateTime ssTokenExpiresOn = DateTime.UtcNow;
        public static string ssTokenRefresh = "";

        public static void Init()
        {
            if (initdone == true)
                return;

                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\mRemoteSSInterface");
                try
                {
                    // display gui and ask for data
                    SSConnectionForm f = new();
                    string? un = key.GetValue("Username") as string;
                    f.tbUsername.Text = un ?? "";
                    f.tbPassword.Text = SSConnectionData.ssPassword;    // in OTP refresh cases, this value might already be filled

                string? url = key.GetValue("URL") as string;
                if (url == null || !url.Contains("://"))
                    url = "https://cred.domain.local/SecretServer";
                f.tbSSURL.Text = url;

                var b = key.GetValue("SSO");
                if (b == null || (string)b != "True")
                    ssSSO = false;
                else
                    ssSSO = true;
                f.cbUseSSO.Checked = ssSSO;
                
                // show dialog
                while (true)
                {
                    _ = f.ShowDialog();

                    if (f.DialogResult != DialogResult.OK)
                        return;

                    // store values to memory
                    ssUsername = f.tbUsername.Text;
                    ssPassword = f.tbPassword.Text;
                    ssUrl = f.tbSSURL.Text;
                    ssSSO = f.cbUseSSO.Checked;
                    ssOTP = f.tbOTP.Text;
                    // check connection first
                    try
                    {
                        if (TestCredentials() == true)
                        {
                            initdone = true;
                            break;
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Test Credentials failed - please check your credentials");
                    }
                }


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

    private static bool TestCredentials()
    {
        if (SSConnectionData.ssSSO)
        {
            // checking creds doesn't really make sense here, as we can't modify them anyway if something is wrong
            return true;
        }
        else
        {

            if (!String.IsNullOrEmpty(GetToken()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    private static SecretsServiceClient ConstructSecretsServiceClient()
    {
        string baseURL = SSConnectionData.ssUrl;
        if (SSConnectionData.ssSSO)
        {
            // REQUIRES IIS CONFIG! https://docs.thycotic.com/ss/11.0.0/api-scripting/webservice-iwa-powershell
            var handler = new HttpClientHandler() { UseDefaultCredentials = true };
            var httpClient = new HttpClient(handler);
            {
                // Call REST API:
                return new SecretsServiceClient($"{baseURL}/winauthwebservices/api", httpClient);
            }
        }
        else
        {
            var httpClient = new HttpClient();
            {

                var token = GetToken();
                // Set credentials (token):
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Call REST API:
                return new SecretsServiceClient($"{baseURL}/api", httpClient);
            }
        }

    }
    private static void FetchSecret(int secretID, out string secretUsername, out string secretPassword, out string secretDomain, out string privatekey)
    {
        var client = ConstructSecretsServiceClient();
        SecretModel secret = client.GetSecretAsync(false, true, secretID, null).Result;

        // clear return variables
        secretDomain = "";
        secretUsername = "";
        secretPassword = "";
        privatekey = "";
        string privatekeypassphrase = "";

        // parse data and extract what we need
        foreach (var item in secret.Items)
        {
            if (item.FieldName.ToLower().Equals("domain"))
                secretDomain = item.ItemValue;
            else if (item.FieldName.ToLower().Equals("username"))
                secretUsername = item.ItemValue;
            else if (item.FieldName.ToLower().Equals("password"))
                secretPassword = item.ItemValue;
            else if (item.FieldName.ToLower().Equals("private key"))
            {
                client.ReadResponseNoJSONConvert = true;
                privatekey = client.GetFieldAsync(false, false, secretID, "private-key").Result;
                client.ReadResponseNoJSONConvert = false;
            }
            else if (item.FieldName.ToLower().Equals("private key passphrase"))
                privatekeypassphrase = item.ItemValue;
        }

        // need to decode the private key?
        if (!string.IsNullOrEmpty(privatekeypassphrase))
        {
            try
            {
                var key = DecodePrivateKey(privatekey, privatekeypassphrase);
                privatekey = key;
            }
            catch(Exception)
            {

            }
        }

        // conversion to putty format necessary?
        if (!string.IsNullOrEmpty(privatekey) && !privatekey.StartsWith("PuTTY-User-Key-File-2"))
        {
            try
            {
                RSACryptoServiceProvider key = ImportPrivateKey(privatekey);
                privatekey = PuttyKeyFileGenerator.ToPuttyPrivateKey(key);
            }
            catch (Exception)
            {

            }
        }
    }

        #region PUTTY KEY HANDLING
        // decode rsa private key with encryption password
        private static string DecodePrivateKey(string encryptedPrivateKey, string password)
        {
            TextReader textReader = new StringReader(encryptedPrivateKey);
            PemReader pemReader = new(textReader, new PasswordFinder(password));

        AsymmetricCipherKeyPair keyPair = (AsymmetricCipherKeyPair)pemReader.ReadObject();

        TextWriter textWriter = new StringWriter();
        var pemWriter = new PemWriter(textWriter);
        pemWriter.WriteObject(keyPair.Private);
        pemWriter.Writer.Flush();

        return ""+textWriter.ToString();
    }
    private class PasswordFinder : IPasswordFinder
    {
        private string password;

        public PasswordFinder(string password)
        {
            this.password = password;
        }


        public char[] GetPassword()
        {
            return password.ToCharArray();
        }
    }

        // read private key pem string to rsacryptoserviceprovider
        public static RSACryptoServiceProvider ImportPrivateKey(string pem)
        {
            PemReader pr = new(new StringReader(pem));
            AsymmetricCipherKeyPair KeyPair = (AsymmetricCipherKeyPair)pr.ReadObject();
            RSAParameters rsaParams = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)KeyPair.Private);
            RSACryptoServiceProvider rsa = new();
            rsa.ImportParameters(rsaParams);
            return rsa;
        }
        #endregion


    #region TOKEN
    private static string GetToken()
    {
        // if there is no token, fetch a fresh one
        if (String.IsNullOrEmpty(SSConnectionData.ssTokenBearer))
        {
            return GetTokenFresh();
        }
        // if there is a token, check if it is valid
        if (SSConnectionData.ssTokenExpiresOn >= DateTime.UtcNow)
        {
            return SSConnectionData.ssTokenBearer;
        }
        else
        {
            // try using refresh token
            using (var httpClient = new HttpClient())
            {
                var tokenClient = new OAuth2ServiceClient(SSConnectionData.ssUrl, httpClient);
                TokenResponse token = new();
                try
                {
                    token = tokenClient.AuthorizeAsync(Grant_type.Refresh_token, null, null, SSConnectionData.ssTokenRefresh, null).Result;
                    var tokenResult = token.Access_token;

                    SSConnectionData.ssTokenBearer = tokenResult;
                    SSConnectionData.ssTokenRefresh = token.Refresh_token;
                    SSConnectionData.ssTokenExpiresOn = token.Expires_on;
                    return tokenResult;
                }
                catch (Exception)
                {
                    // refresh token failed. clean memory and start fresh
                    SSConnectionData.ssTokenBearer = "";
                    SSConnectionData.ssTokenRefresh = "";
                    SSConnectionData.ssTokenExpiresOn = DateTime.Now;
                    // if OTP is required we need to ask user for a new OTP
                    if (!String.IsNullOrEmpty(SSConnectionData.ssOTP))
                    {
                        SSConnectionData.initdone = false;
                        // the call below executes a connection test, which fetches a valid token
                        SSConnectionData.Init();
                        // we now have a fresh token in memory. return it to caller
                        return SSConnectionData.ssTokenBearer;
                    }
                    else
                    {
                        // no user interaction required. get a fresh token and return it to caller
                        return GetTokenFresh();
                    }
                }
            }
        }
    }
    static string GetTokenFresh()
    {
        using (var httpClient = new HttpClient())
        {
            // Authenticate:
            var tokenClient = new OAuth2ServiceClient(SSConnectionData.ssUrl, httpClient);
            // call below will throw an exception if the creds are invalid
            var token = tokenClient.AuthorizeAsync(Grant_type.Password, SSConnectionData.ssUsername, SSConnectionData.ssPassword, null, SSConnectionData.ssOTP).Result;
            // here we can be sure the creds are ok - return success state                   
            var tokenResult = token.Access_token;

            SSConnectionData.ssTokenBearer = tokenResult;
            SSConnectionData.ssTokenRefresh = token.Refresh_token;
            SSConnectionData.ssTokenExpiresOn = token.Expires_on;
            return tokenResult;
        }
    }
    #endregion


    // input must be the secret id to fetch
    public static void FetchSecretFromServer(string input, out string username, out string password, out string domain, out string privatekey)
    {
        // get secret id
        int secretID = Int32.Parse(input);

        // init connection credentials, display popup if necessary
        SSConnectionData.Init();

        // get the secret
        FetchSecret(secretID, out username, out password, out domain, out privatekey);
    }
}
