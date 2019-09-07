using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using System.Collections.Generic;
using System.ComponentModel;

namespace mRemoteNG.Tools
{
    public class SSHTunnelTypeConverter : StringConverter
    {
        public static string[] SSHTunnels
        {
            get
            {
                var sshTunnelList = new List<string>();

                // Add a blank entry to signify that no external tool is selected
                sshTunnelList.Add(string.Empty);
                sshTunnelList.AddRange(getSSHConnectionNames(Runtime.ConnectionsService.ConnectionTreeModel.RootNodes));
                return sshTunnelList.ToArray();
            }
        }

        // recursively traverse the connection tree to find all ConnectionInfo s of type SSH
        private static List<string> getSSHConnectionNames(IEnumerable<ConnectionInfo> rootnodes)
        {
            List<string> result = new List<string>();
            foreach (var node in rootnodes)
            {
                if (node is ContainerInfo container)
                {
                    result.AddRange(getSSHConnectionNames(container.Children));
                }
                else
                {
                    if (!(node is PuttySessionInfo)) // only allow explicetly defined SSH connections as SSH Tunnels
                    {
                        if (node.Protocol == ProtocolType.SSH1 || node.Protocol == ProtocolType.SSH2) result.Add(node.Name);
                    }
                }
            }
            return result;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(SSHTunnels);
        }

        public override bool GetStandardValuesExclusive(System.ComponentModel.ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}