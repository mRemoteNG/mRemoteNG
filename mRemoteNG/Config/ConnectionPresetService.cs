using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Xml;
using mRemoteNG.Connection;
using mRemoteNG.Messages;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config
{
    [SupportedOSPlatform("windows")]
    public class ConnectionPresetService
    {
        private readonly Lock _syncRoot = new();
        private readonly List<ConnectionPreset> _presets = [];
        private readonly string _presetFilePath;
        private readonly XmlConnectionPresetSerializer _serializer;
        private readonly XmlConnectionPresetDeserializer _deserializer;

        public ConnectionPresetService(
            string? presetFilePath = null,
            XmlConnectionPresetSerializer? serializer = null,
            XmlConnectionPresetDeserializer? deserializer = null)
        {
            _presetFilePath = string.IsNullOrWhiteSpace(presetFilePath)
                ? Path.Combine(SettingsFileInfo.SettingsPath, "connectionPresets.xml")
                : presetFilePath;

            _serializer = serializer ?? new XmlConnectionPresetSerializer();
            _deserializer = deserializer ?? new XmlConnectionPresetDeserializer();

            Load();
        }

        public IReadOnlyList<ConnectionPreset> GetPresets()
        {
            lock (_syncRoot)
            {
                return _presets
                    .Select(preset => preset.Clone())
                    .ToList();
            }
        }

        public IReadOnlyList<string> GetPresetNames()
        {
            lock (_syncRoot)
            {
                return _presets
                    .Select(preset => preset.Name)
                    .OrderBy(name => name, StringComparer.OrdinalIgnoreCase)
                    .ToArray();
            }
        }

        public bool SavePreset(string presetName, ConnectionInfo sourceConnection)
        {
            ArgumentNullException.ThrowIfNull(sourceConnection);
            string normalizedName = presetName?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(normalizedName))
                return false;

            ConnectionPreset newPreset = ConnectionPreset.FromConnection(normalizedName, sourceConnection);

            lock (_syncRoot)
            {
                UpsertPresetInternal(newPreset);
                SortPresetsInternal();
            }

            return PersistPresets();
        }

        public bool DeletePreset(string presetName)
        {
            string normalizedName = presetName?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(normalizedName))
                return false;

            bool removed;
            lock (_syncRoot)
            {
                removed = _presets.RemoveAll(preset =>
                    string.Equals(preset.Name, normalizedName, StringComparison.OrdinalIgnoreCase)) > 0;
            }

            if (!removed)
                return false;

            return PersistPresets();
        }

        public bool ApplyPreset(string presetName, IEnumerable<ConnectionInfo> targetConnections)
        {
            ArgumentNullException.ThrowIfNull(targetConnections);

            string normalizedName = presetName?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(normalizedName))
                return false;

            ConnectionPreset? preset;
            lock (_syncRoot)
            {
                preset = _presets
                    .FirstOrDefault(existing => string.Equals(existing.Name, normalizedName, StringComparison.OrdinalIgnoreCase))
                    ?.Clone();
            }

            if (preset == null)
                return false;

            int appliedCount = 0;
            foreach (ConnectionInfo targetConnection in targetConnections.Where(IsApplicableTarget))
            {
                preset.ApplyTo(targetConnection);
                appliedCount++;
            }

            return appliedCount > 0;
        }

        public void Load()
        {
            try
            {
                if (!File.Exists(_presetFilePath))
                {
                    lock (_syncRoot)
                    {
                        _presets.Clear();
                    }

                    return;
                }

                string serializedData = File.ReadAllText(_presetFilePath);
                IReadOnlyList<ConnectionPreset> loadedPresets = _deserializer.Deserialize(serializedData);

                lock (_syncRoot)
                {
                    _presets.Clear();
                    foreach (ConnectionPreset preset in loadedPresets)
                    {
                        if (string.IsNullOrWhiteSpace(preset.Name))
                            continue;

                        UpsertPresetInternal(preset.Clone());
                    }

                    SortPresetsInternal();
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(
                    "Loading connection presets failed.",
                    ex,
                    MessageClass.WarningMsg);
            }
        }

        private bool PersistPresets()
        {
            try
            {
                string? directoryName = Path.GetDirectoryName(_presetFilePath);
                if (!string.IsNullOrWhiteSpace(directoryName) && !Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }

                List<ConnectionPreset> snapshot;
                lock (_syncRoot)
                {
                    snapshot = _presets
                        .Select(preset => preset.Clone())
                        .ToList();
                }

                string serializedData = _serializer.Serialize(snapshot);
                File.WriteAllText(_presetFilePath, serializedData);
                return true;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(
                    "Saving connection presets failed.",
                    ex,
                    MessageClass.WarningMsg);
                return false;
            }
        }

        private void UpsertPresetInternal(ConnectionPreset preset)
        {
            int existingIndex = _presets.FindIndex(existing =>
                string.Equals(existing.Name, preset.Name, StringComparison.OrdinalIgnoreCase));

            if (existingIndex >= 0)
            {
                _presets[existingIndex] = preset;
                return;
            }

            _presets.Add(preset);
        }

        private void SortPresetsInternal()
        {
            _presets.Sort((left, right) =>
                string.Compare(left.Name, right.Name, StringComparison.OrdinalIgnoreCase));
        }

        private static bool IsApplicableTarget(ConnectionInfo connectionInfo)
        {
            return connectionInfo != null && connectionInfo is not RootNodeInfo;
        }
    }
}
