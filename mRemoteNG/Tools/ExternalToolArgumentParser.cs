using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Versioning;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Properties;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tools.Cmdline;

namespace mRemoteNG.Tools
{
    [SupportedOSPlatform("windows")]
    public class ExternalToolArgumentParser(ConnectionInfo connectionInfo, ExternalTool? externalTool = null)
    {
        private readonly ConnectionInfo _connectionInfo = connectionInfo;
        private readonly ExternalTool? _externalTool = externalTool;
        private const int MaxNestedExpansionDepth = 5;

        public string ParseArguments(string input, bool escapeForShell = true)
        {
            return ParseArguments(input, escapeForShell, 0);
        }

        private string ParseArguments(string input, bool escapeForShell, int depth)
        {
            List<Replacement> replacements = BuildReplacementList(input, escapeForShell, depth);
            string result = PerformReplacements(input, replacements);
            return result;
        }

        private List<Replacement> BuildReplacementList(string input, bool escapeForShell, int depth)
        {
            int index = 0;
            List<Replacement> replacements = new();
            do
            {
                int tokenStart = input.IndexOf('%', index);
                if (tokenStart == -1)
                    break;

                int tokenEnd = input.IndexOf('%', tokenStart + 1);
                if (tokenEnd == -1)
                    break;

                int tokenLength = tokenEnd - tokenStart + 1;
                int variableNameStart = tokenStart + 1;
                int variableNameLength = tokenLength - 2;
                bool isEnvironmentVariable = false;
                string variableName = "";

                if (tokenStart > 0)
                {
                    char tokenStartPrefix = input[tokenStart - 1];
                    char tokenEndPrefix = input[tokenEnd - 1];

                    if (tokenStartPrefix == '\\' && tokenEndPrefix == '\\')
                    {
                        isEnvironmentVariable = true;

                        // Add the first backslash to the token
                        tokenStart--;
                        tokenLength++;

                        // Remove the last backslash from the name
                        variableNameLength--;
                    }
                    else if (tokenStartPrefix == '^' && tokenEndPrefix == '^')
                    {
                        // Add the first caret to the token
                        tokenStart--;
                        tokenLength++;

                        // Remove the last caret from the name
                        variableNameLength--;

                        variableName = input.Substring(variableNameStart, variableNameLength);
                        replacements.Add(new Replacement(tokenStart, tokenLength, $"%{variableName}%"));

                        index = tokenEnd;
                        continue;
                    }
                }

                string token = input.Substring(tokenStart, tokenLength);

                EscapeType escape = DetermineEscapeType(token);

                if (escape != EscapeType.All)
                {
                    // Remove the escape character from the name
                    variableNameStart++;
                    variableNameLength--;
                }

                if (variableNameLength == 0)
                {
                    index = tokenEnd;
                    continue;
                }

                variableName = input.Substring(variableNameStart, variableNameLength);

                string replacementValue = token;
                if (!isEnvironmentVariable)
                {
                    replacementValue = GetVariableReplacement(variableName, token);

                    // Expand nested variables in UserField values (e.g., UserField = "%HOSTNAME%:8080")
                    if (replacementValue != token
                        && depth < MaxNestedExpansionDepth
                        && replacementValue.Contains('%')
                        && IsUserFieldVariable(variableName))
                    {
                        replacementValue = ParseArguments(replacementValue, escapeForShell, depth + 1);
                    }
                }

                bool haveReplacement = false;

                if (replacementValue != token)
                {
                    haveReplacement = true;
                }
                else
                {
                    string? envValue = Environment.GetEnvironmentVariable(variableName);
                    if (envValue != null)
                    {
                        replacementValue = envValue;
                        haveReplacement = true;
                    }
                }

                if (haveReplacement)
                {
                    if (escape == EscapeType.UrlEncode)
                    {
                        replacementValue = Uri.EscapeDataString(replacementValue);
                    }
                    else if (escapeForShell)
                    {
                        char trailing = tokenEnd + 2 <= input.Length
                            ? input[tokenEnd + 1]
                            : '\0';

                        if (escape == EscapeType.All)
                        {
                            replacementValue = CommandLineArguments.EscapeBackslashes(replacementValue);
                            if (trailing == '\'')
                                replacementValue = CommandLineArguments.EscapeBackslashesForTrailingQuote(replacementValue);
                        }

                        if (escape == EscapeType.All || escape == EscapeType.ShellMetacharacters)
                            replacementValue = CommandLineArguments.EscapeShellMetacharacters(replacementValue);
                    }

                    replacements.Add(new Replacement(tokenStart, tokenLength, replacementValue));
                    index = tokenEnd + 1;
                }
                else
                {
                    index = tokenEnd;
                }
            } while (true);

            return replacements;
        }

        private static EscapeType DetermineEscapeType(string token)
        {
            EscapeType escape = EscapeType.All;
            char prefix = token[1];
            switch (prefix)
            {
                case '-':
                    escape = EscapeType.ShellMetacharacters;
                    break;
                case '!':
                    escape = EscapeType.None;
                    break;
                case '+':
                    escape = EscapeType.UrlEncode;
                    break;
            }

            return escape;
        }

        private string GetVariableReplacement(string variable, string original)
        {
            string replacement = "";
            if (_connectionInfo == null) return replacement;
            switch (variable.ToLowerInvariant())
            {
                case "name":
                    replacement = _connectionInfo.Name;
                    break;
                case "hostname":
                    replacement = _connectionInfo.Hostname;
                    break;
                case "port":
                    replacement = Convert.ToString(_connectionInfo.Port, CultureInfo.InvariantCulture);
                    break;
                case "username":
                    replacement = _connectionInfo.Username;
                    if (string.IsNullOrEmpty(replacement))
                        if (string.Equals(Properties.OptionsCredentialsPage.Default.EmptyCredentials, "windows", StringComparison.Ordinal))
                            replacement = Environment.UserName;
                        else if (string.Equals(Properties.OptionsCredentialsPage.Default.EmptyCredentials, "custom", StringComparison.Ordinal))
                            replacement = Properties.OptionsCredentialsPage.Default.DefaultUsername;
                    break;
                case "password":
                    //replacement = _connectionInfo.Password.ConvertToUnsecureString();
                    replacement = _connectionInfo.Password;
                    if (string.IsNullOrEmpty(replacement) && string.Equals(Properties.OptionsCredentialsPage.Default.EmptyCredentials, "custom", StringComparison.Ordinal))
                        replacement = new LegacyRijndaelCryptographyProvider().Decrypt(Convert.ToString(Properties.OptionsCredentialsPage.Default.DefaultPassword), Runtime.EncryptionKey);
                    break;
                case "domain":
                    replacement = _connectionInfo.Domain;
                    if (string.IsNullOrEmpty(replacement))
                        if (string.Equals(Properties.OptionsCredentialsPage.Default.EmptyCredentials, "windows", StringComparison.Ordinal))
                            replacement = Environment.UserDomainName;
                        else if (string.Equals(Properties.OptionsCredentialsPage.Default.EmptyCredentials, "custom", StringComparison.Ordinal))
                            replacement = Properties.OptionsCredentialsPage.Default.DefaultDomain;
                    break;
                case "description":
                    replacement = _connectionInfo.Description;
                    break;
                case "macaddress":
                    replacement = _connectionInfo.MacAddress;
                    break;
                case "userfield":
                    replacement = _connectionInfo.UserField;
                    break;
                case "userfield1":
                    replacement = _connectionInfo.UserField1;
                    break;
                case "userfield2":
                    replacement = _connectionInfo.UserField2;
                    break;
                case "userfield3":
                    replacement = _connectionInfo.UserField3;
                    break;
                case "userfield4":
                    replacement = _connectionInfo.UserField4;
                    break;
                case "userfield5":
                    replacement = _connectionInfo.UserField5;
                    break;
                case "userfield6":
                    replacement = _connectionInfo.UserField6;
                    break;
                case "userfield7":
                    replacement = _connectionInfo.UserField7;
                    break;
                case "userfield8":
                    replacement = _connectionInfo.UserField8;
                    break;
                case "userfield9":
                    replacement = _connectionInfo.UserField9;
                    break;
                case "userfield10":
                    replacement = _connectionInfo.UserField10;
                    break;
                case "protocol":
                    replacement = _connectionInfo.Protocol.ToString();
                    break;
                case "environmenttags":
                    replacement = _connectionInfo.EnvironmentTags;
                    break;
                case "sshoptions":
                    replacement = _connectionInfo.SSHOptions;
                    break;
                case "puttysession":
                    replacement = _connectionInfo.PuttySession;
                    break;
                case "authtype":
                    replacement = _externalTool?.AuthenticationType ?? string.Empty;
                    break;
                case "authusername":
                    replacement = _externalTool?.AuthenticationUsername ?? string.Empty;
                    break;
                case "authpassword":
                    replacement = _externalTool?.AuthenticationPassword ?? string.Empty;
                    break;
                case "privatekeyfile":
                    replacement = _externalTool?.PrivateKeyFile ?? string.Empty;
                    break;
                case "passphrase":
                    replacement = _externalTool?.Passphrase ?? string.Empty;
                    break;
                case "ipaddress":
                    replacement = _connectionInfo.IPAddress;
                    break;
                case "loadbalanceinfo":
                    replacement = _connectionInfo.LoadBalanceInfo;
                    break;
                case "privatekeypath":
                    replacement = _connectionInfo.PrivateKeyPath;
                    break;
                case "rdpstartprogram":
                    replacement = _connectionInfo.RDPStartProgram;
                    break;
                case "rdpstartprogramworkdir":
                    replacement = _connectionInfo.RDPStartProgramWorkDir;
                    break;
                case "notes":
                    replacement = _connectionInfo.Notes;
                    break;
                case "panel":
                    replacement = _connectionInfo.Panel;
                    break;
                case "openingcommand":
                    replacement = _connectionInfo.OpeningCommand;
                    break;
                default:
                    return original;
            }

            return replacement;
        }

        private static bool IsUserFieldVariable(string variableName)
        {
            return variableName.ToLowerInvariant().StartsWith("userfield", StringComparison.Ordinal);
        }

        private static string PerformReplacements(string input, List<Replacement> replacements)
        {
            int index;
            string result = input;

            for (index = result.Length; index >= 0; index--)
            {
                foreach (Replacement replacement in replacements)
                {
                    if (replacement.Start != index)
                    {
                        continue;
                    }

                    string before = result.Substring(0, replacement.Start);
                    string after = result.Substring(replacement.Start + replacement.Length);
                    result = before + replacement.Value + after;
                }
            }

            return result;
        }

        private enum EscapeType
        {
            All,
            ShellMetacharacters,
            None,
            UrlEncode
        }

        private struct Replacement(int start, int length, string value)
        {
            public int Start { get; } = start;

            public int Length { get; } = length;

            public string Value { get; } = value;
        }
    }
}
