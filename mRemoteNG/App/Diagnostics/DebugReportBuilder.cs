using mRemoteNG.App.Info;
using mRemoteNG.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Text.RegularExpressions;

namespace mRemoteNG.App.Diagnostics
{
    [SupportedOSPlatform("windows")]
    public static class DebugReportBuilder
    {
        private const string RedactedValue = "<redacted>";
        private static readonly string[] SensitiveSettingNameTokens =
        [
            "password",
            "passphrase",
            "secret",
            "token",
            "apikey",
            "api_key",
            "privatekey",
            "private_key",
            "username",
            "user",
            "login",
            "hostname",
            "host",
            "server",
            "domain",
            "credential",
            "connectionstring"
        ];

        private static readonly Regex CredentialPairRegex = new(
            @"\b(password|passphrase|pwd|token|secret|api[-_ ]?key|private[-_ ]?key|username|user|login|hostname|host|server|domain)\b\s*[:=]\s*([^\s,;]+)",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        private static readonly Regex Ipv4AddressRegex = new(
            @"\b(?:\d{1,3}\.){3}\d{1,3}\b",
            RegexOptions.CultureInvariant | RegexOptions.Compiled);

        private static readonly Regex DomainUserRegex = new(
            @"\b[\w.-]+\[\w.-]+\b",
            RegexOptions.CultureInvariant | RegexOptions.Compiled);

        private static readonly Regex UserAtHostRegex = new(
            @"\b[\w.-]+@[\w.-]+\b",
            RegexOptions.CultureInvariant | RegexOptions.Compiled);

        private static readonly Regex HostnameRegex = new(
            @"\b(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?\.)+[A-Za-z]{2,63}\b",
            RegexOptions.CultureInvariant | RegexOptions.Compiled);

        public static string BuildReport(int maxLogLines = 250)
        {
            int boundedMaxLogLines = Math.Clamp(maxLogLines, 20, 2000);
            StringBuilder reportBuilder = new();

            reportBuilder.AppendLine("# mRemoteNG Debug Report");
            reportBuilder.AppendLine(CultureInfo.InvariantCulture, $"GeneratedUtc: {DateTime.UtcNow:O}");
            reportBuilder.AppendLine();

            AppendApplicationInfo(reportBuilder);
            AppendOperatingSystemInfo(reportBuilder);
            AppendSettingsInfo(reportBuilder);
            AppendRuntimeMessages(reportBuilder, boundedMaxLogLines);
            AppendLogFileExcerpt(reportBuilder, boundedMaxLogLines);

            return reportBuilder.ToString();
        }

        private static void AppendApplicationInfo(StringBuilder reportBuilder)
        {
            reportBuilder.AppendLine("## Application");
            reportBuilder.AppendLine(CultureInfo.InvariantCulture, $"Product: {GeneralAppInfo.ProductName}");
            reportBuilder.AppendLine(CultureInfo.InvariantCulture, $"Version: {GeneralAppInfo.ApplicationVersion}");
            reportBuilder.AppendLine(CultureInfo.InvariantCulture, $"Edition: {(Runtime.IsPortableEdition ? "Portable" : "Installed")}");
            reportBuilder.AppendLine(CultureInfo.InvariantCulture, $".NET CLR: {Environment.Version}");
            reportBuilder.AppendLine(CultureInfo.InvariantCulture, $"CommandLine: {SanitizeText(string.Join(" ", Environment.GetCommandLineArgs()))}");
            reportBuilder.AppendLine(CultureInfo.InvariantCulture, $"SettingsFile: {SanitizeText(SettingsFileInfo.UserSettingsFilePath)}");
            reportBuilder.AppendLine();
        }

        private static void AppendOperatingSystemInfo(StringBuilder reportBuilder)
        {
            reportBuilder.AppendLine("## Operating System");
            reportBuilder.AppendLine(CultureInfo.InvariantCulture, $"OSDescription: {RuntimeInformation.OSDescription}");
            reportBuilder.AppendLine(CultureInfo.InvariantCulture, $"OSVersion: {Environment.OSVersion}");
            reportBuilder.AppendLine(CultureInfo.InvariantCulture, $"OSArchitecture: {RuntimeInformation.OSArchitecture}");
            reportBuilder.AppendLine(CultureInfo.InvariantCulture, $"ProcessArchitecture: {RuntimeInformation.ProcessArchitecture}");
            reportBuilder.AppendLine(CultureInfo.InvariantCulture, $"64BitOS: {Environment.Is64BitOperatingSystem}");
            reportBuilder.AppendLine(CultureInfo.InvariantCulture, $"64BitProcess: {Environment.Is64BitProcess}");
            reportBuilder.AppendLine(CultureInfo.InvariantCulture, $"Culture: {CultureInfo.CurrentCulture.Name}");
            reportBuilder.AppendLine(CultureInfo.InvariantCulture, $"UICulture: {CultureInfo.CurrentUICulture.Name}");
            reportBuilder.AppendLine();
        }

        private static void AppendSettingsInfo(StringBuilder reportBuilder)
        {
            reportBuilder.AppendLine("## Program Settings");

            IEnumerable<ApplicationSettingsBase> settingsCollections = GetSettingsCollections();
            foreach (ApplicationSettingsBase settingsCollection in settingsCollections)
            {
                reportBuilder.AppendLine(CultureInfo.InvariantCulture, $"[{settingsCollection.GetType().Name}]");

                List<SettingsProperty> properties = settingsCollection.Properties.Cast<SettingsProperty>()
                    .OrderBy(p => p.Name, StringComparer.OrdinalIgnoreCase)
                    .ToList();

                foreach (SettingsProperty property in properties)
                {
                    string value = TryGetSettingValue(settingsCollection, property.Name);
                    string sanitizedValue = SanitizeSettingValue(property.Name, value);
                    reportBuilder.AppendLine(CultureInfo.InvariantCulture, $"{property.Name}={sanitizedValue}");
                }

                reportBuilder.AppendLine();
            }
        }

        private static void AppendRuntimeMessages(StringBuilder reportBuilder, int maxLogLines)
        {
            reportBuilder.AppendLine("## Runtime Messages");

            List<IMessage> recentMessages = Runtime.MessageCollector.Messages
                .TakeLast(maxLogLines)
                .ToList();

            if (recentMessages.Count == 0)
            {
                reportBuilder.AppendLine("<none>");
                reportBuilder.AppendLine();
                return;
            }

            foreach (IMessage message in recentMessages)
            {
                string sanitizedText = SanitizeText(message.Text);
                reportBuilder.AppendLine(CultureInfo.InvariantCulture, $"{message.Date:O} [{message.Class}] {sanitizedText}");
            }

            reportBuilder.AppendLine();
        }

        private static void AppendLogFileExcerpt(StringBuilder reportBuilder, int maxLogLines)
        {
            reportBuilder.AppendLine("## Log Excerpt");

            string logFilePath = GetLogFilePath();
            reportBuilder.AppendLine(CultureInfo.InvariantCulture, $"LogPath: {SanitizeText(logFilePath)}");

            if (string.IsNullOrWhiteSpace(logFilePath) || !File.Exists(logFilePath))
            {
                reportBuilder.AppendLine("<log file not found>");
                reportBuilder.AppendLine();
                return;
            }

            try
            {
                foreach (string line in ReadLastLines(logFilePath, maxLogLines))
                {
                    reportBuilder.AppendLine(SanitizeText(line));
                }
            }
            catch (Exception ex)
            {
                reportBuilder.AppendLine(CultureInfo.InvariantCulture, $"<unable to read log file: {ex.GetType().Name}>");
            }

            reportBuilder.AppendLine();
        }

        private static IEnumerable<ApplicationSettingsBase> GetSettingsCollections()
        {
            Type markerType = typeof(Properties.Settings);
            string settingsNamespace = markerType.Namespace ?? string.Empty;

            Type[] allTypes;
            try
            {
                allTypes = markerType.Assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                allTypes = ex.Types.Where(t => t != null).Cast<Type>().ToArray();
            }

            IEnumerable<Type> settingsTypes = allTypes
                .Where(type => type.Namespace == settingsNamespace
                               && !type.IsAbstract
                               && typeof(ApplicationSettingsBase).IsAssignableFrom(type))
                .OrderBy(type => type.Name, StringComparer.OrdinalIgnoreCase);

            foreach (Type settingsType in settingsTypes)
            {
                PropertyInfo? defaultProperty = settingsType.GetProperty("Default", BindingFlags.Public | BindingFlags.Static);
                if (defaultProperty?.GetValue(null) is ApplicationSettingsBase settings)
                {
                    yield return settings;
                }
            }
        }

        private static string TryGetSettingValue(ApplicationSettingsBase settingsCollection, string propertyName)
        {
            try
            {
                object? value = settingsCollection[propertyName];
                return ConvertToSettingText(value);
            }
            catch (Exception ex)
            {
                return $"<unavailable: {ex.GetType().Name}>";
            }
        }

        private static string ConvertToSettingText(object? value)
        {
            if (value == null)
            {
                return "<null>";
            }

            if (value is string stringValue)
            {
                return stringValue;
            }

            if (value is DateTime dateTimeValue)
            {
                return dateTimeValue.ToString("O", CultureInfo.InvariantCulture);
            }

            if (value is DateTimeOffset dateTimeOffsetValue)
            {
                return dateTimeOffsetValue.ToString("O", CultureInfo.InvariantCulture);
            }

            if (value is IEnumerable enumerable)
            {
                List<string> entries = [];
                foreach (object? item in enumerable)
                {
                    entries.Add(item == null
                        ? "<null>"
                        : Convert.ToString(item, CultureInfo.InvariantCulture) ?? item.ToString() ?? string.Empty);
                }

                return $"[{string.Join(", ", entries)}]";
            }

            return Convert.ToString(value, CultureInfo.InvariantCulture) ?? value.ToString() ?? string.Empty;
        }

        private static string SanitizeSettingValue(string settingName, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            if (IsSensitiveSettingName(settingName))
            {
                return RedactedValue;
            }

            return SanitizeText(value);
        }

        private static bool IsSensitiveSettingName(string settingName)
        {
            return SensitiveSettingNameTokens.Any(token =>
                settingName.Contains(token, StringComparison.OrdinalIgnoreCase));
        }

        private static string SanitizeText(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            string sanitized = value;

            string userProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            if (!string.IsNullOrWhiteSpace(userProfilePath))
            {
                sanitized = sanitized.Replace(userProfilePath, "%USERPROFILE%", StringComparison.OrdinalIgnoreCase);
            }

            sanitized = CredentialPairRegex.Replace(sanitized, match => $"{match.Groups[1].Value}={RedactedValue}");
            sanitized = DomainUserRegex.Replace(sanitized, RedactedValue);
            sanitized = UserAtHostRegex.Replace(sanitized, RedactedValue);
            sanitized = Ipv4AddressRegex.Replace(sanitized, RedactedValue);
            sanitized = HostnameRegex.Replace(sanitized, RedactedValue);

            return sanitized;
        }

        private static string GetLogFilePath()
        {
            string configuredPath = Properties.OptionsNotificationsPage.Default.LogFilePath;
            if (Properties.OptionsNotificationsPage.Default.LogToApplicationDirectory || string.IsNullOrWhiteSpace(configuredPath))
            {
                return Logger.DefaultLogPath;
            }

            return configuredPath;
        }

        private static IEnumerable<string> ReadLastLines(string filePath, int maxLines)
        {
            Queue<string> lines = new(maxLines);
            foreach (string line in File.ReadLines(filePath))
            {
                if (lines.Count == maxLines)
                {
                    lines.Dequeue();
                }

                lines.Enqueue(line);
            }

            return lines;
        }
    }
}
