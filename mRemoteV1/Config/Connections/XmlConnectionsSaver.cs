﻿using System;
using System.Linq;
using mRemoteNG.App;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Config.Serializers.Xml;
using mRemoteNG.Security;
using mRemoteNG.Security.Factories;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Connections
{
    public class XmlConnectionsSaver : ISaver<ConnectionTreeModel>
    {
        private readonly string _connectionFileName;
        private readonly SaveFilter _saveFilter;

        public XmlConnectionsSaver(string connectionFileName, SaveFilter saveFilter)
        {
            if (string.IsNullOrEmpty(connectionFileName))
                throw new ArgumentException($"Argument '{nameof(connectionFileName)}' cannot be null or empty");
            if (saveFilter == null)
                throw new ArgumentNullException(nameof(saveFilter));

            _connectionFileName = connectionFileName;
            _saveFilter = saveFilter;
        }

        public void Save(ConnectionTreeModel connectionTreeModel, string propertyNameTrigger = "")
        {
            try
            {
                var cryptographyProvider = new CryptoProviderFactoryFromSettings().Build();
                var serializerFactory = new XmlConnectionSerializerFactory();
                
                var xmlConnectionsSerializer = serializerFactory.Build(
                    cryptographyProvider,
                    connectionTreeModel,
                    _saveFilter,
                    mRemoteNG.Settings.Default.EncryptCompleteConnectionsFile);

                var rootNode = connectionTreeModel.RootNodes.OfType<RootNodeInfo>().First();
                var xml = xmlConnectionsSerializer.Serialize(rootNode);

                var fileDataProvider = new FileDataProviderWithRollingBackup(_connectionFileName);
                fileDataProvider.Save(xml);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector?.AddExceptionStackTrace("SaveToXml failed", ex);
            }
        }
    }
}