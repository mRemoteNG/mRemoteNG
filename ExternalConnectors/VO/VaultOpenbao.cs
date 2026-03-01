using Microsoft.Win32;
using System.Net;
using System.Net.Sockets;
using VaultSharp;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.Token;

namespace ExternalConnectors.VO {
    public class VaultOpenbaoException(string message, string? arguments = null) : Exception(message) {
        public string Arguments { get; set; } = arguments ?? string.Empty;
    }

    public static class VaultOpenbao {
        private static readonly RegistryKey baseKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\mRemoteVaultOpenbao");
        private static string token = "";
        private static VaultClient GetClient() {
            string url = (string)baseKey.GetValue("URL", "");
            using VaultOpenbaoConnectionForm voForm = new();
            voForm.tbUrl.Text = url;
            voForm.tbToken.Text = token;
            _ = voForm.ShowDialog();
            if (voForm.DialogResult != DialogResult.OK)
                throw new VaultOpenbaoException($"No credential provided");
            url = voForm.tbUrl.Text;
            if (!string.IsNullOrEmpty(voForm.tbToken.Text)) // override token if provided
                token = voForm.tbToken.Text;
            IAuthMethodInfo authMethod = new TokenAuthMethodInfo(token);
            var vaultClientSettings = new VaultClientSettings(url, authMethod);
            VaultClient client = new(vaultClientSettings);
            var sysInfo = Task.Run(() => client.V1.System.GetInitStatusAsync()).GetAwaiter().GetResult();
            if (!sysInfo) {
                MessageBox.Show("Test connection failed", "Vault Openbao", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new VaultOpenbaoException("Url not working");
            }
            baseKey.SetValue("URL", url);
            return client;
        }
        private static void TestMountType(VaultClient vaultClient, string mount, int VaultOpenbaoSecretEngine) {
            switch (Task.Run(() => vaultClient.V1.System.GetSecretBackendAsync(mount)).GetAwaiter().GetResult().Data.Type.Type) {
                case "kv" when VaultOpenbaoSecretEngine != 0:
                    throw new VaultOpenbaoException($"Backend of type kv does not match expected type {VaultOpenbaoSecretEngine}");
                case "ldap" when VaultOpenbaoSecretEngine != 1 && VaultOpenbaoSecretEngine != 2:
                    throw new VaultOpenbaoException($"Backend of type ldap does not match expected type {VaultOpenbaoSecretEngine}");
                case "ssh" when VaultOpenbaoSecretEngine != 3:
                    throw new VaultOpenbaoException($"Backend of type ssh does not match expected type {VaultOpenbaoSecretEngine}");
            }
        }
        public static void ReadOtpSSH(string mount, string role, string? username, string address, out string password) {
            VaultClient vaultClient = GetClient();
            TestMountType(vaultClient, mount, 3);
            if (!IPAddress.TryParse(address, out _)) {
                try {
                        var addrs = Task.Run(() => Dns.GetHostAddressesAsync(address)).GetAwaiter().GetResult();
                    if (addrs == null || addrs.Length == 0) {
                        throw new VaultOpenbaoException($"Could not resolve address '{address}'");
                    }
                    // Prefer IPv4, otherwise take first available
                    var selected = addrs.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork) ?? addrs[0];
                    address = selected.ToString();
                } catch (Exception ex) {
                    throw new VaultOpenbaoException($"Failed to resolve address '{address}'", ex.Message);
                }
            }
            var otp = Task.Run(() => vaultClient.V1.Secrets.SSH.GetCredentialsAsync(role, address, username, mount)).GetAwaiter().GetResult();
            password = otp.Data.Key;

        }
        public static void ReadPasswordSSH(int secretEngine, string mount, string role, string username, out string password) {
            VaultClient vaultClient = GetClient();
            TestMountType(vaultClient, mount, secretEngine);
            switch (secretEngine) {
                case 0:
                    var kv = Task.Run(() => vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(role, mountPoint: mount)).GetAwaiter().GetResult();
                    password = kv.Data.Data[username].ToString() ?? string.Empty;
                    return;
                default:
                    throw new VaultOpenbaoException($"Backend of type {secretEngine} is not supported");
            }
        }
        public static void ReadPasswordRDP(int secretEngine, string mount, string role, ref string username, out string password) {
            VaultClient vaultClient = GetClient();
            TestMountType(vaultClient, mount, secretEngine);
            switch (secretEngine) {
                case 0:
                    var kv = Task.Run(() => vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(role, mountPoint: mount)).GetAwaiter().GetResult();
                    password = kv.Data.Data[username].ToString() ?? string.Empty;
                    return;
                case 1:
                    var ldapd = Task.Run(() => vaultClient.V1.Secrets.OpenLDAP.GetDynamicCredentialsAsync(role, mount)).GetAwaiter().GetResult();
                    username = ldapd.Data.Username;
                    password = ldapd.Data.Password;
                    return;
                case 2:
                    var ldaps = Task.Run(() => vaultClient.V1.Secrets.OpenLDAP.GetStaticCredentialsAsync(role, mount)).GetAwaiter().GetResult();
                    username = ldaps.Data.Username;
                    password = ldaps.Data.Password;
                    return;
                default:
                    throw new VaultOpenbaoException($"Backend of type {secretEngine} is not supported");
            }

        }

    }
}
