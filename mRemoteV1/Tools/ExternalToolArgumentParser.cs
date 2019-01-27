using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Credential;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tools.Cmdline;

namespace mRemoteNG.Tools
{
	public class ExternalToolArgumentParser
    {
        private readonly ConnectionInfo _connectionInfo;
        private readonly ICredentialService _credentialService;

        public ExternalToolArgumentParser(ConnectionInfo connectionInfo, ICredentialService credentialService)
        {
            _connectionInfo = connectionInfo;
            _credentialService = credentialService.ThrowIfNull(nameof(credentialService));
        }

        public string ParseArguments(string input)
        {
            var replacements = BuildReplacementList(input);
            var result = PerformReplacements(input, replacements);
            return result;
        }

        private List<Replacement> BuildReplacementList(string input)
        {
            var index = 0;
            var replacements = new List<Replacement>();
            do
            {
                var tokenStart = input.IndexOf("%", index, StringComparison.InvariantCulture);
                if (tokenStart == -1)
                    break;

                var tokenEnd = input.IndexOf("%", tokenStart + 1, StringComparison.InvariantCulture);
                if (tokenEnd == -1)
                    break;

                var tokenLength = tokenEnd - tokenStart + 1;
                var variableNameStart = tokenStart + 1;
                var variableNameLength = tokenLength - 2;
                var isEnvironmentVariable = false;
                var variableName = "";

                if (tokenStart > 0)
                {
                    var tokenStartPrefix = input.Substring(tokenStart - 1, 1).ToCharArray()[0];
                    var tokenEndPrefix = input.Substring(tokenEnd - 1, 1).ToCharArray()[0];

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

                var token = input.Substring(tokenStart, tokenLength);

                var escape = DetermineEscapeType(token);

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

                var replacementValue = token;
                if (!isEnvironmentVariable)
                {
                    replacementValue = GetVariableReplacement(variableName, token);
                }

                var haveReplacement = false;

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
                    var trailing = tokenEnd + 2 <= input.Length ? input.Substring(tokenEnd + 1, 1).ToCharArray()[0] : '\0';

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
            var escape = EscapeType.All;
            var prefix = token[1];
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
            var normalizedVariable = variable.ToLowerInvariant();

            if (normalizedVariable == "name")
                return _connectionInfo?.Name ?? "";
            if (normalizedVariable == "hostname")
                return _connectionInfo?.Hostname ?? "";
            if (normalizedVariable == "port")
                return _connectionInfo?.Port.ToString() ?? "";
            if (normalizedVariable == "description")
                return _connectionInfo?.Description ?? "";
            if (normalizedVariable == "macaddress")
                return _connectionInfo?.MacAddress ?? "";
            if (normalizedVariable == "userfield")
                return _connectionInfo?.UserField ?? "";
            if (normalizedVariable.StartsWith("username"))
                return GetCredentialToUse(normalizedVariable)
                    .FirstOrDefault()?
                    .Username;
            if (normalizedVariable.StartsWith("password"))
                return GetCredentialToUse(normalizedVariable)
                    .FirstOrDefault()?
                    .Password.ConvertToUnsecureString();
            if (normalizedVariable.StartsWith("domain"))
                return GetCredentialToUse(normalizedVariable)
                    .FirstOrDefault()?
                    .Domain;

            return original;
        }

        private Optional<ICredentialRecord> GetCredentialToUse(string variable)
        {
            var specifiedCred = GetSpecifiedCredential(variable);
            return specifiedCred.Any() 
                ? specifiedCred 
                : _credentialService.GetEffectiveCredentialRecord(_connectionInfo?.CredentialRecordId ?? Optional<Guid>.Empty);
        }

        private Optional<ICredentialRecord> GetSpecifiedCredential(string variable)
        {
            var match = Regex.Match(variable.Replace("-", string.Empty), @":(?<id>[0-9a-fA-F]{7,32})$");
            if (!match.Success)
                return Optional<ICredentialRecord>.Empty;

            var id = match.Groups["id"].Value;
            return _credentialService
                .GetCredentialRecords()
                .FirstOrDefault(r => r.Id.ToString("N").StartsWith(id))
                .ToOptional();
        }

        private string PerformReplacements(string input, List<Replacement> replacements)
        {
            int index;
            var result = input;

            for (index = result.Length; index >= 0; index--)
            {
                foreach (var replacement in replacements)
                {
                    if (replacement.Start != index)
                    {
                        continue;
                    }

                    var before = result.Substring(0, replacement.Start);
                    var after = result.Substring(replacement.Start + replacement.Length);
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