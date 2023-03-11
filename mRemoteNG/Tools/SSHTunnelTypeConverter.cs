using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Versioning;

namespace mRemoteNG.Tools
{
    [SupportedOSPlatform("windows")]
    public class SshTunnelTypeConverter : StringConverter
    {
        public static string[] SshTunnels
        {
            get
            {
                var sshTunnelList = new List<string> {string.Empty};

                // Add a blank entry to signify that no external tool is selected
                sshTunnelList.AddRange(GetSshConnectionNames(Runtime.ConnectionsService.ConnectionTreeModel.RootNodes));
                return sshTunnelList.ToArray();
            }
        }

        // recursively traverse the connection tree to find all ConnectionInfo s of type SSH
        private static IEnumerable<string> GetSshConnectionNames(IEnumerable<ConnectionInfo> rootnodes)
        {
            var result = new List<string>();
            foreach (var node in rootnodes)
                if (node is ContainerInfo container)
                {
                    result.AddRange(GetSshConnectionNames(container.Children));
                }
                else
                {
                    if (node is PuttySessionInfo) continue;
                    if (node.Protocol == ProtocolType.SSH1 || node.Protocol == ProtocolType.SSH2)
                        result.Add(node.Name);
                }

            return result;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(SshTunnels);
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}