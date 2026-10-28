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
            foreach (DirectoryInfo appFolder in root.EnumerateDirectories("mRemoteNG.exe_*"))
            {
                foreach (DirectoryInfo versionFolder in appFolder.EnumerateDirectories())
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
                string? name = (string?)setting.Attribute("name");
                XElement? value = setting.Element("value");
                if (name == null || value == null || NotImported.Contains(name)) continue;

                string serialized = (string?)setting.Attribute("serializeAs") == "Xml"
                    ? string.Concat(value.Nodes())
                    : value.Value;

                // 1.76 wrote its default log path into the file; this version logs elsewhere.
                if (name == "LogFilePath" && serialized.StartsWith(oldDataFolder, StringComparison.OrdinalIgnoreCase))
                    continue;
                if (name == "ThemeName" && RenamedThemes.TryGetValue(serialized, out string? themeName))
                    serialized = themeName;

                foreach (ApplicationSettingsBase target in classes)
                {
                    SettingsProperty? property = target.Properties[name];
                    if (property == null) continue;

                    try
                    {
                        // Converted the way the settings provider converts what it reads from a file.
                        // A value it cannot read comes back as the default, flagged as such.
                        SettingsPropertyValue converted = new(property) { SerializedValue = serialized };
                        object? propertyValue = converted.PropertyValue;
                        if (converted.UsingDefaultValue) continue;
                        target[name] = propertyValue;
                        count++;
                    }
                    catch (Exception)
                    {
                        // Or it throws; either way the option keeps its default.
                    }
                }
            }

            return count;
        }
    }
}
