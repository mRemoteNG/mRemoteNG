using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Runtime.Versioning;

namespace mRemoteNG.Config.Settings.Store
{
    /// <summary>
    /// Migrates settings from the legacy <see cref="ApplicationSettingsBase"/>-derived
    /// Properties classes into the new <see cref="ISettingsStore"/>.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static class LegacySettingsMigrator
    {
        private static readonly Dictionary<string, ApplicationSettingsBase> CategoryMap = new(StringComparer.OrdinalIgnoreCase)
        {
            ["App"] = Properties.App.Default,
            ["AppUI"] = Properties.AppUI.Default,
            ["Settings"] = Properties.Settings.Default,
            ["OptionsAdvancedPage"] = Properties.OptionsAdvancedPage.Default,
            ["OptionsAppearancePage"] = Properties.OptionsAppearancePage.Default,
            ["OptionsBackupPage"] = Properties.OptionsBackupPage.Default,
            ["OptionsConnectionsPage"] = Properties.OptionsConnectionsPage.Default,
            ["OptionsCredentialsPage"] = Properties.OptionsCredentialsPage.Default,
            ["OptionsDBsPage"] = Properties.OptionsDBsPage.Default,
            ["OptionsNotificationsPage"] = Properties.OptionsNotificationsPage.Default,
            ["OptionsSecurityPage"] = Properties.OptionsSecurityPage.Default,
            ["OptionsStartupExitPage"] = Properties.OptionsStartupExitPage.Default,
            ["OptionsTabsPanelsPage"] = Properties.OptionsTabsPanelsPage.Default,
            ["OptionsThemePage"] = Properties.OptionsThemePage.Default,
            ["OptionsUpdatesPage"] = Properties.OptionsUpdatesPage.Default,
        };

        /// <summary>
        /// Migrates all legacy settings into the given store.
        /// Skips settings that already exist in the store to avoid overwriting user changes.
        /// </summary>
        /// <returns>The number of settings migrated.</returns>
        public static int MigrateAll(ISettingsStore store)
        {
            if (store is null) throw new ArgumentNullException(nameof(store));

            int count = 0;

            foreach (KeyValuePair<string, ApplicationSettingsBase> entry in CategoryMap)
            {
                string category = entry.Key;
                ApplicationSettingsBase settings = entry.Value;

                try
                {
                    count += MigrateCategory(store, category, settings);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error migrating category '{category}': {ex.Message}");
                }
            }

            store.Flush();
            return count;
        }

        private static int MigrateCategory(ISettingsStore store, string category, ApplicationSettingsBase settings)
        {
            int count = 0;

            foreach (SettingsProperty property in settings.Properties)
            {
                string key = property.Name;

                // Don't overwrite if already migrated
                if (store.Exists(category, key))
                    continue;

                try
                {
                    object value = settings[key];
                    if (value is null)
                        continue;

                    string serialized = SerializeLegacyValue(value);
                    store.Set<string>(category, key, serialized);
                    count++;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error migrating '{category}.{key}': {ex.Message}");
                }
            }

            return count;
        }

        private static string SerializeLegacyValue(object value)
        {
            return value switch
            {
                string s => s,
                bool b => b ? "true" : "false",
                int or long or double or float or decimal => Convert.ToString(value, CultureInfo.InvariantCulture),
                System.Drawing.Point p => $"{p.X},{p.Y}",
                System.Drawing.Size s => $"{s.Width},{s.Height}",
                Enum e => e.ToString(),
                DateTime dt => dt.ToString("o", CultureInfo.InvariantCulture),
                _ => Convert.ToString(value, CultureInfo.InvariantCulture) ?? value.ToString()
            };
        }
    }
}
