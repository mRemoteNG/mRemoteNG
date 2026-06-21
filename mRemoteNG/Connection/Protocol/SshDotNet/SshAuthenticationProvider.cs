// Design Note: Generic catch clauses (catch Exception) are used intentionally in this file.
// Authentication operations interact with external SSH servers and key files, where any exception
// type is possible. All exceptions are logged via SshDotNetDiagnostics and handled gracefully
// to avoid crashing the application on auth failures.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Renci.SshNet;
using Renci.SshNet.Common;

namespace mRemoteNG.Connection.Protocol.SshDotNet
{
    public static class SshAuthenticationProvider
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
            SshDotNetDiagnostics.LogDebug($"Auth: Building authentication methods for user '{username}'");

            var authMethods = new List<AuthenticationMethod>();

            try
            {
                // 1. Public-key authentication if a key file is configured (preferred, like OpenSSH).
                var keyAuthMethod = TryCreateKeyAuthenticationFromConnectionInfo(username, connectionInfo);
                if (keyAuthMethod != null)
                {
                    authMethods.Add(keyAuthMethod);
                }

                // 2. Password authentication if a password is provided.
                if (!string.IsNullOrEmpty(password))
                {
                    SshDotNetDiagnostics.LogAuthAttempt(username, "Password");
                    authMethods.Add(new PasswordAuthenticationMethod(username, password));
                }

                // 3. Keyboard-interactive (for 2FA/MFA).
                SshDotNetDiagnostics.LogDebug("Auth: Adding keyboard-interactive method");
                authMethods.Add(CreateKeyboardInteractiveAuth(username, password));

                SshDotNetDiagnostics.LogDebug($"Auth: Created {authMethods.Count} authentication method(s)");

                return authMethods.ToArray();
            }
            catch (Exception ex)
            {
                SshDotNetDiagnostics.LogException("Auth: Failed to create authentication methods", ex);
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

            SshDotNetDiagnostics.LogAuthAttempt(username, "Password");
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
                SshDotNetDiagnostics.LogError($"Auth: Private key file not found: {privateKeyPath}");
                throw new FileNotFoundException($"Private key file not found: {privateKeyPath}");
            }

            try
            {
                SshDotNetDiagnostics.LogAuthAttempt(username, $"PublicKey (file: {Path.GetFileName(privateKeyPath)})");

                PrivateKeyFile keyFile;
                if (!string.IsNullOrEmpty(passphrase))
                {
                    keyFile = new PrivateKeyFile(privateKeyPath, passphrase);
                    SshDotNetDiagnostics.LogDebug("Auth: Private key loaded with passphrase");
                }
                else
                {
                    keyFile = new PrivateKeyFile(privateKeyPath);
                    SshDotNetDiagnostics.LogDebug("Auth: Private key loaded without passphrase");
                }

                SshDotNetDiagnostics.LogInfo($"Auth: Loaded private key from {Path.GetFileName(privateKeyPath)}");

                return new PrivateKeyAuthenticationMethod(username, keyFile);
            }
            catch (Exception ex)
            {
                SshDotNetDiagnostics.LogException($"Auth: Failed to load private key from {privateKeyPath}", ex);
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
                SshDotNetDiagnostics.LogAuthAttempt(username, "PublicKey (from credential provider)");

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

                    SshDotNetDiagnostics.LogInfo($"Auth: Loaded private key from content");

                    return new PrivateKeyAuthenticationMethod(username, keyFile);
                }
            }
            catch (Exception ex)
            {
                SshDotNetDiagnostics.LogException("Auth: Failed to load private key from content", ex);
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
            SshDotNetDiagnostics.LogAuthAttempt(username, "KeyboardInteractive");

            var keyboardAuth = new KeyboardInteractiveAuthenticationMethod(username);

            keyboardAuth.AuthenticationPrompt += (sender, e) =>
            {
                SshDotNetDiagnostics.LogInfo($"Auth: Keyboard-interactive prompt received: {e.Prompts.Count()} prompt(s)");

                foreach (var prompt in e.Prompts)
                {
                    // Log prompt without exposing sensitive data
                    SshDotNetDiagnostics.LogDebug($"Auth: Prompt: '{prompt.Request}' (Echo: {prompt.IsEchoed})");

                    // If we have a password and this looks like a password prompt, use it
                    if (!string.IsNullOrEmpty(password) &&
                        (prompt.Request.ToLower().Contains("password") ||
                         prompt.Request.Contains(":") && !prompt.IsEchoed))
                    {
                        prompt.Response = password;
                        SshDotNetDiagnostics.LogDebug("Auth: Provided stored password to prompt");
                    }
                    else if (prompt.IsEchoed)
                    {
                        // For echoed prompts, might be username or other info
                        // Could prompt user here in future
                        SshDotNetDiagnostics.LogWarning($"Auth: Cannot auto-respond to echoed prompt: {prompt.Request}");
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
            string keyPath = connectionInfo?.SshDotNetPrivateKeyFile;
            if (string.IsNullOrEmpty(keyPath))
            {
                SshDotNetDiagnostics.LogDebug("Auth: No SSH key path configured in connection");
                return null;
            }

            // Load failures (missing file, wrong passphrase, corrupt key) are allowed to propagate so
            // the user gets an actionable error rather than a silent fallback to password auth.
            return CreatePrivateKeyAuth(username, keyPath, connectionInfo.SshDotNetPrivateKeyPassphrase);
        }

        #endregion
    }
}
