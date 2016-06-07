using System;
using System.Collections.Generic;
using mRemoteNG.Connection;

namespace mRemoteNG.Tools
{
    public class ArgumentParser
    {
        ConnectionInfo _connectionInfo;

        public ArgumentParser(ConnectionInfo connectionInfo)
        {
            _connectionInfo = connectionInfo;
        }

        public string ParseArguments(string input)
        {
            int index = 0;
            List<Replacement> replacements = new List<Replacement>();

            do
            {
                int tokenStart = input.IndexOf("%", index, StringComparison.InvariantCulture);
                if (tokenStart == -1)
                {
                    break;
                }

                int tokenEnd = input.IndexOf("%", tokenStart + 1, StringComparison.InvariantCulture);
                if (tokenEnd == -1)
                {
                    break;
                }

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

                EscapeType escape = EscapeType.All;
                string prefix = input.Substring(variableNameStart, 1);
                switch (prefix)
                {
                    case "-":
                        escape = EscapeType.ShellMetacharacters;
                        break;
                    case "!":
                        escape = EscapeType.None;
                        break;
                }

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
                    char trailing = '\0';
                    if (tokenEnd + 2 <= input.Length)
                        trailing = input.Substring(tokenEnd + 1, 1).ToCharArray()[0];
                    else
                        trailing = string.Empty.ToCharArray()[0];

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

        private string GetVariableReplacement(string variable, string original)
        {
            string replacement = "";
            if (_connectionInfo != null)
            {
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
                        break;
                    case "password":
                        replacement = _connectionInfo.Password;
                        break;
                    case "domain":
                        replacement = _connectionInfo.Domain;
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
            }
            return replacement;
        }

        private enum EscapeType
        {
            All,
            ShellMetacharacters,
            None
        }

        private struct Replacement
        {
            int _start;
            int _length;
            string _value;

            public int Start
            {
                get { return _start; }
                set { _start = value; }
            }
            public int Length 
            {
                get { return _length; }
                set { _length = value; }
            }
            public string Value 
            {
                get { return _value; }
                set { _value = value; }
            }

            public Replacement(int start, int length, string value)
            {
                _start = start;
                _length = length;
                _value = value;
            }
        }
    }
}