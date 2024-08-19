﻿using System;
using System.Collections.Generic;
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
    public class ExternalToolArgumentParser
    {
        private readonly ConnectionInfo _connectionInfo;

        public ExternalToolArgumentParser(ConnectionInfo connectionInfo)
        {
            _connectionInfo = connectionInfo;
        }

        public string ParseArguments(string input)
        {
            List<Replacement> replacements = BuildReplacementList(input);
            string result = PerformReplacements(input, replacements);
            return result;
        }

        private List<Replacement> BuildReplacementList(string input)
        {
            int index = 0;
            List<Replacement> replacements = new();
            do
            {
                int tokenStart = input.IndexOf("%", index, StringComparison.InvariantCulture);
                if (tokenStart == -1)
                    break;

                int tokenEnd = input.IndexOf("%", tokenStart + 1, StringComparison.InvariantCulture);
                if (tokenEnd == -1)
                    break;

                int tokenLength = tokenEnd - tokenStart + 1;
                int variableNameStart = tokenStart + 1;
                int variableNameLength = tokenLength - 2;
                bool isEnvironmentVariable = false;
                string variableName = "";

                if (tokenStart > 0)
                {
                    char tokenStartPrefix = input.Substring(tokenStart - 1, 1).ToCharArray()[0];
                    char tokenEndPrefix = input.Substring(tokenEnd - 1, 1).ToCharArray()[0];

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
                }

                bool haveReplacement = false;

                if (replacementValue != token)
                {
                    haveReplacement = true;
                }
                else
                {
                    replacementValue = Environment.GetEnvironmentVariable(variableName);
                    if (replacementValue != null)
                        haveReplacement = true;
                }

                if (haveReplacement)
                {
                    char trailing = tokenEnd + 2 <= input.Length
                        ? input.Substring(tokenEnd + 1, 1).ToCharArray()[0]
                        : '\0';

                    if (escape == EscapeType.All)
                    {
                        replacementValue = CommandLineArguments.EscapeBackslashes(replacementValue);
                        if (trailing == '\'')
                            replacementValue = CommandLineArguments.EscapeBackslashesForTrailingQuote(replacementValue);
                    }

                    if (escape == EscapeType.All || escape == EscapeType.ShellMetacharacters)
                        replacementValue = CommandLineArguments.EscapeShellMetacharacters(replacementValue);

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

        private EscapeType DetermineEscapeType(string token)
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
                    replacement = Convert.ToString(_connectionInfo.Port);
                    break;
                case "username":
                    replacement = _connectionInfo.Username;
                    if (string.IsNullOrEmpty(replacement))
                        if (Properties.OptionsCredentialsPage.Default.EmptyCredentials == "windows")
                            replacement = Environment.UserName;
                        else if (Properties.OptionsCredentialsPage.Default.EmptyCredentials == "custom")
                            replacement = Properties.OptionsCredentialsPage.Default.DefaultUsername;
                    break;
                case "password":
                    replacement = _connectionInfo.GetPlaintextPassword();
                    if (string.IsNullOrEmpty(replacement) && Properties.OptionsCredentialsPage.Default.EmptyCredentials == "custom")
                        replacement = new LegacyRijndaelCryptographyProvider().Decrypt(Convert.ToString(Properties.OptionsCredentialsPage.Default.DefaultPassword), Runtime.EncryptionKey);
                    break;
                case "domain":
                    replacement = _connectionInfo.Domain;
                    if (string.IsNullOrEmpty(replacement))
                        if (Properties.OptionsCredentialsPage.Default.EmptyCredentials == "windows")
                            replacement = Environment.UserDomainName;
                        else if (Properties.OptionsCredentialsPage.Default.EmptyCredentials == "custom")
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
                default:
                    return original;
            }

            return replacement;
        }

        private string PerformReplacements(string input, List<Replacement> replacements)
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
            None
        }

        private struct Replacement
        {
            public int Start { get; }

            public int Length { get; }

            public string Value { get; }

            public Replacement(int start, int length, string value)
            {
                Start = start;
                Length = length;
                Value = value;
            }
        }
    }
}