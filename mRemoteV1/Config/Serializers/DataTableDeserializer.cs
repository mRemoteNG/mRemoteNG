using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.ICA;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Container;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Serializers
{
    public class DataTableDeserializer : IDeserializer
    {
        private readonly DataTable _dataTable;

        public DataTableDeserializer(DataTable dataTable)
        {
            _dataTable = dataTable;
        }

        public DataTableDeserializer(IDataReader sqlDataReader)
        {
            _dataTable = new DataTable();
            _dataTable.Load(sqlDataReader);
        }

        public ConnectionTreeModel Deserialize()
        {
            var connectionList = CreateNodesFromTable();
            var connectionTreeModel = CreateNodeHierarchy(connectionList);
            return connectionTreeModel;
        }

        private List<ConnectionInfo> CreateNodesFromTable()
        {
            var nodeList = new List<ConnectionInfo>();
            foreach (DataRow row in _dataTable.Rows)
            {
                if ((string)row["Type"] == "Connection")
                    nodeList.Add(DeserializeConnectionInfo(row));
                else if ((string)row["Type"] == "Container")
                    nodeList.Add(DeserializeContainerInfo(row));
            }
            return nodeList;
        }

        private ConnectionInfo DeserializeConnectionInfo(DataRow row)
        {
            var connectionInfo = new ConnectionInfo();
            PopulateConnectionInfoFromDatarow(row, connectionInfo);
            return connectionInfo;
        }

        private ContainerInfo DeserializeContainerInfo(DataRow row)
        {
            var containerInfo = new ContainerInfo();
            PopulateConnectionInfoFromDatarow(row, containerInfo);
            return containerInfo;
        }

        private void PopulateConnectionInfoFromDatarow(DataRow dataRow, ConnectionInfo connectionInfo)
        {
            connectionInfo.Name = (string)dataRow["Name"];
            connectionInfo.ConstantID = (string)dataRow["ConstantID"];
            //connectionInfo.Parent.ConstantID = (string)dataRow["ParentID"];
            //connectionInfo is ContainerInfo ? ((ContainerInfo)connectionInfo).IsExpanded.ToString() : "" = dataRow["Expanded"];
            connectionInfo.Description = (string)dataRow["Description"];
            connectionInfo.Icon = (string)dataRow["Icon"];
            connectionInfo.Panel = (string)dataRow["Panel"];
            connectionInfo.Username = (string)dataRow["Username"];
            connectionInfo.Domain = (string)dataRow["DomainName"];
            connectionInfo.Password = (string)dataRow["Password"];
            connectionInfo.Hostname = (string)dataRow["Hostname"];
            connectionInfo.Protocol = (ProtocolType)Enum.Parse(typeof(ProtocolType), (string)dataRow["Protocol"]);
            connectionInfo.PuttySession = (string)dataRow["PuttySession"];
            connectionInfo.Port = (int)dataRow["Port"];
            connectionInfo.UseConsoleSession = (bool)dataRow["ConnectToConsole"];
            connectionInfo.UseCredSsp = (bool)dataRow["UseCredSsp"];
            connectionInfo.RenderingEngine = (HTTPBase.RenderingEngine)Enum.Parse(typeof(HTTPBase.RenderingEngine), (string)dataRow["RenderingEngine"]);
            connectionInfo.ICAEncryptionStrength = (ProtocolICA.EncryptionStrength)Enum.Parse(typeof(ProtocolICA.EncryptionStrength), (string)dataRow["ICAEncryptionStrength"]);
            connectionInfo.RDPAuthenticationLevel = (ProtocolRDP.AuthenticationLevel)Enum.Parse(typeof(ProtocolRDP.AuthenticationLevel), (string)dataRow["RDPAuthenticationLevel"]);
            connectionInfo.RDPMinutesToIdleTimeout = (int)dataRow["RDPMinutesToIdleTimeout"];
            connectionInfo.LoadBalanceInfo = (string)dataRow["LoadBalanceInfo"];
            connectionInfo.Colors = (ProtocolRDP.RDPColors)Enum.Parse(typeof(ProtocolRDP.RDPColors) ,(string)dataRow["Colors"]);
            connectionInfo.Resolution = (ProtocolRDP.RDPResolutions)Enum.Parse(typeof(ProtocolRDP.RDPResolutions), (string)dataRow["Resolution"]);
            connectionInfo.AutomaticResize = (bool)dataRow["AutomaticResize"];
            connectionInfo.DisplayWallpaper = (bool)dataRow["DisplayWallpaper"];
            connectionInfo.DisplayThemes = (bool)dataRow["DisplayThemes"];
            connectionInfo.EnableFontSmoothing = (bool)dataRow["EnableFontSmoothing"];
            connectionInfo.EnableDesktopComposition = (bool)dataRow["EnableDesktopComposition"];
            connectionInfo.CacheBitmaps = (bool)dataRow["CacheBitmaps"];
            connectionInfo.RedirectDiskDrives = (bool)dataRow["RedirectDiskDrives"];
            connectionInfo.RedirectPorts = (bool)dataRow["RedirectPorts"];
            connectionInfo.RedirectPrinters = (bool)dataRow["RedirectPrinters"];
            connectionInfo.RedirectSmartCards = (bool)dataRow["RedirectSmartCards"];
            connectionInfo.RedirectSound = (ProtocolRDP.RDPSounds)Enum.Parse(typeof(ProtocolRDP.RDPSounds), (string)dataRow["RedirectSound"]);
            connectionInfo.SoundQuality = (ProtocolRDP.RDPSoundQuality)Enum.Parse(typeof(ProtocolRDP.RDPSoundQuality), (string)dataRow["SoundQuality"]);
            connectionInfo.RedirectKeys = (bool)dataRow["RedirectKeys"];
            connectionInfo.PleaseConnect = (bool)dataRow["Connected"];
            connectionInfo.PreExtApp = (string)dataRow["PreExtApp"];
            connectionInfo.PostExtApp = (string)dataRow["PostExtApp"];
            connectionInfo.MacAddress = (string)dataRow["MacAddress"];
            connectionInfo.UserField = (string)dataRow["UserField"];
            connectionInfo.ExtApp = (string)dataRow["ExtApp"];
            connectionInfo.VNCCompression = (ProtocolVNC.Compression)Enum.Parse(typeof(ProtocolVNC.Compression), (string)dataRow["VNCCompression"]);
            connectionInfo.VNCEncoding = (ProtocolVNC.Encoding)Enum.Parse(typeof(ProtocolVNC.Encoding) ,(string)dataRow["VNCEncoding"]);
            connectionInfo.VNCAuthMode = (ProtocolVNC.AuthMode)Enum.Parse(typeof(ProtocolVNC.AuthMode), (string)dataRow["VNCAuthMode"]);
            connectionInfo.VNCProxyType = (ProtocolVNC.ProxyType)Enum.Parse(typeof(ProtocolVNC.ProxyType), (string)dataRow["VNCProxyType"]);
            connectionInfo.VNCProxyIP = (string)dataRow["VNCProxyIP"];
            connectionInfo.VNCProxyPort = (int)dataRow["VNCProxyPort"];
            connectionInfo.VNCProxyUsername = (string)dataRow["VNCProxyUsername"];
            connectionInfo.VNCProxyPassword = (string)dataRow["VNCProxyPassword"];
            connectionInfo.VNCColors = (ProtocolVNC.Colors)Enum.Parse(typeof(ProtocolVNC.Colors), (string)dataRow["VNCColors"]);
            connectionInfo.VNCSmartSizeMode = (ProtocolVNC.SmartSizeMode)Enum.Parse(typeof(ProtocolVNC.SmartSizeMode), (string)dataRow["VNCSmartSizeMode"]);
            connectionInfo.VNCViewOnly = (bool)dataRow["VNCViewOnly"];
            connectionInfo.RDGatewayUsageMethod = (ProtocolRDP.RDGatewayUsageMethod)Enum.Parse(typeof(ProtocolRDP.RDGatewayUsageMethod), (string)dataRow["RDGatewayUsageMethod"]);
            connectionInfo.RDGatewayHostname = (string)dataRow["RDGatewayHostname"];
            connectionInfo.RDGatewayUseConnectionCredentials = (ProtocolRDP.RDGatewayUseConnectionCredentials)Enum.Parse(typeof(ProtocolRDP.RDGatewayUseConnectionCredentials), (string)dataRow["RDGatewayUseConnectionCredentials"]);
            connectionInfo.RDGatewayUsername = (string)dataRow["RDGatewayUsername"];
            connectionInfo.RDGatewayPassword = (string)dataRow["RDGatewayPassword"];
            connectionInfo.RDGatewayDomain = (string)dataRow["RDGatewayDomain"];

            connectionInfo.Inheritance.CacheBitmaps = (bool)dataRow["InheritCacheBitmaps"];
            connectionInfo.Inheritance.Colors = (bool)dataRow["InheritColors"];
            connectionInfo.Inheritance.Description = (bool)dataRow["InheritDescription"];
            connectionInfo.Inheritance.DisplayThemes = (bool)dataRow["InheritDisplayThemes"];
            connectionInfo.Inheritance.DisplayWallpaper = (bool)dataRow["InheritDisplayWallpaper"];
            connectionInfo.Inheritance.EnableFontSmoothing = (bool)dataRow["InheritEnableFontSmoothing"];
            connectionInfo.Inheritance.EnableDesktopComposition = (bool)dataRow["InheritEnableDesktopComposition"];
            connectionInfo.Inheritance.Domain = (bool)dataRow["InheritDomain"];
            connectionInfo.Inheritance.Icon = (bool)dataRow["InheritIcon"];
            connectionInfo.Inheritance.Panel = (bool)dataRow["InheritPanel"];
            connectionInfo.Inheritance.Password = (bool)dataRow["InheritPassword"];
            connectionInfo.Inheritance.Port = (bool)dataRow["InheritPort"];
            connectionInfo.Inheritance.Protocol = (bool)dataRow["InheritProtocol"];
            connectionInfo.Inheritance.PuttySession = (bool)dataRow["InheritPuttySession"];
            connectionInfo.Inheritance.RedirectDiskDrives = (bool)dataRow["InheritRedirectDiskDrives"];
            connectionInfo.Inheritance.RedirectKeys = (bool)dataRow["InheritRedirectKeys"];
            connectionInfo.Inheritance.RedirectPorts = (bool)dataRow["InheritRedirectPorts"];
            connectionInfo.Inheritance.RedirectPrinters = (bool)dataRow["InheritRedirectPrinters"];
            connectionInfo.Inheritance.RedirectSmartCards = (bool)dataRow["InheritRedirectSmartCards"];
            connectionInfo.Inheritance.RedirectSound = (bool)dataRow["InheritRedirectSound"];
            connectionInfo.Inheritance.SoundQuality = (bool)dataRow["InheritSoundQuality"];
            connectionInfo.Inheritance.Resolution = (bool)dataRow["InheritResolution"];
            connectionInfo.Inheritance.AutomaticResize = (bool)dataRow["InheritAutomaticResize"];
            connectionInfo.Inheritance.UseConsoleSession = (bool)dataRow["InheritUseConsoleSession"];
            connectionInfo.Inheritance.UseCredSsp = (bool)dataRow["InheritUseCredSsp"];
            connectionInfo.Inheritance.RenderingEngine = (bool)dataRow["InheritRenderingEngine"];
            connectionInfo.Inheritance.Username = (bool)dataRow["InheritUsername"];
            connectionInfo.Inheritance.ICAEncryptionStrength = (bool)dataRow["InheritICAEncryptionStrength"];
            connectionInfo.Inheritance.RDPAuthenticationLevel = (bool)dataRow["InheritRDPAuthenticationLevel"];
            connectionInfo.Inheritance.RDPMinutesToIdleTimeout = (bool)dataRow["RDPMinutesToIdleTimeout"];
            connectionInfo.Inheritance.LoadBalanceInfo = (bool)dataRow["InheritLoadBalanceInfo"];
            connectionInfo.Inheritance.PreExtApp = (bool)dataRow["InheritPreExtApp"];
            connectionInfo.Inheritance.PostExtApp = (bool)dataRow["InheritPostExtApp"];
            connectionInfo.Inheritance.MacAddress = (bool)dataRow["InheritMacAddress"];
            connectionInfo.Inheritance.UserField = (bool)dataRow["InheritUserField"];
            connectionInfo.Inheritance.ExtApp = (bool)dataRow["InheritExtApp"];
            connectionInfo.Inheritance.VNCCompression = (bool)dataRow["InheritVNCCompression"];
            connectionInfo.Inheritance.VNCEncoding = (bool)dataRow["InheritVNCEncoding"];
            connectionInfo.Inheritance.VNCAuthMode = (bool)dataRow["InheritVNCAuthMode"];
            connectionInfo.Inheritance.VNCProxyType = (bool)dataRow["InheritVNCProxyType"];
            connectionInfo.Inheritance.VNCProxyIP = (bool)dataRow["InheritVNCProxyIP"];
            connectionInfo.Inheritance.VNCProxyPort = (bool)dataRow["InheritVNCProxyPort"];
            connectionInfo.Inheritance.VNCProxyUsername = (bool)dataRow["InheritVNCProxyUsername"];
            connectionInfo.Inheritance.VNCProxyPassword = (bool)dataRow["InheritVNCProxyPassword"];
            connectionInfo.Inheritance.VNCColors = (bool)dataRow["InheritVNCColors"];
            connectionInfo.Inheritance.VNCSmartSizeMode = (bool)dataRow["InheritVNCSmartSizeMode"];
            connectionInfo.Inheritance.VNCViewOnly = (bool)dataRow["InheritVNCViewOnly"];
            connectionInfo.Inheritance.RDGatewayUsageMethod = (bool)dataRow["InheritRDGatewayUsageMethod"];
            connectionInfo.Inheritance.RDGatewayHostname = (bool)dataRow["InheritRDGatewayHostname"];
            connectionInfo.Inheritance.RDGatewayUseConnectionCredentials = (bool)dataRow["InheritRDGatewayUseConnectionCredentials"];
            connectionInfo.Inheritance.RDGatewayUsername = (bool)dataRow["InheritRDGatewayUsername"];
            connectionInfo.Inheritance.RDGatewayPassword = (bool)dataRow["InheritRDGatewayPassword"];
            connectionInfo.Inheritance.RDGatewayDomain = (bool)dataRow["InheritRDGatewayDomain"];
        }

        private ConnectionTreeModel CreateNodeHierarchy(List<ConnectionInfo> connectionList)
        {
            var connectionTreeModel = new ConnectionTreeModel();
            var rootNode = new RootNodeInfo(RootNodeType.Connection) {ConstantID = "0"};
            connectionTreeModel.AddRootNode(rootNode);

            foreach (DataRow row in _dataTable.Rows)
            {
                var id = (string) row["ConstantID"];
                var connectionInfo = connectionList.First(node => node.ConstantID == id);
                var parentId = (string) row["ParentID"];
                if (parentId == "0")
                    rootNode.AddChild(connectionInfo);
                else
                    (connectionList.First(node => node.ConstantID == parentId) as ContainerInfo)?.AddChild(connectionInfo);
            }
            return connectionTreeModel;
        }
    }
}