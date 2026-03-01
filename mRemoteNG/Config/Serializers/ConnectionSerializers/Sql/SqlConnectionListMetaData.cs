using System;

namespace mRemoteNG.Config.Serializers.ConnectionSerializers.Sql
{
    public class SqlConnectionListMetaData
    {
        public string Name { get; set; } = string.Empty;
        public string Protected { get; set; } = string.Empty;
        public bool Export { get; set; }
        public Version ConfVersion { get; set; } = new();

    }
}