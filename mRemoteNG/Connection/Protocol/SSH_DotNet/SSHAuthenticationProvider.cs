using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Renci.SshNet;
using Renci.SshNet.Common;

namespace mRemoteNG.Connection.Protocol.SSH_DotNet
{
    public static class SSHAuthenticationProvider
    {
        #region Public Methods

        /// <summary>
        /// Get all applicable authentication methods for the connection
        /// </summary>
        public static AuthenticationMethod[] GetAuthenticationMethods(
            string username,
            string password,
            ConnectionInfo connectionInfo)
        {
            SSHDotNetDiagnostics.LogDebug($"Auth: Building authentication methods for user '{username}'");

            var authMethods = new List<AuthenticationMethod>();

            try
            {
                // 1. Try password authentication if password is provided
                if (!string.IsNullOrEmpty(password))
                {
                    SSHDotNetDiagnostics.LogAuthAttempt(username, "Password");
                    authMethods.Add(new PasswordAuthenticationMethod(username, password));
                }

                // 2. Try public key authentication if key path is configured
                // (Property to be added in Phase 5 if needed)
                var keyAuthMethod = TryCreateKeyAuthenticationFromConnectionInfo(username, connectionInfo);
                if (keyAuthMethod != null)
                {
                    authMethods.Add(keyAuthMethod);
                }

                // 3. Try keyboard-interactive (for 2FA/MFA)
                SSHDotNetDiagnostics.LogDebug("Auth: Adding keyboard-interactive method");
                authMethods.Add(CreateKeyboardInteractiveAuth(username, password));

                SSHDotNetDiagnostics.LogDebug($"Auth: Created {authMethods.Count} authentication method(s)");

                return authMethods.ToArray();
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Auth: Failed to create authentication methods", ex);
                throw;
            }
        }

        /// <summary>
        /// Create password authentication method
        /// </summary>
        public static PasswordAuthenticationMethod CreatePasswordAuth(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("Username cannot be empty", nameof(username));

            SSHDotNetDiagnostics.LogAuthAttempt(username, "Password");
            return new PasswordAuthenticationMethod(username, password ?? string.Empty);
        }

        /// <summary>
        /// Create public key authentication from file
        /// </summary>
        public static PrivateKeyAuthenticationMethod CreatePrivateKeyAuth(
            string username,
            string privateKeyPath,
            string passphrase = null)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("Username cannot be empty", nameof(username));

            if (string.IsNullOrEmpty(privateKeyPath))
                throw new ArgumentException("Private key path cannot be empty", nameof(privateKeyPath));

            if (!File.Exists(privateKeyPath))
            {
                SSHDotNetDiagnostics.LogError($"Auth: Private key file not found: {privateKeyPath}");
                throw new FileNotFoundException($"Private key file not found: {privateKeyPath}");
            }

            try
            {
                SSHDotNetDiagnostics.LogAuthAttempt(username, $"PublicKey (file: {Path.GetFileName(privateKeyPath)})");

                PrivateKeyFile keyFile;
                if (!string.IsNullOrEmpty(passphrase))
                {
                    keyFile = new PrivateKeyFile(privateKeyPath, passphrase);
                    SSHDotNetDiagnostics.LogDebug("Auth: Private key loaded with passphrase");
                }
                else
                {
                    keyFile = new PrivateKeyFile(privateKeyPath);
                    SSHDotNetDiagnostics.LogDebug("Auth: Private key loaded without passphrase");
                }

                SSHDotNetDiagnostics.LogInfo($"Auth: Loaded private key from {Path.GetFileName(privateKeyPath)}");

                return new PrivateKeyAuthenticationMethod(username, keyFile);
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException($"Auth: Failed to load private key from {privateKeyPath}", ex);
                throw;
            }
        }

        /// <summary>
        /// Create public key authentication from string content
        /// </summary>
        public static PrivateKeyAuthenticationMethod CreatePrivateKeyAuthFromString(
            string username,
            string privateKeyContent,
            string passphrase = null)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("Username cannot be empty", nameof(username));

            if (string.IsNullOrEmpty(privateKeyContent))
                throw new ArgumentException("Private key content cannot be empty", nameof(privateKeyContent));

            try
            {
                SSHDotNetDiagnostics.LogAuthAttempt(username, "PublicKey (from credential provider)");

                using (var keyStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(privateKeyContent)))
                {
                    PrivateKeyFile keyFile;
                    if (!string.IsNullOrEmpty(passphrase))
                    {
                        keyFile = new PrivateKeyFile(keyStream, passphrase);
                    }
                    else
                    {
                        keyFile = new PrivateKeyFile(keyStream);
                    }

                    SSHDotNetDiagnostics.LogInfo($"Auth: Loaded private key from content");

                    return new PrivateKeyAuthenticationMethod(username, keyFile);
                }
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Auth: Failed to load private key from content", ex);
                throw;
            }
        }

        /// <summary>
        /// Create keyboard-interactive authentication (for 2FA/MFA)
        /// </summary>
        public static KeyboardInteractiveAuthenticationMethod CreateKeyboardInteractiveAuth(
            string username,
            string password = null)
        {
            SSHDotNetDiagnostics.LogAuthAttempt(username, "KeyboardInteractive");

            var keyboardAuth = new KeyboardInteractiveAuthenticationMethod(username);

            keyboardAuth.AuthenticationPrompt += (sender, e) =>
            {
                SSHDotNetDiagnostics.LogInfo($"Auth: Keyboard-interactive prompt received: {e.Prompts.Count()} prompt(s)");

                foreach (var prompt in e.Prompts)
                {
                    // Log prompt without exposing sensitive data
                    SSHDotNetDiagnostics.LogDebug($"Auth: Prompt: '{prompt.Request}' (Echo: {prompt.IsEchoed})");

                    // If we have a password and this looks like a password prompt, use it
                    if (!string.IsNullOrEmpty(password) &&
                        (prompt.Request.ToLower().Contains("password") ||
                         prompt.Request.Contains(":") && !prompt.IsEchoed))
                    {
                        prompt.Response = password;
                        SSHDotNetDiagnostics.LogDebug("Auth: Provided stored password to prompt");
                    }
                    else if (prompt.IsEchoed)
                    {
                        // For echoed prompts, might be username or other info
                        // Could prompt user here in future
                        SSHDotNetDiagnostics.LogWarning($"Auth: Cannot auto-respond to echoed prompt: {prompt.Request}");
                    }
                }
            };

            return keyboardAuth;
        }

        #endregion

        #region Private Helper Methods

        private static PrivateKeyAuthenticationMethod TryCreateKeyAuthenticationFromConnectionInfo(
            string username,
            ConnectionInfo connectionInfo)
        {
            // TODO: In Phase 5, if we add SSH key path property to ConnectionInfo, use it here
            // For now, check if credential providers have SSH keys

            try
            {
                // Check for SSH key in connection properties or credential providers
                // This is a placeholder for future implementation

                SSHDotNetDiagnostics.LogDebug("Auth: No SSH key path configured in connection");
                return null;
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Auth: Error checking for SSH key", ex);
                return null;
            }
        }

        #endregion
    }
}
