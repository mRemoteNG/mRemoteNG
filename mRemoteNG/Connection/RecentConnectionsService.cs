using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Versioning;
using System.Threading;
using System.Xml.Serialization;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Messages;

namespace mRemoteNG.Connection
{
    [SupportedOSPlatform("windows")]
    public class RecentConnectionsService
    {
        private const string FileName = "recentConnections.xml";
        private const int MaxRecentConnections = 10;
        private readonly string _filePath;
        private readonly Lock _recentConnectionsLock = new();
        private List<string> _recentConnectionIDs = new();
        private static readonly Lazy<RecentConnectionsService> _instance = new(() => new RecentConnectionsService());

        public static RecentConnectionsService Instance => _instance.Value;

        public event EventHandler? RecentConnectionsChanged;

        private RecentConnectionsService()
        {
            _filePath = Path.Combine(SettingsFileInfo.SettingsPath, FileName);
            Load();
        }

        public void Add(ConnectionInfo connectionInfo)
        {
            if (connectionInfo == null || connectionInfo.IsQuickConnect)
                return;

            if (string.IsNullOrEmpty(connectionInfo.ConstantID))
                return;

            try
            {
                bool changed = false;
                lock (_recentConnectionsLock)
                {
                    // Move to front if already in list
                    if (_recentConnectionIDs.Contains(connectionInfo.ConstantID))
                    {
                        _recentConnectionIDs.Remove(connectionInfo.ConstantID);
                    }
                    
                    _recentConnectionIDs.Insert(0, connectionInfo.ConstantID);

                    // Trim to max size
                    while (_recentConnectionIDs.Count > MaxRecentConnections)
                    {
                        _recentConnectionIDs.RemoveAt(_recentConnectionIDs.Count - 1);
                    }
                    changed = true;
                }

                if (changed)
                {
                    Save();
                    RecentConnectionsChanged?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("Failed to add recent connection", ex, MessageClass.WarningMsg);
            }
        }

        public IEnumerable<ConnectionInfo> GetRecentConnections()
        {
            var treeModel = Runtime.ConnectionsService.ConnectionTreeModel;
            if (treeModel == null)
                return new List<ConnectionInfo>();

            var result = new List<ConnectionInfo>();
            
            lock (_recentConnectionsLock)
            {
                foreach (var id in _recentConnectionIDs)
                {
                    var connection = treeModel.FindConnectionById(id);
                    if (connection != null)
                    {
                        result.Add(connection);
                    }
                }
            }

            return result;
        }

        private void Load()
        {
            try
            {
                if (!File.Exists(_filePath))
                    return;

                var serializer = new XmlSerializer(typeof(List<string>));
                using var stream = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
                var loaded = (List<string>?)serializer.Deserialize(stream);
                if (loaded != null)
                {
                    lock (_recentConnectionsLock)
                    {
                        _recentConnectionIDs = loaded;
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("Failed to load recent connections", ex, MessageClass.WarningMsg);
            }
        }

        private void Save()
        {
            try
            {
                var serializer = new XmlSerializer(typeof(List<string>));
                // Ensure directory exists
                var dir = Path.GetDirectoryName(_filePath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                using var stream = new FileStream(_filePath, FileMode.Create, FileAccess.Write);
                lock (_recentConnectionsLock)
                {
                    serializer.Serialize(stream, _recentConnectionIDs);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("Failed to save recent connections", ex, MessageClass.WarningMsg);
            }
        }
    }
}
