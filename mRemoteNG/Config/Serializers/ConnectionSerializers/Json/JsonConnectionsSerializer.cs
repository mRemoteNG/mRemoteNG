using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Text.Json;
using System.Text.Json.Serialization;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Security;
using mRemoteNG.Tools;

namespace mRemoteNG.Config.Serializers.ConnectionSerializers.Json
{
    [SupportedOSPlatform("windows")]
    public class JsonConnectionsSerializer : ISerializer<ConnectionInfo, string>
    {
        private static readonly JsonSerializerOptions s_jsonOptions = new()
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        private readonly SaveFilter _saveFilter;

        public Version Version { get; } = new(1, 0);

        public JsonConnectionsSerializer(SaveFilter saveFilter)
        {
            _saveFilter = saveFilter.ThrowIfNull(nameof(saveFilter));
        }

        public string Serialize(ConnectionInfo serializationTarget)
        {
            serializationTarget.ThrowIfNull(nameof(serializationTarget));
            object node = SerializeNode(serializationTarget);
            return JsonSerializer.Serialize(node, s_jsonOptions);
        }

        private object SerializeNode(ConnectionInfo connectionInfo)
        {
            Dictionary<string, object> dict = new(StringComparer.Ordinal)
            {
                ["Name"] = connectionInfo.Name,
                ["Id"] = connectionInfo.ConstantID,
                ["Type"] = connectionInfo.GetTreeNodeType().ToString(),
                ["Description"] = connectionInfo.Description,
                ["Icon"] = connectionInfo.Icon,
                ["Panel"] = connectionInfo.Panel,
                ["Hostname"] = connectionInfo.Hostname,
                ["Protocol"] = connectionInfo.Protocol.ToString(),
                ["Port"] = connectionInfo.Port,
                ["PuttySession"] = connectionInfo.PuttySession
            };

            if (_saveFilter.SaveUsername)
                dict["Username"] = connectionInfo.Username;
            if (_saveFilter.SavePassword)
                dict["Password"] = connectionInfo.Password;
            if (_saveFilter.SaveDomain)
                dict["Domain"] = connectionInfo.Domain;

            if (connectionInfo is ContainerInfo container)
            {
                List<object> children = new();
                foreach (ConnectionInfo child in container.Children)
                    children.Add(SerializeNode(child));
                dict["Children"] = children;
            }

            return dict;
        }
    }
}
