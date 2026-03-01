using mRemoteNG.App.Info;
using mRemoteNG.Properties;
using mRemoteNG.Tools.Cmdline;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;

namespace mRemoteNG.App
{
    [SupportedOSPlatform("windows")]
    public sealed class CommandLineParser
    {
        private static readonly string[] ConnectionPathSwitches = ["cons", "c"];
        private static readonly string[] ConfigurationPathSwitches = ["settings", "settingspath", "config", "configpath", "cfg"];
        private static readonly string[] LogPathSwitches = ["log", "logpath", "logfile"];

        private readonly string[] _args;
        private readonly CmdArgumentsInterpreter _arguments;

        public CommandLineParser(IEnumerable<string> args)
        {
            _args = args?.ToArray() ?? [];
            _arguments = new CmdArgumentsInterpreter(_args);
        }

        public void ApplySwitches(bool applyLogPathToActiveLogger = false)
        {
            ApplyConnectionFileOverride();
            ApplyConfigurationPathOverride();
            ApplyLogPathOverride(applyLogPathToActiveLogger);
        }

        public string[] GetNormalizedArguments()
        {
            string[] normalizedArgs = (string[])_args.Clone();
            ExpandSwitchValue(normalizedArgs, ConnectionPathSwitches);
            ExpandSwitchValue(normalizedArgs, ConfigurationPathSwitches);
            ExpandSwitchValue(normalizedArgs, LogPathSwitches);
            return normalizedArgs;
        }

        private void ApplyConnectionFileOverride()
        {
            string? rawPath = GetArgumentValue(ConnectionPathSwitches);
            if (string.IsNullOrWhiteSpace(rawPath))
                return;

            string? resolvedPath = ResolveExistingFilePath(rawPath);
            if (string.IsNullOrWhiteSpace(resolvedPath))
                return;

            OptionsConnectionsPage.Default.ConnectionFilePath = resolvedPath;
            OptionsBackupPage.Default.LoadConsFromCustomLocation = true;
            OptionsBackupPage.Default.BackupLocation = resolvedPath;
        }

        private void ApplyConfigurationPathOverride()
        {
            string? rawPath = GetArgumentValue(ConfigurationPathSwitches);
            if (string.IsNullOrWhiteSpace(rawPath))
                return;

            string? resolvedPath = NormalizeDirectoryPath(rawPath);
            if (string.IsNullOrWhiteSpace(resolvedPath))
                return;

            Settings.Default.CustomConfigurationPath = resolvedPath;
        }

        private void ApplyLogPathOverride(bool applyToActiveLogger)
        {
            string? rawPath = GetArgumentValue(LogPathSwitches);
            if (string.IsNullOrWhiteSpace(rawPath))
                return;

            string? resolvedPath = NormalizeLogFilePath(rawPath);
            if (string.IsNullOrWhiteSpace(resolvedPath))
                return;

            OptionsNotificationsPage.Default.LogToApplicationDirectory = false;
            OptionsNotificationsPage.Default.LogFilePath = resolvedPath;

            if (!applyToActiveLogger)
                return;

            try
            {
                Logger.Instance.SetLogPath(resolvedPath);
            }
            catch
            {
                // Best effort only - do not fail argument processing on logger path errors.
            }
        }

        private string? GetArgumentValue(IEnumerable<string> switchNames)
        {
            foreach (string switchName in switchNames)
            {
                string? value = _arguments[switchName];
                if (!string.IsNullOrWhiteSpace(value))
                    return value;
            }

            return null;
        }

        private static string? ResolveExistingFilePath(string rawPath)
        {
            foreach (string candidatePath in ExpandFilePathCandidates(rawPath))
            {
                if (File.Exists(candidatePath))
                    return candidatePath;
            }

            return null;
        }

        private static IEnumerable<string> ExpandFilePathCandidates(string rawPath)
        {
            string? normalizedRawPath = TryNormalizePath(rawPath);
            if (!string.IsNullOrWhiteSpace(normalizedRawPath))
                yield return normalizedRawPath;

            string? homeRelativePath = TryNormalizePath(Path.Combine(GeneralAppInfo.HomePath, rawPath));
            if (!string.IsNullOrWhiteSpace(homeRelativePath))
                yield return homeRelativePath;

            string? defaultRelativePath = TryNormalizePath(Path.Combine(ConnectionsFileInfo.DefaultConnectionsPath, rawPath));
            if (!string.IsNullOrWhiteSpace(defaultRelativePath))
                yield return defaultRelativePath;
        }

        private static string? NormalizeDirectoryPath(string rawPath)
        {
            string? normalizedPath = TryNormalizePath(rawPath);
            if (string.IsNullOrWhiteSpace(normalizedPath))
                return null;

            string extension = Path.GetExtension(normalizedPath);
            if (string.Equals(extension, ".settings", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(extension, ".config", StringComparison.OrdinalIgnoreCase))
            {
                string? directoryPath = Path.GetDirectoryName(normalizedPath);
                if (!string.IsNullOrWhiteSpace(directoryPath))
                    return directoryPath;
            }

            return normalizedPath;
        }

        private static string? NormalizeLogFilePath(string rawPath)
        {
            string? expandedPath = TryExpandPath(rawPath);
            if (string.IsNullOrWhiteSpace(expandedPath))
                return null;

            bool endsWithSeparator = expandedPath.EndsWith(Path.DirectorySeparatorChar) ||
                                     expandedPath.EndsWith(Path.AltDirectorySeparatorChar);
            if (endsWithSeparator)
            {
                string logFileName = Path.GetFileName(Logger.DefaultLogPath);
                return TryNormalizePath(Path.Combine(expandedPath, logFileName));
            }

            return TryNormalizePath(expandedPath);
        }

        private static string? TryNormalizePath(string path)
        {
            string? expandedPath = TryExpandPath(path);
            if (string.IsNullOrWhiteSpace(expandedPath))
                return null;

            try
            {
                return Path.GetFullPath(expandedPath);
            }
            catch (Exception ex) when (ex is ArgumentException or NotSupportedException or PathTooLongException)
            {
                return null;
            }
        }

        private static string? TryExpandPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return null;

            try
            {
                return Environment.ExpandEnvironmentVariables(path.Trim().Trim('"'));
            }
            catch (Exception ex) when (ex is ArgumentException or NotSupportedException)
            {
                return null;
            }
        }

        private static void ExpandSwitchValue(string[] args, IReadOnlyCollection<string> switchNames)
        {
            for (int i = 0; i < args.Length; i++)
            {
                string argument = args[i];
                if (string.IsNullOrWhiteSpace(argument))
                    continue;

                if (!TryGetNamedSwitch(argument, out string switchName, out string? inlineValue))
                    continue;

                if (!switchNames.Contains(switchName, StringComparer.OrdinalIgnoreCase))
                    continue;

                if (inlineValue != null)
                {
                    string? expandedInlineValue = TryExpandPath(inlineValue);
                    if (!string.IsNullOrWhiteSpace(expandedInlineValue))
                        args[i] = ReplaceSwitchValue(argument, inlineValue, expandedInlineValue);
                    continue;
                }

                int valueIndex = i + 1;
                if (valueIndex >= args.Length)
                    continue;

                string? expandedValue = TryExpandPath(args[valueIndex]);
                if (!string.IsNullOrWhiteSpace(expandedValue))
                    args[valueIndex] = expandedValue;
            }
        }

        private static bool TryGetNamedSwitch(string argument, out string switchName, out string? inlineValue)
        {
            switchName = string.Empty;
            inlineValue = null;

            if (argument.StartsWith("--", StringComparison.Ordinal))
                return ParseSwitch(argument, 2, out switchName, out inlineValue);

            if (argument.StartsWith('-'))
                return ParseSwitch(argument, 1, out switchName, out inlineValue);

            if (argument.StartsWith('/'))
                return ParseSwitch(argument, 1, out switchName, out inlineValue);

            return false;
        }

        private static bool ParseSwitch(string argument, int prefixLength, out string switchName, out string? inlineValue)
        {
            switchName = string.Empty;
            inlineValue = null;

            string withoutPrefix = argument[prefixLength..];
            if (string.IsNullOrWhiteSpace(withoutPrefix))
                return false;

            int separatorIndex = withoutPrefix.IndexOfAny(['=', ':']);
            if (separatorIndex < 0)
            {
                switchName = withoutPrefix;
                return true;
            }

            switchName = withoutPrefix[..separatorIndex];
            if (separatorIndex + 1 < withoutPrefix.Length)
                inlineValue = withoutPrefix[(separatorIndex + 1)..];

            return true;
        }

        private static string ReplaceSwitchValue(string argument, string originalValue, string replacementValue)
        {
            int valueStart = argument.Length - originalValue.Length;
            return argument[..valueStart] + replacementValue;
        }
    }
}
