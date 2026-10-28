using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Xml.Linq;

namespace mRemoteNG.Config.Settings
{
    /// <summary>
    /// Reads the options mRemoteNG 1.76 left behind. 1.76 was built with no company name, so
    /// its user.config is under <c>%LOCALAPPDATA%\mRemoteNG\mRemoteNG.exe_Url_&lt;hash&gt;\&lt;version&gt;</c>,
    /// where later versions do not look; and it kept every option in one class,
    /// mRemoteNG.Settings, which later versions split into several. Each value is found again
    /// by its name, in whichever class has it now.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static class Settings176Importer
    {
        private const string SectionName = "mRemoteNG.Settings";

        /// <summary>Handled by the upgrade itself, or decided by this version on its first start.</summary>
        private static readonly HashSet<string> NotImported = new(StringComparer.Ordinal)
        {
            "DoUpgrade",
            "FirstStart",
            "UpdatePending",
        };

        /// <summary>
        /// The themes 1.76 loaded from its .vstheme files under their own names. Later versions
        /// add "NG" to them, to tell them from the DockPanelSuite themes of the same name, and
        /// treat the old name as an invalid theme.
        /// </summary>
        private static readonly Dictionary<string, string> RenamedThemes = new(StringComparer.Ordinal)
        {
            ["vs2015light"] = "vs2015lightNG",
            ["vs2015dark"] = "vs2015darkNG",
            ["vs2015blue"] = "vs2015blueNG",
            ["darcula"] = "darculaNG",
        };

        /// <summary>The user.config of the newest 1.76 found, or null.</summary>
        public static string? FindNewestUserConfig(string localAppData)
        {
            DirectoryInfo root = new(Path.Combine(localAppData, "mRemoteNG"));
            if (!root.Exists) return null;

            FileInfo? newest = null;
            Version? newestVersion = null;
            // One folder per install location, one per version inside it.
            foreach (DirectoryInfo versionFolder in root.EnumerateDirectories("mRemoteNG.exe_*")
                                                       .SelectMany(appFolder => appFolder.EnumerateDirectories()))
            {
                if (!Version.TryParse(versionFolder.Name, out Version? version)) continue;
                FileInfo file = new(Path.Combine(versionFolder.FullName, "user.config"));
                if (!file.Exists) continue;
                if (newest == null || version > newestVersion
                    || (version == newestVersion && file.LastWriteTimeUtc > newest.LastWriteTimeUtc))
                {
                    newest = file;
                    newestVersion = version;
                }
            }

            return newest?.FullName;
        }

        /// <summary>
        /// Sets each option found in <paramref name="userConfigPath"/> on every class in
        /// <paramref name="targets"/> that has an option of that name. Nothing is saved here.
        /// </summary>
        /// <param name="roamingAppData">Where 1.76 kept its data, to recognize the log path it wrote by itself.</param>
        /// <returns>The number of values set.</returns>
        public static int Import(string userConfigPath, IEnumerable<ApplicationSettingsBase> targets, string roamingAppData)
        {
            XElement? section = XDocument.Load(userConfigPath).Root?.Element("userSettings")?.Element(SectionName);
            if (section == null) return 0;

            List<ApplicationSettingsBase> classes = targets.ToList();
            string oldDataFolder = Path.Combine(roamingAppData, "mRemoteNG") + Path.DirectorySeparatorChar;
            int count = 0;

            foreach (XElement setting in section.Elements("setting"))
            {
                if (TryRead(setting, oldDataFolder, out string name, out string serialized))
                    count += SetOnEveryClass(classes, name, serialized);
            }

            // 1.76 opened a connections file kept elsewhere through LoadConsFromCustomLocation and
            // CustomConsPath; later versions open the one in ConnectionFilePath.
            string? customConnectionsFile = CustomConnectionsFile(section);
            if (customConnectionsFile != null)
                count += SetOnEveryClass(classes, "ConnectionFilePath", customConnectionsFile);

            return count;
        }

        /// <summary>
        /// The option's name and its value as the settings provider would read it from a file;
        /// false for an option that is not imported.
        /// </summary>
        private static bool TryRead(XElement setting, string oldDataFolder, out string name, out string serialized)
        {
            name = (string?)setting.Attribute("name") ?? "";
            serialized = "";
            XElement? value = setting.Element("value");
            if (name.Length == 0 || value == null || NotImported.Contains(name)) return false;

            serialized = (string?)setting.Attribute("serializeAs") == "Xml"
                ? string.Concat(value.Nodes())
                : value.Value;

            // 1.76 wrote its default log path into the file; this version logs elsewhere.
            if (name == "LogFilePath" && serialized.StartsWith(oldDataFolder, StringComparison.OrdinalIgnoreCase))
                return false;
            if (name == "ThemeName" && RenamedThemes.TryGetValue(serialized, out string? themeName))
                serialized = themeName;
            return true;
        }

        /// <returns>The number of classes the option was set on.</returns>
        private static int SetOnEveryClass(List<ApplicationSettingsBase> classes, string name, string serialized)
        {
            int count = 0;
            foreach (ApplicationSettingsBase target in classes)
            {
                SettingsProperty? property = target.Properties[name];
                if (property == null || !TryConvert(property, serialized, out object? value)) continue;
                target[name] = value;
                count++;
            }

            return count;
        }

        /// <summary>Converts the value the way the settings provider converts what it reads from a file.</summary>
        private static bool TryConvert(SettingsProperty property, string serialized, out object? value)
        {
            value = null;
            try
            {
                SettingsPropertyValue converted = new(property) { SerializedValue = serialized };
                value = converted.PropertyValue;
                // A value it cannot read comes back as the default, flagged as such.
                return !converted.UsingDefaultValue;
            }
            catch (Exception)
            {
                // Or it throws; either way the option keeps its default.
                return false;
            }
        }

        private static string? CustomConnectionsFile(XElement section)
        {
            string? Value(string name) => section.Elements("setting")
                .FirstOrDefault(s => (string?)s.Attribute("name") == name)?.Element("value")?.Value;

            string? path = Value("CustomConsPath");
            return string.Equals(Value("LoadConsFromCustomLocation"), "True", StringComparison.OrdinalIgnoreCase)
                   && !string.IsNullOrWhiteSpace(path)
                ? path
                : null;
        }
    }
}
